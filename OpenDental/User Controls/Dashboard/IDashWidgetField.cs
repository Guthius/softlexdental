﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;

namespace OpenDental {
	interface IDashWidgetField {

		///<summary>Refreshes all the data required for display.  Must be implemented to be able to run on a thread.</summary>
		void RefreshData(Patient pat,SheetField sheetField);

		///<summary>Refreshes the view.  Must be implemented in a way to safely invoke back to the UI thread.</summary>
		void RefreshView();
	}
}
