using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness.WebServices;
using System.Collections;
using System.Xml.Serialization;

namespace UnitTests {
	///<summary>This class will use reflection to indicate what methods do not have the correct parameters within their RemotingRole check.</summary>
	[TestClass]
	public class ParamCheck:TestBase {
		private static HashSet<string> _setMiddleTierMethods=new HashSet<string>();

		[ClassInitialize]
		public static void TestInitialize(TestContext context) {
			RunTestsAgainstMiddleTier(new ParamCheckMockIIS());
		}

		[ClassCleanup]
		public static void Cleanup() {
			RevertMiddleTierSettingsIfNeeded();
		}

		[TestMethod]
		public void ParamCheckSClasses() {
			if(DateTime.Now.Year > 1970) {
				return;//This test is a bit dangerous, so it does not run by default. Comment out this line to run.
			}
			StringBuilder retVal=new StringBuilder();
			List<string> arraySClassPaths=Directory.GetFiles(@"C:\Development\OPEN DENTAL SUBVERSION\head\OpenDentBusiness\Data Interface").ToList();
			arraySClassPaths.AddRange(Directory.GetFiles(@"C:\Development\OPEN DENTAL SUBVERSION\head\OpenDentBusiness\Db Multi Table"));
			//Get just the names of the classes without the path and extension information.
			List<string> listSClassNames=arraySClassPaths.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
			Assembly ass=Assembly.LoadFrom(@"C:\Development\OPEN DENTAL SUBVERSION\head\UnitTests\bin\Debug\OpenDentBusiness.dll");
			List<Type> listSClassTypes=ass.GetTypes().Where(x => x.IsClass && x.IsPublic && listSClassNames.Contains(x.Name))
				//.Where(x => x.Name=="AccountModules")
				.OrderBy(x => x.Name)
				.ToList();
			//Loop through all classes and call every public static method at least once.
			//There will be a log created for all methods that end up calling Meth.Dto... which are going to be the only methods we really care about.
			foreach(Type sClass in listSClassTypes) {
				MethodInfo[] methods=sClass.GetMethods(BindingFlags.Static | BindingFlags.Public);
				foreach(MethodInfo method in methods) {
					if(method.IsGenericMethod || method.ContainsGenericParameters || method.IsAbstract || method.IsConstructor || method.IsFinal 
						|| method.IsVirtual) 
					{
						continue;
					}
					if(method.Name.StartsWith("get_") || method.Name.StartsWith("set_")
						//Cache methods
						|| method.Name.In("GetFirstOrDefault","GetWhere","GetExists","GetExists","GetLastOrDefault","GetLast","GetFindIndex","GetFirst",
							"GetFirstOrDefaultFromList","GetWhereFromList","FillCacheFromTable")
						//Methods that are too hard to invoke
						|| (sClass.Name=="Signalods" && method.Name=="SignalsTick")
						|| (sClass.Name=="Signalods" && method.Name=="SubscribeSignalProcessor")
						|| (sClass.Name=="MarkupEdit" && method.Name=="ValidateNodes")
						|| (sClass.Name=="MarkupEdit" && method.Name=="ValidateDuplicateNesting")
						//Methods that are too dangerous to run
						|| (sClass.Name=="MiscData" && method.Name=="SetMaxAllowedPacket")
						//Methods that cause a UI to come up
						|| (sClass.Name=="Security" && method.Name=="IsAuthorized")) 
					{
						continue;
					}
					bool hasOutOrRefParam=false;
					ParameterInfo[] parameters=method.GetParameters();
					object[] arrayObjs=ConstructParameters(parameters,sClass,method,retVal,out hasOutOrRefParam);					
					if(hasOutOrRefParam) {
						continue;//Middle tier does not support out or ref params.
					}
					try {
						method.Invoke(null,arrayObjs);
					}
					catch(Exception ex) {
						//Check that the error message comes from a middle tier bug
						ex=MiscUtils.GetExceptionContainingMessage(ex,"Method not found with ")
							??MiscUtils.GetExceptionContainingMessage(ex,"Middle Tier, incorrect number of parameters")
							??MiscUtils.GetExceptionContainingMessage(ex,"does not have a parameterless constructor.")
							??MiscUtils.GetExceptionContainingMessage(ex,"serializ")
							??MiscUtils.GetExceptionContainingMessage(ex,"There was an error generating the XML document")
							??MiscUtils.GetExceptionContainingMessage(ex,"There is an error in XML document")
							??MiscUtils.GetExceptionContainingMessage(ex,"No longer allowed to send SQL directly.")
							??MiscUtils.GetExceptionContainingMessage(ex,"Not allowed to send sql directly.");
						if(ex==null) {
							continue;
						}
						string error="Method: "+sClass?.Name+"."+method?.Name+"\r\n"
							+"\tError: "+ex.Message;
						Console.WriteLine(error);
						retVal.AppendLine(error);
					}
				}
			}
			Assert.AreEqual("",retVal.ToString());
		}

		///<summary>Creates an array of parameters with the default value for each parameter.</summary>
		public static object[] ConstructParameters(ParameterInfo[] parameters,Type sClass,MethodInfo method,StringBuilder sbErrors,out bool hasOutOrRefParam) {
			object[] arrayObjs=new object[parameters.Length];
			hasOutOrRefParam=false;
			for(int i=0;i<parameters.Length;i++) {
				try {
					Type parameterType=parameters[i].ParameterType;
					if(parameterType.IsByRef) {
						hasOutOrRefParam=true;
						break;
					}
					if(parameterType==typeof(string)) {
						arrayObjs[i]="";
					}
					else if(parameterType.IsArray) {
						arrayObjs[i]=Array.CreateInstance(parameterType.GetElementType(),0);
					}
					else if(parameterType==typeof(DataRow)) {
						arrayObjs[i]=new DataTable().NewRow();
					}
					else if(parameterType==typeof(DateTime)) {
						arrayObjs[i]=DateTime.Now;
					}
					else if(parameterType==typeof(Version)) {
						arrayObjs[i]=new Version(1,1,1,1);//Versions cannot be 0.0 for JSON
					}
					else if(parameterType==typeof(CultureInfo)) {
						arrayObjs[i]=CultureInfo.CurrentCulture;
					}
					else if(parameterType==typeof(CodeSystems.ProgressArgs)) {
						arrayObjs[i]=new CodeSystems.ProgressArgs((Action<int,int>)delegate (int a,int b) { });
					}
					else if(parameterType==typeof(Logger.WriteLineDelegate)) {
						arrayObjs[i]=new Logger.WriteLineDelegate((Action<string,LogLevel>)delegate (string a,LogLevel b) { });
					}
					else if(parameterType==typeof(Logger.IWriteLine)) {
						arrayObjs[i]=new LogDelegate(new Logger.WriteLineDelegate((a,b) => { }));
					}
					else if(parameterType==typeof(IODProgress)) {
						arrayObjs[i]=new ODProgressDoNothing();
					}
					else if(parameterType==typeof(IODProgressExtended)) {
						arrayObjs[i]=new ODProgressExtendedNull();
					}
					else if(parameterType==typeof(MethodInfo)) {
						arrayObjs[i]=method;
					}
					else if(parameterType==typeof(FeeCache)) {
						arrayObjs[i]=null;
					}
					else if(parameterType.IsEnum) {
						foreach(var enumVal in Enum.GetValues(parameterType)) {
							arrayObjs[i]=enumVal;
							break;
						}
					}
					else {
						arrayObjs[i]=Activator.CreateInstance(parameterType);
						if(!parameterType.IsValueType) {
							//A class type
							FieldInfo[] fiArray=parameterType.GetFields().Where(x => x!=null).ToArray();
							foreach(FieldInfo fi in fiArray) {
								object objCur=fi.GetValue(arrayObjs[i]);
								if(objCur==null) {
									continue;
								}
								if(fi.FieldType.IsEnum) {
									foreach(var enumVal in Enum.GetValues(fi.FieldType)) {
										objCur=enumVal;
										fi.SetValue(arrayObjs[i],objCur);
										break;
									}
								}
							}
						}
					}
				}
				catch(Exception ex) {
					ex=MiscUtils.GetInnermostException(ex);
					string error="!!!!!UNSUPPORTED PARAMETER TYPE!!!!\r\n"
								+"Method: "+sClass?.Name+"."+method?.Name+"\r\n"
								+"\tError: "+ex.Message;
					sbErrors.AppendLine(error);
				}
			}
			return arrayObjs;
		}

		[TestMethod]
		public void NoDataTableOrDataSetFields() {
			if(_setMiddleTierMethods.Count==0) {
				ParamCheckSClasses();//This method will populate the list of Middle Tier methods.
			}
			foreach(string fullMethodName in _setMiddleTierMethods) {
				string[] fullNameComponents=fullMethodName.Split('.');
				string assemblyName=fullNameComponents[0];//OpenDentBusiness
				string className=fullNameComponents[1];
				string methodName=fullNameComponents[2];
				Type classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
					+"."+className+","+assemblyName);
				MethodInfo[] methods=classType.GetMethods(BindingFlags.Public|BindingFlags.Static)
					.Where(x => x.Name==methodName).ToArray();
				Type[] parameters=methods.SelectMany(x => x.GetParameters()).Select(x => x.ParameterType).ToArray();
				Assert.IsFalse(parameters.Any(x => HasDataTableOrSetField(x)),"Method "+fullMethodName+" has a parameter that contains "
					+" a DataTable/Set.");
				Type[] returnTypes=methods.Select(x => x.ReturnType).ToArray();
				Assert.IsFalse(returnTypes.Any(x => HasDataTableOrSetField(x)),"Method "+fullMethodName+" has a return type that contains a DataTable/Set.");
			}
		}

		private bool HasDataTableOrSetField(Type type) {
			if(type==typeof(DataTable) || type==typeof(DataSet)) {
				return false;
			}
			return HasDataTableOrSetFieldRecursive(type,0);
		}

		private bool HasDataTableOrSetFieldRecursive(Type type,int depth) {
			if(depth==50) {
				return false;//In case an object contains itself as a field.
			}
			if(type==typeof(string)) {
				return false;
			}
			if(type.IsValueType) {
				return false;
			}
			if(type==typeof(DataTable) || type==typeof(DataSet)) {
				return true;
			}
			if(type.Namespace.StartsWith("System")) {
				return false;
			}
			if(type.IsGenericType) {
				Type[] genericTypes=type.GetGenericArguments();
				int newDepth=depth+1;
				return genericTypes.Any(x => HasDataTableOrSetFieldRecursive(x,newDepth));
			}
			if(typeof(IEnumerable).IsAssignableFrom(type)) {//type is probably an array
				Type elementType=type.GetElementType();
				if(elementType==null) {
					throw new Exception("Unsupported type: "+type.Name);
				}
				return HasDataTableOrSetFieldRecursive(elementType,++depth);
			}
			else {
				//if the object is not a value type and is not in the System namespace (besides strings, lists, and arrays) then it must be a class object,
				//i.e. a Patient or an Appointment object
				Type[] fieldTypes=type.GetFields(BindingFlags.Public|BindingFlags.Instance).Where(x => x!=null)
					.Where(x => x.GetCustomAttributes<XmlIgnoreAttribute>().Count()==0)
					.Select(x => x.FieldType).ToArray();
				int newDepth=depth+1;
				if(fieldTypes.Any(x => HasDataTableOrSetFieldRecursive(x,newDepth))) {
					return true;
				}
				Type[] propTypes=type.GetProperties().Where(x => x!=null)
					.Where(x => x.GetCustomAttributes<XmlIgnoreAttribute>().Count()==0)
					.Select(x => x.PropertyType).ToArray();
				if(propTypes.Any(x => HasDataTableOrSetFieldRecursive(x,newDepth))) {
					return true;
				}
				return false;
			}
		}

		private class ParamCheckMockIIS : OpenDentalServerMockIIS, IOpenDentalServer {
			public new string ProcessRequest(string dtoString) {
				DataTransferObject dto=DataTransferObject.Deserialize(dtoString);
				_setMiddleTierMethods.Add(dto.MethodName);
				return base.ProcessRequest(dtoString);
			}
		}
	}
}
