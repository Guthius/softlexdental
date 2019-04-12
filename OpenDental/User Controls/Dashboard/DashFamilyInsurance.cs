using System.Collections.Generic;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class DashFamilyInsurance:UserControl,IDashWidgetField {
		private List<InsPlan> _listInsPlans;
		private List<InsSub> _listInsSubs;
		private List<PatPlan> _listPatPlans;
		private List<Benefit> _listBenefits;
		private Patient _pat;

		public DashFamilyInsurance() {
			InitializeComponent();
			foreach(Control ctr in this.GetAllControls()) {
				ctr.MouseDown+=OnChildMouseDown;
				ctr.DragOver+=OnChildDragOver;
				ctr.DragDrop+=OnChildDragDrop;
			}
		}

		public double FamPriMax {
			get {
				return PIn.Double(textFamPriMax.Text);
			}
		}

		public double FamPriDed {
			get {
				return PIn.Double(textFamPriDed.Text);
			}
		}

		public double FamSecMax {
			get {
				return PIn.Double(textFamSecMax.Text);
			}
		}

		public double FamSecDed {
			get {
				return PIn.Double(textFamSecDed.Text);
			}
		}

		public void RefreshData(Patient pat,SheetField sheetField) {
			_listInsPlans=new List<InsPlan>();
			_listInsSubs=new List<InsSub>();
			_listPatPlans=new List<PatPlan>();
			_listBenefits=new List<Benefit>();
			_pat=pat;
			if(_pat==null) {
				return;
			}
			_listPatPlans=PatPlans.Refresh(_pat.PatNum);
			_listInsSubs=InsSubs.RefreshForFam(Patients.GetFamily(_pat.PatNum));
			_listInsPlans=InsPlans.RefreshForSubList(_listInsSubs);
			_listBenefits=Benefits.Refresh(_listPatPlans,_listInsSubs);
		}

		public void RefreshView() {
			RefreshInsurance(_pat,_listInsPlans,_listInsSubs,_listPatPlans,_listBenefits);
		}

		public void RefreshInsurance(Patient pat,List<InsPlan> listInsPlans,List<InsSub> listInsSubs,List<PatPlan> listPatPlans,List<Benefit> listBenefits){
			textFamPriMax.Text="";
			textFamPriDed.Text="";
			textFamSecMax.Text="";
			textFamSecDed.Text="";
			if(pat==null){
				return;
			}
			double maxFam=0;
			double maxInd=0;
			double dedFam=0;
			InsPlan PlanCur;//=new InsPlan();
			InsSub SubCur;
			if(listPatPlans.Count>0){
				SubCur=InsSubs.GetSub(listPatPlans[0].InsSubNum,listInsSubs);
				PlanCur=InsPlans.GetPlan(SubCur.PlanNum,listInsPlans);
				maxFam=Benefits.GetAnnualMaxDisplay(listBenefits,PlanCur.PlanNum,listPatPlans[0].PatPlanNum,true);
				maxInd=Benefits.GetAnnualMaxDisplay(listBenefits,PlanCur.PlanNum,listPatPlans[0].PatPlanNum,false);
				if(maxFam==-1){
					textFamPriMax.Text="";
				}
				else{
					textFamPriMax.Text=maxFam.ToString("F");
				}
				//deductible:
				dedFam=Benefits.GetDeductGeneralDisplay(listBenefits,PlanCur.PlanNum,listPatPlans[0].PatPlanNum,BenefitCoverageLevel.Family);
				if(dedFam!=-1) {
					textFamPriDed.Text=dedFam.ToString("F");
				}
			}
			if(listPatPlans.Count>1){
				SubCur=InsSubs.GetSub(listPatPlans[1].InsSubNum,listInsSubs);
				PlanCur=InsPlans.GetPlan(SubCur.PlanNum,listInsPlans);
				//max=Benefits.GetAnnualMaxDisplay(listBenefits,PlanCur.PlanNum,listPatPlans[1].PatPlanNum);
				maxFam=Benefits.GetAnnualMaxDisplay(listBenefits,PlanCur.PlanNum,listPatPlans[1].PatPlanNum,true);
				maxInd=Benefits.GetAnnualMaxDisplay(listBenefits,PlanCur.PlanNum,listPatPlans[1].PatPlanNum,false);
				if(maxFam==-1){
					textFamSecMax.Text="";
				}
				else{
					textFamSecMax.Text=maxFam.ToString("F");
				}
				//deductible:
				dedFam=Benefits.GetDeductGeneralDisplay(listBenefits,PlanCur.PlanNum,listPatPlans[1].PatPlanNum,BenefitCoverageLevel.Family);
				if(dedFam!=-1) {
					textFamSecDed.Text=dedFam.ToString("F");
				}
			}
		}

		private void OnChildMouseDown(object sender,MouseEventArgs e) {
			//Convert the click location to a position relative to the UserControlDashboardWidget.
			int x=e.X+((Control)sender).Location.X;
			int y=e.Y+((Control)sender).Location.Y;
			OnMouseDown(new MouseEventArgs(e.Button,e.Clicks,x,y,e.Delta));
			base.OnMouseDown(e);
		}

		private void OnChildDragOver(object sender,DragEventArgs drgevent) {
			OnDragOver(drgevent);
		}

		private void OnChildDragDrop(object sender,DragEventArgs drgevent) {
			OnDragDrop(drgevent);
		}
	}
}
