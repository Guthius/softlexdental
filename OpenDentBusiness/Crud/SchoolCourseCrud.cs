//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class SchoolCourseCrud {
		///<summary>Gets one SchoolCourse object from the database using the primary key.  Returns null if not found.</summary>
		public static SchoolCourse SelectOne(long schoolCourseNum) {
			string command="SELECT * FROM schoolcourse "
				+"WHERE SchoolCourseNum = "+POut.Long(schoolCourseNum);
			List<SchoolCourse> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one SchoolCourse object from the database using a query.</summary>
		public static SchoolCourse SelectOne(string command) {
			
			List<SchoolCourse> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of SchoolCourse objects from the database using a query.</summary>
		public static List<SchoolCourse> SelectMany(string command) {
			
			List<SchoolCourse> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<SchoolCourse> TableToList(DataTable table) {
			List<SchoolCourse> retVal=new List<SchoolCourse>();
			SchoolCourse schoolCourse;
			foreach(DataRow row in table.Rows) {
				schoolCourse=new SchoolCourse();
				schoolCourse.SchoolCourseNum= PIn.Long  (row["SchoolCourseNum"].ToString());
				schoolCourse.CourseID       = PIn.String(row["CourseID"].ToString());
				schoolCourse.Descript       = PIn.String(row["Descript"].ToString());
				retVal.Add(schoolCourse);
			}
			return retVal;
		}

		///<summary>Converts a list of SchoolCourse into a DataTable.</summary>
		public static DataTable ListToTable(List<SchoolCourse> listSchoolCourses,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="SchoolCourse";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("SchoolCourseNum");
			table.Columns.Add("CourseID");
			table.Columns.Add("Descript");
			foreach(SchoolCourse schoolCourse in listSchoolCourses) {
				table.Rows.Add(new object[] {
					POut.Long  (schoolCourse.SchoolCourseNum),
					            schoolCourse.CourseID,
					            schoolCourse.Descript,
				});
			}
			return table;
		}

		///<summary>Inserts one SchoolCourse into the database.  Returns the new priKey.</summary>
		public static long Insert(SchoolCourse schoolCourse) {
			return Insert(schoolCourse,false);
		}

		///<summary>Inserts one SchoolCourse into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(SchoolCourse schoolCourse,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				schoolCourse.SchoolCourseNum=ReplicationServers.GetKey("schoolcourse","SchoolCourseNum");
			}
			string command="INSERT INTO schoolcourse (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SchoolCourseNum,";
			}
			command+="CourseID,Descript) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(schoolCourse.SchoolCourseNum)+",";
			}
			command+=
				 "'"+POut.String(schoolCourse.CourseID)+"',"
				+"'"+POut.String(schoolCourse.Descript)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				schoolCourse.SchoolCourseNum=Db.NonQ(command,true,"SchoolCourseNum","schoolCourse");
			}
			return schoolCourse.SchoolCourseNum;
		}

		///<summary>Inserts one SchoolCourse into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SchoolCourse schoolCourse) {
			return InsertNoCache(schoolCourse,false);
		}

		///<summary>Inserts one SchoolCourse into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SchoolCourse schoolCourse,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO schoolcourse (";
			if(!useExistingPK && isRandomKeys) {
				schoolCourse.SchoolCourseNum=ReplicationServers.GetKeyNoCache("schoolcourse","SchoolCourseNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="SchoolCourseNum,";
			}
			command+="CourseID,Descript) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(schoolCourse.SchoolCourseNum)+",";
			}
			command+=
				 "'"+POut.String(schoolCourse.CourseID)+"',"
				+"'"+POut.String(schoolCourse.Descript)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				schoolCourse.SchoolCourseNum=Db.NonQ(command,true,"SchoolCourseNum","schoolCourse");
			}
			return schoolCourse.SchoolCourseNum;
		}

		///<summary>Updates one SchoolCourse in the database.</summary>
		public static void Update(SchoolCourse schoolCourse) {
			string command="UPDATE schoolcourse SET "
				+"CourseID       = '"+POut.String(schoolCourse.CourseID)+"', "
				+"Descript       = '"+POut.String(schoolCourse.Descript)+"' "
				+"WHERE SchoolCourseNum = "+POut.Long(schoolCourse.SchoolCourseNum);
			Db.NonQ(command);
		}

		///<summary>Updates one SchoolCourse in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(SchoolCourse schoolCourse,SchoolCourse oldSchoolCourse) {
			string command="";
			if(schoolCourse.CourseID != oldSchoolCourse.CourseID) {
				if(command!="") { command+=",";}
				command+="CourseID = '"+POut.String(schoolCourse.CourseID)+"'";
			}
			if(schoolCourse.Descript != oldSchoolCourse.Descript) {
				if(command!="") { command+=",";}
				command+="Descript = '"+POut.String(schoolCourse.Descript)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE schoolcourse SET "+command
				+" WHERE SchoolCourseNum = "+POut.Long(schoolCourse.SchoolCourseNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(SchoolCourse,SchoolCourse) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(SchoolCourse schoolCourse,SchoolCourse oldSchoolCourse) {
			if(schoolCourse.CourseID != oldSchoolCourse.CourseID) {
				return true;
			}
			if(schoolCourse.Descript != oldSchoolCourse.Descript) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one SchoolCourse from the database.</summary>
		public static void Delete(long schoolCourseNum) {
			string command="DELETE FROM schoolcourse "
				+"WHERE SchoolCourseNum = "+POut.Long(schoolCourseNum);
			Db.NonQ(command);
		}

	}
}