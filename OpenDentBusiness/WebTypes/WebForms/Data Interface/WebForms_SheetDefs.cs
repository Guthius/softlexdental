using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase;
using WebServiceSerializer;

namespace OpenDentBusiness.WebTypes.WebForms {
	public class WebForms_SheetDefs {

		/// <summary></summary>
		/// <param name="regKey"></param>
		/// <returns></returns>
		public static bool TryDownloadSheetDefs(out List<WebForms_SheetDef> listWebFormSheetDefs,string regKey=null) {
			if(string.IsNullOrEmpty(regKey)) {
				regKey=PrefC.GetString(PrefName.RegistrationKey);
			}
			listWebFormSheetDefs=new List<WebForms_SheetDef>();
			try {
				string payload=PayloadHelper.CreatePayloadWebHostSynch(regKey,new PayloadItem(regKey,"RegKey"));
				listWebFormSheetDefs=WebSerializer.DeserializeTag<List<WebForms_SheetDef>>(
					SheetsSynchProxy.GetWebServiceInstance().DownloadSheetDefs(payload),"Success"
				);
			}
			catch(Exception ex) {
				ex.DoNothing();
				return false;
			}
			return true;
		}

		///<summary>This method can throw an exception. Tries to upload a sheet def to HQ.</summary>
		///<param name="sheetDef">The SheetDef object to be uploaded.</param>
		public static void TryUploadSheetDef(SheetDef sheetDef) {
			string regKey=PrefC.GetString(PrefName.RegistrationKey);
			List<PayloadItem> listPayloadItems=new List<PayloadItem> {
				new PayloadItem(regKey,"RegKey"),
				new PayloadItem(sheetDef,"SheetDef")
			};
			string payload=PayloadHelper.CreatePayloadWebHostSynch(regKey,listPayloadItems.ToArray());
			SheetsSynchProxy.GetWebServiceInstance().UpLoadSheetDef(payload);
		}

		/// <summary></summary>
		/// <param name="regKey"></param>
		/// <param name="webSheetDefId"></param>
		public static bool DeleteSheetDef(long webSheetDefId,string regKey=null) {
			if(string.IsNullOrEmpty(regKey)) {
				regKey=PrefC.GetString(PrefName.RegistrationKey);
			}
			try {
				List<PayloadItem> listPayloadItems=new List<PayloadItem> {
					new PayloadItem(regKey,"RegKey"),
					new PayloadItem(webSheetDefId,"WebSheetDefID")
				};
				string payload=PayloadHelper.CreatePayloadWebHostSynch(regKey,listPayloadItems.ToArray());
				SheetsSynchProxy.GetWebServiceInstance().DeleteSheetDef(payload);
			}
			catch (Exception ex) {
				ex.DoNothing();
				return false;
			}
			return true;
		}

		/// <summary></summary>
		/// <param name="regKey"></param>
		/// <param name="webSheetDefId"></param>
		/// <param name="sheetDef"></param>
		public static bool UpdateSheetDef(long webSheetDefId,SheetDef sheetDef,string regKey=null,bool doCatchExceptions=true) {
			if(string.IsNullOrEmpty(regKey)) {
				regKey=PrefC.GetString(PrefName.RegistrationKey);
			}
			try {
				List<PayloadItem> listPayloadItems=new List<PayloadItem> {
					new PayloadItem(regKey,"RegKey"),
					new PayloadItem(webSheetDefId,"WebSheetDefID"),
					new PayloadItem(sheetDef,"SheetDef")
				};
				string payload=PayloadHelper.CreatePayloadWebHostSynch(regKey,listPayloadItems.ToArray());
				SheetsSynchProxy.GetWebServiceInstance().UpdateSheetDef(payload);
			}
			catch (Exception ex) {
				if(!doCatchExceptions) {
					throw;
				}
				ex.DoNothing();
				return false;
			}
			return true;
		}

	}
}
