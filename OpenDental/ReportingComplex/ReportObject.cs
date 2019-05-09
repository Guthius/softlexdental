using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDental.ReportingComplex
{
    /// <summary>
    /// There is one ReportObject for each element of an ODReport that gets printed on the page.
    /// There are many different kinds of reportObjects.
    /// </summary>
    public class ReportObject
    {
        Point location;
        Size _size;
        Font font;
        string staticText;

        /// <summary>
        /// The section to which this object is attached.
        /// For lines and boxes that span multiple sections, this is the section in which the upper part of the object resides.
        /// </summary>
        public AreaSectionType SectionType { get; set; }

        /// <summary>
        /// Location within the section. Frequently, y=0
        /// </summary>
        public Point Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        /// <summary>
        /// Typically not set since this is set by a helper function when important properties for size change.
        /// </summary>
        public Size Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        /// <summary>
        /// The unique name of the ReportObject.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// For instance, FieldObject, or TextObject.
        /// </summary>
        public ReportObjectType ObjectType { get; set; } = ReportObjectType.FieldObject;

        /// <summary>
        /// Setting this will also set the size.
        /// </summary>
        public Font Font
        {
            get => font;
            set
            {
                font = value;
                _size = CalculateNewSize(staticText, font);
            }
        }

        ///<summary>Horizontal alignment of the text.</summary>
        public ContentAlignment ContentAlignment { get; set; }

        ///<summary>Can be used for text color or for line color.</summary>
        public Color ForeColor { get; set; } = Color.Black;

        ///<summary>The text to display for a TextObject. Setting this will also set the size.</summary>
        public string StaticText
        {
            get
            {
                return staticText;
            }
            set
            {
                staticText = value;
                _size = CalculateNewSize(staticText, font);
            }
        }

        /// <summary>
        /// For a FieldObject, a C# format string that specifies how to print dates, times, numbers, and currency based on the country or on a custom format.
        /// </summary>
        /// <remarks>
        /// There are a LOT of options for this string.
        /// Look in C# help under Standard Numeric Format Strings, Custom Numeric Format Strings, 
        /// Standard DateTime Format Strings, Custom DateTime Format Strings, and Enumeration Format Strings. 
        /// Once users are allowed to edit reports, we will assemble a help page with all of the common options. 
        /// The best options are "n" for number, and "d" for date.
        /// </remarks>
        public string StringFormat { get; set; }

        /// <summary>
        /// Suppresses this field if the field for the previous record was the same.
        /// Only used with data fields. E.g. So that a query ordered by a date column doesn't print the same date over and over.
        /// </summary>
        public bool SuppressIfDuplicate { get; set; }

        public float FloatLineThickness { get; set; }

        /// <summary>
        /// Used to determine whether the line is vertical or horizontal.
        /// </summary>
        public LineOrientation LineOrientation { get; set; }

        /// <summary>
        /// Used to determine intial starting position of the line.
        /// </summary>
        public LinePosition LinePosition { get; set; }

        /// <summary>
        /// Used to determine what percentage of the section the line will draw on.
        /// </summary>
        public int IntLinePercent { get; set; }

        /// <summary>
        /// Used to offset lines, boxes, and text by a specific number of pixels.
        /// </summary>
        public int OffSetX { get; set; }

        /// <summary>
        /// Used to offset lines, boxes, and text by a specific number of pixels.
        /// </summary>
        public int OffSetY { get; set; }

        /// <summary>
        /// Used to underline text objects and titles.
        /// </summary>
        public bool IsUnderlined { get; set; }

        /// <summary>
        /// The kind of field, like FormulaField, SummaryField, or DataTableField.
        /// </summary>
        public FieldDefKind FieldDefKind { get; set; }

        /// <summary>
        /// The value type of field, like string or datetime.
        /// </summary>
        public FieldValueType FieldValueType { get; set; }

        /// <summary>
        /// For FieldKind=FieldDefKind.SpecialField, this is the type.  eg. pagenumber
        /// </summary>
        public SpecialFieldType SpecialFieldType { get; set; }

        /// <summary>
        /// For FieldKind=FieldDefKind.SummaryField, the summary operation type.
        /// </summary>
        public SummaryOperation SummaryOperation { get; set; }

        /// <summary>
        /// For FieldKind=FieldDefKind.SummaryField, the name of the dataField that is being summarized.
        /// This might later be changed to refer to a ReportObject name instead (or maybe not).
        /// </summary>
        public string SummarizedField { get; set; }

        /// <summary>
        /// For objectKind=ReportObjectKind.FieldObject, the name of the dataField column.
        /// </summary>
        public string DataField { get; set; }

        /// <summary>
        /// The location of the summary label around the summary field
        /// </summary>
        public SummaryOrientation SummaryOrientation { get; set; }

        /// <summary>
        /// The numeric value of the QueryGroup. Used when summarizing groups of queries.
        /// </summary>
        public List<int> SummaryGroups { get; set; }

        public ReportObject()
        {
        }

        /// <summary>
        /// Creates a TextObject with the specified name, section, location and size.
        /// The staticText and font will determine what and how it displays, while the contentAlignment will determine the relative location in the text area.
        /// </summary>
        public ReportObject(string name, AreaSectionType sectionType, Point location, Size size, string staticText, Font font, ContentAlignment contentAlignment)
            : this(name, sectionType, location, size, staticText, font, contentAlignment, 0, 0)
        {
        }

        /// <summary>
        /// Creates a TextObject with the specified name, section, location and size.
        /// The staticText and font will determine what and how it displays, while the contentAlignment will determine the relative location in the text area.
        /// The text will be offset of its position in pixels according to the given X/Y values.
        /// </summary>
        public ReportObject(string name, AreaSectionType sectionType, Point location, Size size, string staticText, Font font, ContentAlignment contentAlignment, int offSetX, int offSetY)
        {
            Name = name;
            SectionType = sectionType;
            this.location = location;
            _size = size;
            this.staticText = staticText;
            this.font = font;
            ContentAlignment = contentAlignment;
            OffSetX = offSetX;
            OffSetY = offSetY;
            ObjectType = ReportObjectType.TextObject;
        }

        /// <summary>
        /// Creates a BoxObject with the specified name, section, color and line thickness.
        /// </summary>
        public ReportObject(string name, AreaSectionType sectionType, Color color, float lineThickness)
            : this(name, sectionType, color, lineThickness, 0, 0)
        {
        }

        /// <summary>
        /// Creates a BoxObject with the specified name, section, color and line thickness.
        /// The box will be offset of its position in pixels according to the given X/Y values.
        /// </summary>
        public ReportObject(string name, AreaSectionType sectionType, Color color, float lineThickness, int offSetX, int offSetY)
        {
            Name = name;
            SectionType = sectionType;
            ForeColor = color;
            FloatLineThickness = lineThickness;
            OffSetX = offSetX;
            OffSetY = offSetY;
            ObjectType = ReportObjectType.BoxObject;
        }

        /// <summary>
        /// Creates a LineObject with the specified name, section, color, line thickness, line orientation, line position and percent.
        /// Orientation determines whether the line is horizontal or vertical.  Position determines which side of the section the line draws on.
        /// Percent determines how much of available space the line will take up.
        /// The line will be offset of its position in pixels according to the given X/Y values.
        /// </summary>
        public ReportObject(string name, AreaSectionType sectionType, Color color, float lineThickness, LineOrientation lineOrientation, LinePosition linePosition, int linePercent, int offSetX, int offSetY)
        {
            Name = name;
            SectionType = sectionType;
            ForeColor = color;
            FloatLineThickness = lineThickness;
            LineOrientation = lineOrientation;
            LinePosition = linePosition;
            IntLinePercent = linePercent;
            OffSetX = offSetX;
            OffSetY = offSetY;
            ObjectType = ReportObjectType.LineObject;
        }

        /// <summary>
        /// Mainly used from inside QueryObject. 
        /// Creates a DataTableFieldObject with the specified name, section, location, size, dataFieldName, fieldValueType, font, contentAlignment and stringFormat.
        /// DataFieldName determines what the field will be filled with from the table.
        /// FieldValueType determines how the field will be filled with data (i.e Number will be formatted as a number and have a summary added to the bottom of a column).
        /// ContentAlignment determines where the text will be drawn in the box.
        /// StringFormat is used to determined how a ToString() method call will format the field text.
        /// </summary>
        public ReportObject(string name, AreaSectionType sectionType, Point location, Size size, string dataFieldName, FieldValueType fieldValueType, Font font, ContentAlignment contentAlignment, string stringFormat)
        {
            Name = name;
            SectionType = sectionType;
            this.location = location;
            _size = size;
            this.font = font;
            ContentAlignment = contentAlignment;
            StringFormat = stringFormat;
            FieldDefKind = FieldDefKind.DataTableField;
            DataField = dataFieldName;
            FieldValueType = fieldValueType;
            //defaults:
            ObjectType = ReportObjectType.FieldObject;
        }

        /// <summary>
        /// Mainly used from inside QueryObject.
        /// Creates a SummaryFieldObject with the specified name, section, location, size, summaryOperation, summarizedFieldName, font, contentAlignment and stringFormat.
        /// SummaryOperation determines what calculation will be used when summarizing the column.
        /// SummarizedFieldName determines the field that will be summarized at the bottom of the column.
        /// ContentAlignment determines where the text will be drawn in the box.
        /// StringFormat is used to determined how a ToString() method call will format the field text.
        /// </summary>
        public ReportObject(string name, AreaSectionType sectionType, Point location, Size size, SummaryOperation summaryOperation, string summarizedFieldName, Font font, ContentAlignment contentAlignment, string stringFormat)
        {
            Name = name;
            SectionType = sectionType;
            this.location = location;
            _size = size;
            this.font = font;
            ContentAlignment = contentAlignment;
            StringFormat = stringFormat;
            FieldDefKind = FieldDefKind.SummaryField;
            FieldValueType = FieldValueType.Number;
            SummaryOperation = summaryOperation;
            SummarizedField = summarizedFieldName;
            //defaults:
            ForeColor = Color.Black;
            ObjectType = ReportObjectType.FieldObject;
        }

        /// <summary>
        /// Mainly used from inside QueryObject.
        /// Creates a GroupSummaryObject with the specified name, section, location, size, color, summaryOperation, summarizedFieldName, font, datafield, and offsets.
        /// SummaryOperation determines what calculation will be used when summarizing the group of column.
        /// SummarizedFieldName determines the field that will be summarized and must be the same in each of the queries.
        /// Datafield determines which column the summary will draw under.
        /// The summary will be offset of its position in pixels according to the given X/Y values.
        /// </summary>
        public ReportObject(string name, AreaSectionType sectionType, Point location, Size size, Color color, SummaryOperation summaryOperation, string summarizedFieldName, Font font, ContentAlignment contentAlignment, string datafield, int offSetX, int offSetY)
        {
            Name = name;
            SectionType = sectionType;
            this.location = location;
            _size = size;
            DataField = datafield;
            this.font = font;
            FieldDefKind = FieldDefKind.SummaryField;
            FieldValueType = FieldValueType.Number;
            SummaryOperation = summaryOperation;
            SummarizedField = summarizedFieldName;
            OffSetX = offSetX;
            OffSetY = offSetY;
            ForeColor = color;
            //defaults:
            ContentAlignment = contentAlignment;
            ObjectType = ReportObjectType.TextObject;
        }

        /// <summary>
        /// Currently only used for page numbers.
        /// </summary>
        public ReportObject(string name, AreaSectionType sectionType, Point location, Size size, FieldValueType fieldValueType, SpecialFieldType specialType, Font font, ContentAlignment contentAlignment, string stringFormat)
        {
            Name = name;
            SectionType = sectionType;
            this.location = location;
            _size = size;
            this.font = font;
            ContentAlignment = contentAlignment;
            StringFormat = stringFormat;
            FieldDefKind = FieldDefKind.SpecialField;
            FieldValueType = fieldValueType;
            SpecialFieldType = specialType;
            //defaults:
            ForeColor = Color.Black;
            ObjectType = ReportObjectType.FieldObject;
        }

        /// <summary>
        /// Converts contentAlignment into a combination of StringAlignments used to format strings.
        /// This method is mostly called for drawing text on reportObjects.
        /// </summary>
        public static StringFormat GetStringFormatAlignment(ContentAlignment contentAlignment)
        {
            if (!Enum.IsDefined(typeof(ContentAlignment), (int)contentAlignment))
                throw new System.ComponentModel.InvalidEnumArgumentException(
                    "contentAlignment", (int)contentAlignment, typeof(ContentAlignment));

            var stringFormat = new StringFormat();
            switch (contentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Center;
                    break;

                case ContentAlignment.MiddleLeft:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.MiddleRight:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Far;
                    break;

                case ContentAlignment.TopCenter:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    stringFormat.Alignment = StringAlignment.Center;
                    break;

                case ContentAlignment.TopLeft:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    stringFormat.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    stringFormat.Alignment = StringAlignment.Far;
                    break;

                case ContentAlignment.BottomCenter:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    stringFormat.Alignment = StringAlignment.Center;
                    break;

                case ContentAlignment.BottomLeft:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    stringFormat.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.BottomRight:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    stringFormat.Alignment = StringAlignment.Far;
                    break;
            }
            return stringFormat;
        }

        /// <summary>
        /// Used to copy a report object when creating new QueryObjects.
        /// </summary>
        public ReportObject DeepCopyReportObject()
        {
            var reportObject = new ReportObject();
            reportObject.SectionType = SectionType;
            reportObject.location = new Point(location.X, location.Y);
            reportObject._size = new Size(_size.Width, _size.Height);
            reportObject.Name = Name;
            reportObject.ObjectType = ObjectType;
            reportObject.font = (Font)font.Clone();
            reportObject.ContentAlignment = ContentAlignment;
            reportObject.ForeColor = ForeColor;
            reportObject.staticText = staticText;
            reportObject.StringFormat = StringFormat;
            reportObject.SuppressIfDuplicate = SuppressIfDuplicate;
            reportObject.FloatLineThickness = FloatLineThickness;
            reportObject.FieldDefKind = FieldDefKind;
            reportObject.FieldValueType = FieldValueType;
            reportObject.SpecialFieldType = SpecialFieldType;
            reportObject.SummaryOperation = SummaryOperation;
            reportObject.LineOrientation = LineOrientation;
            reportObject.LinePosition = LinePosition;
            reportObject.IntLinePercent = IntLinePercent;
            reportObject.OffSetX = OffSetX;
            reportObject.OffSetY = OffSetY;
            reportObject.IsUnderlined = IsUnderlined;
            reportObject.SummarizedField = SummarizedField;
            reportObject.DataField = DataField;
            reportObject.SummaryOrientation = SummaryOrientation;

            var summaryGroupsNew = new List<int>();
            if (SummaryGroups != null)
            {
                for (int i = 0; i < SummaryGroups.Count; i++)
                {
                    summaryGroupsNew.Add(SummaryGroups[i]);
                }
            }
            reportObject.SummaryGroups = summaryGroupsNew;

            return reportObject;
        }

        /// <summary>
        /// Once a dataTable has been set, this method can be run to get the summary value of this field. 
        /// It will still need to be formatted. 
        /// It loops through all records to get this value.
        /// </summary>
        public double GetSummaryValue(DataTable dataTable, int col)
        {
            double result = 0;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (SummaryOperation == SummaryOperation.Sum)
                {
                    result += PIn.Double(dataTable.Rows[i][col].ToString());
                }
                else if (SummaryOperation == SummaryOperation.Count)
                {
                    result++;
                }
                else if (SummaryOperation == SummaryOperation.Average)
                {
                    result += (PIn.Double(dataTable.Rows[i][col].ToString()) / dataTable.Rows.Count);
                }
            }
            return result;
        }

        /// <summary>
        /// Used to automatically calculate the new size when something important changes. Also recalculates location for report headers.
        /// </summary>
        Size CalculateNewSize(string text, Font font)
        {
            Size size;

            using (var graphics = Graphics.FromImage(new Bitmap(1, 1)))
            {
                if (SectionType == AreaSectionType.GroupHeader || SectionType == AreaSectionType.GroupFooter || SectionType == AreaSectionType.Detail)
                {
                    size = new Size(_size.Width, (int)(graphics.MeasureString(text, font).Height / graphics.DpiY * 100 + 2));
                }
                else
                {
                    size = new Size((int)(graphics.MeasureString(text, font).Width / graphics.DpiX * 100 + 2), (int)(graphics.MeasureString(text, font).Height / graphics.DpiY * 100 + 2));
                }

                if (SectionType == AreaSectionType.ReportHeader)
                {
                    location.X += _size.Width / 2;
                    location.X -= size.Width / 2;
                }
            }

            return size;
        }
    }

    /// <summary>
    /// Specifies the field kind in the FieldKind property of the ReportObject class.  Used in Queries and Datatables.
    /// </summary>
    public enum FieldDefKind
    {
        ///<summary>Basic informational cell/field for a Datatable.</summary>
        DataTableField,
        ///<summary>Currently not in use.</summary>
        FormulaField,
        ///<summary>Used in conjunction with SpecialFieldType to determine special logic for certain objects.  Currently only used for PageNumbers</summary>
        SpecialField,
        ///<summary>Used when creating summaries of a column.  Uses the current SummaryOperation to determine which calculation to make.</summary>
        SummaryField
    }

    /// <summary>
    /// Used in the Kind field of each ReportObject to provide a quick way to tell what kind of reportObject.
    /// </summary>
    public enum ReportObjectType
    {
        /// <summary>
        /// Object is a box and will draw a rectangle with the specified parameters.
        /// </summary>
        BoxObject,
        
        /// <summary>
        /// Object is a field object and will be used in drawing datatables.
        /// </summary>
        FieldObject,
        
        /// <summary>
        /// Object is a line and will draw a straight line with the specified parameters.
        /// </summary>
        LineObject,
        
        /// <summary>
        /// Object is a special subset of ReportObject.
        /// Contains its own list of ReportObjects and always contains a query or datatable of information that will be drawn in the report.
        /// </summary>
        QueryObject,

        /// <summary>
        /// Object is a text object.
        /// Can be placed anywhere and is used in multiple sections.
        /// Not to be confused with Datatable cell/field objects.
        /// </summary>
        TextObject
    }

    /// <summary>
    /// Specifies the special field type in the SpecialType property of the ReportObject class.
    /// </summary>
    public enum SpecialFieldType
    {
        /// <summary>
        /// Field returns "Page [current page number] of [total page count]" formula. Not functional yet.
        /// </summary>
        PageCounter,

        PageNumber,
        PrintDate
    }

    public enum SummaryOperation
    {
        Count,
        Sum,
        Average,
    }

    /// <summary>
    /// Used to determine how a line draws in a section.
    /// </summary>
    public enum LineOrientation
    {
        Horizontal,
        Vertical
    }

    /// <summary>
    /// Used to determine where a line draws in a section.
    /// </summary>
    public enum LinePosition
    {
        Center,
        East,
        North,
        South,
        West
    }

    /// <summary>
    /// This determines what type of column the table will be splitting on. 
    /// </summary>
    public enum SplitByKind
    {
        None,
        Date,
        Enum,
        Definition,
        Value
    }

    /// <summary>
    /// This determines which side of the summaryfield the label will be drawn on.
    /// </summary>
    public enum SummaryOrientation
    {
        None,
        North,
        South,
        East,
        West
    }
}