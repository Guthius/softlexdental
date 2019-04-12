using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeBase;
using DataConnectionBase;

namespace OpenDentBusiness {
	public class WebChatMisc {

		public delegate void DbActionDelegate();

		private static DataConnection SetConnection(bool isWebChatDb) {
			DataConnection con=new DataConnection();
#if DEBUG
			con.SetDbT("localhost",isWebChatDb?"webchat":"customers","root","","","",DatabaseType.MySql,true);
#else
			con.SetDbT(isWebChatDb?"server201":"server",isWebChatDb?"webchat":"customers","root","","","",DatabaseType.MySql,true);
#endif
			return con;
		}

		///<summary>Creates an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.</summary>
		public static void DbAction(DbActionDelegate actionDelegate,bool isWebChatDb=true) {
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				DataConnection con=SetConnection(isWebChatDb);
				actionDelegate();
				con.Dispose();//Could use DataAction if DataConnection was correctly disposed inside DataAction.
			}));
			odThread.AddExceptionHandler((ex) => LogException(ex));
			odThread.Start(true);
			odThread.Join(Timeout.Infinite);
		}

		public static void LogException(Exception ex) {
			Logger.WriteException(ex,"WebChatLogs");
		}

		public static void LogError(string errorMsg) {
			Logger.WriteError(errorMsg,"WebChatLogs");
		}

	}
}