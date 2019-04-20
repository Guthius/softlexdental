using System;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	///<summary>This table is not part of the general release.  User would have to add it manually.  
	///All schema changes are done directly on our live database as needed.</summary>
	[Serializable]
	[ODTable(IsMissingInGeneral=true)]
	public class ChatUser:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long ChatUserNum;
		///<summary></summary>
		public int Extension;
		///<summary></summary>
		public int CurrentSessions;
		///<summary>Milliseconds.</summary>
		public long SessionTime;
		///<summary></summary>

		public ChatUser Copy() {
			return (ChatUser)this.MemberwiseClone();
		}

	}

	
}




