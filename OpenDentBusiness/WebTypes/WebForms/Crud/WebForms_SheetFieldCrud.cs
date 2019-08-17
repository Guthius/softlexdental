//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.WebTypes.WebForms.Crud{
	public class WebForms_SheetFieldCrud {
		///<summary>Gets one WebForms_SheetField object from the database using the primary key.  Returns null if not found.</summary>
		public static WebForms_SheetField SelectOne(long sheetFieldID) {
			string command="SELECT * FROM webforms_sheetfield "
				+"WHERE SheetFieldID = "+POut.Long(sheetFieldID);
			List<WebForms_SheetField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one WebForms_SheetField object from the database using a query.</summary>
		public static WebForms_SheetField SelectOne(string command) {
			List<WebForms_SheetField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of WebForms_SheetField objects from the database using a query.</summary>
		public static List<WebForms_SheetField> SelectMany(string command) {
			List<WebForms_SheetField> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<WebForms_SheetField> TableToList(DataTable table) {
			List<WebForms_SheetField> retVal=new List<WebForms_SheetField>();
			WebForms_SheetField webForms_SheetField;
			foreach(DataRow row in table.Rows) {
				webForms_SheetField=new WebForms_SheetField();
				webForms_SheetField.SheetFieldID            = PIn.Long  (row["SheetFieldID"].ToString());
				webForms_SheetField.SheetID                 = PIn.Long  (row["SheetID"].ToString());
				webForms_SheetField.FieldType               = (OpenDentBusiness.SheetFieldType)PIn.Int(row["FieldType"].ToString());
				webForms_SheetField.FieldName               = PIn.String(row["FieldName"].ToString());
				webForms_SheetField.FieldValue              = PIn.String(row["FieldValue"].ToString());
				webForms_SheetField.FontSize                = PIn.Float (row["FontSize"].ToString());
				webForms_SheetField.FontName                = PIn.String(row["FontName"].ToString());
				webForms_SheetField.FontIsBold              = PIn.Bool  (row["FontIsBold"].ToString());
				webForms_SheetField.XPos                    = PIn.Int   (row["XPos"].ToString());
				webForms_SheetField.YPos                    = PIn.Int   (row["YPos"].ToString());
				webForms_SheetField.Width                   = PIn.Int   (row["Width"].ToString());
				webForms_SheetField.Height                  = PIn.Int   (row["Height"].ToString());
				webForms_SheetField.GrowthBehavior          = (OpenDentBusiness.GrowthBehaviorEnum)PIn.Int(row["GrowthBehavior"].ToString());
				webForms_SheetField.RadioButtonValue        = PIn.String(row["RadioButtonValue"].ToString());
				webForms_SheetField.RadioButtonGroup        = PIn.String(row["RadioButtonGroup"].ToString());
				webForms_SheetField.IsRequired              = PIn.Bool  (row["IsRequired"].ToString());
				webForms_SheetField.TabOrder                = PIn.Int   (row["TabOrder"].ToString());
				webForms_SheetField.ReportableName          = PIn.String(row["ReportableName"].ToString());
				webForms_SheetField.TextAlign               = (System.Windows.Forms.HorizontalAlignment)PIn.Int(row["TextAlign"].ToString());
				webForms_SheetField.ItemColor               = Color.FromArgb(PIn.Int(row["ItemColor"].ToString()));
				webForms_SheetField.TabOrderMobile          = PIn.Int   (row["TabOrderMobile"].ToString());
				webForms_SheetField.UiLabelMobile           = PIn.String(row["UiLabelMobile"].ToString());
				webForms_SheetField.UiLabelMobileRadioButton= PIn.String(row["UiLabelMobileRadioButton"].ToString());
				retVal.Add(webForms_SheetField);
			}
			return retVal;
		}

		///<summary>Converts a list of WebForms_SheetField into a DataTable.</summary>
		public static DataTable ListToTable(List<WebForms_SheetField> listWebForms_SheetFields,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="WebForms_SheetField";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("SheetFieldID");
			table.Columns.Add("SheetID");
			table.Columns.Add("FieldType");
			table.Columns.Add("FieldName");
			table.Columns.Add("FieldValue");
			table.Columns.Add("FontSize");
			table.Columns.Add("FontName");
			table.Columns.Add("FontIsBold");
			table.Columns.Add("XPos");
			table.Columns.Add("YPos");
			table.Columns.Add("Width");
			table.Columns.Add("Height");
			table.Columns.Add("GrowthBehavior");
			table.Columns.Add("RadioButtonValue");
			table.Columns.Add("RadioButtonGroup");
			table.Columns.Add("IsRequired");
			table.Columns.Add("TabOrder");
			table.Columns.Add("ReportableName");
			table.Columns.Add("TextAlign");
			table.Columns.Add("ItemColor");
			table.Columns.Add("TabOrderMobile");
			table.Columns.Add("UiLabelMobile");
			table.Columns.Add("UiLabelMobileRadioButton");
			foreach(WebForms_SheetField webForms_SheetField in listWebForms_SheetFields) {
				table.Rows.Add(new object[] {
					POut.Long  (webForms_SheetField.SheetFieldID),
					POut.Long  (webForms_SheetField.SheetID),
					POut.Int   ((int)webForms_SheetField.FieldType),
					            webForms_SheetField.FieldName,
					            webForms_SheetField.FieldValue,
					POut.Float (webForms_SheetField.FontSize),
					            webForms_SheetField.FontName,
					POut.Bool  (webForms_SheetField.FontIsBold),
					POut.Int   (webForms_SheetField.XPos),
					POut.Int   (webForms_SheetField.YPos),
					POut.Int   (webForms_SheetField.Width),
					POut.Int   (webForms_SheetField.Height),
					POut.Int   ((int)webForms_SheetField.GrowthBehavior),
					            webForms_SheetField.RadioButtonValue,
					            webForms_SheetField.RadioButtonGroup,
					POut.Bool  (webForms_SheetField.IsRequired),
					POut.Int   (webForms_SheetField.TabOrder),
					            webForms_SheetField.ReportableName,
					POut.Int   ((int)webForms_SheetField.TextAlign),
					POut.Int   (webForms_SheetField.ItemColor.ToArgb()),
					POut.Int   (webForms_SheetField.TabOrderMobile),
					            webForms_SheetField.UiLabelMobile,
					            webForms_SheetField.UiLabelMobileRadioButton,
				});
			}
			return table;
		}

		///<summary>Inserts one WebForms_SheetField into the database.  Returns the new priKey.</summary>
		public static long Insert(WebForms_SheetField webForms_SheetField) {
			return Insert(webForms_SheetField,false);
		}

		///<summary>Inserts one WebForms_SheetField into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(WebForms_SheetField webForms_SheetField,bool useExistingPK) {
			string command="INSERT INTO webforms_sheetfield (";
			if(useExistingPK) {
				command+="SheetFieldID,";
			}
			command+="SheetID,FieldType,FieldName,FieldValue,FontSize,FontName,FontIsBold,XPos,YPos,Width,Height,GrowthBehavior,RadioButtonValue,RadioButtonGroup,IsRequired,TabOrder,ReportableName,TextAlign,ItemColor,TabOrderMobile,UiLabelMobile,UiLabelMobileRadioButton) VALUES(";
			if(useExistingPK) {
				command+=POut.Long(webForms_SheetField.SheetFieldID)+",";
			}
			command+=
				     POut.Long  (webForms_SheetField.SheetID)+","
				+    POut.Int   ((int)webForms_SheetField.FieldType)+","
				+"'"+POut.String(webForms_SheetField.FieldName)+"',"
				+    DbHelper.ParamChar+"paramFieldValue,"
				+    POut.Float (webForms_SheetField.FontSize)+","
				+"'"+POut.String(webForms_SheetField.FontName)+"',"
				+    POut.Bool  (webForms_SheetField.FontIsBold)+","
				+    POut.Int   (webForms_SheetField.XPos)+","
				+    POut.Int   (webForms_SheetField.YPos)+","
				+    POut.Int   (webForms_SheetField.Width)+","
				+    POut.Int   (webForms_SheetField.Height)+","
				+    POut.Int   ((int)webForms_SheetField.GrowthBehavior)+","
				+"'"+POut.String(webForms_SheetField.RadioButtonValue)+"',"
				+"'"+POut.String(webForms_SheetField.RadioButtonGroup)+"',"
				+    POut.Bool  (webForms_SheetField.IsRequired)+","
				+    POut.Int   (webForms_SheetField.TabOrder)+","
				+"'"+POut.String(webForms_SheetField.ReportableName)+"',"
				+    POut.Int   ((int)webForms_SheetField.TextAlign)+","
				+    POut.Int   (webForms_SheetField.ItemColor.ToArgb())+","
				+    POut.Int   (webForms_SheetField.TabOrderMobile)+","
				+"'"+POut.String(webForms_SheetField.UiLabelMobile)+"',"
				+"'"+POut.String(webForms_SheetField.UiLabelMobileRadioButton)+"')";
			if(webForms_SheetField.FieldValue==null) {
				webForms_SheetField.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,POut.StringParam(webForms_SheetField.FieldValue));
			if(useExistingPK) {
				Db.NonQ(command,paramFieldValue);
			}
			else {
				webForms_SheetField.SheetFieldID=Db.NonQ(command,paramFieldValue);
			}
			return webForms_SheetField.SheetFieldID;
		}

		///<summary>Inserts one WebForms_SheetField into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(WebForms_SheetField webForms_SheetField) {
			return InsertNoCache(webForms_SheetField,false);
		}

		///<summary>Inserts one WebForms_SheetField into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(WebForms_SheetField webForms_SheetField,bool useExistingPK) {
			string command="INSERT INTO webforms_sheetfield (";
			if(useExistingPK) {
				command+="SheetFieldID,";
			}
			command+="SheetID,FieldType,FieldName,FieldValue,FontSize,FontName,FontIsBold,XPos,YPos,Width,Height,GrowthBehavior,RadioButtonValue,RadioButtonGroup,IsRequired,TabOrder,ReportableName,TextAlign,ItemColor,TabOrderMobile,UiLabelMobile,UiLabelMobileRadioButton) VALUES(";
			if(useExistingPK) {
				command+=POut.Long(webForms_SheetField.SheetFieldID)+",";
			}
			command+=
				     POut.Long  (webForms_SheetField.SheetID)+","
				+    POut.Int   ((int)webForms_SheetField.FieldType)+","
				+"'"+POut.String(webForms_SheetField.FieldName)+"',"
				+    DbHelper.ParamChar+"paramFieldValue,"
				+    POut.Float (webForms_SheetField.FontSize)+","
				+"'"+POut.String(webForms_SheetField.FontName)+"',"
				+    POut.Bool  (webForms_SheetField.FontIsBold)+","
				+    POut.Int   (webForms_SheetField.XPos)+","
				+    POut.Int   (webForms_SheetField.YPos)+","
				+    POut.Int   (webForms_SheetField.Width)+","
				+    POut.Int   (webForms_SheetField.Height)+","
				+    POut.Int   ((int)webForms_SheetField.GrowthBehavior)+","
				+"'"+POut.String(webForms_SheetField.RadioButtonValue)+"',"
				+"'"+POut.String(webForms_SheetField.RadioButtonGroup)+"',"
				+    POut.Bool  (webForms_SheetField.IsRequired)+","
				+    POut.Int   (webForms_SheetField.TabOrder)+","
				+"'"+POut.String(webForms_SheetField.ReportableName)+"',"
				+    POut.Int   ((int)webForms_SheetField.TextAlign)+","
				+    POut.Int   (webForms_SheetField.ItemColor.ToArgb())+","
				+    POut.Int   (webForms_SheetField.TabOrderMobile)+","
				+"'"+POut.String(webForms_SheetField.UiLabelMobile)+"',"
				+"'"+POut.String(webForms_SheetField.UiLabelMobileRadioButton)+"')";
			if(webForms_SheetField.FieldValue==null) {
				webForms_SheetField.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,POut.StringParam(webForms_SheetField.FieldValue));
			if(useExistingPK) {
				Db.NonQ(command,paramFieldValue);
			}
			else {
				webForms_SheetField.SheetFieldID=Db.NonQ(command,paramFieldValue);
			}
			return webForms_SheetField.SheetFieldID;
		}

		///<summary>Updates one WebForms_SheetField in the database.</summary>
		public static void Update(WebForms_SheetField webForms_SheetField) {
			string command="UPDATE webforms_sheetfield SET "
				+"SheetID                 =  "+POut.Long  (webForms_SheetField.SheetID)+", "
				+"FieldType               =  "+POut.Int   ((int)webForms_SheetField.FieldType)+", "
				+"FieldName               = '"+POut.String(webForms_SheetField.FieldName)+"', "
				+"FieldValue              =  "+DbHelper.ParamChar+"paramFieldValue, "
				+"FontSize                =  "+POut.Float (webForms_SheetField.FontSize)+", "
				+"FontName                = '"+POut.String(webForms_SheetField.FontName)+"', "
				+"FontIsBold              =  "+POut.Bool  (webForms_SheetField.FontIsBold)+", "
				+"XPos                    =  "+POut.Int   (webForms_SheetField.XPos)+", "
				+"YPos                    =  "+POut.Int   (webForms_SheetField.YPos)+", "
				+"Width                   =  "+POut.Int   (webForms_SheetField.Width)+", "
				+"Height                  =  "+POut.Int   (webForms_SheetField.Height)+", "
				+"GrowthBehavior          =  "+POut.Int   ((int)webForms_SheetField.GrowthBehavior)+", "
				+"RadioButtonValue        = '"+POut.String(webForms_SheetField.RadioButtonValue)+"', "
				+"RadioButtonGroup        = '"+POut.String(webForms_SheetField.RadioButtonGroup)+"', "
				+"IsRequired              =  "+POut.Bool  (webForms_SheetField.IsRequired)+", "
				+"TabOrder                =  "+POut.Int   (webForms_SheetField.TabOrder)+", "
				+"ReportableName          = '"+POut.String(webForms_SheetField.ReportableName)+"', "
				+"TextAlign               =  "+POut.Int   ((int)webForms_SheetField.TextAlign)+", "
				+"ItemColor               =  "+POut.Int   (webForms_SheetField.ItemColor.ToArgb())+", "
				+"TabOrderMobile          =  "+POut.Int   (webForms_SheetField.TabOrderMobile)+", "
				+"UiLabelMobile           = '"+POut.String(webForms_SheetField.UiLabelMobile)+"', "
				+"UiLabelMobileRadioButton= '"+POut.String(webForms_SheetField.UiLabelMobileRadioButton)+"' "
				+"WHERE SheetFieldID = "+POut.Long(webForms_SheetField.SheetFieldID);
			if(webForms_SheetField.FieldValue==null) {
				webForms_SheetField.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,POut.StringParam(webForms_SheetField.FieldValue));
			Db.NonQ(command,paramFieldValue);
		}

		///<summary>Updates one WebForms_SheetField in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(WebForms_SheetField webForms_SheetField,WebForms_SheetField oldWebForms_SheetField) {
			string command="";
			if(webForms_SheetField.SheetID != oldWebForms_SheetField.SheetID) {
				if(command!="") { command+=",";}
				command+="SheetID = "+POut.Long(webForms_SheetField.SheetID)+"";
			}
			if(webForms_SheetField.FieldType != oldWebForms_SheetField.FieldType) {
				if(command!="") { command+=",";}
				command+="FieldType = "+POut.Int   ((int)webForms_SheetField.FieldType)+"";
			}
			if(webForms_SheetField.FieldName != oldWebForms_SheetField.FieldName) {
				if(command!="") { command+=",";}
				command+="FieldName = '"+POut.String(webForms_SheetField.FieldName)+"'";
			}
			if(webForms_SheetField.FieldValue != oldWebForms_SheetField.FieldValue) {
				if(command!="") { command+=",";}
				command+="FieldValue = "+DbHelper.ParamChar+"paramFieldValue";
			}
			if(webForms_SheetField.FontSize != oldWebForms_SheetField.FontSize) {
				if(command!="") { command+=",";}
				command+="FontSize = "+POut.Float(webForms_SheetField.FontSize)+"";
			}
			if(webForms_SheetField.FontName != oldWebForms_SheetField.FontName) {
				if(command!="") { command+=",";}
				command+="FontName = '"+POut.String(webForms_SheetField.FontName)+"'";
			}
			if(webForms_SheetField.FontIsBold != oldWebForms_SheetField.FontIsBold) {
				if(command!="") { command+=",";}
				command+="FontIsBold = "+POut.Bool(webForms_SheetField.FontIsBold)+"";
			}
			if(webForms_SheetField.XPos != oldWebForms_SheetField.XPos) {
				if(command!="") { command+=",";}
				command+="XPos = "+POut.Int(webForms_SheetField.XPos)+"";
			}
			if(webForms_SheetField.YPos != oldWebForms_SheetField.YPos) {
				if(command!="") { command+=",";}
				command+="YPos = "+POut.Int(webForms_SheetField.YPos)+"";
			}
			if(webForms_SheetField.Width != oldWebForms_SheetField.Width) {
				if(command!="") { command+=",";}
				command+="Width = "+POut.Int(webForms_SheetField.Width)+"";
			}
			if(webForms_SheetField.Height != oldWebForms_SheetField.Height) {
				if(command!="") { command+=",";}
				command+="Height = "+POut.Int(webForms_SheetField.Height)+"";
			}
			if(webForms_SheetField.GrowthBehavior != oldWebForms_SheetField.GrowthBehavior) {
				if(command!="") { command+=",";}
				command+="GrowthBehavior = "+POut.Int   ((int)webForms_SheetField.GrowthBehavior)+"";
			}
			if(webForms_SheetField.RadioButtonValue != oldWebForms_SheetField.RadioButtonValue) {
				if(command!="") { command+=",";}
				command+="RadioButtonValue = '"+POut.String(webForms_SheetField.RadioButtonValue)+"'";
			}
			if(webForms_SheetField.RadioButtonGroup != oldWebForms_SheetField.RadioButtonGroup) {
				if(command!="") { command+=",";}
				command+="RadioButtonGroup = '"+POut.String(webForms_SheetField.RadioButtonGroup)+"'";
			}
			if(webForms_SheetField.IsRequired != oldWebForms_SheetField.IsRequired) {
				if(command!="") { command+=",";}
				command+="IsRequired = "+POut.Bool(webForms_SheetField.IsRequired)+"";
			}
			if(webForms_SheetField.TabOrder != oldWebForms_SheetField.TabOrder) {
				if(command!="") { command+=",";}
				command+="TabOrder = "+POut.Int(webForms_SheetField.TabOrder)+"";
			}
			if(webForms_SheetField.ReportableName != oldWebForms_SheetField.ReportableName) {
				if(command!="") { command+=",";}
				command+="ReportableName = '"+POut.String(webForms_SheetField.ReportableName)+"'";
			}
			if(webForms_SheetField.TextAlign != oldWebForms_SheetField.TextAlign) {
				if(command!="") { command+=",";}
				command+="TextAlign = "+POut.Int   ((int)webForms_SheetField.TextAlign)+"";
			}
			if(webForms_SheetField.ItemColor != oldWebForms_SheetField.ItemColor) {
				if(command!="") { command+=",";}
				command+="ItemColor = "+POut.Int(webForms_SheetField.ItemColor.ToArgb())+"";
			}
			if(webForms_SheetField.TabOrderMobile != oldWebForms_SheetField.TabOrderMobile) {
				if(command!="") { command+=",";}
				command+="TabOrderMobile = "+POut.Int(webForms_SheetField.TabOrderMobile)+"";
			}
			if(webForms_SheetField.UiLabelMobile != oldWebForms_SheetField.UiLabelMobile) {
				if(command!="") { command+=",";}
				command+="UiLabelMobile = '"+POut.String(webForms_SheetField.UiLabelMobile)+"'";
			}
			if(webForms_SheetField.UiLabelMobileRadioButton != oldWebForms_SheetField.UiLabelMobileRadioButton) {
				if(command!="") { command+=",";}
				command+="UiLabelMobileRadioButton = '"+POut.String(webForms_SheetField.UiLabelMobileRadioButton)+"'";
			}
			if(command=="") {
				return false;
			}
			if(webForms_SheetField.FieldValue==null) {
				webForms_SheetField.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,POut.StringParam(webForms_SheetField.FieldValue));
			command="UPDATE webforms_sheetfield SET "+command
				+" WHERE SheetFieldID = "+POut.Long(webForms_SheetField.SheetFieldID);
			Db.NonQ(command,paramFieldValue);
			return true;
		}

		///<summary>Returns true if Update(WebForms_SheetField,WebForms_SheetField) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(WebForms_SheetField webForms_SheetField,WebForms_SheetField oldWebForms_SheetField) {
			if(webForms_SheetField.SheetID != oldWebForms_SheetField.SheetID) {
				return true;
			}
			if(webForms_SheetField.FieldType != oldWebForms_SheetField.FieldType) {
				return true;
			}
			if(webForms_SheetField.FieldName != oldWebForms_SheetField.FieldName) {
				return true;
			}
			if(webForms_SheetField.FieldValue != oldWebForms_SheetField.FieldValue) {
				return true;
			}
			if(webForms_SheetField.FontSize != oldWebForms_SheetField.FontSize) {
				return true;
			}
			if(webForms_SheetField.FontName != oldWebForms_SheetField.FontName) {
				return true;
			}
			if(webForms_SheetField.FontIsBold != oldWebForms_SheetField.FontIsBold) {
				return true;
			}
			if(webForms_SheetField.XPos != oldWebForms_SheetField.XPos) {
				return true;
			}
			if(webForms_SheetField.YPos != oldWebForms_SheetField.YPos) {
				return true;
			}
			if(webForms_SheetField.Width != oldWebForms_SheetField.Width) {
				return true;
			}
			if(webForms_SheetField.Height != oldWebForms_SheetField.Height) {
				return true;
			}
			if(webForms_SheetField.GrowthBehavior != oldWebForms_SheetField.GrowthBehavior) {
				return true;
			}
			if(webForms_SheetField.RadioButtonValue != oldWebForms_SheetField.RadioButtonValue) {
				return true;
			}
			if(webForms_SheetField.RadioButtonGroup != oldWebForms_SheetField.RadioButtonGroup) {
				return true;
			}
			if(webForms_SheetField.IsRequired != oldWebForms_SheetField.IsRequired) {
				return true;
			}
			if(webForms_SheetField.TabOrder != oldWebForms_SheetField.TabOrder) {
				return true;
			}
			if(webForms_SheetField.ReportableName != oldWebForms_SheetField.ReportableName) {
				return true;
			}
			if(webForms_SheetField.TextAlign != oldWebForms_SheetField.TextAlign) {
				return true;
			}
			if(webForms_SheetField.ItemColor != oldWebForms_SheetField.ItemColor) {
				return true;
			}
			if(webForms_SheetField.TabOrderMobile != oldWebForms_SheetField.TabOrderMobile) {
				return true;
			}
			if(webForms_SheetField.UiLabelMobile != oldWebForms_SheetField.UiLabelMobile) {
				return true;
			}
			if(webForms_SheetField.UiLabelMobileRadioButton != oldWebForms_SheetField.UiLabelMobileRadioButton) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one WebForms_SheetField from the database.</summary>
		public static void Delete(long sheetFieldID) {
			string command="DELETE FROM webforms_sheetfield "
				+"WHERE SheetFieldID = "+POut.Long(sheetFieldID);
			Db.NonQ(command);
		}

	}
}