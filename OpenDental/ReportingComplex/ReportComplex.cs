using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace OpenDental.ReportingComplex
{
    /// <summary>
    /// This class is loosely modeled after CrystalReports.ReportDocument, but with less inheritence and heirarchy.
    /// </summary>
    public class ReportComplex
    {
        bool hasGridLines;

        /// <summary>
        /// This is simply used to measure strings for alignment purposes.
        /// </summary>
        readonly Graphics graphics;

        /// <summary>
        /// Invoke this action in order to close the report progress window.  Gets instantiated in the constructor.
        /// </summary>
        readonly Action closeReportProgressAction = null;

        /// <summary>
        /// Collection of Sections.
        /// </summary>
        public SectionCollection Sections { get; set; } = new SectionCollection();
        
        /// <summary>
        /// A collection of ReportObjects
        /// </summary>
        public ReportObjectCollection ReportObjects { get; set; } = new ReportObjectCollection();
        
        /// <summary>
        /// Collection of ParameterFields that are available for the query.
        /// </summary>
        public ParameterFieldCollection ParameterFields { get; set; } = new ParameterFieldCollection();

        public bool IsLandscape { get; set; } = false;

        /// <summary>
        /// The name to display in the menu.
        /// </summary>
        public string ReportName { get; set; }
        
        /// <summary>
        /// Gives the user a description and some guidelines about what they can expect from this report.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// For instance OD12 or JoeDeveloper9.
        /// If you are a developer releasing reports, then this should be your name or company followed by a unique number.
        /// This will later make it easier to maintain your reports for your customers.
        /// All reports that we release will be of the form OD##.
        /// Reports that the user creates will have this field blank.
        /// </summary>
        public string AuthorID { get; set; }
        
        /// <summary>
        /// The 1-based order to show in the Letter menu, or 0 to not show in that menu.
        /// </summary>
        public int LetterOrder { get; set; }

        /// <summary>
        /// The total number of rows to print.
        /// </summary>
        public int TotalRows { get; set; }

        /// <summary>
        /// This can add a title, subtitle, grid lines, and page nums to the report using defaults. 
        /// If the parameters are blank or false the object will not be added. 
        /// Set showProgress false to hide the progress window from showing up when generating the report.
        /// </summary>
        public ReportComplex(bool hasGridLines, bool isLandscape, bool showProgress = true)
        {
            if (showProgress)
            {
                closeReportProgressAction = 
                    ODProgress.Show(
                        ODEventType.ReportComplex, 
                        typeof(ReportComplexEvent), 
                        startingMessage: "Running Report Query...", 
                        hasHistory: Preferences.GetBool(PrefName.ReportsShowHistory));
            }

            graphics = Graphics.FromImage(new Bitmap(1, 1));
            IsLandscape = isLandscape;

            if (hasGridLines) AddGridLines();

            if (Sections[AreaSectionType.ReportHeader] == null)
            {
                Sections.Add(new Section(AreaSectionType.ReportHeader, 0));
            }

            if (Sections[AreaSectionType.PageHeader] == null)
            {
                Sections.Add(new Section(AreaSectionType.PageHeader, 0));
            }

            if (Sections[AreaSectionType.PageFooter] == null)
            {
                Sections.Add(new Section(AreaSectionType.PageFooter, 0));
            }

            if (Sections[AreaSectionType.ReportFooter] == null)
            {
                Sections.Add(new Section(AreaSectionType.ReportFooter, 0));
            }
        }

        /// <summary>
        /// Adds a ReportObject, Tahoma font, 17-point and bold, to the top-center of the Report Header Section.
        /// Should only be done once, and done before any subTitles.
        /// </summary>
        public void AddTitle(string name, string title) => AddTitle(name, title, new Font("Tahoma", 17, FontStyle.Bold));

        /// <summary>
        /// Adds a ReportObject with the given font, to the top-center of the Report Header Section.
        /// Should only be done once, and done before any subTitles.
        /// </summary>
        public void AddTitle(string name, string title, Font font)
        {
            ReportComplexEvent.Fire(ODEventType.ReportComplex, "Adding Title To Report...");

            var size = 
                new Size(
                    (int)(graphics.MeasureString(title, font).Width / graphics.DpiX * 100 + 2),
                    (int)(graphics.MeasureString(title, font).Height / graphics.DpiY * 100 + 2));

            int xPos;
            if (IsLandscape)
            {
                xPos = 1100 / 2;
                xPos -= 50;
            }
            else
            {
                xPos = 850 / 2;
                xPos -= 30;
            }
            xPos -= size.Width / 2;
            if (Sections[AreaSectionType.ReportHeader] == null)
            {
                Sections.Add(new Section(AreaSectionType.ReportHeader, 0));
            }
            ReportObjects.Add(new ReportObject(name, AreaSectionType.ReportHeader, new Point(xPos, 0), size, title, font, ContentAlignment.MiddleCenter));

            // This is the only place a white buffer is added to a header.
            Sections[AreaSectionType.ReportHeader].Height = size.Height + 20;
        }

        /// <summary>
        /// Adds a ReportObject, Tahoma font, 10-point and bold, at the bottom-center of the Report Header Section.
        /// Should only be done after AddTitle.  You can add as many subtitles as you want.
        /// </summary>
        public void AddSubTitle(string name, string subTitle) => AddSubTitle(name, subTitle, new Font("Tahoma", 10, FontStyle.Bold));
        
        /// <summary>
        /// Adds a ReportObject, Tahoma font, 10-point and bold, at the bottom-center of the Report Header Section.
        /// Should only be done after AddTitle.
        /// You can add as many subtitles as you want.
        /// Padding is added to the height only of the subtitle.
        /// </summary>
        public void AddSubTitle(string name, string subTitle, int padding) => AddSubTitle(name, subTitle, new Font("Tahoma", 10, FontStyle.Bold), padding);

        /// <summary>
        /// Adds a ReportObject with the given font, at the bottom-center of the Report Header Section.
        /// Should only be done after AddTitle.  You can add as many subtitles as you want.
        /// </summary>
        public void AddSubTitle(string name, string subTitle, Font font) => AddSubTitle(name, subTitle, font, 0);
        
        /// <summary>
        /// Adds a ReportObject with the given font, at the bottom-center of the Report Header Section.
        /// Should only be done after AddTitle.
        /// You can add as many subtitles as you want. 
        /// Padding is added to the height only of the subtitle.
        /// </summary>
        public void AddSubTitle(string name, string subTitle, Font font, int padding)
        {
            ReportComplexEvent.Fire(ODEventType.ReportComplex, "Adding SubTitle To Report...");

            var size = 
                new Size(
                    (int)(graphics.MeasureString(subTitle, font).Width / graphics.DpiX * 100 + 2) , 
                    (int)(graphics.MeasureString(subTitle, font).Height / graphics.DpiY * 100 + 2));

            int xPos;
            if (IsLandscape)
            {
                xPos = 1100 / 2;
                xPos -= 50;
            }
            else
            {
                xPos = 850 / 2;
                xPos -= 30;
            }

            xPos -= size.Width / 2;
            if (Sections[AreaSectionType.ReportHeader] == null)
            {
                Sections.Add(new Section(AreaSectionType.ReportHeader, 0));
            }

            // Find the yPos+Height of the last reportObject in the Report Header section
            int yPos = 0;
            foreach (ReportObject reportObject in ReportObjects)
            {
                if (reportObject.SectionType != AreaSectionType.ReportHeader) continue;
                if (reportObject.Location.Y + reportObject.Size.Height > yPos)
                {
                    yPos = reportObject.Location.Y + reportObject.Size.Height;
                }
            }
            ReportObjects.Add(new ReportObject(name, AreaSectionType.ReportHeader, new Point(xPos, yPos + padding), size, subTitle, font, ContentAlignment.MiddleCenter));
            Sections[AreaSectionType.ReportHeader].Height += (int)size.Height + padding;
        }

        /// <summary>
        /// Adds a report object with the given font at the footer of the report, with the given alignment.
        /// </summary>
        public void AddFooterText(string name, string text, Font font, int padding, ContentAlignment contentAlign)
        {
            Size size;
            int borderPadding = 50;

            if (IsLandscape)
            {
                size = new Size((int)(1100 - (borderPadding * 2)), (int)(graphics.MeasureString(text, font).Height / graphics.DpiY * 100 + 2));
            }
            else
            {
                size = new Size((int)(850 - (borderPadding * 2)), (int)(graphics.MeasureString(text, font).Height / graphics.DpiY * 100 + 2));
            }

            if (Sections[AreaSectionType.ReportFooter] == null)
            {
                Sections.Add(new Section(AreaSectionType.ReportFooter, 0));
            }

            //Find the yPos+Height of the last reportObject in the Report Footer section
            int yPos = 0;
            foreach (ReportObject reportObject in ReportObjects)
            {
                if (reportObject.SectionType != AreaSectionType.ReportFooter)
                {
                    continue;
                }
                if (reportObject.Location.Y + reportObject.Size.Height > yPos)
                {
                    yPos = reportObject.Location.Y + reportObject.Size.Height;
                }
            }
            ReportObjects.Add(new ReportObject(name, AreaSectionType.ReportFooter, new Point(borderPadding, yPos + padding), size, text, font, contentAlign));
            Sections[AreaSectionType.ReportFooter].Height += (int)size.Height + padding;
        }

        public QueryObject AddQuery(string query, string title)
        {
            QueryObject queryObject = new QueryObject(query, title);
            ReportObjects.Add(queryObject);
            return queryObject;
        }

        public QueryObject AddQuery(string query, string title, string columnNameToSplitOn, SplitByKind splitByKind)
        {
            QueryObject queryObject = new QueryObject(query, title, columnNameToSplitOn, splitByKind);
            ReportObjects.Add(queryObject);
            return queryObject;
        }

        public QueryObject AddQuery(string query, string title, string columnNameToSplitOn, SplitByKind splitByKind, int queryGroup)
        {
            QueryObject queryObject = new QueryObject(query, title, queryGroup, columnNameToSplitOn, splitByKind);
            ReportObjects.Add(queryObject);
            return queryObject;
        }

        public QueryObject AddQuery(string query, string title, string columnNameToSplitOn, SplitByKind splitByKind, int queryGroup, bool isCentered)
        {
            QueryObject queryObject = new QueryObject(query, title, isCentered, queryGroup, columnNameToSplitOn, splitByKind);
            ReportObjects.Add(queryObject);
            return queryObject;
        }

        public QueryObject AddQuery(string query, string title, string columnNameToSplitOn, SplitByKind splitByKind, int queryGroup, bool isCentered, List<string> enumNames, Font font)
        {
            QueryObject queryObject = new QueryObject(query, title, font, isCentered, queryGroup, columnNameToSplitOn, splitByKind, enumNames, null);
            ReportObjects.Add(queryObject);
            return queryObject;
        }

        public QueryObject AddQuery(string query, string title, string columnNameToSplitOn, SplitByKind splitByKind, int queryGroup, bool isCentered, Dictionary<long, string> dictDefNames, Font font)
        {
            QueryObject queryObject = new QueryObject(query, title, font, isCentered, queryGroup, columnNameToSplitOn, splitByKind, null, dictDefNames);
            ReportObjects.Add(queryObject);
            return queryObject;
        }

        public QueryObject AddQuery(DataTable query, string title)
        {
            QueryObject queryObject = new QueryObject(query, title);
            ReportObjects.Add(queryObject);
            return queryObject;
        }

        public QueryObject AddQuery(DataTable query, string title, string columnNameToSplitOn, SplitByKind splitByKind, int queryGroup)
        {
            QueryObject queryObject = new QueryObject(query, title, queryGroup, columnNameToSplitOn, splitByKind);
            ReportObjects.Add(queryObject);
            return queryObject;
        }

        public QueryObject AddQuery(DataTable query, string title, string columnNameToSplitOn, SplitByKind splitByKind, int queryGroup, bool isCentered)
        {
            QueryObject queryObject = new QueryObject(query, title, isCentered, queryGroup, columnNameToSplitOn, splitByKind);
            ReportObjects.Add(queryObject);
            return queryObject;
        }

        public QueryObject AddQuery(DataTable query, string title, string columnNameToSplitOn, SplitByKind splitByKind, int queryGroup, bool isCentered, List<string> enumNames, Font font)
        {
            QueryObject queryObject = new QueryObject(query, title, font, isCentered, queryGroup, columnNameToSplitOn, splitByKind, enumNames, null);
            ReportObjects.Add(queryObject);
            return queryObject;
        }

        public QueryObject AddQuery(DataTable query, string title, string columnNameToSplitOn, SplitByKind splitByKind, int queryGroup, bool isCentered, Dictionary<long, string> dictDefNames, Font font)
        {
            QueryObject queryObject = new QueryObject(query, title, font, isCentered, queryGroup, columnNameToSplitOn, splitByKind, null, dictDefNames);
            ReportObjects.Add(queryObject);
            return queryObject;
        }

        public void AddLine(string name, AreaSectionType sectionType, Color color, float lineThickness, LineOrientation lineOrientation, LinePosition linePosition, int linePercent, int offSetX, int offSetY)
        {
            ReportObjects.Add(new ReportObject(name, sectionType, color, lineThickness, lineOrientation, linePosition, linePercent, offSetX, offSetY));
        }

        public void AddBox(string name, AreaSectionType sectionType, Color color, float lineThickness, int offSetX, int offSetY)
        {
            ReportObjects.Add(new ReportObject(name, sectionType, color, lineThickness, offSetX, offSetY));
        }

        public ReportObject GetObjectByName(string name)
        {
            for (int i = ReportObjects.Count - 1; i >= 0; i--)
            {
                if (ReportObjects[i].Name == name)
                {
                    return ReportObjects[i];
                }
            }
            return null;
        }

        public ReportObject GetTitle(string name)
        {
            for (int i = ReportObjects.Count - 1; i >= 0; i--)
            {
                if (ReportObjects[i].Name == name)
                {
                    return ReportObjects[i];
                }
            }
            return null;
        }

        public ReportObject GetSubTitle(string subName)
        {
            for (int i = ReportObjects.Count - 1; i >= 0; i--)
            {
                if (ReportObjects[i].Name == subName)
                {
                    return ReportObjects[i];
                }
            }
            return null;
        }

        public void AddPageNum() => AddPageNum(new Font("Tahoma", 9));
        
        /// <summary>
        /// Put a pagenumber object on lower left of page footer section. Object is named PageNum.
        /// </summary>
        public void AddPageNum(Font font)
        {
            Size size = new Size(150, (int)(graphics.MeasureString("anytext", font).Height / graphics.DpiY * 100 + 2));
            if (Sections[AreaSectionType.PageFooter] == null)
            {
                Sections.Add(new Section(AreaSectionType.PageFooter, 0));
            }
            if (Sections[AreaSectionType.PageFooter].Height == 0)
            {
                Sections[AreaSectionType.PageFooter].Height = size.Height;
            }

            ReportObjects.Add(
                new ReportObject(
                    "PageNum", 
                    AreaSectionType.PageFooter, 
                    new Point(0, 0), size , 
                    FieldValueType.String, 
                    SpecialFieldType.PageNumber, 
                    font, 
                    ContentAlignment.MiddleLeft, ""));
        }

        public void AddGridLines() => hasGridLines = true;
        
        ///<summary>Adds a parameterField which will be used in the query to represent user-entered data.</summary>
        ///<param name="myName">The unique formula name of the parameter.</param>
        ///<param name="myValueType">The data type that this parameter stores.</param>
        ///<param name="myDefaultValue">The default value of the parameter</param>
        ///<param name="myPromptingText">The text to prompt the user with.</param>
        ///<param name="mySnippet">The SQL snippet that this parameter represents.</param>
        public void AddParameter(string myName, FieldValueType myValueType, object myDefaultValue, string myPromptingText, string mySnippet)
        {
            ParameterFields.Add(new ParameterField(myName, myValueType, myDefaultValue, myPromptingText, mySnippet));
        }

        /// <summary>
        /// Overload for ValueKind enum.
        /// </summary>
        public void AddParameter(string myName, FieldValueType myValueType, ArrayList myDefaultValues, string myPromptingText, string mySnippet, EnumType myEnumerationType)
        {
            ParameterFields.Add(new ParameterField(myName, myValueType, myDefaultValues, myPromptingText, mySnippet, myEnumerationType));
        }

        /// <summary>
        /// Overload for ValueKind defCat.
        /// </summary>
        public void AddParameter(string myName, FieldValueType myValueType, ArrayList myDefaultValues, string myPromptingText, string mySnippet, DefCat myDefCategory)
        {
            ParameterFields.Add(new ParameterField(myName, myValueType, myDefaultValues, myPromptingText, mySnippet, myDefCategory));
        }

        /// <summary>
        /// Overload for ValueKind defCat.
        /// </summary>
        public void AddParameter(string myName, FieldValueType myValueType, ArrayList myDefaultValues, string myPromptingText, string mySnippet, ReportFKType myReportFKType)
        {
            ParameterFields.Add(new ParameterField(myName, myValueType, myDefaultValues, myPromptingText, mySnippet, myReportFKType));
        }

        /// <summary>
        /// Submits the queries to the database and makes query objects for each query with the results.
        /// Returns false if one of the queries failed.
        /// </summary>
        public bool SubmitQueries(bool isShowMessage = false)
        {
            bool hasRows = false;
            bool hasReportServer = !string.IsNullOrEmpty(Preferences.ReportingServer.DisplayStr);
            Graphics grfx = Graphics.FromImage(new Bitmap(1, 1));
            string displayText;
            ReportObjectCollection newReportObjects = new ReportObjectCollection();
            Sections.Add(new Section(AreaSectionType.Query, 0));
            for (int i = 0; i < ReportObjects.Count; i++)
            {
                if (ReportObjects[i].ObjectType == ReportObjectType.QueryObject)
                {
                    var query = (QueryObject)ReportObjects[i];
                    if (!query.SubmitQuery())
                    {
                        closeReportProgressAction?.Invoke();
                        MessageBox.Show(
                            "There was an error generating this report."
                            + (hasReportServer ? "\r\nVerify or remove the report server connection settings and try again." : ""),
                            "Report", 
                            MessageBoxButtons.OK, MessageBoxIcon.
                            Error);

                        return false;
                    }

                    if (query.ReportTable.Rows.Count == 0) continue;

                    hasRows = true;
                    TotalRows += query.ReportTable.Rows.Count;
                    //Check if the query needs to be split up into sub queries.  E.g. one payment report query split up via payment type.
                    if (!string.IsNullOrWhiteSpace(query.ColumnNameToSplitOn))
                    {
                        ReportComplexEvent.Fire(ODEventType.ReportComplex, "Creating Splits Based On " + query.ColumnNameToSplitOn + "...");
                        //The query needs to be split up into sub queries every time the ColumnNameToSplitOn cell changes.  
                        //Therefore, we need to create a separate QueryObject for every time the cell value changes.
                        string lastCellValue = "";
                        query.IsLastSplit = false;
                        QueryObject newQuery = null;
                        for (int j = 0; j < query.ReportTable.Rows.Count; j++)
                        {
                            if (query.ReportTable.Rows[j][query.ColumnNameToSplitOn].ToString() == lastCellValue)
                            {
                                if (newQuery == null)
                                {
                                    newQuery = query.DeepCopyQueryObject();
                                    newQuery.AddInitialHeader(newQuery.GetGroupTitle().StaticText, newQuery.GetGroupTitle().Font);
                                }
                                newQuery.ReportTable.ImportRow(query.ReportTable.Rows[j]);
                            }
                            else
                            {
                                //Must happen the first time through
                                if (newQuery != null)
                                {
                                    switch (newQuery.SplitByKind)
                                    {
                                        case SplitByKind.None:
                                            return false;
                                        case SplitByKind.Enum:
                                            if (newQuery.ListEnumNames == null)
                                            {
                                                return false;
                                            }
                                            displayText = newQuery.ListEnumNames[PIn.Int(newQuery.ReportTable.Rows[0][query.ColumnNameToSplitOn].ToString())];
                                            newQuery.GetGroupTitle().Size = new Size((int)(grfx.MeasureString(displayText, newQuery.GetGroupTitle().Font).Width / grfx.DpiX * 100 + 2), (int)(grfx.MeasureString(displayText, newQuery.GetGroupTitle().Font).Height / grfx.DpiY * 100 + 2));
                                            newQuery.GetGroupTitle().StaticText = displayText;
                                            break;
                                        case SplitByKind.Definition:
                                            if (newQuery.DictDefNames == null)
                                            {
                                                return false;
                                            }
                                            if (newQuery.DictDefNames.ContainsKey(PIn.Long(newQuery.ReportTable.Rows[0][query.ColumnNameToSplitOn].ToString())))
                                            {
                                                displayText = newQuery.DictDefNames[PIn.Long(newQuery.ReportTable.Rows[0][query.ColumnNameToSplitOn].ToString())];
                                                newQuery.GetGroupTitle().Size = new Size((int)(grfx.MeasureString(displayText, newQuery.GetGroupTitle().Font).Width / grfx.DpiX * 100 + 2), (int)(grfx.MeasureString(displayText, newQuery.GetGroupTitle().Font).Height / grfx.DpiY * 100 + 2));
                                                newQuery.GetGroupTitle().StaticText = displayText;
                                            }
                                            else
                                            {
                                                newQuery.GetGroupTitle().StaticText = "Undefined";
                                            }
                                            break;
                                        case SplitByKind.Date:
                                            displayText = PIn.Date(newQuery.ReportTable.Rows[0][query.ColumnNameToSplitOn].ToString()).ToShortDateString();
                                            newQuery.GetGroupTitle().StaticText = displayText;
                                            newQuery.GetGroupTitle().Size = new Size((int)(grfx.MeasureString(displayText, newQuery.GetGroupTitle().Font).Width / grfx.DpiX * 100 + 2), (int)(grfx.MeasureString(displayText, newQuery.GetGroupTitle().Font).Height / grfx.DpiY * 100 + 2));
                                            break;
                                        case SplitByKind.Value:
                                            displayText = newQuery.ReportTable.Rows[0][query.ColumnNameToSplitOn].ToString();
                                            newQuery.GetGroupTitle().StaticText = displayText;
                                            newQuery.GetGroupTitle().Size = new Size((int)(grfx.MeasureString(displayText, newQuery.GetGroupTitle().Font).Width / grfx.DpiX * 100 + 2), (int)(grfx.MeasureString(displayText, newQuery.GetGroupTitle().Font).Height / grfx.DpiY * 100 + 2));
                                            break;
                                    }
                                    newQuery.SubmitQuery();
                                    newReportObjects.Add(newQuery);
                                }
                                if (newQuery == null && query.GetGroupTitle().StaticText != "")
                                {
                                    newQuery = query.DeepCopyQueryObject();
                                    newQuery.ReportTable.ImportRow(query.ReportTable.Rows[j]);
                                    newQuery.AddInitialHeader(newQuery.GetGroupTitle().StaticText, newQuery.GetGroupTitle().Font);
                                }
                                else
                                {
                                    newQuery = query.DeepCopyQueryObject();
                                    newQuery.ReportTable.ImportRow(query.ReportTable.Rows[j]);
                                }
                            }
                            lastCellValue = query.ReportTable.Rows[j][query.ColumnNameToSplitOn].ToString();
                        }
                        switch (newQuery.SplitByKind)
                        {
                            case SplitByKind.None:
                                return false;
                            case SplitByKind.Enum:
                                if (newQuery.ListEnumNames == null)
                                {
                                    return false;
                                }
                                displayText = newQuery.ListEnumNames[PIn.Int(newQuery.ReportTable.Rows[0][query.ColumnNameToSplitOn].ToString())];
                                newQuery.GetGroupTitle().Size = new Size((int)(grfx.MeasureString(displayText, newQuery.GetGroupTitle().Font).Width / grfx.DpiX * 100 + 2), (int)(grfx.MeasureString(displayText, newQuery.GetGroupTitle().Font).Height / grfx.DpiY * 100 + 2));
                                newQuery.GetGroupTitle().StaticText = displayText;
                                break;
                            case SplitByKind.Definition:
                                if (newQuery.DictDefNames == null)
                                {
                                    return false;
                                }
                                if (newQuery.DictDefNames.ContainsKey(PIn.Long(newQuery.ReportTable.Rows[0][query.ColumnNameToSplitOn].ToString())))
                                {
                                    displayText = newQuery.DictDefNames[PIn.Long(newQuery.ReportTable.Rows[0][query.ColumnNameToSplitOn].ToString())];
                                    newQuery.GetGroupTitle().Size = new Size((int)(grfx.MeasureString(displayText, newQuery.GetGroupTitle().Font).Width / grfx.DpiX * 100 + 2), (int)(grfx.MeasureString(displayText, newQuery.GetGroupTitle().Font).Height / grfx.DpiY * 100 + 2));
                                    newQuery.GetGroupTitle().StaticText = displayText;
                                }
                                else
                                {
                                    newQuery.GetGroupTitle().StaticText = Lans.g(this, "Undefined");
                                }
                                break;
                            case SplitByKind.Date:
                                newQuery.GetGroupTitle().StaticText = PIn.Date(newQuery.ReportTable.Rows[0][query.ColumnNameToSplitOn].ToString()).ToShortDateString();
                                break;
                            case SplitByKind.Value:
                                newQuery.GetGroupTitle().StaticText = newQuery.ReportTable.Rows[0][query.ColumnNameToSplitOn].ToString();
                                break;
                        }
                        newQuery.SubmitQuery();
                        newQuery.IsLastSplit = true;
                        newReportObjects.Add(newQuery);
                    }
                    else
                    {
                        newReportObjects.Add(ReportObjects[i]);
                    }
                }
                else
                {
                    newReportObjects.Add(ReportObjects[i]);
                }
            }
            if (!hasRows && isShowMessage)
            {
                closeReportProgressAction?.Invoke();

                MessageBox.Show(
                    "The report has no results to show.", 
                    "Report", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return false;
            }
            ReportObjects = newReportObjects;
            return true;
        }

        /// <summary>
        /// If the specified section exists, then this returns its height. Otherwise it returns 0.
        /// </summary>
        public int GetSectionHeight(AreaSectionType sectionType)
        {
            if (!Sections.Contains(sectionType))
            {
                return 0;
            }
            return Sections[sectionType].Height;
        }

        public bool HasGridLines() => hasGridLines;

        /// <summary>
        /// Closes the progress bar if it is open.
        /// </summary>
        public void CloseProgressBar() =>  closeReportProgressAction?.Invoke();
    }
}