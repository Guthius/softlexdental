using CodeBase;
using OpenDental.Properties;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace OpenDental.ReportingComplex
{
    ///<summary></summary>
    public partial class FormReportComplex : ODForm
    {
        private ODprintout _printout;
        ///<summary>The report to display.</summary>
        private ReportComplex _myReport;
        ///<summary>The name of the last section printed. It really only keeps track of whether the details section and the reportfooter have finished printing. This variable will be refined when groups are implemented.</summary>
        private AreaSectionType _lastSectionPrinted;
        private int _rowsPrinted;
        private int _totalRowsPrinted;
        private int _totalPages;
        private int _pagesPrinted;
        private int _heightRemaining = 0;

        private bool _isWrappingText;
        ///<summary>An arbitrary buffer amount for AreaSectionType.GroupFooter added to give a buffer between split tables.</summary>
        private const int GROUP_FOOTER_BUFFER = 20;

        public FormReportComplex(ReportComplex myReport)
        {
            InitializeComponent();// Required for Windows Form Designer support
            _myReport = myReport;
        }


        private void FormReport_Load(object sender, System.EventArgs e)
        {
            _isWrappingText = Preference.GetBool(PreferenceName.ReportsWrapColumns);

            RefreshWindow();
        }

        /// <summary>
        /// Used to refresh the print window when something changes.
        /// </summary>
        public void RefreshWindow()
        {
            LayoutToolBar();
            if (ResetODprintout())
            {
                SetDefaultZoom();
                printPreviewControl2.Document = _printout.PrintDoc;
            }
        }

        void LayoutToolBar()
        {
            ToolBarMain.Buttons.Clear();
            ToolBarMain.Buttons.Add(new ODToolBarButton("Print", Resources.IconPrint, "", "Print"));
            ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
            ToolBarMain.Buttons.Add(new ODToolBarButton("", Resources.IconResultPrevious, "Go Back One Page", "Back"));
            ToolBarMain.Buttons.Add(new ODToolBarButton(0, 0, "", "PageNum"));
            ToolBarMain.Buttons.Add(new ODToolBarButton("", Resources.IconResultNext, "Go Forward One Page", "Fwd"));
            ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
            ToolBarMain.Buttons.Add(new ODToolBarButton("", Resources.IconZoomIn, "", "ZoomIn"));
            ToolBarMain.Buttons.Add(new ODToolBarButton("", Resources.IconZoomOut, "", "ZoomOut"));
            ToolBarMain.Buttons.Add(new ODToolBarButton("100", Resources.IconZoomReset, "", "ZoomReset"));
            ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));

            var wrapTextButton = new ODToolBarButton("Wrap Text", null, "Wrap Text In Columns", "WrapText");
            wrapTextButton.Style = ODToolBarButtonStyle.ToggleButton;
            wrapTextButton.Pushed = _isWrappingText;
            ToolBarMain.Buttons.Add(wrapTextButton);

            ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
            ToolBarMain.Buttons.Add(new ODToolBarButton("Export", Resources.IconExportTable, "", "Export"));
            ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
            //ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Close"),-1,Lan.g(this,"Close This Window"),"Close"));
            //ToolBarMain.Invalidate();
        }

        /// <summary>
        /// Sets the default zoom factor based on the reports orientation.
        /// </summary>
        void SetDefaultZoom()
        {
            if (_myReport.IsLandscape)
            {
                printPreviewControl2.Zoom = (double)printPreviewControl2.ClientSize.Height / _printout.PrintDoc.DefaultPageSettings.PaperSize.Width;
            }
            else
            {
                printPreviewControl2.Zoom = (double)printPreviewControl2.ClientSize.Height / _printout.PrintDoc.DefaultPageSettings.PaperSize.Height;
            }
        }

        private void FormReport_Layout(object sender, LayoutEventArgs e)
        {
            printPreviewControl2.Location = new Point(0, ToolBarMain.Height);
            printPreviewControl2.Height = ClientSize.Height - ToolBarMain.Height;
            printPreviewControl2.Width = ClientSize.Width;
        }

        bool ResetODprintout()
        {
            ReportComplexEvent.Fire(ODEventType.ReportComplex, "Retrieving Printer Settings...");
            _printout = PrinterL.CreateODprintout(
                pd2_PrintPage,
                auditDescription: "Report printed " + _myReport.ReportName,
                printoutOrientation: (_myReport.IsLandscape ? PrintoutOrientation.Landscape : PrintoutOrientation.Default),
                margins: new Margins(0, 0, 0, 0),
                printoutOrigin: PrintoutOrigin.AtMargin,
                isErrorSuppressed: true
            );

            if (_printout.SettingsErrorCode != PrintoutErrorCode.Success)
            {
                ReportComplexEvent.Fire(
                    ODEventType.ReportComplex,
                    PrinterL.GetErrorStringFromCode(_printout.SettingsErrorCode));

                _myReport.CloseProgressBar();
                return false;
            }

            _printout.PrintDoc.EndPrint += new PrintEventHandler(pd2_EndPrint);
            _lastSectionPrinted = AreaSectionType.None;
            _rowsPrinted = 0;
            _totalRowsPrinted = 0;
            _pagesPrinted = 0;

            ReportComplexEvent.Fire(ODEventType.ReportComplex, "Calculating Row Heights...");
            foreach (ReportObject reportObject in _myReport.ReportObjects)
            {
                if (reportObject.ObjectType == ReportObjectType.QueryObject)
                {
                    var queryObject = (QueryObject)reportObject;
                    queryObject.CalculateRowHeights(_isWrappingText);
                    if (queryObject.IsPrinted == true)
                    {
                        queryObject.IsPrinted = false;
                    }
                }
            }

            return true;
        }

        void ToolBarMain_ButtonClick(object sender, ODToolBarButtonClickEventArgs e)
        {
            switch (e.Button.Tag.ToString())
            {
                case "Print":
                    OnPrint_Click();
                    break;
                case "Back":
                    OnBack_Click();
                    break;
                case "Fwd":
                    OnFwd_Click();
                    break;
                case "ZoomIn":
                    OnZoomIn_Click();
                    break;
                case "ZoomOut":
                    OnZoomOut_Click();
                    break;
                case "ZoomReset":
                    OnZoomReset_Click();
                    break;
                case "Export":
                    OnExport_Click();
                    break;
                case "WrapText":
                    OnWrapText_Click();
                    break;
            }
        }

        private void ToolBarMain_PageNav(object sender, ODToolBarButtonPageNavEventArgs e)
        {
            if (e.NavValue == 0)
            {
                return;
            }
            printPreviewControl2.StartPage = e.NavValue - 1;
            SetPageNavString();
        }

        ///<summary>raised for each page to be printed.</summary>
        private void pd2_PrintPage(object sender, PrintPageEventArgs ev)
        {
            ReportComplexEvent.Fire(ODEventType.ReportComplex, new ProgressBarHelper("Printing Page " + (_pagesPrinted + 1) + "...", "", 
                _totalRowsPrinted, _myReport.TotalRows, ProgBarStyle.Blocks));

            //Note that the locations of the reportObjects are not absolute.  They depend entirely upon the margins.  When the report is initially created, it is pushed up against the upper and the left.
            Graphics grfx = ev.Graphics;
            //xPos and yPos represent the upper left of current section after margins are accounted for.
            //All reportObjects are then placed relative to this origin.
            Margins currentMargins = null;
            Size paperSize;
            if (_myReport.IsLandscape)
            {
                paperSize = new Size(1100, 850);
            }
            else
            {
                paperSize = new Size(850, 1100);
            }
            currentMargins = new Margins(30, 0, 50, 50);
            int xPos = currentMargins.Left;
            int yPos = currentMargins.Top;
            int printableHeight = paperSize.Height - currentMargins.Top - currentMargins.Bottom;
            int yLimit = paperSize.Height - currentMargins.Bottom;//the largest yPos allowed
                                                                  //Now calculate and layout each section in sequence.
            Section section;
            //Technically the ReportFooter should only be subtracted from the printableHeight of the last page, but we have no way to know how many pages
            //the report will end up taking so we will subtract it from the printable height of all pages.
            //Used to determine the max height of a single grid cell.
            int maxGridCellHeight = printableHeight - _myReport.GetSectionHeight(AreaSectionType.PageHeader)
                - _myReport.GetSectionHeight(AreaSectionType.GroupFooter) - _myReport.GetSectionHeight(AreaSectionType.GroupTitle)
                - _myReport.GetSectionHeight(AreaSectionType.GroupHeader) - _myReport.GetSectionHeight(AreaSectionType.ReportFooter);
            if (_pagesPrinted == 0)
            {
                maxGridCellHeight -= _myReport.GetSectionHeight(AreaSectionType.ReportHeader);
            }
            foreach (ReportObject reportObject in _myReport.ReportObjects)
            {
                if (reportObject.ObjectType != ReportObjectType.QueryObject)
                {
                    continue;
                }
                QueryObject queryObject = (QueryObject)reportObject;
                for (int i = 0; i < queryObject.RowHeightValues.Count; i++)
                {
                    queryObject.RowHeightValues[i] = Math.Min(queryObject.RowHeightValues[i], maxGridCellHeight);
                }
                foreach (ReportObject rObject in queryObject.ReportObjects)
                {
                    if (rObject.SectionType != AreaSectionType.Detail && rObject.SectionType != AreaSectionType.GroupFooter)
                    {
                        rObject.ContentAlignment = ContentAlignment.TopCenter;
                        continue;
                    }
                    if (rObject.ObjectType == ReportObjectType.FieldObject && rObject.FieldValueType == FieldValueType.Number)
                    {
                        rObject.ContentAlignment = ContentAlignment.TopRight;
                    }
                }
            }
            while (true)
            {//will break out if no more room on page
             //if no sections have been printed yet, print a report header.
                if (_lastSectionPrinted == AreaSectionType.None)
                {
                    if (_myReport.Sections.Contains(AreaSectionType.ReportHeader))
                    {
                        ReportComplexEvent.Fire(ODEventType.ReportComplex, "Printing Page " + (_pagesPrinted + 1) + " - Printing Report Header...");
                        section = _myReport.Sections[AreaSectionType.ReportHeader];
                        PrintSection(grfx, section, xPos, yPos);
                        yPos += section.Height;
                        if (section.Height > printableHeight)
                        {//this can happen if the reportHeader takes up the full page
                         //if there are no other sections to print
                         //this will keep the second page from printing:
                            _lastSectionPrinted = AreaSectionType.ReportFooter;
                            break;
                        }
                    }
                    else
                    {//no report header
                     //it will still be marked as printed on the next line
                    }
                    _lastSectionPrinted = AreaSectionType.ReportHeader;
                }
                //always print a page header if it exists
                if (_myReport.Sections.Contains(AreaSectionType.PageHeader))
                {
                    ReportComplexEvent.Fire(ODEventType.ReportComplex, "Printing Page " + (_pagesPrinted + 1) + " - Printing Page Header...");
                    section = _myReport.Sections[AreaSectionType.PageHeader];
                    PrintSection(grfx, section, xPos, yPos);
                    yPos += section.Height;
                }
                _heightRemaining = yLimit - yPos - _myReport.GetSectionHeight(AreaSectionType.PageFooter);
                section = _myReport.Sections[AreaSectionType.Query];
                PrintQuerySection(grfx, section, xPos, yPos);
                yPos += section.Height;
                bool isRoomForReportFooter = true;
                if (_heightRemaining - _myReport.GetSectionHeight(AreaSectionType.ReportFooter) <= 0)
                {
                    isRoomForReportFooter = false;
                }
                //print the reportfooter section if there is room
                if (isRoomForReportFooter)
                {
                    if (_myReport.Sections.Contains(AreaSectionType.ReportFooter))
                    {
                        ReportComplexEvent.Fire(ODEventType.ReportComplex, "Printing Page " + (_pagesPrinted + 1) + " - Printing Report Footer...");
                        section = _myReport.Sections[AreaSectionType.ReportFooter];
                        PrintSection(grfx, section, xPos, yPos);
                        yPos += section.Height;
                    }
                    //mark the reportfooter as printed. This will prevent another loop.
                    _lastSectionPrinted = AreaSectionType.ReportFooter;
                }
                //print the pagefooter
                if (_myReport.Sections.Contains(AreaSectionType.PageFooter))
                {
                    ReportComplexEvent.Fire(ODEventType.ReportComplex, "Printing Page " + (_pagesPrinted + 1) + " - Printing Page Footer...");
                    section = _myReport.Sections[AreaSectionType.PageFooter];
                    yPos = yLimit - section.Height;
                    PrintSection(grfx, section, xPos, yPos);
                    yPos += section.Height;
                }
                break;
            }//while			
            _pagesPrinted++;
            ReportComplexEvent.Fire(ODEventType.ReportComplex, "Printing Page " + (_pagesPrinted + 1) + " - Page Printed. Preparing Next Page...");
            //if the reportfooter has been printed, then there are no more pages.
            if (_lastSectionPrinted == AreaSectionType.ReportFooter)
            {
                ev.HasMorePages = false;
                //labelTotPages.Text="1 / "+totalPages.ToString();
            }
            else
            {
                ev.HasMorePages = true;
            }
        }

        /// <summary>
        /// Either the report finished printing OR the user canceled out of the print job.
        /// </summary>
        private void pd2_EndPrint(object sender, PrintEventArgs ev)
        {
            _totalPages = _pagesPrinted;

            SetPageNavString();

            _myReport.CloseProgressBar();
        }

        /// <summary>
        /// Prints one section other than details at the specified x and y position on the page.
        /// The math to decide whether it will fit on the current page is done ahead of time.
        /// </summary>
        void PrintSection(Graphics g, Section section, int xPos, int yPos)
        {
            ReportObject textObject;
            ReportObject fieldObject;
            ReportObject lineObject;
            ReportObject boxObject;
            StringFormat strFormat;//used each time text is drawn to handle alignment issues
            string displayText = "";//The formatted text to print
            foreach (ReportObject reportObject in _myReport.ReportObjects)
            {
                if (reportObject.SectionType != section.SectionType)
                {
                    continue;
                }
                if (reportObject.ObjectType == ReportObjectType.TextObject)
                {
                    textObject = reportObject;
                    Font newFont = textObject.Font;
                    strFormat = ReportObject.GetStringFormatAlignment(textObject.ContentAlignment);
                    if (section.SectionType == AreaSectionType.ReportFooter)
                    {
                        if (textObject.Name == "ReportSummaryText")
                        {
                            xPos += _myReport.ReportObjects["ReportSummaryLabel"].Size.Width;
                            if (textObject.IsUnderlined)
                            {
                                newFont = new Font(textObject.Font.FontFamily, textObject.Font.Size, FontStyle.Bold | FontStyle.Underline);
                            }
                            else
                            {
                                newFont = new Font(textObject.Font.FontFamily, textObject.Font.Size, FontStyle.Bold);
                            }
                            SizeF size = g.MeasureString(textObject.StaticText, newFont);
                            textObject.Size = new Size((int)size.Width + 1, (int)size.Height + 1);
                        }
                        strFormat = ReportObject.GetStringFormatAlignment(textObject.ContentAlignment);
                        RectangleF layoutRect = new RectangleF(xPos + textObject.Location.X + textObject.OffSetX
                            , yPos + textObject.Location.Y + textObject.OffSetY
                            , textObject.Size.Width, textObject.Size.Height);
                        if (textObject.IsUnderlined)
                        {
                            g.DrawString(textObject.StaticText, new Font(textObject.Font.FontFamily, textObject.Font.Size, textObject.Font.Style | FontStyle.Underline), Brushes.Black, layoutRect, strFormat);
                        }
                        else
                        {
                            g.DrawString(textObject.StaticText, newFont, Brushes.Black, layoutRect, strFormat);
                            //g.DrawLine(new Pen(textObject.ForeColor),xPos+textObject.Location.X+textObject.OffSetX,yPos+textObject.Location.Y+textObject.OffSetY+textObject.Size.Height,xPos+textObject.Location.X+textObject.Size.Width,yPos+textObject.Location.Y+textObject.Size.Height);
                        }
                    }
                    else
                    {
                        strFormat = ReportObject.GetStringFormatAlignment(textObject.ContentAlignment);
                        RectangleF layoutRect = new RectangleF(xPos + textObject.Location.X
                            , yPos + textObject.Location.Y
                            , textObject.Size.Width, textObject.Size.Height);
                        if (textObject.IsUnderlined)
                        {
                            g.DrawString(textObject.StaticText, new Font(textObject.Font.FontFamily, textObject.Font.Size, textObject.Font.Style | FontStyle.Underline), Brushes.Black, layoutRect, strFormat);
                        }
                        else
                        {
                            g.DrawString(textObject.StaticText, textObject.Font, Brushes.Black, layoutRect, strFormat);
                        }
                    }
                }
                else if (reportObject.ObjectType == ReportObjectType.FieldObject)
                {
                    fieldObject = reportObject;
                    strFormat = ReportObject.GetStringFormatAlignment(fieldObject.ContentAlignment);
                    RectangleF layoutRect = new RectangleF(xPos + fieldObject.Location.X
                        , yPos + fieldObject.Location.Y
                        , fieldObject.Size.Width, fieldObject.Size.Height);
                    displayText = "";
                    if (fieldObject.FieldDefKind == FieldDefKind.SummaryField)
                    {
                        //displayText=fieldObject.GetSummaryValue
                        //	(_myReport.ReportTables,_myReport.DataFields.IndexOf
                        //	(fieldObject.SummarizedField))
                        //	.ToString(fieldObject.FormatString);
                    }
                    else if (fieldObject.FieldDefKind == FieldDefKind.SpecialField)
                    {
                        if (fieldObject.SpecialFieldType == SpecialFieldType.PageCounter)
                        {//not functional yet
                         //displayText=Lan.g(this,"Page")+" "+(pagesPrinted+1).ToString()
                         //	+Lan.g(
                        }
                        else if (fieldObject.SpecialFieldType == SpecialFieldType.PageNumber)
                        {
                            displayText = Lan.g(this, "Page") + " " + (_pagesPrinted + 1).ToString();
                        }
                    }
                    g.DrawString(displayText, fieldObject.Font, Brushes.Black, layoutRect, strFormat);
                }
                else if (reportObject.ObjectType == ReportObjectType.BoxObject)
                {
                    boxObject = reportObject;
                    int x1 = xPos + boxObject.OffSetX;
                    int x2 = xPos - boxObject.OffSetX;
                    int y1 = yPos + boxObject.OffSetY;
                    int y2 = yPos - boxObject.OffSetY;
                    int maxHorizontalLength = 1100;
                    if (!_myReport.IsLandscape)
                    {
                        maxHorizontalLength = 850;
                    }
                    x2 += maxHorizontalLength;
                    y2 += _myReport.GetSectionHeight(boxObject.SectionType);
                    g.DrawRectangle(new Pen(boxObject.ForeColor, boxObject.FloatLineThickness), x1, y1, x2 - x1, y2 - y1);
                }
                else if (reportObject.ObjectType == ReportObjectType.LineObject)
                {
                    lineObject = reportObject;
                    int length;
                    int x = lineObject.OffSetX;
                    int y = yPos + lineObject.OffSetY;
                    int maxHorizontalLength = 1100;
                    if (!_myReport.IsLandscape)
                    {
                        maxHorizontalLength = 850;
                    }
                    if (lineObject.LineOrientation == LineOrientation.Horizontal)
                    {
                        length = maxHorizontalLength * lineObject.IntLinePercent / 100;
                        if (lineObject.LinePosition == LinePosition.South)
                        {
                            y += _myReport.GetSectionHeight(lineObject.SectionType);
                        }
                        else if (lineObject.LinePosition == LinePosition.North)
                        {
                            //Do Nothing Here
                        }
                        else if (lineObject.LinePosition == LinePosition.Center)
                        {
                            y += (_myReport.GetSectionHeight(lineObject.SectionType) / 2);
                        }
                        else
                        {
                            continue;
                        }
                        x += (maxHorizontalLength / 2) - (length / 2);
                        g.DrawLine(new Pen(reportObject.ForeColor, reportObject.FloatLineThickness), x, y, x + length, y);
                    }
                    else if (lineObject.LineOrientation == LineOrientation.Vertical)
                    {
                        length = _myReport.GetSectionHeight(lineObject.SectionType) * lineObject.IntLinePercent / 100;
                        if (lineObject.LinePosition == LinePosition.West)
                        {
                            //Do Nothing Here
                        }
                        else if (lineObject.LinePosition == LinePosition.East)
                        {
                            x += maxHorizontalLength;
                        }
                        else if (lineObject.LinePosition == LinePosition.Center)
                        {
                            x += maxHorizontalLength / 2;
                        }
                        else
                        {
                            continue;
                        }
                        y += (_myReport.GetSectionHeight(lineObject.SectionType) / 2) - (length / 2);
                        g.DrawLine(new Pen(reportObject.ForeColor, reportObject.FloatLineThickness), x, y, x, y + length);
                    }
                }
            }
        }

        ///<summary>Prints some rows of the details section at the specified x and y position on the page.  The math to decide how many rows to print is done ahead of time.  The number of rows printed so far is kept global so that it can be used in calculating the layout of this section.</summary>
        private void PrintQuerySection(Graphics g, Section section, int xPos, int yPos)
        {
            ReportComplexEvent.Fire(ODEventType.ReportComplex, Lan.g("ReportComplex", "Printing Page") + " " + (_pagesPrinted + 1) + " - "
                + Lan.g("ReportComplex", "Printing Query Section") + "...");
            section.Height = 0;
            ReportObject textObject;
            ReportObject lineObject;
            ReportObject boxObject;
            QueryObject queryObject;
            StringFormat strFormat;//used each time text is drawn to handle alignment issues
            #region Lines And Boxes
            foreach (ReportObject reportObject in _myReport.ReportObjects)
            {
                if (reportObject.SectionType != section.SectionType)
                {
                    //skip any reportObjects that are not in this section
                    continue;
                }
                if (reportObject.ObjectType == ReportObjectType.BoxObject)
                {
                    boxObject = reportObject;
                    int x1 = xPos + boxObject.OffSetX;
                    int x2 = xPos - boxObject.OffSetX;
                    int y1 = yPos + boxObject.OffSetY;
                    int y2 = yPos - boxObject.OffSetY;
                    int maxHorizontalLength = 1100;
                    if (!_myReport.IsLandscape)
                    {
                        maxHorizontalLength = 850;
                    }
                    x2 += maxHorizontalLength - xPos;
                    y2 += _heightRemaining * _myReport.GetSectionHeight(boxObject.SectionType);
                    g.DrawRectangle(new Pen(boxObject.ForeColor, boxObject.FloatLineThickness), x1, y1, x2 - x1, y2 - y1);
                }
                else if (reportObject.ObjectType == ReportObjectType.LineObject)
                {
                    lineObject = reportObject;
                    int length;
                    int x = lineObject.OffSetX;
                    int y = yPos + lineObject.OffSetY;
                    int maxHorizontalLength = 1100;
                    if (!_myReport.IsLandscape)
                    {
                        maxHorizontalLength = 850;
                    }
                    if (lineObject.LineOrientation == LineOrientation.Horizontal)
                    {
                        length = maxHorizontalLength * lineObject.IntLinePercent / 100;
                        if (lineObject.LinePosition == LinePosition.South)
                        {
                            y += _myReport.GetSectionHeight(lineObject.SectionType);
                        }
                        else if (lineObject.LinePosition == LinePosition.North)
                        {
                            //Do Nothing Here
                        }
                        else if (lineObject.LinePosition == LinePosition.Center)
                        {
                            y += (_myReport.GetSectionHeight(lineObject.SectionType) / 2);
                        }
                        else
                        {
                            continue;
                        }
                        x += (maxHorizontalLength / 2) - (length / 2);
                        g.DrawLine(new Pen(reportObject.ForeColor, reportObject.FloatLineThickness), x, y, x + length, y);
                    }
                    else if (lineObject.LineOrientation == LineOrientation.Vertical)
                    {
                        length = _myReport.GetSectionHeight(lineObject.SectionType) * lineObject.IntLinePercent / 100;
                        if (lineObject.LinePosition == LinePosition.West)
                        {
                            //Do Nothing Here
                        }
                        else if (lineObject.LinePosition == LinePosition.East)
                        {
                            x = maxHorizontalLength;
                        }
                        else if (lineObject.LinePosition == LinePosition.Center)
                        {
                            x = maxHorizontalLength / 2;
                        }
                        else
                        {
                            continue;
                        }
                        y = y + (_myReport.GetSectionHeight(lineObject.SectionType) / 2) - (length / 2);
                        g.DrawLine(new Pen(reportObject.ForeColor, reportObject.FloatLineThickness), x, y, x, y + length);
                    }
                    else
                    {
                        //Do nothing since it has already been done for each row.
                    }
                }
            }
            #endregion
            foreach (ReportObject reportObject in _myReport.ReportObjects)
            {
                if (reportObject.SectionType != section.SectionType)
                {
                    //skip any reportObjects that are not in this section
                    continue;
                }
                if (reportObject.ObjectType == ReportObjectType.TextObject)
                {
                    //not typical to print textobject in details section, but allowed
                    textObject = reportObject;
                    strFormat = ReportObject.GetStringFormatAlignment(textObject.ContentAlignment);
                    RectangleF layoutRect = new RectangleF(xPos + textObject.Location.X
                        , yPos + textObject.Location.Y
                        , textObject.Size.Width, textObject.Size.Height);
                    g.DrawString(textObject.StaticText, textObject.Font
                        , new SolidBrush(textObject.ForeColor), layoutRect, strFormat);
                    if (textObject.IsUnderlined)
                    {
                        g.DrawLine(new Pen(textObject.ForeColor), xPos + textObject.Location.X, yPos + textObject.Location.Y + textObject.Size.Height, xPos + textObject.Location.X + textObject.Size.Width, yPos + textObject.Location.Y + textObject.Size.Height);
                    }
                }
                else if (reportObject.ObjectType == ReportObjectType.QueryObject)
                {
                    queryObject = (QueryObject)reportObject;
                    if (queryObject.IsPrinted == true)
                    {
                        continue;
                    }
                    if (queryObject.IsCentered)
                    {
                        if (_myReport.IsLandscape)
                        {
                            xPos = 1100 / 2 - (queryObject.QueryWidth / 2);
                        }
                        else
                        {
                            xPos = 850 / 2 - (queryObject.QueryWidth / 2);
                        }
                    }
                    if (_heightRemaining > 0)
                    {
                        PrintQueryObjectSection(queryObject, g, queryObject.Sections[AreaSectionType.GroupTitle], xPos, yPos);
                        yPos += queryObject.Sections[AreaSectionType.GroupTitle].Height;
                        section.Height += queryObject.Sections[AreaSectionType.GroupTitle].Height;
                    }
                    if (_heightRemaining > 0)
                    {
                        PrintQueryObjectSection(queryObject, g, queryObject.Sections[AreaSectionType.GroupHeader], xPos, yPos);
                        yPos += queryObject.Sections[AreaSectionType.GroupHeader].Height;
                        section.Height += queryObject.Sections[AreaSectionType.GroupHeader].Height;
                    }
                    if (_heightRemaining > 0)
                    {
                        PrintQueryObjectSection(queryObject, g, queryObject.Sections[AreaSectionType.Detail], xPos, yPos);
                        yPos += queryObject.Sections[AreaSectionType.Detail].Height;
                        section.Height += queryObject.Sections[AreaSectionType.Detail].Height;
                    }
                    if (_heightRemaining > 0)
                    {
                        PrintQueryObjectSection(queryObject, g, queryObject.Sections[AreaSectionType.GroupFooter], xPos, yPos);
                        yPos += queryObject.Sections[AreaSectionType.GroupFooter].Height;
                        section.Height += queryObject.Sections[AreaSectionType.GroupFooter].Height;
                    }
                    if (_heightRemaining <= 0)
                    {
                        return;
                    }
                }
            }
        }

        ///<summary>Prints sections inside a QueryObject</summary>
        private void PrintQueryObjectSection(QueryObject queryObj, Graphics g, Section section, int xPos, int yPos)
        {
            section.Height = 0;
            ReportObject textObject;
            ReportObject fieldObject;
            ReportObject lineObject;
            ReportObject boxObject;
            string rawText = "";//the raw text for a given field as taken from the database
            string displayText = "";//The formatted text to print
            string prevDisplayText = "";//The formatted text of the previous row. Used to test suppress dupl.	
            StringFormat strFormat;//used each time text is drawn to handle alignment issues
            int yPosAdd = 0;
            if (queryObj.SuppressIfDuplicate
                && section.SectionType == AreaSectionType.GroupTitle && _rowsPrinted > 0)
            {
                return;//Only print the group title for each query object once.
            }
            //loop through each row in the table and make sure that the row can fit.  If it can fit, print it.  Otherwise go to next page.
            for (int i = _rowsPrinted; i < queryObj.ReportTable.Rows.Count; i++)
            {
                //Figure out the current row height
                if (section.SectionType == AreaSectionType.Detail && queryObj.RowHeightValues[i] > _heightRemaining)
                {
                    _heightRemaining = 0;
                    return;
                }
                //Find the Group Header height to see if printing at least one row is possible.
                if (section.SectionType == AreaSectionType.GroupTitle)
                {
                    int titleHeight = 0;
                    int headerHeight = 0;
                    foreach (ReportObject reportObject in queryObj.ReportObjects)
                    {
                        if (reportObject.SectionType == AreaSectionType.GroupTitle)
                        {
                            titleHeight += reportObject.Size.Height;
                        }
                        else if (reportObject.SectionType == AreaSectionType.GroupHeader && reportObject.Size.Height > headerHeight)
                        {
                            headerHeight = reportObject.Size.Height;
                        }
                    }
                    //This is a new table and we want to know if we can print the first row
                    if (titleHeight + headerHeight + queryObj.RowHeightValues[0] > _heightRemaining)
                    {
                        _heightRemaining = 0;
                        return;
                    }
                }
                //Find the Group Footer height to see if printing the last row should happen on another page.
                if (section.SectionType == AreaSectionType.Detail && _rowsPrinted == queryObj.ReportTable.Rows.Count - 1)
                {
                    int groupSummaryLabelHeight = 0;
                    int tallestTotalSummaryHeight = 0;
                    foreach (ReportObject reportObject in queryObj.ReportObjects)
                    {
                        if (reportObject.SectionType == AreaSectionType.GroupFooter
                            && !reportObject.Name.Contains("GroupSummaryLabel")
                            && !reportObject.Name.Contains("GroupSummaryText")
                            && tallestTotalSummaryHeight < reportObject.Size.Height + reportObject.OffSetY)
                        {
                            tallestTotalSummaryHeight = reportObject.Size.Height + reportObject.OffSetY;
                        }
                        //Find the height of the group footer using GroupSummaryLabel because GroupSummaryText has not been filled yet.
                        if (reportObject.SectionType == AreaSectionType.GroupFooter && reportObject.Name.Contains("GroupSummaryLabel"))
                        {
                            groupSummaryLabelHeight += reportObject.Size.Height + reportObject.OffSetY;
                        }
                        //The GroupSummaryText hasn't been filled yet so we use GroupSummaryLabel again
                        //If it is North or South then we need to add its height a second time because the GroupSummaryLabel is located above or below the text.
                        if (reportObject.SectionType == AreaSectionType.GroupFooter && reportObject.Name.Contains("GroupSummaryLabel")
                            && (reportObject.SummaryOrientation == SummaryOrientation.North || reportObject.SummaryOrientation == SummaryOrientation.South))
                        {
                            groupSummaryLabelHeight += reportObject.Size.Height;
                        }
                    }
                    int groupFooterHeight = groupSummaryLabelHeight + tallestTotalSummaryHeight + GROUP_FOOTER_BUFFER;
                    //For reports without group footers, check to see if we can print the last row. 
                    if (groupFooterHeight == GROUP_FOOTER_BUFFER && queryObj.RowHeightValues[queryObj.ReportTable.Rows.Count - 1] > _heightRemaining)
                    {
                        _heightRemaining = 0;
                        return;
                    }
                    //See if we can print the Group Footer and the Last row
                    else if (groupFooterHeight + queryObj.RowHeightValues[queryObj.ReportTable.Rows.Count - 1] > _heightRemaining)
                    {
                        _heightRemaining = 0;
                        return;
                    }
                }
                int greatestObjectHeight = 0;
                int groupTitleHeight = 0;
                //Now figure out if anything in the header, footer, or title sections can still fit on the page
                foreach (ReportObject reportObject in queryObj.ReportObjects)
                {
                    if (reportObject.SectionType != section.SectionType)
                    {
                        continue;
                    }
                    if (reportObject.ObjectType != ReportObjectType.FieldObject && reportObject.Size.Height > _heightRemaining)
                    {
                        _heightRemaining = 0;
                        return;
                    }
                    if (reportObject.SectionType == AreaSectionType.GroupFooter && reportObject.Name.Contains("GroupSummary"))
                    {
                        if (!queryObj.IsLastSplit)
                        {
                            continue;
                        }
                        if (reportObject.Name.Contains("GroupSummaryLabel"))
                        {
                            yPos += reportObject.OffSetY;
                        }
                        if (reportObject.Name.Contains("GroupSummaryText"))
                        {
                            if (reportObject.SummaryOperation == SummaryOperation.Sum)
                            {
                                reportObject.StaticText = GetGroupSummaryValue(reportObject.DataField, reportObject.SummaryGroups, reportObject.SummaryOperation).ToString("c");
                            }
                            else if (reportObject.SummaryOperation == SummaryOperation.Count)
                            {
                                reportObject.StaticText = GetGroupSummaryValue(reportObject.DataField, reportObject.SummaryGroups, reportObject.SummaryOperation).ToString();
                            }
                            int width = (int)g.MeasureString(reportObject.StaticText, reportObject.Font).Width + 2;
                            int height = (int)g.MeasureString(reportObject.StaticText, reportObject.Font).Height + 2;
                            if (width < queryObj.GetObjectByName(reportObject.SummarizedField + "Header").Size.Width)
                            {
                                width = queryObj.GetObjectByName(reportObject.SummarizedField + "Header").Size.Width;
                            }
                            reportObject.Size = new Size(width, height);
                        }
                    }
                    if (section.SectionType == AreaSectionType.GroupTitle && _rowsPrinted > 0 && reportObject.Name == "Initial Group Title")
                    {
                        continue;
                    }
                    if (section.SectionType == AreaSectionType.GroupFooter && reportObject.SummaryOrientation == SummaryOrientation.South)
                    {
                        ReportObject summaryField = queryObj.GetObjectByName(reportObject.DataField + "Footer");
                        yPos += summaryField.Size.Height;
                    }
                    if (reportObject.ObjectType == ReportObjectType.TextObject)
                    {
                        textObject = reportObject;
                        strFormat = ReportObject.GetStringFormatAlignment(textObject.ContentAlignment);
                        RectangleF layoutRect = new RectangleF(xPos + textObject.Location.X + textObject.OffSetX
                            , yPos + textObject.Location.Y + textObject.OffSetY
                            , textObject.Size.Width, textObject.Size.Height);
                        if (textObject.IsUnderlined)
                        {
                            g.DrawString(textObject.StaticText, new Font(textObject.Font.FontFamily, textObject.Font.Size, textObject.Font.Style | FontStyle.Underline), Brushes.Black, layoutRect, strFormat);
                        }
                        else
                        {
                            g.DrawString(textObject.StaticText, textObject.Font, Brushes.Black, layoutRect, strFormat);
                        }
                        if (greatestObjectHeight < textObject.Size.Height)
                        {
                            greatestObjectHeight = textObject.Size.Height;
                        }
                        groupTitleHeight += textObject.Size.Height;
                        if (section.SectionType == AreaSectionType.GroupTitle)
                        {
                            yPos += textObject.Size.Height;
                        }
                        if (section.SectionType == AreaSectionType.GroupFooter
                            && ((reportObject.SummaryOrientation == SummaryOrientation.North || reportObject.SummaryOrientation == SummaryOrientation.South)
                                || (reportObject.Name.Contains("GroupSummaryText"))))
                        {
                            yPosAdd += textObject.Size.Height;
                            yPos += textObject.Size.Height;
                        }
                    }
                    else if (reportObject.ObjectType == ReportObjectType.BoxObject)
                    {
                        boxObject = reportObject;
                        int x1 = xPos + boxObject.OffSetX;
                        int x2 = xPos - boxObject.OffSetX;
                        int y1 = yPos + boxObject.OffSetY;
                        int y2 = yPos - boxObject.OffSetY;
                        int maxHorizontalLength = 1100;
                        if (!_myReport.IsLandscape)
                        {
                            maxHorizontalLength = 850;
                        }
                        x2 += maxHorizontalLength;
                        y2 += queryObj.GetSectionHeight(boxObject.SectionType);
                        g.DrawRectangle(new Pen(boxObject.ForeColor, boxObject.FloatLineThickness), x1, y1, x2 - x1, y2 - y1);
                        if (greatestObjectHeight < boxObject.Size.Height)
                        {
                            greatestObjectHeight = boxObject.Size.Height;
                        }
                        groupTitleHeight += boxObject.Size.Height;
                    }
                    else if (reportObject.ObjectType == ReportObjectType.LineObject)
                    {
                        lineObject = reportObject;
                        int length;
                        int x = lineObject.OffSetX;
                        int y = yPos + lineObject.OffSetY;
                        int maxHorizontalLength = 1100;
                        if (!_myReport.IsLandscape)
                        {
                            maxHorizontalLength = 850;
                        }
                        if (lineObject.LineOrientation == LineOrientation.Horizontal)
                        {
                            length = maxHorizontalLength * lineObject.IntLinePercent / 100;
                            if (lineObject.LinePosition == LinePosition.South)
                            {
                                y += queryObj.GetSectionHeight(lineObject.SectionType);
                            }
                            else if (lineObject.LinePosition == LinePosition.North)
                            {
                                //Do Nothing Here
                            }
                            else if (lineObject.LinePosition == LinePosition.Center)
                            {
                                y += (queryObj.GetSectionHeight(lineObject.SectionType) / 2);
                            }
                            else
                            {
                                continue;
                            }
                            x += (maxHorizontalLength / 2) - (length / 2);
                            g.DrawLine(new Pen(reportObject.ForeColor, reportObject.FloatLineThickness), x, y, x + length, y);
                        }
                        else if (lineObject.LineOrientation == LineOrientation.Vertical)
                        {
                            length = queryObj.GetSectionHeight(lineObject.SectionType) * lineObject.IntLinePercent / 100;
                            if (lineObject.LinePosition == LinePosition.West)
                            {
                                //Do Nothing Here
                            }
                            else if (lineObject.LinePosition == LinePosition.East)
                            {
                                x += maxHorizontalLength;
                            }
                            else if (lineObject.LinePosition == LinePosition.Center)
                            {
                                x += maxHorizontalLength / 2;
                            }
                            else
                            {
                                continue;
                            }
                            y += (queryObj.GetSectionHeight(lineObject.SectionType) / 2) - (length / 2);
                            g.DrawLine(new Pen(reportObject.ForeColor, reportObject.FloatLineThickness), x, y, x, y + length);
                        }
                        if (greatestObjectHeight < lineObject.Size.Height)
                        {
                            greatestObjectHeight = lineObject.Size.Height;
                        }
                        groupTitleHeight += lineObject.Size.Height;
                    }
                    else if (reportObject.ObjectType == ReportObjectType.FieldObject)
                    {
                        fieldObject = reportObject;
                        RectangleF layoutRect;
                        strFormat = ReportObject.GetStringFormatAlignment(fieldObject.ContentAlignment);
                        if (fieldObject.FieldDefKind == FieldDefKind.DataTableField)
                        {
                            layoutRect = new RectangleF(xPos + fieldObject.Location.X, yPos + fieldObject.Location.Y, fieldObject.Size.Width, queryObj.RowHeightValues[i]);
                            if (_myReport.HasGridLines())
                            {
                                g.DrawRectangle(new Pen(Brushes.LightGray), Rectangle.Round(layoutRect));
                            }
                            rawText = queryObj.ReportTable.Rows
                                [i][queryObj.ArrDataFields.IndexOf(fieldObject.DataField)].ToString();
                            displayText = rawText;
                            List<string> listString = GetDisplayString(displayText, prevDisplayText, fieldObject, i, queryObj);
                            displayText = listString[0];
                            prevDisplayText = listString[1];
                            //suppress if duplicate:
                            if (i > 0 && fieldObject.SuppressIfDuplicate && displayText == prevDisplayText)
                            {
                                displayText = "";
                            }
                        }
                        else
                        {
                            displayText = fieldObject.GetSummaryValue(queryObj.ReportTable, queryObj.ArrDataFields.IndexOf(fieldObject.SummarizedField)).ToString(fieldObject.StringFormat);
                            using (Font fontBold = new Font(fieldObject.Font.FontFamily, fieldObject.Font.Size, FontStyle.Bold))
                            {
                                layoutRect = new RectangleF(xPos + fieldObject.Location.X, yPos + fieldObject.Location.Y, fieldObject.Size.Width,
                                    g.MeasureString(displayText, fontBold, fieldObject.Size.Width).Height);
                            }
                        }
                        g.DrawString(displayText, fieldObject.Font
                        , new SolidBrush(fieldObject.ForeColor), new RectangleF(layoutRect.X + 1, layoutRect.Y + 1, layoutRect.Width, layoutRect.Height - 1), strFormat);
                        yPosAdd = (int)layoutRect.Height;
                    }
                }
                if (section.SectionType == AreaSectionType.GroupFooter)
                {
                    yPosAdd += GROUP_FOOTER_BUFFER;//Added to give a buffer between split tables.
                    section.Height += yPosAdd;
                    _heightRemaining -= section.Height;
                    yPos += yPosAdd;
                    break;
                }
                else if (section.SectionType == AreaSectionType.GroupTitle)
                {
                    section.Height += groupTitleHeight;
                    _heightRemaining -= section.Height;
                    break;
                }
                else if (section.SectionType == AreaSectionType.GroupHeader)
                {
                    section.Height = greatestObjectHeight;
                    _heightRemaining -= section.Height;
                    break;
                }
                if (section.SectionType == AreaSectionType.Detail)
                {
                    _rowsPrinted++;
                    _totalRowsPrinted++;
                    yPos += yPosAdd;
                    _heightRemaining -= yPosAdd;
                    section.Height += yPosAdd;
                }
            }
            if (_rowsPrinted == queryObj.ReportTable.Rows.Count)
            {
                _rowsPrinted = 0;
                queryObj.IsPrinted = true;
            }
        }

        double GetGroupSummaryValue(string columnName, List<int> summaryGroups, SummaryOperation operation)
        {
            double retVal = 0;
            for (int i = 0; i < _myReport.ReportObjects.Count; i++)
            {
                if (_myReport.ReportObjects[i].ObjectType != ReportObjectType.QueryObject)
                {
                    continue;
                }
                QueryObject queryObj = (QueryObject)_myReport.ReportObjects[i];
                if (!summaryGroups.Contains(queryObj.QueryGroupValue))
                {
                    continue;
                }
                for (int j = 0; j < queryObj.ReportTable.Rows.Count; j++)
                {
                    if (operation == SummaryOperation.Sum)
                    {
                        //This could be enhanced in the future to only sum up the cells that match the column name within the current query group.
                        //Right now, if multiple query groups share the same column name that is being summed, the total will include both sets.
                        if (queryObj.IsNegativeSummary)
                        {
                            retVal -= PIn.Double(queryObj.ReportTable.Rows[j][queryObj.ReportTable.Columns.IndexOf(columnName)].ToString());
                        }
                        else
                        {
                            retVal += PIn.Double(queryObj.ReportTable.Rows[j][queryObj.ReportTable.Columns.IndexOf(columnName)].ToString());
                        }
                    }
                    else if (operation == SummaryOperation.Count)
                    {
                        retVal++;
                    }
                }
            }
            return retVal;
        }

        List<string> GetDisplayString(string rawText, string prevDisplayText, ReportObject reportObject, int i, QueryObject queryObj)
        {
            return GetDisplayString(rawText, prevDisplayText, reportObject, i, queryObj, false);
        }

        List<string> GetDisplayString(string rawText, string prevDisplayText, ReportObject reportObject, int i, QueryObject queryObj, bool isExport)
        {
            string displayText = "";
            List<string> retVals = new List<string>();
            DataTable dt = queryObj.ReportTable;
            //For exporting, we need to use the ExportTable which is the data that is visible to the user.  Using ReportTable would show raw query data (potentially different than what the user sees).
            if (isExport)
            {
                dt = queryObj.ExportTable;
            }
            if (reportObject.FieldValueType == FieldValueType.Age)
            {
                displayText = Patients.AgeToString(Patients.DateToAge(PIn.Date(rawText)));//(fieldObject.FormatString);
            }
            else if (reportObject.FieldValueType == FieldValueType.Boolean)
            {
                if (PIn.Bool(dt.Rows[i][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()))
                {
                    displayText = "X";
                }
                else
                {
                    displayText = "";
                }
                if (i > 0 && reportObject.SuppressIfDuplicate)
                {
                    prevDisplayText = PIn.Bool(dt.Rows[i - 1][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString();
                }
            }
            else if (reportObject.FieldValueType == FieldValueType.Date)
            {
                displayText = PIn.DateT(dt.Rows[i][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
                if (i > 0 && reportObject.SuppressIfDuplicate)
                {
                    prevDisplayText = PIn.DateT(dt.Rows[i - 1][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
                }
            }
            else if (reportObject.FieldValueType == FieldValueType.Integer)
            {
                displayText = PIn.Long(dt.Rows[i][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
                if (i > 0 && reportObject.SuppressIfDuplicate)
                {
                    prevDisplayText = PIn.Long(dt.Rows[i - 1][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
                }
            }
            else if (reportObject.FieldValueType == FieldValueType.Number)
            {
                displayText = PIn.Double(dt.Rows[i][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
                if (i > 0 && reportObject.SuppressIfDuplicate)
                {
                    prevDisplayText = PIn.Double(dt.Rows[i - 1][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
                }
            }
            else if (reportObject.FieldValueType == FieldValueType.String)
            {
                displayText = rawText;
                if (i > 0 && reportObject.SuppressIfDuplicate)
                {
                    prevDisplayText = dt.Rows[i - 1][queryObj.ArrDataFields.IndexOf(reportObject.DataField)].ToString();
                }
            }
            retVals.Add(displayText);
            retVals.Add(prevDisplayText);
            return retVals;
        }

        void OnPrint_Click()
        {
            if (ResetODprintout())
            {
                PrinterL.TryPrint(_printout);
            }
        }

        private void OnBack_Click() => PrevPage();

        private void OnFwd_Click() => NextPage();

        void OnZoomIn_Click() => printPreviewControl2.Zoom = printPreviewControl2.Zoom * 2;
        
        void OnZoomOut_Click() => printPreviewControl2.Zoom = printPreviewControl2.Zoom / 2;

        void OnZoomReset_Click() => SetDefaultZoom();

        void PrevPage()
        {
            if (printPreviewControl2.StartPage == 0)
            {
                return;
            }
            printPreviewControl2.StartPage--;
            SetPageNavString();
        }

        void NextPage()
        {
            if (printPreviewControl2.StartPage == _totalPages - 1)
            {
                return;
            }
            printPreviewControl2.StartPage++;
            SetPageNavString();
        }

        void SetPageNavString()
        {
            ToolBarMain.Buttons["PageNum"].PageValue = (printPreviewControl2.StartPage + 1);
            ToolBarMain.Buttons["PageNum"].PageMax = _totalPages;
            ToolBarMain.Invalidate();
        }

        void OnExport_Click()
        {
            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.AddExtension = true;
            saveFileDialog2.FileName = _myReport.ReportName + ".txt";

            if (!Directory.Exists(Preference.GetString(PreferenceName.ExportPath)))
            {
                try
                {
                    Directory.CreateDirectory(Preference.GetString(PreferenceName.ExportPath));
                    saveFileDialog2.InitialDirectory = Preference.GetString(PreferenceName.ExportPath);
                }
                catch
                {
                    //initialDirectory will be blank
                }
            }
            else
            {
                saveFileDialog2.InitialDirectory = Preference.GetString(PreferenceName.ExportPath);
            }

            saveFileDialog2.Filter = "Text files(*.txt)|*.txt|Excel Files(*.xls)|*.xls|All files(*.*)|*.*";
            saveFileDialog2.FilterIndex = 0;
            if (saveFileDialog2.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            try
            {
                using (var streamWriter = new StreamWriter(saveFileDialog2.FileName, false))
                {
                    string line = "";
                    foreach (ReportObject reportObject in _myReport.ReportObjects)
                    {
                        if (reportObject.ObjectType == ReportObjectType.QueryObject)
                        {
                            var query = (QueryObject)reportObject;
                            line = query.GetGroupTitle().StaticText;
                            streamWriter.WriteLine(line);
                            line = "";
                            for (int i = 0; i < query.ExportTable.Columns.Count; i++)
                            {
                                line += query.ExportTable.Columns[i].Caption;
                                if (i < query.ExportTable.Columns.Count - 1)
                                {
                                    line += "\t";
                                }
                            }
                            streamWriter.WriteLine(line);
                            string cell;
                            for (int i = 0; i < query.ExportTable.Rows.Count; i++)
                            {
                                line = "";
                                string displayText = "";
                                foreach (ReportObject reportObj in query.ReportObjects)
                                {
                                    if (reportObj.SectionType != AreaSectionType.Detail)
                                    {
                                        continue;
                                    }
                                    string rawText = "";
                                    if (reportObj.ObjectType == ReportObjectType.FieldObject)
                                    {
                                        rawText = query.ExportTable.Rows[i][query.ArrDataFields.IndexOf(reportObj.DataField)].ToString();
                                        if (string.IsNullOrWhiteSpace(rawText))
                                        {
                                            line += "\t";
                                            continue;
                                        }
                                        List<string> listString = GetDisplayString(rawText, "", reportObj, i, query, true);
                                        displayText = listString[0];
                                    }
                                    cell = displayText;
                                    cell = cell.Replace("\r", "");
                                    cell = cell.Replace("\n", "");
                                    cell = cell.Replace("\t", "");
                                    cell = cell.Replace("\"", "");
                                    line += cell;
                                    if (query.ArrDataFields.IndexOf(reportObj.DataField) < query.ArrDataFields.Count - 1)
                                    {
                                        line += "\t";
                                    }
                                }
                                streamWriter.WriteLine(line);
                            }
                            int columnValue = -1;

                            line = "";
                            foreach (ReportObject reportObjQuery in query.ReportObjects)
                            {
                                if (reportObjQuery.SectionType == AreaSectionType.GroupFooter && reportObjQuery.Name.Contains("Footer"))
                                {
                                    if (columnValue == -1)
                                    {
                                        columnValue = query.ArrDataFields.IndexOf(reportObjQuery.SummarizedField);
                                        for (int i = 0; i < columnValue; i++)
                                        {
                                            line += " \t";
                                        }
                                    }
                                    line += reportObjQuery.GetSummaryValue(query.ExportTable, query.ArrDataFields.IndexOf(reportObjQuery.SummarizedField)).ToString(reportObjQuery.StringFormat) + "\t";
                                }
                            }
                            streamWriter.WriteLine(line);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("File in use by another program. Close and try again.");
                return;
            }

            MessageBox.Show("File created successfully");
        }

        void OnWrapText_Click()
        {
            _isWrappingText = !_isWrappingText;

            RefreshWindow();
        }

        void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}