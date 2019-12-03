using System;
using System.ComponentModel;

namespace OpenDentBusiness
{


    ///<summary>For sheetFields</summary>
    public enum GrowthBehaviorEnum
    {
        ///<Summary>Not allowed to grow.  Max size would be Height and Width.</Summary>
        None,
        ///<Summary>Can grow down if needed, and will push nearby objects out of the way so that there is no overlap.</Summary>
        DownLocal,
        ///<Summary>Can grow down, and will push down all objects on the sheet that are below it.  Mostly used when drawing grids.</Summary>
        DownGlobal,
        ///<summary>Used with dynamic grids to grow the grid to fill to the right and bottom of the parent control, does not check for overlap.</summary>
        [SheetGrowthAttribute(true)]
        FillRightDown,
        ///<summary>Used with dynamic grids to grow the grid to fill to the bottom of the parent control, does not check for overlap.</summary>
        [SheetGrowthAttribute(true)]
        FillDown,
        ///<summary>Used with dynamic grids to grow the grid to fill to the right of the parent control, does not check for overlap.</summary>
        [SheetGrowthAttribute(true)]
        FillRight,
        ///<summary>Used with dynamic grids to grow the grid to fill vertical space in parent control and fit grid width to include all columns.
        ///Primarily for ProgressNotes grid in Chart Module.</summary>
        [SheetGrowthAttribute(true, true)]
        FillDownFitColumns,
    }

    ///<summary></summary>
    public enum SheetFieldType
    {
        ///<Summary>0-Pulled from the database to be printed on the sheet.  Or also possibly just generated at runtime even though not pulled from the database.   User still allowed to change the output text as they are filling out the sheet so that it can different from what was initially generated.</Summary>
        OutputText,
        ///<Summary>1-A blank box that the user is supposed to fill in.</Summary>
        [Description("Input")]
        InputField,
        ///<Summary>2-This is text that is defined as part of the sheet and will never change from sheet to sheet.  </Summary>
        StaticText,
        ///<summary>3-Stores a parameter other than the PatNum.  Not meant to be seen on the sheet.  Only used for SheetField, not SheetFieldDef.</summary>
        Parameter,
        ///<Summary>4-Any image of any size, typically a background image for a form.</Summary>
        Image,
        ///<summary>5-One sequence of dots that makes a line.  Continuous without any breaks.  Each time the pen is picked up, it creates a new field row in the database.</summary>
        Drawing,
        ///<Summary>6-A simple line drawn from x,y to x+width,y+height.  So for these types, we must allow width and height to be negative or zero.</Summary>
        Line,
        ///<Summary>7-A simple rectangle outline.</Summary>
        Rectangle,
        ///<summary>8-A clickable area on the screen.  It's a form of input, so treated similarly to an InputField.  The X will go from corner to corner of the rectangle specified.  It can also behave like a radio button</summary>
        [Description("Check Box")]
        CheckBox,
        ///<summary>9-A signature box, either Topaz pad or directly on the screen with stylus/mouse.  The signature is encrypted based an a hash of all
        ///other field values in the entire sheet, excluding other SigBoxes.  The order is critical.</summary>
        [Description("Signature")]
        SigBox,
        ///<Summary>10-An image specific to one patient.</Summary>
        PatImage,
        ///<Summary>11-Special: Currently only used for Toothgrid</Summary>
        Special,
        ///<summary>12-Grid: Placable grids similar to ODGrids. Used primarily in statements.</summary>
        Grid,
        ///<summary>13-ComboBox: Placeable combo box for selecting filled options.</summary>
        [Description("Combo Box")]
        ComboBox,
        ///<summary>14-ScreenChart: A tooth chart that is desiged for screenings.</summary>
        ScreenChart,
        ///<summary>15-MobileHeader: The parent field of a group of fields. All fields in between this field and the next MobileHeader will be grouped toghether in the mobile view.
        ///EG... "Personal", "Address and Home Phone", "Insurance".</summary>
        [Description("Header")]
        MobileHeader,
        ///<summary>16-A signature box, either Topaz pad or directly on the screen with stylus/mouse.  The signature is encrypted based an a hash of all 
        ///other field values in the entire sheet, excluding other SigBoxes.  The order is critical.</summary>
        [Description("Practice Signature")]
        SigBoxPractice
        //<summary></summary>
        //RadioButton

        //<Summary>Not yet supported.  This might be redundant, and we might use border element instead as the preferred way of drawing a box.</Summary>
        //Box
    }

    public enum SheetInternalType
    {
        LabelPatientMail,
        LabelPatientLFAddress,
        LabelPatientLFChartNumber,
        LabelPatientLFPatNum,
        LabelPatientRadiograph,
        LabelText,
        LabelCarrier,
        LabelReferral,
        ReferralSlip,
        LabelAppointment,
        Rx,
        Consent,
        PatientLetter,
        PatientLetterTxFinder,
        ReferralLetter,
        RoutingSlip,
        PatientRegistration,
        FinancialAgreement,
        HIPAA,
        MedicalHistSimple,
        MedicalHistNewPat,
        MedicalHistUpdate,
        LabSlip,
        ExamSheet,
        DepositSlip,
        Statement,
        ///<summary>Users are NEVER allowed to use this sheet type. It is for internal use only. It should be hidden in all lists and unselectable.</summary>
        MedLabResults,
        TreatmentPlan,
        Screening,
        PaymentPlan,
        RxMulti,
        ERA,
        ERAGridHeader,
        RxInstruction,
        PatientTransferCEMT,
        ChartModule,
        PatientDashboard,
    }

    public enum OutInCheck
    {
        Out,
        In,
        Check
    }

    ///<summary>Defines the views that a SheetFieldDef should show in.
    ///Typically most sheets only use SheetViewMode.Default.
    ///SheetDefs that are associated to a dynamic layout must define each mode the control can be viewed in(i.e. chart module).</summary>
    public enum SheetFieldLayoutMode
    {
        ///<summary>Valid for every SheetTypeEnum.
        ///When SheetTypeEnum is associated to a dynamic layout this is the way the layout will show by default.</summary>
        Default,
        ///<summary>Chart module dynamic layout when we are viewing the SheetFieldLayoutMode.Default and the Treatment Plans checkbox is checked.</summary>
        [DescriptionAttribute("Treatment plan view")]
        TreatPlan,
        ///<summary>Chart module dynamic layout when ECW is enabled.</summary>
        [DescriptionAttribute("ECW enabled view")]
        Ecw,
        ///<summary>>Chart module dynamic layout when ECW is enabled and the Treatment Plans checkbox is checked.</summary>
        [DescriptionAttribute("ECW treatment plan view")]
        EcwTreatPlan,
        ///<summary>Chart module dynamic layout when Orion is enabled.</summary>
        [DescriptionAttribute("Orion enabled view")]
        Orion,
        ///<summary>Chart module dynamic layout when Orion is enabled and the Treatment Plans checkbox is checked.</summary>
        [DescriptionAttribute("Orion treatment plan view")]
        OrionTreatPlan,
        ///<summary>Chart module dynamic layout when current clinic is associated to a medical clinic or practice.</summary>
        [DescriptionAttribute("Medical Practice")]
        MedicalPractice,
        ///<summary>Chart module dynamic layout when current clinic is associated to a medical clinic or practice and the Treatment Plans checkbox is checked.</summary>
        [DescriptionAttribute("Medical practice treatment plan view")]
        MedicalPracticeTreatPlan,
    }

    ///<summary>Used when we want to link a SheetTypeEnum to a set of SheetFieldLayoutModes.  Ex: ChartModule link to ECW, Medical, Orion, etc.
    ///SheetTypeEnums which are not dynamic will have IsDynamic=false and ArraySheetFieldLayoutModes will only contain SheetFieldLayoutMode.Default.</summary>
    public class SheetLayoutAttribute : Attribute
    {
        ///<summary>False by default.
        ///True when the SheetTypeEnum is associated to a dynamic layout SheetEnumType, like SheetEnumType.ChartModule.</summary>
        public bool IsDynamic = false;
        ///<summary>Always contains SheetFieldLayoutMode.Default.</summary>
        public SheetFieldLayoutMode[] ArraySheetFieldLayoutModes = new SheetFieldLayoutMode[] { SheetFieldLayoutMode.Default };

        public SheetLayoutAttribute()
        {
            //Empty constructor needed, default values used.
        }

        ///<summary>The arraySheetFieldLayoutModes must not contain Default option as it will be automatically included.</summary>
        public SheetLayoutAttribute(bool isDynamic, params SheetFieldLayoutMode[] arraySheetFieldLayoutModes)
        {
            IsDynamic = isDynamic;
            ArraySheetFieldLayoutModes = new SheetFieldLayoutMode[arraySheetFieldLayoutModes.Length + 1];//+1 for default.
            ArraySheetFieldLayoutModes[0] = SheetFieldLayoutMode.Default;
            for (int i = 0; i < arraySheetFieldLayoutModes.Length; i++)
            {
                ArraySheetFieldLayoutModes[i + 1] = arraySheetFieldLayoutModes[i];
            }
        }
    }

    /// <summary>
    /// Used to identify various dynamic sheet related enums.
    /// </summary>
    public class SheetGrowthAttribute : Attribute
    {
        /// <summary>
        /// True when the GrowthBehavior is associated to a dynamic layout sheetFieldDef, like chart module grids.
        /// </summary>
        public bool IsDynamic = false;
        
        /// <summary>
        /// When true the associated GrowthBehaviorEnum will only show in the UI for ODGrid sheetFieldDefs
        /// </summary>
        public bool IsGridOnly = false;

        public SheetGrowthAttribute()
        {
        }

        public SheetGrowthAttribute(bool isDynamic, bool isGridOnly = false)
        {
            IsDynamic = isDynamic;
            IsGridOnly = isGridOnly;
        }
    }
}
