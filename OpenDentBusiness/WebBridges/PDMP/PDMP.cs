using CodeBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	public class PDMP {

		public static string GetURL(Patient pat) {
			PDMPScript.PrescriptionSummaryRequestType request=new PDMPScript.PrescriptionSummaryRequestType();
			request.Requester=MakeRequester();
			request.PrescriptionRequest=MakePrescriptionRequest();
			PDMPScript.PrescriptionSummaryResponseType response=Request(ApiRoute.PrescriptionSummary,HttpMethod.Post
				,MakeBasicAuthHeader("",""),"TODO: convert request to xml and put string here",new PDMPScript.PrescriptionSummaryResponseType());
			return "";
		}

		private static PDMPScript.RequesterType MakeRequester() {
			return null;//TODO:
		}

		private static PDMPScript.PrescriptionSummaryRequestTypePrescriptionRequest MakePrescriptionRequest() {
			return null;//TODO:
		}

		private static string MakeBasicAuthHeader(string userName,string password) {
			//A basic auth header looks like this: "Basic Z3Vlc3Q6d2VsY29tZTEyMw==" (without quotes).
			//The Z3Vlc3Q6d2VsY29tZTEyMw== auth content is the userName:password converted to RawBase64.  See DoseSpotREST.GetToken for an example.
			//Don't forget the space between the word Basic and the auth content.
			return "";//TOOD:
		}

		///<summary>Throws exception if the response from the server returned an http code of 300 or greater.</summary>
		private static T Request<T>(ApiRoute route,HttpMethod method,string authHeader,string body,T responseType) {
			using(WebClient client=new WebClient()) {
				client.Headers[HttpRequestHeader.Accept]="application/xml";
				client.Headers[HttpRequestHeader.ContentType]="application/xml";
				client.Headers[HttpRequestHeader.Authorization]=authHeader;
				client.Encoding=UnicodeEncoding.UTF8;
				try {
					string res="";
					if(method==HttpMethod.Get) {
						res=client.DownloadString(GetApiUrl(route));
					}
					else if(method==HttpMethod.Post) {
						res=client.UploadString(GetApiUrl(route),HttpMethod.Post.Method,body);
					}
					else if(method==HttpMethod.Put) {
						res=client.UploadString(GetApiUrl(route),HttpMethod.Put.Method,body);
					}
					else {
						throw new Exception("Unsupported HttpMethod type: "+method.Method);
					}
#if DEBUG
					if((typeof(T)==typeof(string))) {//If user wants the entire json response as a string
						return (T)Convert.ChangeType(res,typeof(T));
					}
#endif
					return JsonConvert.DeserializeAnonymousType(res,responseType);
				}
				catch(WebException wex) {
					string res="";
					using(var sr=new StreamReader(((HttpWebResponse)wex.Response).GetResponseStream())) {
						res=sr.ReadToEnd();
					}
					if(string.IsNullOrWhiteSpace(res)) {
						//The response didn't contain a body.  Through my limited testing, it only happens for 401 (Unauthorized) requests.
						if(wex.Response.GetType()==typeof(HttpWebResponse)) {
							HttpStatusCode statusCode=((HttpWebResponse)wex.Response).StatusCode;
							if(statusCode==HttpStatusCode.Unauthorized) {
								throw new ODException(Lans.g("PDMP","Invalid PDMP credentials."));
							}
						}
					}
					string errorMsg=wex.Message+(string.IsNullOrWhiteSpace(res) ? "" : "\r\nRaw response:\r\n"+res);
					throw new Exception(errorMsg,wex);//If we got this far and haven't rethrown, simply throw the entire exception.
				}
				catch {
					//WebClient returned an http status code >= 300
	
					//For now, rethrow error and let whoever is expecting errors to handle them.
					//We may enhance this to care about codes at some point.
					throw;
				}
			}
		}

		///<summary>Returns the full URL according to the route/route id given.</summary>
		private static string GetApiUrl(ApiRoute route,string routeId="") {
			//string apiUrl=Introspection.GetOverride(Introspection.IntrospectionEntity.PDMPDebugURL,"https://TODO_prod");
			string apiUrl="TODO: Introspection";
#if DEBUG
			apiUrl="https://openid.logicoy.com/ilpdmp/test/getReport/";
#endif
			switch(route) {
				case ApiRoute.Root:
					//Do nothing.  This is to allow someone to quickly grab the URL without having to make a copy+paste reference.
					break;
				case ApiRoute.PrescriptionSummary:
					apiUrl+="/prescriptionSummary";
					break;
				default:
					break;
			}
			return apiUrl;
		}

		private enum ApiRoute {
			Root,
			PrescriptionSummary,
		}

	}
}
