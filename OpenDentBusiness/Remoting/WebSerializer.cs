using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

//This file is used in conjunction with and must match WebServiceCustomerUpdates\WebSerialize.cs.
namespace WebServiceSerializer
{
    public static class WebSerializer
    {
        ///<summary>If the delimiter character is found in a given cell, then the cell's value will be updated to include the place holder value in lieu of the delimiter. This ensures that the delimiter is reserved for only delimiting cells. The place holder will be replaced by the delimiter value on the other end once the cells have been properly delimited.</summary>
        private const string _cellDelimiterPlaceHolder = "zzzzzzzzzz";

        ///<summary>This value is reserved strictly for delimiting cells in a serialized data row.</summary>
        private const string _cellDelimiter = "~";

        ///<summary>Format necessary for C#/Java date/time.</summary>
        private const string DotNetDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        ///<summary>Format necessary for MySql date/time.</summary>
        private const string MySqlDateTimeFormat = "%Y-%m-%d %H:%i:%s";

        public static XmlWriterSettings CreateXmlWriterSettings(bool omitXmlDeclaration)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");
            settings.OmitXmlDeclaration = omitXmlDeclaration;
            return settings;
        }

        private static string EscapeForXml(string myString)
        {
            if (string.IsNullOrEmpty(myString))
            {
                return "";
            }
            StringBuilder strBuild = new StringBuilder();
            int length = myString.Length;
            for (int i = 0; i < length; i++)
            {
                String character = myString.Substring(i, 1);
                if (character.Equals("<"))
                {
                    strBuild.Append("&lt;");
                    continue;
                }
                else if (character.Equals(">"))
                {
                    strBuild.Append("&gt;");
                    continue;
                }
                else if (character.Equals("\""))
                {
                    strBuild.Append("&quot;");
                    continue;
                }
                else if (character.Equals("\'"))
                {
                    strBuild.Append("&#039;");
                    continue;
                }
                else if (character.Equals("&"))
                {
                    strBuild.Append("&amp;");
                    continue;
                }
                strBuild.Append(character);
            }
            return strBuild.ToString();
        }

        private static string ReplaceEscapes(string myString)
        {
            if (string.IsNullOrEmpty(myString))
            {
                return "";
            }
            StringBuilder processedXml = new StringBuilder();
            for (int i = 0; i < myString.Length; i++)
            {
                //if at any point this char is not a match then ONLY append the start char, then continue
                //every continue should be accompanied by processedXml.Append(startChar)
                //search for consecutive [[ to open the special char indicator
                string startChar = myString.Substring(i, 1);
                if (startChar != "[")
                {
                    processedXml.Append(startChar);
                    continue;
                }
                string nextChar = myString.Substring(i + 1, 1);
                if (nextChar != "[")
                {
                    processedXml.Append(startChar);
                    continue;
                }
                //search for the consecutive ]] to close the special char indicator
                string remaining = myString.Substring(i, myString.Length - i);
                int endsAt = remaining.IndexOf("]]");
                if (endsAt < 0)
                { //make sure the special char is closed before the end of this xml tag
                    processedXml.Append(startChar);
                    continue;
                }
                //we have a good special char to translate it, append it, and set the new index location
                //get the guts of the special char
                string specialChar = remaining.Substring(2, remaining.IndexOf("]]") - 2);
                //convert to asci
                int asciiAsInt;
                if (!int.TryParse(specialChar, out asciiAsInt))
                { //not a valid ascii value
                    processedXml.Append(startChar);
                    continue;
                }
                //append the ascii char as a string
                processedXml.Append(Char.ConvertFromUtf32(asciiAsInt));
                //set the new index location, we have skipped a good chunk... [[123]]
                i += (endsAt + 1);
            }
            return processedXml.ToString();
        }

        public static string SerializeForCSharp(string objectType, Object obj)
        {
            if (obj == null)
            {
                return "<" + objectType + "/>";//Return an empty node?
            }
            if (obj.GetType().IsEnum)
            { //Serialize value as int.
                return "<" + objectType + ">" + POut.PInt((int)obj) + "</" + objectType + ">";
            }
            //Primitives--------------------------------------------------------------------
            if (objectType == "System.Int32" || objectType == "int")
            {
                return "<int>" + POut.PInt((int)obj) + "</int>";
            }
            if (objectType == "System.Int64" || objectType == "long")
            {
                return "<long>" + Convert.ToInt64(((long)obj)).ToString() + "</long>";
            }
            if (objectType == "System.Boolean" || objectType == "bool")
            {
                return "<bool>" + POut.PBool((bool)obj) + "</boolean>";
            }
            if (objectType == "System.String" || objectType == "string")
            {
                return "<string>" + EscapeForXml((string)obj) + "</string>";
            }
            if (objectType == "System.Char" || objectType == "char")
            {
                return "<char>" + Convert.ToChar((char)obj).ToString() + "</char>";
            }
            if (objectType == "System.Single" || objectType == "Single")
            {
                return "<float>" + POut.PFloat((float)obj) + "</float>";
            }
            if (objectType == "System.Byte" || objectType == "byte")
            {
                return "<byte>" + POut.PByte((byte)obj) + "</byte>";
            }
            if (objectType == "System.Double" || objectType == "double")
            {
                return "<double>" + POut.PDouble((double)obj) + "</double>";
            }
            if (objectType.StartsWith("List"))
            {//Lists.
                return SerializeList(objectType, obj);
            }
            if (objectType.Contains("["))
            {//Arrays.
                return SerializeArray(objectType, obj);
            }
            //DateTime----------------------------------------------------------------------
            if (objectType == "DateTime")
            {
                return "<DateTime>" + ((DateTime)obj).ToString(DotNetDateTimeFormat) + "</DateTime>";
            }
            //DataTable---------------------------------------------------------------------
            if (objectType == "DataTable")
            {
                return SerializeDataTable((DataTable)obj);
            }
            //DataSet-----------------------------------------------------------------------
            if (objectType == "DataSet")
            {
                return SerializeDataSet((DataSet)obj);
            }
            throw new NotSupportedException("SerializeForCSharp, unsupported class type: " + objectType);
        }

        public static object Deserialize(string typeName, string xml)
        {
            //Handle enums special.
            Type type = null;
            try { type = Type.GetType(typeName); }
            catch { /* We couldn't get the typeIn from the typeName so it won't be an enum. Swallow this error. */}
            if (type != null)
            { //Our input type is an enum so deserialize it according to what type of enum and if it was serialized as a string or an int.
                if (type.IsEnum)
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
                    {
                        if (reader.Read())
                        { //Value was serialized by int.
                            return Enum.ToObject(type, PIn.PInt(reader.ReadString()));
                        }
                    }
                }
                else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type listItemType = type.GetGenericArguments()[0];
                    return DeserializeList(xml);
                }
            }
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
                {
                    while (reader.Read())
                    {//In a loop just in case there is whitespace and it needs to read more than once.  Shouldn't need to loop more than once.
                     //Primitives--------------------------------------------------------------------
                        switch (reader.Name.ToLower())
                        { //Comes from xWebAppsCodeGenerator\GenerateTableTypes.GetSerializableTypeName().
                            case "int":
                            case "int32":
                                return PIn.PInt(reader.ReadString());
                            case "long":
                            case "int64":
                                return PIn.PLong(reader.ReadString());
                            case "bool":
                            case "boolean":
                                return PIn.PBool(reader.ReadString());
                            case "string":
                                return PIn.PString(reader.ReadString());
                            case "char":
                                return Convert.ToChar(reader.ReadString());
                            case "float":
                                return PIn.PFloat(reader.ReadString());
                            case "byte":
                                return PIn.PByte(reader.ReadString());
                            case "double":
                                return PIn.PDouble(reader.ReadString());
                            case "datetime": //Format matters here. Java put it in this format in Serializing.getSerializedObject().
                                return DateTime.ParseExact(reader.ReadString(), DotNetDateTimeFormat, null);
                            case "datatable":
                                return DeserializeDataTable(reader.ReadOuterXml());
                            case "dataset":
                                return DeserializeDataSet(reader.ReadOuterXml());
                        }
                        //Arrays------------------------------------------------------------------------
                        if (typeName.Contains("["))
                        {
                            //TODO: This will need to be enhanced to handle simple and possibly multidimensional arrays.
                            throw new Exception("Multidimensional arrays not supported");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Deserializing known type failed.
                string context = "Deserialize, error deserializing primitive or general type: " + typeName + "\r\n" + xml;
                throw e;
            }
            //Type must not be supported yet.
            Exception ex = new Exception("Deserialize, unsupported primitive or general type: " + typeName);
            throw ex;
        }

        public static T DeserializeTag<T>(string resultXml, string tagName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(resultXml);
            //Validate output.
            XmlNode node = doc.SelectSingleNode("//Error");
            if (node != null)
            {
                throw new Exception(node.InnerText);
            }
            node = doc.SelectSingleNode("//" + tagName);
            if (node == null)
            {
                throw new Exception("tagName node not found: " + tagName);
            }
            T retVal;
            using (XmlReader reader = XmlReader.Create(new StringReader(node.InnerXml)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                retVal = (T)serializer.Deserialize(reader);
            }
            if (retVal == null)
            {
                throw new Exception("tagName node invalid: " + tagName);
            }
            return retVal;
        }

        private static DataTable DeserializeDataTable(string xml)
        {
            DataTable dataTable = new DataTable();
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                if (!reader.ReadToFollowing("Name"))
                {
                    throw new Exception("Name tag not found");
                }
                dataTable.TableName = ReplaceEscapes(reader.ReadString());
                while (reader.Read())
                {
                    if (!reader.IsStartElement())
                    {
                        continue;
                    }
                    if (reader.Name == "")
                    {
                        continue;
                    }
                    if (reader.Name == "Cols")
                    {
                        continue;
                    }
                    if (reader.Name == "Col")
                    { //new column header
                        dataTable.Columns.Add(ReplaceEscapes(reader.ReadString()));
                        continue;
                    }
                    if (reader.Name == "Cells")
                    { //starting rows
                        continue;
                    }
                    if (reader.Name == "y")
                    { //new row						
                        DataRow row = dataTable.NewRow();
                        string pipedRow = reader.ReadString();
                        string[] cells = pipedRow.Split(new string[1] { _cellDelimiter }, StringSplitOptions.None);
                        if (cells.Length == dataTable.Columns.Count)
                        {
                            for (int i = 0; i < cells.Length; i++)
                            {
                                cells[i] = ReplaceEscapes(cells[i].Replace(_cellDelimiterPlaceHolder, _cellDelimiter));
                            }
                            row.ItemArray = cells;
                            dataTable.Rows.Add(row);
                        }
                        continue;
                    }
                }
            }
            return dataTable;
        }

        private static string SerializeDataTable(DataTable table)
        {
            StringBuilder result = new StringBuilder();
            result.Append("<DataTable>");
            //Table name.
            result.Append("<Name>").Append(table.TableName).Append("</Name>");
            //Column names.
            result.Append("<Cols>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                result.Append("<Col>").Append(table.Columns[i].ColumnName).Append("</Col>");
            }
            result.Append("</Cols>");
            //Set each cell by looping through each column row by row.
            result.Append("<Cells>");
            for (int i = 0; i < table.Rows.Count; i++)
            {//Row loop.
                result.Append("<y>");
                for (int j = 0; j < table.Columns.Count; j++)
                {//Column loop.
                    string cellValue = table.Rows[i][j].ToString();
                    if (table.Columns[j].DataType.Name == "DateTime")
                    { //DateTime requires special formatting so it can be deserialized by java in DataTable.getCellDateFromFormatString().
                        DateTime dt;
                        if (!DateTime.TryParse(table.Rows[i][j].ToString(), out dt))
                        { //Shouldn't get here but just in case, give it a default value.
                            dt = new DateTime(1, 1, 1);
                        }
                        cellValue = dt.ToString(DotNetDateTimeFormat);
                    }
                    //Add the formatted string.
                    result.Append(EscapeForXml(cellValue).Replace(_cellDelimiter, _cellDelimiterPlaceHolder));
                    if (j < table.Columns.Count - 1)
                    {
                        result.Append(_cellDelimiter);
                    }
                }
                result.Append("</y>");
            }
            result.Append("</Cells>");
            result.Append("</DataTable>");
            return result.ToString();
        }

        private static DataSet DeserializeDataSet(string xml)
        {
            DataSet dataSet = new DataSet();
            string typeName = "";
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                reader.MoveToContent();//Moves to root node, <List>.
                reader.Read();//Moves to first objects node, this will be the type of the object list.
                typeName = reader.Name;//Should be "DataTable"
                if (typeName == "DataSet")
                {//Can happen if passed an empty data set.
                    return dataSet;
                }
                do
                {
                    dataSet.Tables.Add(DeserializeDataTable(reader.ReadOuterXml()));
                } while (reader.Name == typeName);
            }
            return dataSet;
        }

        private static string SerializeDataSet(DataSet ds)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("<DataSet>");
            strb.Append("<DataTables>");
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                strb.Append(SerializeDataTable(ds.Tables[i]));
            }
            strb.Append("</DataTables>");
            strb.Append("</DataSet>");
            return strb.ToString();
        }

        public static string SerializeList(string objectType, Object obj)
        {
            string listType = "";
            //Strip out what kind of objects this list contains.
            Match m = Regex.Match(objectType, @"^List\[\[60\]\]([a-zA-Z0-9._%+-]*)\[\[62\]\]$");
            if (!m.Success)
            {
                throw new Exception("SerializeList, unknown object list: " + objectType);
            }
            listType = m.Result("$1");
            //Cast to a list of objects and loop through all the objects and call each objects corresponding serialize method.
            StringBuilder result = new StringBuilder();
            result.Append("<List>");
            IEnumerable enumerable = obj as IEnumerable;
            if (enumerable != null)
            {
                foreach (object item in enumerable)
                {
                    result.Append(SerializeForCSharp(listType, item));
                }
            }
            result.Append("</List>");
            return result.ToString();
        }

        public static List<object> DeserializeList(string xml)
        {
            List<object> listObject = new List<object>();
            string typeName = "";
            //Find out what type of list this is.
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                reader.MoveToContent();//Moves to root node, <List>.
                reader.Read();//Moves to first objects node, this will be the type of the object list.
                typeName = reader.Name;
                if (typeName == "List")
                {//Can happen if passed an empty list.
                    return listObject;
                }
                do
                {
                    listObject.Add(Deserialize(typeName, reader.ReadOuterXml()));
                } while (reader.Name == typeName);
            }
            return listObject;
        }

        public static string SerializeArray(string typeName, Object obj)
        {
            throw new Exception("SerializeArray, arrays not supported yet.");
        }
    }

    /// <summary>
    /// Converts various datatypes into strings formatted correctly for MySQL.
    /// "P" was originally short for Parameter because this class was written specifically to replace parameters 
    /// in the mysql queries. Using strings instead of parameters is much easier to debug.  This will later be 
    /// rewritten as a System.IConvertible interface on custom mysql types. I would rather not ever depend 
    /// on the mysql connector for this so that this program remains very db independent.
    /// Marked internal so it doesn't get mistaken or misused in place of OpenDentBusiness.POut.
    /// </summary>
    internal class POut
    {
        public static string PBool(bool myBool)
        {
            if (myBool == true)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public static string PByte(byte myByte)
        {
            return myByte.ToString();
        }

        public static string PInt(int myInt)
        {
            return myInt.ToString();
        }

        public static string PFloat(float myFloat)
        {
            return myFloat.ToString();
        }
    }

    /// <summary>
    /// Converts strings coming in from the database into the appropriate type. 
    /// "P" was originally short for Parameter because this class was written specifically to 
    /// replace parameters in the mysql queries. Using strings instead of parameters is much easier to debug. 
    /// This will later be rewritten as a System.IConvertible interface on custom mysql types. 
    /// I would rather not ever depend on the mysql connector for this so that this program remains very db independent.
    /// Marked internal so it doesn't get mistaken or misused in place of OpenDentBusiness.PIn.
    /// </summary>
    internal class PIn
    {
        public static bool PBool(string myString)
        {
            return myString == "1";
        }

        public static byte PByte(string myString)
        {
            if (myString == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToByte(myString);
            }
        }

        public static double PDouble(string myString)
        {
            if (myString == "")
            {
                return 0;
            }
            else
            {
                try
                {
                    return Convert.ToDouble(myString);
                }
                catch
                {
                    return 0;
                }
            }

        }

        public static int PInt(string myString)
        {
            if (myString == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(myString);
            }
        }

        public static long PLong(string myString)
        {
            if (myString == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(myString);
            }
        }

        public static float PFloat(string myString)
        {
            if (myString == "")
            {
                return 0;
            }
            return Convert.ToSingle(myString);
        }

        public static string PString(string myString)
        {
            return myString;
        }
    }
}