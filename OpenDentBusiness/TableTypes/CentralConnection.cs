using System;
using System.Collections;
using System.Xml.Serialization;

namespace OpenDentBusiness{

	///<summary>Used by the Central Manager.  Stores the information needed to establish a connection to a remote database.</summary>
	[Serializable()]
	[ODTable(IsSynchable=true)]
	public class CentralConnection:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long CentralConnectionNum;
		///<summary>If direct db connection.  Can be ip address.</summary>
		public string ServerName;
		///<summary>If direct db connection.</summary>
		public string DatabaseName;
		///<summary>If direct db connection.</summary>
		public string MySqlUser;
		///<summary>If direct db connection.  Symmetrically encrypted.</summary>
		public string MySqlPassword;
		///<summary>If connecting to the web service. Can be on VPN, or can be over https.</summary>
		public string ServiceURI;
		///<summary>Deprecated.  If connecting to the web service.</summary>
		public string OdUser;
		///<summary>Deprecated.  If connecting to the web service.  Symmetrically encrypted.</summary>
		public string OdPassword;
		///<summary>When being used by ConnectionStore xml file, must deserialize to a ConnectionNames enum value. Otherwise just used as a generic notes field.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Note;
		///<summary>0-based.</summary>
		public int ItemOrder;
		///<summary>If set to true, the password hash is calculated differently.</summary>
		public bool WebServiceIsEcw;
		///<summary>Contains the most recent information about this connection.  OK if no problems, version information if version mismatch, 
		///nothing for not checked, and OFFLINE if previously couldn't connect.</summary>
		public string ConnectionStatus;
		///<summary>Set when reading from the config file. Not an actual DB column.</summary>
		[ODTableColumn(IsNotDbColumn=true)]
		public bool IsAutomaticLogin;
		///<summary>This is a helper variable used for Reports. If we want to start supporting connection string for the 
		///Reporting Server, we need to add this as a db column. This was needed for the scenario where a customer connected to OD using a connection string.</summary>
		[ODTableColumn(IsNotDbColumn=true)]
		public string ConnectionString;

		///<summary>Returns a copy.</summary>
		public CentralConnection Copy() {
			return (CentralConnection)this.MemberwiseClone();
		}

		public bool IsConnectionValid() {
			return this!=null && ConnectionStatus=="OK";
		}
	}
	


}













