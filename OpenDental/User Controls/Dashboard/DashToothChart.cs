/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using OpenDentBusiness;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class DashToothChart:PictureBox,IDashWidgetField {
		private Image _imgToothChart;
		private SheetField _sheetField;
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
