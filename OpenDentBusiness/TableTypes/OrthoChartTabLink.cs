﻿using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness {
	///<summary>Links one orthocharttab to one displayfield.  Allows for displayfields to be part of multiple orthocharttabs.</summary>
	[Serializable]
	[ODTable(IsSynchable=true)]
	public class OrthoChartTabLink:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long OrthoChartTabLinkNum;
		///<summary>Overrides the displayfield ItemOrder, so that each display field can have a different order in each ortho chart tab.</summary>
		public int ItemOrder;
		///<summary>FK to orthocharttab.OrthoChartTabNum.</summary>
		public long OrthoChartTabNum;
		///<summary>FK to displayfield.DisplayFieldNum.</summary>
		public long DisplayFieldNum;

		public OrthoChartTabLink Copy() {
			return (OrthoChartTabLink)this.MemberwiseClone();
		}

	}
}