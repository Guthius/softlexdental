using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeBase;

namespace OpenDentBusiness {
	public class WebChatMisc {

		public delegate void DbActionDelegate();

        private static void SetConnection(bool isWebChatDb)
        {
            DataConnection.SetDb("localhost", isWebChatDb ? "webchat" : "customers", "root", "", true);
        }

		///<summary>Creates an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.</summary>
		public static void DbAction(DbActionDelegate actionDelegate,bool isWebChatDb=true) {
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				SetConnection(isWebChatDb);
				actionDelegate();
			}));
			odThread.AddExceptionHandler((ex) => LogException(ex));
			odThread.Start(true);
			odThread.Join(Timeout.Infinite);
		}

		public static void LogException(Exception ex) {
			Logger.Write(ex);
		}

		public static void LogError(string errorMsg) {
			Logger.Write(LogLevel.Error, errorMsg);
		}

	}
}