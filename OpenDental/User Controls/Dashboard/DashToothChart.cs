using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Drawing.Drawing2D;

namespace OpenDental {
	public partial class DashToothChart:PictureBox,IDashWidgetField {
		private Image _imgToothChart;
		private SheetField _sheetField;
		public DashToothChart() {
			InitializeComponent();
		}

		public void RefreshData(Patient pat,SheetField sheetField) {
			long patNum=pat?.PatNum??0;
			_sheetField=sheetField;
			TreatPlan treatPlan=TreatPlans.GetActiveForPat(patNum);
			if(treatPlan!=null) {
				List<TreatPlanAttach> listTreatPlanAttaches=TreatPlanAttaches.GetAllForTreatPlan(treatPlan.TreatPlanNum);
				List<Procedure> listProcs=Procedures.GetManyProc(listTreatPlanAttaches.Select(x=>x.ProcNum).ToList(),false)
					.FindAll(x => x.ProcStatus==ProcStat.TP || x.ProcStatus==ProcStat.TPi);
				treatPlan.ListProcTPs=GetTreatProcTPs(treatPlan,listProcs);
			}
			_imgToothChart=SheetPrinting.GetToothChartHelper(patNum,true,treatPlan);
		}

		///<summary>Returns a list of limited versions of ProcTP corresponding to the Procedures in listProcs.  Intended to mimic the logic in 
		///ContrTreat.LoadActiveTP, on a limited data basis in order to extract the necessary data to create a ToothChart.</summary>
		private List<ProcTP> GetTreatProcTPs(TreatPlan treatPlan,List<Procedure> listProcs) {
			List<ProcTP> listProcTPs=new List<ProcTP>();
			foreach(Procedure proc in listProcs) {
				ProcTP procTP=new ProcTP();
				procTP.ProcNumOrig=proc.ProcNum;
				procTP.ToothNumTP=Tooth.ToInternat(proc.ToothNum);
				procTP.ProcCode=ProcedureCodes.GetStringProcCode(proc.CodeNum);
				if(ProcedureCodes.GetProcCode(proc.CodeNum).TreatArea==TreatmentArea.Surf) {
					procTP.Surf=Tooth.SurfTidyFromDbToDisplay(proc.Surf,proc.ToothNum);
				}
				else {
					procTP.Surf=proc.Surf;//for UR, L, etc.
				}
				listProcTPs.Add(procTP);
			}
			return listProcTPs;
		}

		public void RefreshView() {
			Image img=new Bitmap(_sheetField.Width,_sheetField.Height);
			Graphics g=Graphics.FromImage(img);
			g.SmoothingMode=SmoothingMode.HighQuality;
			SheetPrinting.DrawScaledImage(0,0,_sheetField.Width,_sheetField.Height,g,null,_imgToothChart);
			if(Image!=null) {
				Image.Dispose();
			}
			Image=img;
			g.Dispose();
		}
	}
}
