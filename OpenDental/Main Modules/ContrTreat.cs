/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Linq;
using OpenDental.UI;
using OpenDentBusiness.HL7;
using SparksToothChart;
using OpenDentBusiness;
using CodeBase;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using MigraDoc.Rendering.Printing;
using Document=OpenDentBusiness.Document;
using OpenDental.Properties;
using SLDental.Storage;

namespace OpenDental{
///<summary></summary>
	public class ContrTreat : System.Windows.Forms.UserControl{
		//private AxFPSpread.AxvaSpread axvaSpread2;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components;// Required designer variable.
		private System.Windows.Forms.ListBox listSetPr;
		//private int linesPrinted=0;
		///<summary></summary>
    public FormRpPrintPreview pView;
//		private System.Windows.Forms.PrintDialog printDialog2;
		//private bool headingPrinted;
		//private bool graphicsPrinted;
		//private bool mainPrinted;
		//private bool benefitsPrinted;
		//private bool notePrinted;
		//private double[] ColTotal;
		private System.Drawing.Font bodyFont=new System.Drawing.Font("Arial",9);
		private System.Drawing.Font nameFont=new System.Drawing.Font("Arial",9,FontStyle.Bold);
		//private Font headingFont=new Font("Arial",13,FontStyle.Bold);
		private System.Drawing.Font subHeadingFont=new System.Drawing.Font("Arial",10,FontStyle.Bold);
		private System.Drawing.Printing.PrintDocument pd2;
		private System.Drawing.Font totalFont=new System.Drawing.Font("Arial",9,FontStyle.Bold);
		private OpenDental.UI.ODToolBar ToolBarMain;
    private ArrayList ALPreAuth;
		///<summary>This is a list of all procedures for the patient.</summary>
		private List<Procedure> ProcList;
		///<summary>This is a filtered list containing only TP procedures.  It's also already sorted by priority and tooth number.</summary>
		private Procedure[] ProcListTP;
		///<summary>List of ClaimProcs with status of Estimate or CapEstimate for the patient.</summary>
		private List<ClaimProc> ClaimProcList;
		private Family FamCur;
		private Patient PatCur;
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.ODGrid gridPrint;
		private OpenDental.UI.ODGrid gridPreAuth;
		private List<InsPlan> InsPlanList;
		private List<SubstitutionLink> _listSubstLinks=null;
		private List<InsSub> SubList;
		private OpenDental.UI.ODGrid gridPlans;
		private List<TreatPlan> _listTreatPlans;
		//private List<TreatPlan> _listTPCurrent;
		///<summary>A list of all ProcTP objects for this patient.</summary>
		private ProcTP[] ProcTPList;
		private ODtextBox textNote;
		private System.Windows.Forms.ImageList imageListMain;
		///<summary>A list of all ProcTP objects for the selected tp.</summary>
		private ProcTP[] ProcTPSelectList;
		private List <PatPlan> PatPlanList;
		private List <Benefit> BenefitList;
		private List<Procedure> ProcListFiltered;
		///<summary>Only used for printing graphical chart.</summary>
		private List<ToothInitial> ToothInitialList;
		///<summary>Only used for printing graphical chart.</summary>
		private ToothChartWrapper toothChart;
		///<summary>Only used for printing graphical chart.</summary>
		private Bitmap chartBitmap;
		private List<Claim> ClaimList;
		private bool InitializedOnStartup;
		private List<ClaimProcHist> HistList;
		private List<ClaimProcHist> LoopList;
		private bool checkShowInsNotAutomatic;
		private bool checkShowDiscountNotAutomatic;
		private List<TpRow> RowsMain;
		private UI.Button butInsRem;
		private UI.Button butNewTP;
		private UI.Button butSaveTP;
		///<summary>Gets updated to PatCur.PatNum that the last security log was made with so that we don't make too many security logs for this patient.  When _patNumLast no longer matches PatCur.PatNum (e.g. switched to a different patient within a module), a security log will be entered.  Gets reset (cleared and the set back to PatCur.PatNum) any time a module button is clicked which will cause another security log to be entered.</summary>
		private long _patNumLast;
		private DateTimePicker dateTimeTP;
		private UI.Button butRefresh;
		private TabControl tabShowSort;
		private TabPage tabPageShow;
		private CheckBox checkShowDiscount;
		private CheckBox checkShowTotals;
		private CheckBox checkShowMaxDed;
		private CheckBox checkShowSubtotals;
		private CheckBox checkShowFees;
		private CheckBox checkShowIns;
		private CheckBox checkShowCompleted;
		private TabPage tabPageSort;
		private Label label6;
		private RadioButton radioTreatPlanSortTooth;
		private RadioButton radioTreatPlanSortOrder;
		private Label labelCheckInsFrequency;
		private TabPage tabPagePrint;
		private CheckBox checkPrintClassic;
		private UI.Button butPlannedAppt;
		private DashFamilyInsurance userControlFamIns;
		private DashIndividualInsurance userControlIndIns;

		///<summary>Set to true when TP Note changes.  Public so this can be checked from FormOpenDental and the note can be saved.  Necessary because in
		///some cases the leave event doesn't fire, like when a non-modal form is selected, like big phones, and the selected patient is changed from that form.</summary>
		public bool HasNoteChanged=false;

		///<summary></summary>
		public ContrTreat(){
			Logger.Write(LogLevel.Info, "Initializing treatment module...");
			InitializeComponent();// This call is required by the Windows.Forms Form Designer.
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrTreat));
			this.label1 = new System.Windows.Forms.Label();
			this.listSetPr = new System.Windows.Forms.ListBox();
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.gridMain = new OpenDental.UI.ODGrid();
			this.gridPrint = new OpenDental.UI.ODGrid();
			this.gridPreAuth = new OpenDental.UI.ODGrid();
			this.gridPlans = new OpenDental.UI.ODGrid();
			this.dateTimeTP = new System.Windows.Forms.DateTimePicker();
			this.tabShowSort = new System.Windows.Forms.TabControl();
			this.tabPageShow = new System.Windows.Forms.TabPage();
			this.checkShowDiscount = new System.Windows.Forms.CheckBox();
			this.checkShowTotals = new System.Windows.Forms.CheckBox();
			this.checkShowMaxDed = new System.Windows.Forms.CheckBox();
			this.checkShowSubtotals = new System.Windows.Forms.CheckBox();
			this.checkShowFees = new System.Windows.Forms.CheckBox();
			this.checkShowIns = new System.Windows.Forms.CheckBox();
			this.checkShowCompleted = new System.Windows.Forms.CheckBox();
			this.tabPageSort = new System.Windows.Forms.TabPage();
			this.label6 = new System.Windows.Forms.Label();
			this.radioTreatPlanSortTooth = new System.Windows.Forms.RadioButton();
			this.radioTreatPlanSortOrder = new System.Windows.Forms.RadioButton();
			this.tabPagePrint = new System.Windows.Forms.TabPage();
			this.checkPrintClassic = new System.Windows.Forms.CheckBox();
			this.labelCheckInsFrequency = new System.Windows.Forms.Label();
			this.userControlIndIns = new OpenDental.DashIndividualInsurance();
			this.userControlFamIns = new OpenDental.DashFamilyInsurance();
			this.butPlannedAppt = new OpenDental.UI.Button();
			this.butRefresh = new OpenDental.UI.Button();
			this.butSaveTP = new OpenDental.UI.Button();
			this.butNewTP = new OpenDental.UI.Button();
			this.butInsRem = new OpenDental.UI.Button();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.textNote = new OpenDental.ODtextBox();
			this.tabShowSort.SuspendLayout();
			this.tabPageShow.SuspendLayout();
			this.tabPageSort.SuspendLayout();
			this.tabPagePrint.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(808, 178);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 15);
			this.label1.TabIndex = 4;
			this.label1.Text = "Set Priority";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listSetPr
			// 
			this.listSetPr.Location = new System.Drawing.Point(810, 195);
			this.listSetPr.Name = "listSetPr";
			this.listSetPr.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listSetPr.Size = new System.Drawing.Size(70, 199);
			this.listSetPr.TabIndex = 5;
			this.listSetPr.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listSetPr_MouseDown);
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0, "");
			this.imageListMain.Images.SetKeyName(1, "");
			this.imageListMain.Images.SetKeyName(2, "");
			this.imageListMain.Images.SetKeyName(3, "Add.gif");
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.EditableEnterMovesDown = false;
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(0, 182);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
			this.gridMain.Size = new System.Drawing.Size(790, 469);
			this.gridMain.TabIndex = 59;
			this.gridMain.Title = "Procedures";
			this.gridMain.CellDoubleClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.gridMain_CellClick);
			// 
			// gridPrint
			// 
			this.gridPrint.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPrint.EditableEnterMovesDown = false;
			this.gridPrint.HasAddButton = false;
			this.gridPrint.HasDropDowns = false;
			this.gridPrint.HasMultilineHeaders = false;
			this.gridPrint.HScrollVisible = false;
			this.gridPrint.Location = new System.Drawing.Point(0, 0);
			this.gridPrint.Name = "gridPrint";
			this.gridPrint.ScrollValue = 0;
			this.gridPrint.Size = new System.Drawing.Size(150, 150);
			this.gridPrint.TabIndex = 0;
			this.gridPrint.Title = null;
			// 
			// gridPreAuth
			// 
			this.gridPreAuth.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPreAuth.EditableEnterMovesDown = false;
			this.gridPreAuth.HasAddButton = false;
			this.gridPreAuth.HasDropDowns = false;
			this.gridPreAuth.HasMultilineHeaders = false;
			this.gridPreAuth.HScrollVisible = false;
			this.gridPreAuth.Location = new System.Drawing.Point(684, 29);
			this.gridPreAuth.Name = "gridPreAuth";
			this.gridPreAuth.ScrollValue = 0;
			this.gridPreAuth.Size = new System.Drawing.Size(252, 146);
			this.gridPreAuth.TabIndex = 62;
			this.gridPreAuth.Title = "Pre Authorizations";
			this.gridPreAuth.CellDoubleClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.gridPreAuth_CellDoubleClick);
			this.gridPreAuth.CellClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.gridPreAuth_CellClick);
			// 
			// gridPlans
			// 
			this.gridPlans.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPlans.EditableEnterMovesDown = false;
			this.gridPlans.HasAddButton = false;
			this.gridPlans.HasDropDowns = false;
			this.gridPlans.HasMultilineHeaders = false;
			this.gridPlans.HScrollVisible = false;
			this.gridPlans.Location = new System.Drawing.Point(0, 29);
			this.gridPlans.Name = "gridPlans";
			this.gridPlans.ScrollValue = 0;
			this.gridPlans.Size = new System.Drawing.Size(426, 151);
			this.gridPlans.TabIndex = 60;
			this.gridPlans.Title = "Treatment Plans";
			this.gridPlans.CellDoubleClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.gridPlans_CellDoubleClick);
			this.gridPlans.CellClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.gridPlans_CellClick);
			// 
			// dateTimeTP
			// 
			this.dateTimeTP.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTimeTP.Location = new System.Drawing.Point(429, 99);
			this.dateTimeTP.Name = "dateTimeTP";
			this.dateTimeTP.Size = new System.Drawing.Size(81, 20);
			this.dateTimeTP.TabIndex = 71;
			this.dateTimeTP.CloseUp += new System.EventHandler(this.dateTimeTP_CloseUp);
			// 
			// tabShowSort
			// 
			this.tabShowSort.Controls.Add(this.tabPageShow);
			this.tabShowSort.Controls.Add(this.tabPageSort);
			this.tabShowSort.Controls.Add(this.tabPagePrint);
			this.tabShowSort.Location = new System.Drawing.Point(512, 29);
			this.tabShowSort.Name = "tabShowSort";
			this.tabShowSort.SelectedIndex = 0;
			this.tabShowSort.Size = new System.Drawing.Size(166, 151);
			this.tabShowSort.TabIndex = 73;
			// 
			// tabPageShow
			// 
			this.tabPageShow.Controls.Add(this.checkShowDiscount);
			this.tabPageShow.Controls.Add(this.checkShowTotals);
			this.tabPageShow.Controls.Add(this.checkShowMaxDed);
			this.tabPageShow.Controls.Add(this.checkShowSubtotals);
			this.tabPageShow.Controls.Add(this.checkShowFees);
			this.tabPageShow.Controls.Add(this.checkShowIns);
			this.tabPageShow.Controls.Add(this.checkShowCompleted);
			this.tabPageShow.Location = new System.Drawing.Point(4, 22);
			this.tabPageShow.Name = "tabPageShow";
			this.tabPageShow.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageShow.Size = new System.Drawing.Size(158, 125);
			this.tabPageShow.TabIndex = 0;
			this.tabPageShow.Text = "Show";
			this.tabPageShow.UseVisualStyleBackColor = true;
			// 
			// checkShowDiscount
			// 
			this.checkShowDiscount.Checked = true;
			this.checkShowDiscount.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowDiscount.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowDiscount.Location = new System.Drawing.Point(23, 71);
			this.checkShowDiscount.Name = "checkShowDiscount";
			this.checkShowDiscount.Size = new System.Drawing.Size(128, 17);
			this.checkShowDiscount.TabIndex = 32;
			this.checkShowDiscount.Text = "Discount";
			this.checkShowDiscount.Click += new System.EventHandler(this.checkShowDiscount_Click);
			// 
			// checkShowTotals
			// 
			this.checkShowTotals.Checked = true;
			this.checkShowTotals.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowTotals.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowTotals.Location = new System.Drawing.Point(23, 105);
			this.checkShowTotals.Name = "checkShowTotals";
			this.checkShowTotals.Size = new System.Drawing.Size(128, 15);
			this.checkShowTotals.TabIndex = 31;
			this.checkShowTotals.Text = "Totals";
			this.checkShowTotals.Click += new System.EventHandler(this.checkShowTotals_Click);
			// 
			// checkShowMaxDed
			// 
			this.checkShowMaxDed.Checked = true;
			this.checkShowMaxDed.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowMaxDed.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowMaxDed.Location = new System.Drawing.Point(5, 20);
			this.checkShowMaxDed.Name = "checkShowMaxDed";
			this.checkShowMaxDed.Size = new System.Drawing.Size(146, 17);
			this.checkShowMaxDed.TabIndex = 30;
			this.checkShowMaxDed.Text = "Use Ins Max and Deduct";
			this.checkShowMaxDed.Click += new System.EventHandler(this.checkShowMaxDed_Click);
			// 
			// checkShowSubtotals
			// 
			this.checkShowSubtotals.Checked = true;
			this.checkShowSubtotals.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowSubtotals.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowSubtotals.Location = new System.Drawing.Point(23, 88);
			this.checkShowSubtotals.Name = "checkShowSubtotals";
			this.checkShowSubtotals.Size = new System.Drawing.Size(128, 17);
			this.checkShowSubtotals.TabIndex = 29;
			this.checkShowSubtotals.Text = "Subtotals";
			this.checkShowSubtotals.Click += new System.EventHandler(this.checkShowSubtotals_Click);
			// 
			// checkShowFees
			// 
			this.checkShowFees.Checked = true;
			this.checkShowFees.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowFees.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowFees.Location = new System.Drawing.Point(5, 37);
			this.checkShowFees.Name = "checkShowFees";
			this.checkShowFees.Size = new System.Drawing.Size(146, 17);
			this.checkShowFees.TabIndex = 28;
			this.checkShowFees.Text = "Fees";
			this.checkShowFees.Click += new System.EventHandler(this.checkShowFees_Click);
			// 
			// checkShowIns
			// 
			this.checkShowIns.Checked = true;
			this.checkShowIns.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowIns.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowIns.Location = new System.Drawing.Point(23, 54);
			this.checkShowIns.Name = "checkShowIns";
			this.checkShowIns.Size = new System.Drawing.Size(128, 17);
			this.checkShowIns.TabIndex = 27;
			this.checkShowIns.Text = "Insurance Estimates";
			this.checkShowIns.Click += new System.EventHandler(this.checkShowIns_Click);
			// 
			// checkShowCompleted
			// 
			this.checkShowCompleted.Checked = true;
			this.checkShowCompleted.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowCompleted.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowCompleted.Location = new System.Drawing.Point(5, 3);
			this.checkShowCompleted.Name = "checkShowCompleted";
			this.checkShowCompleted.Size = new System.Drawing.Size(146, 17);
			this.checkShowCompleted.TabIndex = 26;
			this.checkShowCompleted.Text = "Graphical Completed Tx";
			// 
			// tabPageSort
			// 
			this.tabPageSort.Controls.Add(this.label6);
			this.tabPageSort.Controls.Add(this.radioTreatPlanSortTooth);
			this.tabPageSort.Controls.Add(this.radioTreatPlanSortOrder);
			this.tabPageSort.Location = new System.Drawing.Point(4, 22);
			this.tabPageSort.Name = "tabPageSort";
			this.tabPageSort.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageSort.Size = new System.Drawing.Size(158, 125);
			this.tabPageSort.TabIndex = 1;
			this.tabPageSort.Text = "Sort by";
			this.tabPageSort.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(3, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(146, 33);
			this.label6.TabIndex = 74;
			this.label6.Text = "Order procedures by priority, then date, then";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// radioTreatPlanSortTooth
			// 
			this.radioTreatPlanSortTooth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioTreatPlanSortTooth.Location = new System.Drawing.Point(14, 44);
			this.radioTreatPlanSortTooth.Name = "radioTreatPlanSortTooth";
			this.radioTreatPlanSortTooth.Size = new System.Drawing.Size(116, 15);
			this.radioTreatPlanSortTooth.TabIndex = 73;
			this.radioTreatPlanSortTooth.Text = "Tooth";
			this.radioTreatPlanSortTooth.UseVisualStyleBackColor = true;
			this.radioTreatPlanSortTooth.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioTreatPlanSortTooth_MouseClick);
			// 
			// radioTreatPlanSortOrder
			// 
			this.radioTreatPlanSortOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioTreatPlanSortOrder.Checked = true;
			this.radioTreatPlanSortOrder.Location = new System.Drawing.Point(14, 59);
			this.radioTreatPlanSortOrder.Name = "radioTreatPlanSortOrder";
			this.radioTreatPlanSortOrder.Size = new System.Drawing.Size(126, 19);
			this.radioTreatPlanSortOrder.TabIndex = 72;
			this.radioTreatPlanSortOrder.TabStop = true;
			this.radioTreatPlanSortOrder.Text = "Order Entered";
			this.radioTreatPlanSortOrder.UseVisualStyleBackColor = true;
			this.radioTreatPlanSortOrder.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioTreatPlanSortTooth_MouseClick);
			// 
			// tabPagePrint
			// 
			this.tabPagePrint.Controls.Add(this.checkPrintClassic);
			this.tabPagePrint.Location = new System.Drawing.Point(4, 22);
			this.tabPagePrint.Name = "tabPagePrint";
			this.tabPagePrint.Padding = new System.Windows.Forms.Padding(3);
			this.tabPagePrint.Size = new System.Drawing.Size(158, 125);
			this.tabPagePrint.TabIndex = 2;
			this.tabPagePrint.Text = "Printing";
			this.tabPagePrint.UseVisualStyleBackColor = true;
			// 
			// checkPrintClassic
			// 
			this.checkPrintClassic.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPrintClassic.Location = new System.Drawing.Point(7, 12);
			this.checkPrintClassic.Name = "checkPrintClassic";
			this.checkPrintClassic.Size = new System.Drawing.Size(146, 17);
			this.checkPrintClassic.TabIndex = 27;
			this.checkPrintClassic.Text = "Print using classic view";
			// 
			// labelCheckInsFrequency
			// 
			this.labelCheckInsFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCheckInsFrequency.Location = new System.Drawing.Point(426, 83);
			this.labelCheckInsFrequency.Name = "labelCheckInsFrequency";
			this.labelCheckInsFrequency.Size = new System.Drawing.Size(84, 15);
			this.labelCheckInsFrequency.TabIndex = 74;
			this.labelCheckInsFrequency.Text = "Estimates as of:";
			this.labelCheckInsFrequency.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// userControlIndIns
			// 
			this.userControlIndIns.Location = new System.Drawing.Point(799, 491);
			this.userControlIndIns.Name = "userControlIndIns";
			this.userControlIndIns.Size = new System.Drawing.Size(193, 160);
			this.userControlIndIns.TabIndex = 77;
			// 
			// userControlFamIns
			// 
			this.userControlFamIns.Location = new System.Drawing.Point(799, 411);
			this.userControlFamIns.Name = "userControlFamIns";
			this.userControlFamIns.Size = new System.Drawing.Size(193, 80);
			this.userControlFamIns.TabIndex = 76;
			// 
			// butPlannedAppt
			// 
			this.butPlannedAppt.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPlannedAppt.Autosize = true;
			this.butPlannedAppt.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butPlannedAppt.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butPlannedAppt.CornerRadius = 4F;
			this.butPlannedAppt.Image = global::OpenDental.Properties.Resources.Add;
			this.butPlannedAppt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPlannedAppt.Location = new System.Drawing.Point(429, 154);
			this.butPlannedAppt.Name = "butPlannedAppt";
			this.butPlannedAppt.Size = new System.Drawing.Size(80, 23);
			this.butPlannedAppt.TabIndex = 75;
			this.butPlannedAppt.Text = "Plan Appt";
			this.butPlannedAppt.Click += new System.EventHandler(this.butPlannedAppt_Click);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRefresh.Location = new System.Drawing.Point(429, 125);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(80, 23);
			this.butRefresh.TabIndex = 72;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butSaveTP
			// 
			this.butSaveTP.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSaveTP.Autosize = true;
			this.butSaveTP.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butSaveTP.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butSaveTP.CornerRadius = 4F;
			this.butSaveTP.Image = global::OpenDental.Properties.Resources.butCopy;
			this.butSaveTP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSaveTP.Location = new System.Drawing.Point(429, 58);
			this.butSaveTP.Name = "butSaveTP";
			this.butSaveTP.Size = new System.Drawing.Size(80, 23);
			this.butSaveTP.TabIndex = 70;
			this.butSaveTP.Text = "Save TP";
			this.butSaveTP.Click += new System.EventHandler(this.butSaveTP_Click);
			// 
			// butNewTP
			// 
			this.butNewTP.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNewTP.Autosize = true;
			this.butNewTP.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butNewTP.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butNewTP.CornerRadius = 4F;
			this.butNewTP.Image = global::OpenDental.Properties.Resources.Add;
			this.butNewTP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNewTP.Location = new System.Drawing.Point(429, 29);
			this.butNewTP.Name = "butNewTP";
			this.butNewTP.Size = new System.Drawing.Size(80, 23);
			this.butNewTP.TabIndex = 69;
			this.butNewTP.Text = "New TP";
			this.butNewTP.Click += new System.EventHandler(this.butNewTP_Click);
			// 
			// butInsRem
			// 
			this.butInsRem.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInsRem.Autosize = true;
			this.butInsRem.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butInsRem.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butInsRem.CornerRadius = 4F;
			this.butInsRem.Location = new System.Drawing.Point(917, 400);
			this.butInsRem.Name = "butInsRem";
			this.butInsRem.Size = new System.Drawing.Size(75, 16);
			this.butInsRem.TabIndex = 68;
			this.butInsRem.Text = "Ins Rem";
			this.butInsRem.Visible = false;
			this.butInsRem.Click += new System.EventHandler(this.butInsRem_Click);
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(1195, 25);
			this.ToolBarMain.TabIndex = 58;
			this.ToolBarMain.ButtonClick += new EventHandler<ODToolBarButtonClickEventArgs>(this.ToolBarMain_ButtonClick);
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textNote.BackColor = System.Drawing.SystemColors.Control;
			this.textNote.DetectLinksEnabled = false;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(0, 654);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.TreatPlan;
			this.textNote.ReadOnly = true;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(790, 52);
			this.textNote.SpellCheckIsEnabled = false;
			this.textNote.TabIndex = 54;
			this.textNote.Text = "";
			this.textNote.TextChanged += new System.EventHandler(this.textNote_TextChanged);
			this.textNote.Leave += new System.EventHandler(this.textNote_Leave);
			// 
			// ContrTreat
			// 
			this.Controls.Add(this.butInsRem);
			this.Controls.Add(this.userControlIndIns);
			this.Controls.Add(this.userControlFamIns);
			this.Controls.Add(this.butPlannedAppt);
			this.Controls.Add(this.labelCheckInsFrequency);
			this.Controls.Add(this.tabShowSort);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.dateTimeTP);
			this.Controls.Add(this.butSaveTP);
			this.Controls.Add(this.butNewTP);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.listSetPr);
			this.Controls.Add(this.ToolBarMain);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridPreAuth);
			this.Controls.Add(this.gridPlans);
			this.Controls.Add(this.textNote);
			this.Name = "ContrTreat";
			this.Size = new System.Drawing.Size(1195, 708);
			this.tabShowSort.ResumeLayout(false);
			this.tabPageShow.ResumeLayout(false);
			this.tabPageSort.ResumeLayout(false);
			this.tabPagePrint.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		///<summary>Only called on startup, but after local data loaded from db.</summary>
		public void InitializeOnStartup() {
			if(InitializedOnStartup) {
				return;
			}
			InitializedOnStartup=true;
			checkShowCompleted.Checked=Preference.GetBool(PreferenceName.TreatPlanShowCompleted);
			if(Preferences.RandomKeys) {//random PKs don't get the option or sorting by order entered
				tabShowSort.TabPages.Remove(tabPageSort);
			}
			else {
				radioTreatPlanSortTooth.Checked=Preference.GetBool(PreferenceName.TreatPlanSortByTooth);
			}
			//checkShowIns.Checked=PrefC.GetBool(PrefName.TreatPlanShowIns");
			//checkShowDiscount.Checked=PrefC.GetBool(PrefName.TreatPlanShowDiscount");
			//showHidden=true;//shows hidden priorities
			//can't use 
			LayoutToolBar();//redundant?
			tabShowSort.TabPages.Remove(tabPagePrint);//We may add this back in gridPlans_CellClick.
		}

		///<summary>Called every time local data is changed from any workstation.  Refreshes priority lists and lays out the toolbar.</summary>
		public void InitializeLocalData() {
            List<Definition> listDefs = Definition.GetByCategory(DefinitionCategory.TxPriorities);
			listDefs.Insert(0,new Definition { Description=Lan.g(this,"no priority") });
			listSetPr.Items.Clear();
			foreach(Definition def in listDefs) {
				listSetPr.Items.Add(new ODBoxItem<Definition>(def.Description,def));
			}
			LayoutToolBar();
			if(Preference.GetBool(PreferenceName.EasyHideInsurance)){
				checkShowIns.Visible=false;
				checkShowIns.Checked=false;
				checkShowMaxDed.Visible=false;
				//checkShowMaxDed.Checked=false;
			}
			else{
				checkShowIns.Visible=true;
				checkShowMaxDed.Visible=true;
			}
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar(){
			ToolBarMain.Buttons.Clear();
			//ODToolBarButton button;
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"PreAuthorization"),null,"","PreAuth"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Discount"),null,"","Discount"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Update Fees"), Resources.IconRefresh, "","Update"));
			//ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Save TP"),3,"","Create"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Print TP"), Resources.IconPrint, "","Print"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Email TP"), Resources.IconEmail, "","Email"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Sign TP"),null,"","Sign"));
			ProgramL.LoadToolbar(ToolBarMain,ToolBarsAvail.TreatmentPlanModule);
			ToolBarMain.Invalidate();
			//Plugins.HookAddCode(this,"ContrTreat.LayoutToolBar_end",PatCur);
			UpdateToolbarButtons();
		}

		///<summary></summary>
		public void ModuleSelected(long patNum,bool hasDefaultDate=false) {
			if(hasDefaultDate) {
				dateTimeTP.Value=DateTime.Today;
			}
			RefreshModuleData(patNum);
			RefreshModuleScreen(false);
			//Plugins.HookAddCode(this,"ContrTreat.ModuleSelected_end",patNum);
		}

		///<summary></summary>
		public void ModuleUnselected(){
			UpdateTPNoteIfNeeded();//Handle this here because this happens before textNote_Leave() and we dont want anything nulled before saving;
			FamCur=null;
			PatCur=null;
			InsPlanList=null;
			//Claims.List=null;
			//ClaimProcs.List=null;
			//from FillMain:
			ProcList=null;
			ProcListTP=null;
			//Procedures.HList=null;
			//Procedures.MissingTeeth=null;
			_patNumLast=0;//Clear out the last pat num so that a security log gets entered that the module was "visited" or "refreshed".
			HasNoteChanged=false;
			//Plugins.HookAddCode(this,"ContrTreat.ModuleUnselected_end");
		}

		private void RefreshModuleData(long patNum) {
			UpdateTPNoteIfNeeded();
			if(patNum!=0) {
				bool doMakeSecLog=false;
				if(_patNumLast!=patNum) {
					doMakeSecLog=true;
					_patNumLast=patNum;
				}
				TPModuleData tpData=TreatmentPlanModules.GetModuleData(patNum,doMakeSecLog);
				FamCur=tpData.Fam;
				PatCur=tpData.Pat;
				SubList=tpData.SubList;
				InsPlanList=tpData.InsPlanList;
				_listSubstLinks=tpData.ListSubstLinks;
				PatPlanList=tpData.PatPlanList;
				BenefitList=tpData.BenefitList;
				ClaimList=tpData.ClaimList;
				HistList=tpData.HistList;
				ProcList=tpData.ListProcedures;
				_listTreatPlans=tpData.ListTreatPlans;
				ProcTPList=tpData.ArrProcTPs;
			}
		}

		private void RefreshModuleScreen(bool doRefreshData=true){
			//ParentForm.Text=Patients.GetMainTitle(PatCur);
			FillPlans(doRefreshData);
			UpdateToolbarButtons();
			if(PatCur==null) {
				butNewTP.Enabled=false;
			}
			else {
				butNewTP.Enabled=true;
			}
			if(Preference.GetBool(PreferenceName.InsChecksFrequency)) {
				butRefresh.Visible=true;
				labelCheckInsFrequency.Visible=true;
				dateTimeTP.Visible=true;
			}
			else {
				butRefresh.Visible=false;
				labelCheckInsFrequency.Visible=false;
				dateTimeTP.Visible=false;
			}
			FillMain();
			FillSummary();
      FillPreAuth();
			//FillMisc();
			if(Clinic.GetById(Clinics.ClinicId).IsMedicalOnly) {
				checkShowCompleted.Visible=false;
			}
			else {
				checkShowCompleted.Visible=true;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				//Since the bonus information in FormInsRemain is currently only helpful in Canada,
				//we have decided not to show this button in other countries for now.
				butInsRem.Visible=true;
			}
			if(_listTreatPlans!=null && _listTreatPlans.Count==0) {//_listTreatPlans will be null when no patient is selected.
				textNote.Text="";
				HasNoteChanged=false;
			}
		}

		private delegate void ToolBarClick();

		private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)){
				//standard predefined button
				switch(e.Button.Tag.ToString()){
					case "PreAuth":
						ToolBarMainPreAuth_Click();
						break;
					case "Discount":
						ToolBarMainDiscount_Click();
						break;
					case "Update":
						ToolBarMainUpdate_Click();
						break;
					//case "Create":
					//	ToolBarMainCreate_Click();
					//	break;
					case "Print":
						//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
						//when it comes from a toolbar click.
						//https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
						ToolBarClick toolClick=ToolBarMainPrint_Click;
						this.BeginInvoke(toolClick);
						break;
					case "Email":
						ToolBarMainEmail_Click();
						break;
					case "Sign":
						ToolBarMainSign_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(Program)) {
				ProgramL.Execute(((Program)e.Button.Tag).Id,PatCur);
			}
			//Plugins.HookAddCode(this,"ContrTreat.ToolBarMain_ButtonClick_end",PatCur,e);
		}

		private void butNewTP_Click(object sender,EventArgs e) {
			FormTreatPlanCurEdit FormTPCE=new FormTreatPlanCurEdit();
			FormTPCE.TreatPlanCur=new TreatPlan() {
				Heading="Inactive Treatment Plan",
				Note=Preference.GetString(PreferenceName.TreatmentPlanNote),
				PatNum=PatCur.PatNum,
				TPStatus=TreatPlanStatus.Inactive,
			};
			FormTPCE.ShowDialog();
			if(FormTPCE.DialogResult!=DialogResult.OK) {
				return;
			}
			long tpNum=FormTPCE.TreatPlanCur.TreatPlanNum;
			ModuleSelected(PatCur.PatNum);//refreshes TPs
			for(int i=0;i<_listTreatPlans.Count;i++) {
				if(_listTreatPlans[i].TreatPlanNum==tpNum) {
					gridPlans.SetSelected(i,true);
				}
			}
			FillMain();
		}

		private void butSaveTP_Click(object sender,EventArgs e) {
			ToolBarMainCreate_Click();
		}

		private void FillPlans(bool doRefreshData=true){
			gridPlans.BeginUpdate();
			gridPlans.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableTPList","Date"),70);
			gridPlans.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTPList","Status"),50);
			gridPlans.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTPList","Heading"),230);
			gridPlans.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTPList","Signed"),76,HorizontalAlignment.Center);
			gridPlans.Columns.Add(col);
			gridPlans.Rows.Clear();
			if(PatCur==null){
				gridPlans.EndUpdate();
				return;
			}
			if(doRefreshData || ProcList==null) {
				ProcList=Procedures.Refresh(PatCur.PatNum);
			}
			ProcListTP=Procedures.SortListByTreatPlanPriority(ProcList.FindAll(x => x.ProcStatus==ProcStat.TP || x.ProcStatus==ProcStat.TPi)
				,Preferences.IsTreatPlanSortByTooth);//sorted by priority, then (conditionally) toothnum
			//_listTPCurrent=TreatPlans.Refresh(PatCur.PatNum,new[] {TreatPlanStatus.Active,TreatPlanStatus.Inactive});
			if(doRefreshData || _listTreatPlans==null) {
				_listTreatPlans=TreatPlans.GetAllForPat(PatCur.PatNum);
			}
			_listTreatPlans=_listTreatPlans
					.OrderBy(x => x.TPStatus!=TreatPlanStatus.Active)
					.ThenBy(x => x.TPStatus!=TreatPlanStatus.Inactive)
					.ThenBy(x => x.DateTP).ToList();
			if(doRefreshData || ProcTPList==null) {
				ProcTPList=ProcTPs.Refresh(PatCur.PatNum);
			}
			OpenDental.UI.ODGridRow row;
			//row=new ODGridRow();
			//row.Cells.Add("");//date empty
			//row.Cells.Add("");//date empty
			//row.Cells.Add(Lan.g(this,"Current Treatment Plans"));
			//gridPlans.Rows.Add(row);
			string str;
			Sheet sheetTP=null;
			if(DoPrintUsingSheets()) {
				sheetTP=SheetUtil.CreateSheet(SheetDefs.GetInternalOrCustom(SheetInternalType.TreatmentPlan),PatCur.PatNum);
			}
			bool hasPracticeSig=false;
			for(int i=0;i<_listTreatPlans.Count;i++){
				row=new ODGridRow();
				TreatPlan treatPlanCur=_listTreatPlans[i];
				row.Cells.Add(treatPlanCur.TPStatus==TreatPlanStatus.Saved? treatPlanCur.DateTP.ToShortDateString():"");
				row.Cells.Add(treatPlanCur.TPStatus.ToString());
				str=treatPlanCur.Heading;
				if(treatPlanCur.ResponsParty!=0){
					str+="\r\n"+Lan.g(this,"Responsible Party: ")+Patients.GetLim(treatPlanCur.ResponsParty).GetNameLF();
				}
				row.Cells.Add(str);
				hasPracticeSig=sheetTP?.SheetFields?.Any(x => x.FieldType==SheetFieldType.SigBoxPractice)??false;
				if(string.IsNullOrEmpty(treatPlanCur.Signature) || (hasPracticeSig && string.IsNullOrEmpty(treatPlanCur.SignaturePractice))) {
					row.Cells.Add("");
				}
				else{
					row.Cells.Add("X");
				}
				row.Tag=treatPlanCur;
				gridPlans.Rows.Add(row);
			}
			gridPlans.EndUpdate();
			gridPlans.SetSelected(0,true);
		}

		private void FillMain() {
			if((gridPlans.GetSelectedIndex()>=0 && _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="")//disable changing priorities for signed TPs
				|| PatCur==null ||_listTreatPlans.Count<1)//disable if the patient has no TPs
			{
				listSetPr.Enabled=false; 
			}
			else {
				listSetPr.Enabled=true;//allow changing priority for un-signed TPs
			}
			FillMainData();
			FillMainDisplay();
		}

		/// <summary>Fills RowsMain list for gridMain display.</summary>
		private void FillMainData() {
			decimal subfee=0;
			decimal suballowed=0;
			decimal subpriIns=0;
			decimal subsecIns=0;
			decimal subdiscount=0;
			decimal subpat=0;
			decimal subTaxEst=0;
			decimal totFee=0;
			decimal totPriIns=0;
			decimal totSecIns=0;
			decimal totDiscount=0;
			decimal totPat=0;
			decimal totAllowed=0;
			decimal totTaxEst=0;
			if(!checkShowDiscountNotAutomatic) {//default to false. Will be set below if there are any discounts to show. 
				checkShowDiscount.Checked=false;
			}
			RowsMain=new List<TpRow>();
			if(PatCur==null || gridPlans.Rows.Count==0) {
				return;
			}
			TpRow row;
			TreatPlan treatPlanTemp=_listTreatPlans[gridPlans.SelectedIndices[0]];
			//Active and Inactive Treatment Plans========================================================================
			if(treatPlanTemp.TPStatus==TreatPlanStatus.Active
				 || treatPlanTemp.TPStatus==TreatPlanStatus.Inactive)
			{
				LoadActiveTP(ref treatPlanTemp);
				return;
			}
			//Archived TPs below this point==============================================================================
			ProcTPSelectList=ProcTPs.GetListForTP(_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum,ProcTPList);
			bool isDone;
			for(int i=0;i<ProcTPSelectList.Length;i++) {
				row=new TpRow();
				isDone=false;
				if(ProcTPSelectList[i].ODTag == null) {
					ProcTPSelectList[i].ODTag=0L;
				}
				for(int j=0;j<ProcList.Count;j++) {
					if(ProcList[j].ProcNum==ProcTPSelectList[i].ProcNumOrig) {
						if((long)ProcTPSelectList[i].ODTag == 0) {
							ProcTPSelectList[i].ODTag = ProcList[j].ProcNumLab;
						}
						if(ProcList[j].ProcStatus==ProcStat.C) {
							isDone=true;
						}
					}
				}
				if(isDone) {
					row.Done="X";
				}
				row.ProcAbbr=ProcTPSelectList[i].ProcAbbr;
				row.Priority=Defs.GetName(DefinitionCategory.TxPriorities,ProcTPSelectList[i].Priority);
				row.Tth=ProcTPSelectList[i].ToothNumTP;
				row.Surf=ProcTPSelectList[i].Surf;
				row.Code=ProcTPSelectList[i].ProcCode;
				row.Description=ProcTPSelectList[i].Descript;
				row.Fee=(decimal)ProcTPSelectList[i].FeeAmt; //Fee
				row.PriIns=(decimal)ProcTPSelectList[i].PriInsAmt; //PriIns or DiscountPlan
				row.SecIns=(decimal)ProcTPSelectList[i].SecInsAmt; //SecIns
				row.FeeAllowed=(decimal)ProcTPSelectList[i].FeeAllowed;
				row.TaxEst=(decimal)ProcTPSelectList[i].TaxAmt; //Estimated Tax
				if(PatCur.DiscountPlanNum!=0) {
					if(!checkShowIns.Checked) {
						row.Fee=row.Fee-row.PriIns;
					}
				}
				//Totals
				subfee+=row.Fee;
				if(row.FeeAllowed.IsGreaterThan(-1)) {//-1 means the proc is DoNotBillIns
					suballowed+=row.FeeAllowed;
					totAllowed+=(decimal)ProcTPSelectList[i].FeeAllowed;
				}
				totFee+=row.Fee;
				subpriIns+=(decimal)ProcTPSelectList[i].PriInsAmt;
				totPriIns+=PatCur.DiscountPlanNum==0 ? (decimal)ProcTPSelectList[i].PriInsAmt : row.PriIns;
				subsecIns+=(decimal)ProcTPSelectList[i].SecInsAmt;
				totSecIns+=(decimal)ProcTPSelectList[i].SecInsAmt;
				row.Discount=(decimal)ProcTPSelectList[i].Discount; //Discount
				subdiscount+=(decimal)ProcTPSelectList[i].Discount;
				totDiscount+=(decimal)ProcTPSelectList[i].Discount;
				row.Pat=(decimal)ProcTPSelectList[i].PatAmt; //Pat
				subpat+=(decimal)ProcTPSelectList[i].PatAmt;
				totPat+=(decimal)ProcTPSelectList[i].PatAmt;
				subTaxEst+=(decimal)ProcTPSelectList[i].TaxAmt;
				totTaxEst+=(decimal)ProcTPSelectList[i].TaxAmt;
				row.Prognosis=ProcTPSelectList[i].Prognosis; //Prognosis
				row.Dx=ProcTPSelectList[i].Dx;
				row.ColorText=Defs.GetColor(DefinitionCategory.TxPriorities,ProcTPSelectList[i].Priority);
				if(row.ColorText==System.Drawing.Color.White) {
					row.ColorText=System.Drawing.Color.Black;
				}
				row.Tag=ProcTPSelectList[i].Copy();
				RowsMain.Add(row);
				if(!checkShowDiscountNotAutomatic && !checkShowDiscount.Checked && totDiscount>0) {
					checkShowDiscount.Checked=true;
				}
				if(checkShowSubtotals.Checked &&
					 (i==ProcTPSelectList.Length-1 || ProcTPSelectList[i+1].Priority!=ProcTPSelectList[i].Priority)) {
					row=new TpRow();
					row.Description=Lan.g("TableTP","Subtotal");
					row.Fee=subfee;
					row.PriIns=subpriIns;
					row.SecIns=subsecIns;
					row.Discount=subdiscount;
					row.Pat=subpat;
					row.FeeAllowed=suballowed;
					row.TaxEst=subTaxEst;
					row.ColorText=Defs.GetColor(DefinitionCategory.TxPriorities,ProcTPSelectList[i].Priority);
					if(row.ColorText==System.Drawing.Color.White) {
						row.ColorText=System.Drawing.Color.Black;
					}
					row.Bold=true;
					row.ColorLborder=System.Drawing.Color.Black;
					RowsMain.Add(row);
					subfee=0;
					subpriIns=0;
					subsecIns=0;
					subdiscount=0;
					subpat=0;
					suballowed=0;
					subTaxEst=0;
				}
			}
			textNote.Text=_listTreatPlans[gridPlans.SelectedIndices[0]].Note;
			HasNoteChanged=false;
			if((_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Saved 
				&& _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="") 
				|| (_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Inactive 
				&& _listTreatPlans[gridPlans.SelectedIndices[0]].Heading==Lan.g("TreatPlan","Unassigned")))
			{
				textNote.ReadOnly=true;
			}
			else {
				textNote.ReadOnly=false;
			}
			if(checkShowTotals.Checked) {
				row=new TpRow();
				row.Description=Lan.g("TableTP","Total");
				row.Fee=totFee;
				row.PriIns=totPriIns;
				row.SecIns=totSecIns;
				row.Discount=totDiscount;
				row.Pat=totPat;
				row.FeeAllowed=totAllowed;
				row.TaxEst=totTaxEst;
				row.Bold=true;
				row.ColorText=System.Drawing.Color.Black;
				RowsMain.Add(row);
			}
		}

		private void FillMainDisplay(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			List<DisplayField> fields=DisplayFields.GetForCategory(DisplayFieldCategory.TreatmentPlanModule);
			if(PatCur==null || gridPlans.Rows.Count==0 || _listTreatPlans[gridPlans.SelectedIndices[0]].TPType==TreatPlanType.Insurance) {
				fields.RemoveAll(x => x.InternalName=="DPlan");//If patient doesn't have discount plan, don't show column.
			}
			else {
				fields.RemoveAll(x => x.InternalName=="Pri Ins" || x.InternalName=="Sec Ins" || x.InternalName=="Allowed");//If patient does have discount plan, don't show Pri or Sec Ins or allowed fee.
			}
			bool hasSalesTax=HasSalesTax(fields);
			for(int i=0;i<fields.Count;i++){
				if(fields[i].Description==""){
					col=new ODGridColumn(fields[i].InternalName,fields[i].ColumnWidth);
				}
				else{
					col=new ODGridColumn(fields[i].Description,fields[i].ColumnWidth);
				}
				if(fields[i].InternalName=="Fee" && !checkShowFees.Checked){
					continue;
				}
				if((fields[i].InternalName=="Pri Ins" || fields[i].InternalName=="Sec Ins" || fields[i].InternalName=="DPlan" || fields[i].InternalName=="Allowed") 
					&& !checkShowIns.Checked){
					continue;
				}
				if(fields[i].InternalName=="Discount" && !checkShowDiscount.Checked){
					continue;
				}
				if(fields[i].InternalName=="Pat" && !checkShowIns.Checked && !checkShowDiscount.Checked && !hasSalesTax){
					continue;
				}
				if(fields[i].InternalName=="Tax Est" && !hasSalesTax) {
					continue;
				}
				if(fields[i].InternalName=="Fee" 
					|| fields[i].InternalName=="Pri Ins"
					|| fields[i].InternalName=="Sec Ins"
					|| fields[i].InternalName=="DPlan"
					|| fields[i].InternalName=="Discount"
					|| fields[i].InternalName=="Pat"
					|| fields[i].InternalName=="Allowed"
					|| fields[i].InternalName=="Tax Est") 
				{
					col.TextAlign=HorizontalAlignment.Right;
				}
				if(fields[i].InternalName=="Sub") {
					col.TextAlign=HorizontalAlignment.Center;
				}
				gridMain.Columns.Add(col);
			}			
			gridMain.Rows.Clear();
			if(PatCur==null){
				gridMain.EndUpdate();
				return;
			}
			if(RowsMain==null || RowsMain.Count==0) {
				gridMain.EndUpdate();
				return;
			}
			ODGridRow row;
			for(int i=0;i<RowsMain.Count;i++){
				row=new ODGridRow();
				for(int j=0;j<fields.Count;j++) {
					switch(fields[j].InternalName) {
						case "Done":
							if(RowsMain[i].Done!=null) {
								row.Cells.Add(RowsMain[i].Done.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Priority":
							if(RowsMain[i].Priority!=null) {
								row.Cells.Add(RowsMain[i].Priority.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Tth":
							if(RowsMain[i].Tth!=null) {
								row.Cells.Add(RowsMain[i].Tth.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Surf":
							if(RowsMain[i].Surf!=null) {
								row.Cells.Add(RowsMain[i].Surf.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Code":
							if(RowsMain[i].Code!=null) {
								row.Cells.Add(RowsMain[i].Code.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Sub":
							if(HasSubstCodeForTpRow(RowsMain[i])) {
								row.Cells.Add("X");//They allow substitutions.
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Description":
							if(RowsMain[i].Description!=null) {
								string description=RowsMain[i].Description.ToString();
								if(ProcedureCodes.GetProcCode(RowsMain[i].Code).IsCanadianLab) {
									description="^ ^ "+description;
								}
								row.Cells.Add(description);
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Fee":
							if(checkShowFees.Checked) {
								row.Cells.Add(RowsMain[i].Fee.ToString("F"));
							}
							break;
						case "Pri Ins":
						case "DPlan":
							if(checkShowIns.Checked) {
								row.Cells.Add(RowsMain[i].PriIns.ToString("F"));
							}
							break;
						case "Sec Ins":
							if(checkShowIns.Checked) {
								row.Cells.Add(RowsMain[i].SecIns.ToString("F"));
							}
							break;
						case "Discount":
							if(checkShowDiscount.Checked) {
								row.Cells.Add(RowsMain[i].Discount.ToString("F"));
							}
							break;
						case "Pat":
							if(checkShowIns.Checked || checkShowDiscount.Checked || hasSalesTax) {
								row.Cells.Add(RowsMain[i].Pat.ToString("F"));
							}
							break;
						case "Prognosis":
							if(RowsMain[i].Prognosis!=null) {
								row.Cells.Add(RowsMain[i].Prognosis.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Dx":
							if(RowsMain[i].Dx!=null) {
								row.Cells.Add(RowsMain[i].Dx.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Abbr":
							if(!String.IsNullOrEmpty(RowsMain[i].ProcAbbr)){
								row.Cells.Add(RowsMain[i].ProcAbbr.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Allowed":
							if(checkShowIns.Checked) {
								if(RowsMain[i].FeeAllowed.IsGreaterThan(-1)) {//-1 means the proc is DoNotBillIns
									row.Cells.Add(RowsMain[i].FeeAllowed.ToString("F"));
								}
								else {
									row.Cells.Add("X");
								}
							}
							break;
						case "Tax Est":
							if(hasSalesTax) {
								row.Cells.Add(RowsMain[i].TaxEst.ToString("F"));
							}
							break;
					}
				}
				if(RowsMain[i].ColorText!=null) {
					row.ColorText=RowsMain[i].ColorText;
				}
				if(RowsMain[i].ColorLborder!=null) {
					row.ColorLborder=RowsMain[i].ColorLborder;
				}
				if(RowsMain[i].Tag!=null) {
					row.Tag=RowsMain[i].Tag;
				}
				row.Bold=RowsMain[i].Bold;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FillGridPrint() {
			this.Controls.Add(gridPrint);
			gridPrint.BeginUpdate();
			gridPrint.Columns.Clear();
			ODGridColumn col;
			DisplayFields.RefreshCache();//probably needs to be removed.
			List<DisplayField> fields=DisplayFields.GetForCategory(DisplayFieldCategory.TreatmentPlanModule);
			if(PatCur==null || gridPlans.Rows.Count==0 || _listTreatPlans[gridPlans.SelectedIndices[0]].TPType==TreatPlanType.Insurance) {
				fields.RemoveAll(x => x.InternalName=="DPlan");//If patient doesn't have discount plan, don't show column.
			}
			else {
				fields.RemoveAll(x => x.InternalName=="Pri Ins" || x.InternalName=="Sec Ins");//If patient does have discount plan, don't show Pri or Sec Ins.
			}
			bool hasSalesTax=HasSalesTax(fields);
			for(int i=0;i<fields.Count;i++) {
				if(fields[i].Description=="") {
					col=new ODGridColumn(fields[i].InternalName,fields[i].ColumnWidth);
				}
				else {
					col=new ODGridColumn(fields[i].Description,fields[i].ColumnWidth);
				}
				if(fields[i].InternalName=="Fee" && !checkShowFees.Checked) {
					continue;
				}
				if((fields[i].InternalName.In("Pri Ins","Sec Ins","DPlan","Allowed")) && !checkShowIns.Checked) {
					continue;
				}
				if(fields[i].InternalName=="Discount" && !checkShowDiscount.Checked) {
					continue;
				}
				if(fields[i].InternalName=="Pat" && !checkShowIns.Checked && !checkShowDiscount.Checked && !hasSalesTax) {
					continue;
				}
				if(fields[i].InternalName=="Tax Est" && !hasSalesTax) {
					continue;
				}
				if(fields[i].InternalName=="Fee" 
					|| fields[i].InternalName=="Pri Ins"
					|| fields[i].InternalName=="Sec Ins"
					|| fields[i].InternalName=="DPlan"
					|| fields[i].InternalName=="Discount"
					|| fields[i].InternalName=="Pat"
					|| fields[i].InternalName=="Tax Est")
				{
					col.TextAlign=HorizontalAlignment.Right;
				}
				if(fields[i].InternalName=="Sub") {
					col.TextAlign=HorizontalAlignment.Center;
				}
				gridPrint.Columns.Add(col);
			}
			gridPrint.Rows.Clear();
			if(PatCur==null) {
				gridPrint.EndUpdate();
				return;
			}
			ODGridRow row;
			for(int i=0;i<RowsMain.Count;i++) {
				row=new ODGridRow();
				for(int j=0;j<fields.Count;j++) {
					switch(fields[j].InternalName) {
						case "Done":
							if(RowsMain[i].Done!=null) {
								row.Cells.Add(RowsMain[i].Done.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Priority":
							if(RowsMain[i].Priority!=null) {
								row.Cells.Add(RowsMain[i].Priority.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Tth":
							if(RowsMain[i].Tth!=null) {
								row.Cells.Add(RowsMain[i].Tth.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Surf":
							if(RowsMain[i].Surf!=null) {
								row.Cells.Add(RowsMain[i].Surf.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Code":
							if(RowsMain[i].Code!=null) {
								row.Cells.Add(RowsMain[i].Code.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Sub":
							if(HasSubstCodeForTpRow(RowsMain[i])) {
								row.Cells.Add("X");//They allow substitutions.
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Description":
							if(RowsMain[i].Description!=null) {
								row.Cells.Add(RowsMain[i].Description.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Fee":
							if(checkShowFees.Checked) {
								if(Preference.GetBool(PreferenceName.TreatPlanItemized) || RowsMain[i].Description.ToString()==Lan.g("TableTP","Total")
									|| RowsMain[i].Description.ToString()==Lan.g("TableTP","Subtotal")) 
								{
									row.Cells.Add(RowsMain[i].Fee.ToString("F"));
								}
								else {
									row.Cells.Add("");
								}
							}
							break;
						case "Pri Ins":
						case "DPlan":
							if(checkShowIns.Checked) {
								if(Preference.GetBool(PreferenceName.TreatPlanItemized) || RowsMain[i].Description.ToString()==Lan.g("TableTP","Total")
									|| RowsMain[i].Description.ToString()==Lan.g("TableTP","Subtotal")) 
								{
									row.Cells.Add(RowsMain[i].PriIns.ToString("F"));
								}
								else {
									row.Cells.Add("");
								}
							}
							break;
						case "Sec Ins":
							if(checkShowIns.Checked) {
								if(Preference.GetBool(PreferenceName.TreatPlanItemized) || RowsMain[i].Description.ToString()==Lan.g("TableTP","Total")
									|| RowsMain[i].Description.ToString()==Lan.g("TableTP","Subtotal")) 
								{
									row.Cells.Add(RowsMain[i].SecIns.ToString("F"));
								}
								else {
									row.Cells.Add("");
								}
							}
							break;
						case "Discount":
							if(checkShowDiscount.Checked) {
								if(Preference.GetBool(PreferenceName.TreatPlanItemized) || RowsMain[i].Description.ToString()==Lan.g("TableTP","Total")
									|| RowsMain[i].Description.ToString()==Lan.g("TableTP","Subtotal"))
								{
									row.Cells.Add(RowsMain[i].Discount.ToString("F"));
								}
								else {
									row.Cells.Add("");
								}
							}
							break;
						case "Pat":
							if(checkShowIns.Checked || checkShowDiscount.Checked || hasSalesTax) {
								if(Preference.GetBool(PreferenceName.TreatPlanItemized) || RowsMain[i].Description.ToString()==Lan.g("TableTP","Total")
									|| RowsMain[i].Description.ToString()==Lan.g("TableTP","Subtotal")) 
								{
									row.Cells.Add(RowsMain[i].Pat.ToString("F"));
								}
								else {
									row.Cells.Add("");
								}
							}
							break;
						case "Prognosis":
							if(RowsMain[i].Prognosis!=null) {
								row.Cells.Add(RowsMain[i].Prognosis.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Dx":
							if(RowsMain[i].Dx!=null) {
								row.Cells.Add(RowsMain[i].Dx.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Abbr":
							if(!String.IsNullOrEmpty(RowsMain[i].ProcAbbr)){
								row.Cells.Add(RowsMain[i].ProcAbbr.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Tax Est":
							if(hasSalesTax) {
								row.Cells.Add(RowsMain[i].TaxEst.ToString("F"));
							}
							break;
					}
				}
				if(RowsMain[i].ColorText!=null) {
					row.ColorText=RowsMain[i].ColorText;
				}
				if(RowsMain[i].ColorLborder!=null) {
					row.ColorLborder=RowsMain[i].ColorLborder;
				}
				if(RowsMain[i].Tag!=null) {
					row.Tag=RowsMain[i].Tag;
				}
				row.Bold=RowsMain[i].Bold;
				gridPrint.Rows.Add(row);
			}
			gridPrint.EndUpdate();
		}

		private bool HasSubstCodeForTpRow(TpRow row) {
			//If any patient insplan allows subst codes (if !plan.CodeSubstNone) and the code has a valid substitution code, then indicate the substitution.
			ProcedureCode procCode=ProcedureCodes.GetProcCode(row.Code);
			if(!ProcedureCodes.IsValidCode(procCode.ProcCode)) {
				//TpRow is not a valid procedure. Return false.
				return false;
			}
			//The lists gotten at the beginning of ContrTreat are not patient specific with the exception of the PatPlanList.
			//Get all patient-specific InsSubs
			List<InsSub> listPatInsSubs=SubList.FindAll(x => PatPlanList.Any(y => y.InsSubNum==x.InsSubNum));
			//Get all patient-specific InsPlans
			List<InsPlan> listPatInsPlans=InsPlanList.FindAll(x => listPatInsSubs.Any(y => y.PlanNum==x.PlanNum));
			return SubstitutionLinks.HasSubstCodeForProcCode(procCode,row.Tth.ToString(),_listSubstLinks,listPatInsPlans);
		}

		public static bool HasSalesTax(long patNum, List<DisplayField> fields) {
			return AvaTax.IsTaxable(patNum) && fields.Any(x => x.InternalName=="Tax Est");
		}

		private bool HasSalesTax(List<DisplayField> fields) {
			return PatCur!=null && HasSalesTax(PatCur.PatNum, fields);
		}

		private void FillSummary(){
			userControlFamIns.RefreshInsurance(PatCur,InsPlanList,SubList,PatPlanList,BenefitList);
			userControlIndIns.RefreshInsurance(PatCur,InsPlanList,SubList,PatPlanList,BenefitList,HistList);
		}		

    private void FillPreAuth(){
			gridPreAuth.BeginUpdate();
			gridPreAuth.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TablePreAuth","Date Sent"),80);
			gridPreAuth.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePreAuth","Carrier"),100);
			gridPreAuth.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePreAuth","Status"),53);
			gridPreAuth.Columns.Add(col);
			gridPreAuth.Rows.Clear();
      if(PatCur==null){
				gridPreAuth.EndUpdate();
				return;
			}
      ALPreAuth=new ArrayList();			
      for(int i=0;i<ClaimList.Count;i++){
        if(ClaimList[i].ClaimType=="PreAuth"){
          ALPreAuth.Add(ClaimList[i]);
        }
      }
			OpenDental.UI.ODGridRow row;
      for(int i=0;i<ALPreAuth.Count;i++){
				InsPlan PlanCur=InsPlans.GetPlan(((Claim)ALPreAuth[i]).PlanNum,InsPlanList);
				row=new ODGridRow();
				if(((Claim)ALPreAuth[i]).DateSent.Year<1880){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(((Claim)ALPreAuth[i]).DateSent.ToShortDateString());
				}
				row.Cells.Add(Carriers.GetName(PlanCur.CarrierNum));
				row.Cells.Add(((Claim)ALPreAuth[i]).ClaimStatus.ToString());
				gridPreAuth.Rows.Add(row);
      }
			gridPreAuth.EndUpdate();
    }

		private void gridMain_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			gridPreAuth.SetSelected(false);//is this a desirable behavior?
			if(gridMain.Rows[e.Row].Tag==null) {
				return;//skip any hightlighted subtotal lines
			}
			CanadianSelectedRowHelper(((ProcTP)gridMain.Rows[e.Row].Tag));
		}

		///<summary>Selects any associated lab procedures for the given selectedProcTp in gridMain.</summary>
		private void CanadianSelectedRowHelper(ProcTP selectedProcTp) {
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				return;
			}
			long selectedProcNumLab=(long)selectedProcTp.ODTag;//0 or FK to parent proc
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if(gridMain.Rows[i].Tag==null){
					continue;//skip any hightlighted subtotal lines
				}
				long rowProcNumOrig=((ProcTP)gridMain.Rows[i].Tag).ProcNumOrig;
				long rowParentProcNum=(long)((ProcTP)gridMain.Rows[i].Tag).ODTag;//0 or FK to parent proc
				if(rowProcNumOrig==selectedProcNumLab //User clicked lab, select parent proc too.
					|| (rowParentProcNum!=0 && rowParentProcNum==selectedProcNumLab)//User clicked lab, select other labs associated to same parent proc.
					|| (selectedProcTp.ProcNumOrig==rowParentProcNum))//User clicked parent, select associated lab procs.
				{
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void gridMain_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(gridMain.Rows[e.Row].Tag==null){
				return;//user double clicked on a subtotal row
			}
			if(gridPlans.GetSelectedIndex()>-1 
				&& (_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Active 
					||_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Inactive))
			{//current plan
				Procedure ProcCur=Procedures.GetOneProc(((ProcTP)gridMain.Rows[e.Row].Tag).ProcNumOrig,true); 
				//generate a new loop list containing only the procs before this one in it
				LoopList=new List<ClaimProcHist>();
				for(int i=0;i<ProcListTP.Length;i++) {
					if(ProcListTP[i].ProcNum==ProcCur.ProcNum) {
						break;
					}
					LoopList.AddRange(ClaimProcs.GetHistForProc(ClaimProcList,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
				}
				FormProcEdit FormPE=new FormProcEdit(ProcCur,PatCur,FamCur);
				FormPE.LoopList=LoopList;
				FormPE.HistList=HistList;
				FormPE.ShowDialog();
				long treatPlanNum=_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum;
				ModuleSelected(PatCur.PatNum);
				gridPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(x=>x.TreatPlanNum==treatPlanNum)),true);
				//This only updates the grid of procedures, in case any changes were made.
				FillMain();
				for(int i=0;i<gridMain.Rows.Count;i++){
					if(gridMain.Rows[i].Tag !=null && ((ProcTP)gridMain.Rows[i].Tag).ProcNumOrig==ProcCur.ProcNum){
						gridMain.SetSelected(i,true);
					}
				}
				return;
			}
			//any other TP
			ProcTP procT=(ProcTP)gridMain.Rows[e.Row].Tag;
			DateTime dateTP=_listTreatPlans[gridPlans.SelectedIndices[0]].DateTP;
			bool isSigned=false;
			if(_listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="") {
				isSigned=true;
			}
			FormProcTPEdit FormP=new FormProcTPEdit(procT,dateTP,isSigned);
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.Cancel){
				return;
			}
			int selectedPlanI=gridPlans.SelectedIndices[0];
			long selectedProc=procT.ProcTPNum;
			ModuleSelected(PatCur.PatNum);
			gridPlans.SetSelected(selectedPlanI,true);
			FillMain();
			for(int i=0;i<gridMain.Rows.Count;i++){
				if(gridMain.Rows[i].Tag !=null && ((ProcTP)gridMain.Rows[i].Tag).ProcTPNum==selectedProc){ 
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void gridPlans_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			FillMain();
			gridPreAuth.SetSelected(false);
			if(gridPlans.SelectedIndices.Length > 0) {
				TreatPlan treatPlan=_listTreatPlans[gridPlans.SelectedIndices[0]];
				if(treatPlan.TPStatus==TreatPlanStatus.Saved && treatPlan.DateTP < UpdateHistories.GetDateForVersion(new Version(17,1,0,0))) {
					//In 17.1 we forced everyone to switch to using sheets for TPs. In order to avoid making it appear that historical data has changed,
					//we give the option to print using the classic view for treatment plans that were saved before updating to 17.1.
					if(!tabShowSort.TabPages.Contains(tabPagePrint)) {
						tabShowSort.TabPages.Add(tabPagePrint);
					}
				}
				else {
					if(tabShowSort.TabPages.Contains(tabPagePrint)) {
						tabShowSort.TabPages.Remove(tabPagePrint);
					}
				}
			}
		}

		private void gridPlans_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			//if(e.Row==0){
			//	return;//there is nothing to edit if user clicks on current.
			//}
			long tpNum=_listTreatPlans[e.Row].TreatPlanNum;
			TreatPlan tpSelected=_listTreatPlans[e.Row];
			if(tpSelected.TPStatus==TreatPlanStatus.Saved) {
				FormTreatPlanEdit FormT=new FormTreatPlanEdit(_listTreatPlans[e.Row]);
				FormT.ShowDialog();
			}
			else {
				FormTreatPlanCurEdit FormTPC=new FormTreatPlanCurEdit();
				FormTPC.TreatPlanCur=tpSelected;
				FormTPC.ShowDialog();
			}
			ModuleSelected(PatCur.PatNum);
			for(int i=0;i<_listTreatPlans.Count;i++){
				if(_listTreatPlans[i].TreatPlanNum==tpNum){
					gridPlans.SetSelected(i,true);
				}
			}
			FillMain();
		}

		private void listSetPr_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			int clickedRow=listSetPr.IndexFromPoint(e.X,e.Y);
			if(!clickedRow.Between(0,listSetPr.Items.Count-1)) {
				return;
			}
			Definition selectedPriority=((ODBoxItem<Definition>)listSetPr.Items[clickedRow]).Tag;
			if(selectedPriority==null) {
				return;
			}
			TreatPlan selectedTp=gridPlans.SelectedIndices.Where(x => x>-1 && x<gridPlans.Rows.Count)
				.Select(x => (TreatPlan)gridPlans.Rows[x].Tag).FirstOrDefault();
			if(selectedTp==null) {
				return;
			}
			SetPriority(selectedPriority,selectedTp,gridMain.SelectedTags<ProcTP>());
		}

		///<summary>Sets the priorities for the selected ProcTP.</summary>
		private void SetPriority(Definition selectedPriority,TreatPlan selectedTp,List<ProcTP> listSelectedProcTps) {
			if(_listTreatPlans.Count>0
				 && (selectedTp.TPStatus==TreatPlanStatus.Active || selectedTp.TPStatus==TreatPlanStatus.Inactive)) 
			{
				List<TreatPlanAttach> listTreatPlanAttaches=TreatPlanAttaches.GetAllForTreatPlan(selectedTp.TreatPlanNum);
				foreach(ProcTP procSelected in listSelectedProcTps) {
					if(procSelected==null) {
						continue;
					}
					TreatPlanAttach tpa=listTreatPlanAttaches.FirstOrDefault(x => x.ProcNum==procSelected.ProcNumOrig);
					if(tpa==null) {
						continue;
					}
					tpa.Priority=selectedPriority.Id;
					TreatPlanAttaches.Update(tpa);
				}
				ModuleSelected(PatCur.PatNum);
				gridPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(x => x.TreatPlanNum==selectedTp.TreatPlanNum)),true);
				FillMain();
				for(int i=0;i<gridMain.Rows.Count;i++) {
					if(gridMain.Rows[i].Tag!=null 
						&& listSelectedProcTps.Where(x => x!=null).Select(x => x.ProcNumOrig).ToList().Contains(((ProcTP)gridMain.Rows[i].Tag).ProcNumOrig)) 
					{
						gridMain.SetSelected(i,true);
					}
				}
			}
			else { //any Saved TP
				DateTime dateTP=selectedTp.DateTP;
				if(!Security.IsAuthorized(Permissions.TreatPlanEdit,dateTP)) {
					return;
				}
				foreach(ProcTP procSelected in listSelectedProcTps) {
					if(procSelected==null) {
						//user must have highlighted a subtotal row, so ignore
						continue;
					}
					procSelected.Priority=selectedPriority.Id;
					ProcTPs.InsertOrUpdate(procSelected,false);
				}
				ModuleSelected(PatCur.PatNum);
				gridPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(x => x.TreatPlanNum==selectedTp.TreatPlanNum)),true);
				FillMain();
			}
		}

		private void checkShowMaxDed_Click(object sender,EventArgs e) {
			FillMain();
		}

		private void checkShowFees_Click(object sender,EventArgs e) {
			if(checkShowFees.Checked){
				//checkShowStandard.Checked=true;
				if(!checkShowInsNotAutomatic){
					checkShowIns.Checked=true;
				}
				if(!checkShowDiscountNotAutomatic) {
					checkShowDiscount.Checked=true;
				}
				checkShowSubtotals.Checked=true;
				checkShowTotals.Checked=true;
			}
			else{
				//checkShowStandard.Checked=false;
				if(!checkShowInsNotAutomatic){
					checkShowIns.Checked=false;
				}
				if(!checkShowDiscountNotAutomatic) {
					checkShowDiscount.Checked=false;
				}
				checkShowSubtotals.Checked=false;
				checkShowTotals.Checked=false;
			}
			FillMain();
		}

		private void checkShowStandard_Click(object sender,EventArgs e) {
			FillMain();
		}

		private void checkShowIns_Click(object sender,EventArgs e) {
			if(!checkShowIns.Checked) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Turn off automatic checking of this box for the rest of this session?")) {
					checkShowInsNotAutomatic=true;
				}
			}
			FillMain();
		}

		private void checkShowDiscount_Click(object sender,EventArgs e) {
			if(!checkShowDiscount.Checked && !checkShowDiscountNotAutomatic) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Turn off automatic checking of this box for the rest of this session?")) {
					checkShowDiscountNotAutomatic=true;
				}
			}
			FillMain();
		}

		private void checkShowSubtotals_Click(object sender,EventArgs e) {
			FillMain();
		}

		private void checkShowTotals_Click(object sender,EventArgs e) {
			FillMain();
		}

		private void ToolBarMainPrint_Click() {
			if(gridPlans.SelectedIndices.Length < 1) {
				MsgBox.Show(this,"Select a Treatment Plan to print.");
				return;
			}
			#region FuchsOptionOn
			if(Preference.GetBool(PreferenceName.FuchsOptionsOn)) {
				if(checkShowDiscount.Checked || checkShowIns.Checked) {
					if(MessageBox.Show(this,string.Format(Lan.g(this,"Do you want to remove insurance estimates and discounts from printed treatment plan?")),"Open Dental",MessageBoxButtons.YesNo,MessageBoxIcon.Question) != DialogResult.No) {
						checkShowDiscount.Checked=false;
						checkShowIns.Checked=false;
						FillMain();
					}
				}
			}
			#endregion
			if(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Saved
				&& Preference.GetBool(PreferenceName.TreatPlanSaveSignedToPdf)
			  && _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!=""
			  && Documents.DocExists(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum)) 
			{
				//Open PDF and allow user to print from pdf software.
				Cursor=Cursors.WaitCursor;
				Documents.OpenDoc(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum);
				Cursor=Cursors.Default;
				return;
			}
			Sheet sheetTP=null;
			TreatPlan treatPlan;
			if(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Saved) {
				treatPlan=_listTreatPlans[gridPlans.SelectedIndices[0]].Copy();
				treatPlan.ListProcTPs=ProcTPs.RefreshForTP(treatPlan.TreatPlanNum);
			}
			else {
				treatPlan=_listTreatPlans[gridPlans.SelectedIndices[0]];
				LoadActiveTP(ref treatPlan);
			}
			if(DoPrintUsingSheets()) {
				sheetTP=TreatPlanToSheet(treatPlan);
				SheetPrinting.Print(sheetTP);
			}
			else { //clasic TPs
				PrepImageForPrinting();
				MigraDoc.DocumentObjectModel.Document doc=CreateDocument();
				MigraDoc.Rendering.Printing.MigraDocPrintDocument printdoc=new MigraDoc.Rendering.Printing.MigraDocPrintDocument();
				MigraDoc.Rendering.DocumentRenderer renderer=new MigraDoc.Rendering.DocumentRenderer(doc);
				renderer.PrepareDocument();
				printdoc.Renderer=renderer;
				//we might want to surround some of this with a try-catch
				//TODO: Implement ODprintout pattern - MigraDoc
#if DEBUG
				pView=new FormRpPrintPreview(printdoc);
				pView.ShowDialog();
#else
				if(PrinterL.SetPrinter(pd2,PrintSituation.TPPerio,PatCur.PatNum,"Treatment plan for printed")){
					printdoc.PrinterSettings=pd2.PrinterSettings;
					printdoc.Print();
				}
#endif
			}
			SaveTPAsDocument(false,sheetTP);
		}

		private void ToolBarMainEmail_Click() {
			if(!Security.IsAuthorized(Permissions.EmailSend)) {
				return;
			}
			#region FuchsOptionOn
			if(Preference.GetBool(PreferenceName.FuchsOptionsOn)) {
				if(checkShowDiscount.Checked || checkShowIns.Checked) {
					if(MessageBox.Show(this,string.Format(Lan.g(this,"Do you want to remove insurance estimates and discounts from e-mailed treatment plan?")),"Open Dental",MessageBoxButtons.YesNo,MessageBoxIcon.Question) != DialogResult.No) {
						checkShowDiscount.Checked=false;
						checkShowIns.Checked=false;
						FillMain();
					}
				}
			}
			#endregion
			PrepImageForPrinting();
			string attachPath=EmailAttachment.GetAttachmentPath();
			Random rnd=new Random();
			string fileName=DateTime.Now.ToString("yyyyMMdd")+"_"+DateTime.Now.TimeOfDay.Ticks.ToString()+rnd.Next(1000).ToString()+".pdf";
			string filePathAndName= Storage.Default.CombinePath(attachPath,fileName);
			if(gridPlans.SelectedIndices[0]>0 //not the default plan.
				&& Preference.GetBool(PreferenceName.TreatPlanSaveSignedToPdf) //preference enabled
			  && _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="" //and document is signed
			  && Documents.DocExists(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum)) //and file exists
			{
				string filePathAndNameTemp=Documents.GetPath(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum);
                //copy file to email attach folder so files will be where they are exptected to be.
                Storage.Default.DeleteFile(filePathAndName);
                Storage.Default.CopyFile(filePathAndNameTemp,filePathAndName);
			}
			else if(DoPrintUsingSheets())	{
				TreatPlan treatPlan;
				if(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Saved) {
					treatPlan=_listTreatPlans[gridPlans.SelectedIndices[0]].Copy();
					treatPlan.ListProcTPs=ProcTPs.RefreshForTP(treatPlan.TreatPlanNum);
				}
				else {
					treatPlan=_listTreatPlans[gridPlans.SelectedIndices[0]];
					LoadActiveTP(ref treatPlan);
				}
				Sheet sheetTP=TreatPlanToSheet(treatPlan);
				SheetPrinting.CreatePdf(sheetTP,filePathAndName,null);
			}
			else{//generate and save a new document from scratch
				MigraDoc.Rendering.PdfDocumentRenderer pdfRenderer=new MigraDoc.Rendering.PdfDocumentRenderer(true,PdfFontEmbedding.Always);
				pdfRenderer.Document=CreateDocument();
				pdfRenderer.RenderDocument();
				pdfRenderer.PdfDocument.Save(filePathAndName);
			}
			//Process.Start(filePathAndName);
			//if(CloudStorage.IsCloudStorage) {
			//	FileAtoZ.Copy(filePathAndName,FileAtoZ.CombinePaths(attachPath,fileName),FileAtoZSourceDestination.LocalToAtoZ);
			//}
			EmailMessage message=new EmailMessage();
			message.PatientId=PatCur.PatNum;
			message.ToAddress=PatCur.Email;
			EmailAddress address=EmailAddress.GetByClinic(PatCur.ClinicNum);
			message.FromAddress=address.GetFrom();
			message.Subject=Lan.g(this,"Treatment Plan");
			EmailAttachment attach=new EmailAttachment();
			attach.Description="TreatmentPlan.pdf";
			attach.FileName=fileName;
			message.Attachments.Add(attach);
			FormEmailMessageEdit FormE=new FormEmailMessageEdit(message,address);
			FormE.ShowDialog();
			//if(FormE.DialogResult==DialogResult.OK) {
			//	RefreshCurrentModule();
			//}
		}

		private void PrepImageForPrinting(){
			//linesPrinted=0;
			//ColTotal = new double[10];
			//headingPrinted=false;
			//graphicsPrinted=false;
			//mainPrinted=false;
			//benefitsPrinted=false;
			//notePrinted=false;
			//pagesPrinted=0;
			if(Clinic.GetById(Clinics.ClinicId).IsMedicalOnly)
			{
				return;
			}
			List<Definition> listDefs=Definition.GetByCategory(DefinitionCategory.ChartGraphicColors);;
			//prints the graphical tooth chart and legend
			//Panel panelHide=new Panel();
			//panelHide.Size=new Size(600,500);
			//panelHide.BackColor=this.BackColor;
			//panelHide.SendToBack();
			//this.Controls.Add(panelHide);
			toothChart=new ToothChartWrapper();
			toothChart.ColorBackground=listDefs[14].Color;
			toothChart.ColorText=listDefs[15].Color;
			//toothChart.TaoRenderEnabled=true;
			//toothChart.TaoInitializeContexts();
			toothChart.Size=new Size(500,370);
			//toothChart.Location=new Point(-600,-500);//off the visible screen
			//toothChart.SendToBack();
			//ComputerPref computerPref=ComputerPrefs.GetForLocalComputer();
			toothChart.UseHardware=ComputerPrefs.LocalComputer.GraphicsUseHardware;
			toothChart.SetToothNumberingNomenclature((ToothNumberingNomenclature)Preference.GetInt(PreferenceName.UseInternationalToothNumbers));
			toothChart.PreferredPixelFormatNumber=ComputerPrefs.LocalComputer.PreferredPixelFormatNum;
			toothChart.DeviceFormat=new ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);
			//Must be last setting set for preferences, because
																													//this is the line where the device pixel format is
																													//recreated.
																													//The preferred pixel format number changes to the selected pixel format number after a context is chosen.
			toothChart.DrawMode=ComputerPrefs.LocalComputer.GraphicsSimple;
			ComputerPrefs.LocalComputer.PreferredPixelFormatNum=toothChart.PreferredPixelFormatNumber;
			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			this.Controls.Add(toothChart);
			toothChart.BringToFront();
			toothChart.ResetTeeth();
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			//first, primary.  That way, you can still set a primary tooth missing afterwards.
			for(int i=0;i<ToothInitialList.Count;i++) {
				if(ToothInitialList[i].InitialType==ToothInitialType.Primary) {
					toothChart.SetPrimary(ToothInitialList[i].ToothNum);
				}
			}
			for(int i=0;i<ToothInitialList.Count;i++) {
				switch(ToothInitialList[i].InitialType) {
					case ToothInitialType.Missing:
						toothChart.SetMissing(ToothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Hidden:
						toothChart.SetHidden(ToothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Rotate:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,ToothInitialList[i].Movement,0,0,0,0,0);
						break;
					case ToothInitialType.TipM:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,ToothInitialList[i].Movement,0,0,0,0);
						break;
					case ToothInitialType.TipB:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,ToothInitialList[i].Movement,0,0,0);
						break;
					case ToothInitialType.ShiftM:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,ToothInitialList[i].Movement,0,0);
						break;
					case ToothInitialType.ShiftO:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,ToothInitialList[i].Movement,0);
						break;
					case ToothInitialType.ShiftB:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,0,ToothInitialList[i].Movement);
						break;
					case ToothInitialType.Drawing:
						toothChart.AddDrawingSegment(ToothInitialList[i].Copy());
						break;
				}
			}
			ComputeProcListFiltered();
			DrawProcsGraphics();
			toothChart.AutoFinish=true;
			chartBitmap=toothChart.GetBitmap();
			toothChart.Dispose();
		}

		///<summary>Gets the active treatment plan as a list of ProcTP.  
		///Uses the static variable 'PrefC.IsTreatPlanSortByTooth' to determine if procedures should be sorted by tooth order.</summary>
		private List<ProcTP> LoadActiveTP(ref TreatPlan treatPlan) {
			LoadActiveTPData loadActiveData=TreatmentPlanModules.GetLoadActiveTpData(PatCur,treatPlan.TreatPlanNum,BenefitList,PatPlanList,
				InsPlanList,dateTimeTP.Value,SubList,Preference.GetBool(PreferenceName.InsChecksFrequency),Preferences.IsTreatPlanSortByTooth,_listSubstLinks);
			List<TreatPlanAttach> listTreatPlanAttaches=loadActiveData.ListTreatPlanAttaches;
			List<Procedure> listProcForTP=loadActiveData.listProcForTP;
			Lookup<FeeKey2,Fee> lookupFees=null;
			if(loadActiveData.ListFees!=null){
				lookupFees=(Lookup<FeeKey2,Fee>)loadActiveData.ListFees.ToLookup(x => new FeeKey2(x.CodeNum,x.FeeSched));
			}
			InsPlan priPlanCur=null;
			if(PatPlanList.Count>0) { //primary
				InsSub priSubCur=InsSubs.GetSub(PatPlanList[0].InsSubNum,SubList);
				priPlanCur=InsPlans.GetPlan(priSubCur.PlanNum,InsPlanList);
			}
			InsPlan secPlanCur=null;
			if(PatPlanList.Count>1) { //secondary
				InsSub secSubCur=InsSubs.GetSub(PatPlanList[1].InsSubNum,SubList);
				secPlanCur=InsPlans.GetPlan(secSubCur.PlanNum,InsPlanList);
			}
			ClaimProcList=loadActiveData.ClaimProcList;
			List<ClaimProc> claimProcListOld=ClaimProcList.Select(x => x.Copy()).ToList();
			LoopList=new List<ClaimProcHist>();
			//foreach(Procedure tpProc in listProcForTP){
			if(Preference.GetBool(PreferenceName.InsChecksFrequency)) {
				//Taking into account insurance frequency, use the date picker date when loading or when the Refresh button is pressed.  Defaults to today.
				HistList=loadActiveData.HistList??ClaimProcs.GetHistList(PatCur.PatNum,BenefitList,PatPlanList,InsPlanList,-1,dateTimeTP.Value,SubList);
				for(int i=0;i<listProcForTP.Count;i++) {
					listProcForTP[i].ProcDate=dateTimeTP.Value;
					if(listProcForTP[i].ProcNumLab!=0){
							//Lab fees will be calculated and added to looplist when its parent is calculated.
							continue;
					}
					Procedures.ComputeEstimates(listProcForTP[i],PatCur.PatNum,ref ClaimProcList,false,InsPlanList,PatPlanList,BenefitList,
						HistList,LoopList,false,
						PatCur.Age,SubList,
						null,false,true,_listSubstLinks,false,
						loadActiveData.ListFees,null);
					//then, add this information to loopList so that the next procedure is aware of it.
					LoopList.AddRange(ClaimProcs.GetHistForProc(ClaimProcList,listProcForTP[i].ProcNum,listProcForTP[i].CodeNum));
				}
				SyncCanadianLabs(ClaimProcList,listProcForTP);
				//We don't want to save the claimprocs if it's a date other than DateTime.Today, since they are calculated using modified date information.
				if(dateTimeTP.Value==DateTime.Today) {
					ClaimProcs.Synch(ref ClaimProcList,claimProcListOld);
				}
			}
			else { 
				for(int i=0;i<listProcForTP.Count;i++) {
					if(listProcForTP[i].ProcNumLab!=0){
							//Lab fees will be calculated and added to looplist when its parent is calculated.
							continue;
					}
					Procedures.ComputeEstimates(listProcForTP[i],PatCur.PatNum,ref ClaimProcList,false,InsPlanList,PatPlanList,BenefitList,HistList,LoopList
						,false,PatCur.Age,SubList,listSubstLinks:_listSubstLinks);
					//then, add this information to loopList so that the next procedure is aware of it.
					LoopList.AddRange(ClaimProcs.GetHistForProc(ClaimProcList,listProcForTP[i].ProcNum,listProcForTP[i].CodeNum));
				}
				SyncCanadianLabs(ClaimProcList,listProcForTP);
				//save changes in the list to the database
				ClaimProcs.Synch(ref ClaimProcList,claimProcListOld);
			}
			//claimProcList=ClaimProcs.RefreshForTP(PatCur.PatNum);
			string estimateNote;
			if(!checkShowDiscountNotAutomatic) {
				checkShowDiscount.Checked=false;
			}
			decimal subfee,suballowed,totFee,priIns,secIns,subpriIns,allowed,totPriIns,subsecIns,totSecIns,subdiscount,totDiscount,subpat,totPat,totAllowed
				,taxAmt,subTaxAmt,totTaxAmt;
			subfee=suballowed=totFee=priIns=secIns=subpriIns=allowed=totPriIns=subsecIns=totSecIns=subdiscount=totDiscount=subpat=totPat=totAllowed=
				taxAmt=subTaxAmt=totTaxAmt=0;
			RowsMain.Clear();
			List<ProcTP> retVal=new List<ProcTP>();
			DiscountPlan discountPlan=null;
			if(PatCur.DiscountPlanNum!=0) {
				discountPlan=DiscountPlans.GetPlan(PatCur.DiscountPlanNum);
			}
			for(int i=0;i<listProcForTP.Count;i++) {
				ProcedureCode procCodeCur=ProcedureCodes.GetProcCode(listProcForTP[i].CodeNum);
				TpRow row=new TpRow();
				row.ProcAbbr=procCodeCur.AbbrDesc;
				decimal fee=(decimal)listProcForTP[i].ProcFeeTotal;
				if(PatCur.DiscountPlanNum!=0) {
					Fee procFee=Fees.GetFee(procCodeCur.CodeNum,discountPlan.FeeSchedNum,listProcForTP[i].ClinicNum,listProcForTP[i].ProvNum,loadActiveData.ListFees);
					if(procFee==null) {//No fee for discount plan's feesched and proc's provider
						Provider patProv=Provider.GetById(PatCur.PriProv);
						procFee=Fees.GetFee(procCodeCur.CodeNum,patProv.FeeScheduleId,listProcForTP[i].ClinicNum,patProv.Id,loadActiveData.ListFees);
						if(procFee==null) {//No fee for pat's pri prov feesched and pat's pri prov
							patProv=Provider.GetById(Preference.GetLong(PreferenceName.PracticeDefaultProv));
							procFee=Fees.GetFee(procCodeCur.CodeNum,patProv.FeeScheduleId,listProcForTP[i].ClinicNum,patProv.Id,loadActiveData.ListFees);
						}
					}
					decimal procFeeAmt=(procFee == null) ? fee : (decimal)procFee.Amount;
					if(checkShowIns.Checked) {
						priIns=fee-procFeeAmt > 0 ? fee-procFeeAmt : 0;
					}
					else {
						fee=procFeeAmt;
					}
				}
				subfee+=fee;
				totFee+=fee;
				taxAmt=(decimal)listProcForTP[i].TaxAmt;
				#region ShowMaxDed
				string showPriDeduct="";
				string showSecDeduct="";
				ClaimProc claimproc; //holds the estimate.
				if(PatPlanList.Count>0) { //Primary
					claimproc=ClaimProcs.GetEstimate(ClaimProcList,listProcForTP[i].ProcNum,priPlanCur.PlanNum,PatPlanList[0].InsSubNum);
					if(claimproc==null || claimproc.EstimateNote.Contains("Frequency Limitation")) {
						if(claimproc!=null && claimproc.InsEstTotalOverride!=-1) {
							priIns=(decimal)claimproc.InsEstTotalOverride;
						}
						else { 
							priIns=0;
						}
					}
					else {
						if(checkShowMaxDed.Checked) { //whether visible or not
							priIns=(decimal)ClaimProcs.GetInsEstTotal(claimproc);
							double ded=ClaimProcs.GetDeductibleDisplay(claimproc);
							if(ded>0) {
								showPriDeduct="\r\n"+Lan.g(this,"Pri Deduct Applied: ")+ded.ToString("c");
							}
						}
						else {
							priIns=(decimal)claimproc.BaseEst;
						}
					}
					if((claimproc!=null && claimproc.NoBillIns) || (claimproc==null && procCodeCur.NoBillIns)) {
						allowed=-1;
					}
					else {
						allowed=ComputeAllowedAmount(listProcForTP[i],claimproc,this._listSubstLinks,lookupFees);
					}
				}
				else if(PatCur.DiscountPlanNum==0) { //no primary ins and no discount plan
					priIns=0;
				}
				if(PatPlanList.Count>1) { //Secondary
					claimproc=ClaimProcs.GetEstimate(ClaimProcList,listProcForTP[i].ProcNum,secPlanCur.PlanNum,PatPlanList[1].InsSubNum);
					if(claimproc==null) {
						secIns=0;
					}
					else {
						if(checkShowMaxDed.Checked) {
							secIns=(decimal)ClaimProcs.GetInsEstTotal(claimproc);
							decimal ded=(decimal)ClaimProcs.GetDeductibleDisplay(claimproc);
							if(ded>0) {
								showSecDeduct="\r\n"+Lan.g(this,"Sec Deduct Applied: ")+ded.ToString("c");
							}
						}
						else {
							secIns=(decimal)claimproc.BaseEst;
						}
					}
				} //secondary
				else { //no secondary ins
					secIns=0;
				}
				#endregion ShowMaxDed
				subpriIns+=priIns;
				totPriIns+=priIns;
				subsecIns+=secIns;
				totSecIns+=secIns;
				if(allowed.IsGreaterThan(-1)) {//-1 means the proc is DoNotBillIns
					suballowed+=allowed;
					totAllowed+=allowed;
				}
				subTaxAmt+=taxAmt;
				totTaxAmt+=taxAmt;
				decimal discount=(decimal)ClaimProcs.GetTotalWriteOffEstimateDisplay(ClaimProcList,listProcForTP[i].ProcNum); 
				if(!checkShowDiscountNotAutomatic && !checkShowDiscount.Checked	 && (listProcForTP[i].Discount!=0 || discount!=0)) {
					checkShowDiscount.Checked=true;
				}
				discount+=(decimal)listProcForTP[i].Discount;
				subdiscount+=discount;
				totDiscount+=discount;
				decimal pat=fee-priIns-secIns-discount+taxAmt;
				if(pat<0) {
					pat=0;
				}
				subpat+=pat;
				totPat+=pat;
				//Fill TpRow object with information.
				row.Priority=Defs.GetName(DefinitionCategory.TxPriorities,listTreatPlanAttaches.FirstOrDefault(x => x.ProcNum==listProcForTP[i].ProcNum).Priority);//(Defs.GetName(DefCat.TxPriorities,listProcForTP[i].Priority));
				row.Tth=(Tooth.ToInternat(listProcForTP[i].ToothNum));
				if(ProcedureCodes.GetProcCode(listProcForTP[i].CodeNum).TreatArea==TreatmentArea.Surf) {
					row.Surf=(Tooth.SurfTidyFromDbToDisplay(listProcForTP[i].Surf,listProcForTP[i].ToothNum));
				}
				else if(ProcedureCodes.GetProcCode(listProcForTP[i].CodeNum).TreatArea==TreatmentArea.Sextant) {
					row.Surf=Tooth.GetSextant(listProcForTP[i].Surf,(ToothNumberingNomenclature)Preference.GetInt(PreferenceName.UseInternationalToothNumbers));
				}
				else {
					row.Surf=(listProcForTP[i].Surf); //I think this will properly allow UR, L, etc.
				}
				row.Code=procCodeCur.ProcCode;
				string descript=ProcedureCodes.GetLaymanTerm(listProcForTP[i].CodeNum);
				if(listProcForTP[i].ToothRange!="") {
					descript+=" #"+Tooth.FormatRangeForDisplay(listProcForTP[i].ToothRange);
				}
				if(checkShowMaxDed.Checked) {
					estimateNote=ClaimProcs.GetEstimateNotes(listProcForTP[i].ProcNum,ClaimProcList);
					if(estimateNote!="") {
						descript+="\r\n"+estimateNote;
					}
				}
				row.Description=(descript);
				if(showPriDeduct!="") {
					row.Description+=showPriDeduct;
				}
				if(showSecDeduct!="") {
					row.Description+=showSecDeduct;
				}
				row.Prognosis=Defs.GetName(DefinitionCategory.Prognosis,PIn.Long(listProcForTP[i].Prognosis.ToString()));
				row.Dx=Defs.GetValue(DefinitionCategory.Diagnosis,PIn.Long(listProcForTP[i].Dx.ToString()));
				row.Fee=fee;
				row.PriIns=priIns;
				row.SecIns=secIns;
				row.Discount=discount;
				row.Pat=pat;
				row.FeeAllowed=allowed;
				row.TaxEst=taxAmt;
				row.ColorText=Defs.GetColor(DefinitionCategory.TxPriorities,listTreatPlanAttaches.FirstOrDefault(y => y.ProcNum==listProcForTP[i].ProcNum).Priority);
				if(row.ColorText==System.Drawing.Color.White) {
					row.ColorText=System.Drawing.Color.Black;
				}
				//row.Tag=listProcForTP[i].Copy();
				Procedure proc=listProcForTP[i].Copy();
				//procList.Add(proc);
				ProcTP procTP=new ProcTP();
				//procTP.TreatPlanNum=tp.TreatPlanNum;
				procTP.PatNum=PatCur.PatNum;
				procTP.ProcNumOrig=proc.ProcNum;
				procTP.ItemOrder=i;
				procTP.Priority=listTreatPlanAttaches.FirstOrDefault(x => x.ProcNum==proc.ProcNum).Priority;//proc.Priority;
				procTP.ToothNumTP=Tooth.ToInternat(proc.ToothNum);
				if(ProcedureCodes.GetProcCode(proc.CodeNum).TreatArea==TreatmentArea.Surf) {
					procTP.Surf=Tooth.SurfTidyFromDbToDisplay(proc.Surf,proc.ToothNum);
				}
				else {
					procTP.Surf=proc.Surf;//for UR, L, etc.
				}
				procTP.ProcCode=ProcedureCodes.GetStringProcCode(proc.CodeNum);
				procTP.Descript=row.Description;
				procTP.FeeAmt=PIn.Double(row.Fee.ToString());
				procTP.PriInsAmt=PIn.Double(row.PriIns.ToString());
				procTP.SecInsAmt=PIn.Double(row.SecIns.ToString());
				procTP.Discount=PIn.Double(row.Discount.ToString());
				procTP.PatAmt=PIn.Double(row.Pat.ToString());
				procTP.Prognosis=row.Prognosis;
				procTP.Dx=row.Dx;
				procTP.ProcAbbr=row.ProcAbbr;
				procTP.FeeAllowed=PIn.Double(row.FeeAllowed.ToString());
				procTP.TaxAmt=PIn.Double(row.TaxEst.ToString());
				retVal.Add(procTP);
				procTP.ODTag = proc.ProcNumLab;//Used for selection logic. See gridMain_CellClick(...).
				row.Tag=procTP;
				RowsMain.Add(row);
				#region subtotal
				if(checkShowSubtotals.Checked &&
					 (i==listProcForTP.Count-1 || listTreatPlanAttaches.FirstOrDefault(x => x.ProcNum==listProcForTP[i+1].ProcNum).Priority!=procTP.Priority)) {
					row=new TpRow();
					row.Description=Lan.g("TableTP","Subtotal");
					row.Fee=subfee;
					row.PriIns=subpriIns;
					row.SecIns=subsecIns;
					row.Discount=subdiscount;
					row.Pat=subpat;
					row.FeeAllowed=suballowed;
					row.TaxEst=subTaxAmt;
					row.ColorText=Defs.GetColor(DefinitionCategory.TxPriorities,listTreatPlanAttaches.FirstOrDefault(y => y.ProcNum==listProcForTP[i].ProcNum).Priority);
					if(row.ColorText==System.Drawing.Color.White) {
						row.ColorText=System.Drawing.Color.Black;
					}
					row.Bold=true;
					row.ColorLborder=System.Drawing.Color.Black;
					RowsMain.Add(row);
					subfee=0;
					subpriIns=0;
					subsecIns=0;
					subdiscount=0;
					subpat=0;
					suballowed=0;
					subTaxAmt=0;
				}
				#endregion subtotal
			}
			textNote.Text=_listTreatPlans[gridPlans.SelectedIndices[0]].Note;
			HasNoteChanged=false;
			if((_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Saved 
					&& _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="") 
				|| (_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Inactive 
					&& _listTreatPlans[gridPlans.SelectedIndices[0]].Heading==Lan.g(this,"Unassigned"))) 
			{
				textNote.ReadOnly=true;
			}
			else {
				textNote.ReadOnly=false;
			}
			#region Totals
			if(checkShowTotals.Checked) {
				TpRow row=new TpRow();
				row.Description=Lan.g("TableTP","Total");
				row.Fee=totFee;
				row.PriIns=totPriIns;
				row.SecIns=totSecIns;
				row.Discount=totDiscount;
				row.Pat=totPat;
				row.FeeAllowed=totAllowed;
				row.TaxEst=totTaxAmt;
				row.Bold=true;
				row.ColorText=System.Drawing.Color.Black;
				RowsMain.Add(row);
			}
			#endregion Totals
			treatPlan.ListProcTPs=retVal;
			return retVal;
		}

		///<summary>Refreshes Canadian Lab Fee ClaimProcs in listClaimProcs from the Db, and adds any new Canadian Lab Fee ClaimProcs that were 
		///added to Db when computing estimates</summary>
		private static void SyncCanadianLabs(List<ClaimProc> listClaimProcs,List<Procedure> listProcTp) {
			//1. Get all lab Cp for lab proc nums from Db
			//2. Copy Db lab Cp to original listClaimProcs (Db updates in ClaimProcs.ComputeBaseEst())
			//3. Add any new lab Cp (Db inserts in ClaimProcs.ComputeBaseEst())
			List<long> listLabProcNums=listProcTp.Where(x => x.ProcNumLab!=0).Select(x => x.ProcNum).ToList();
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA") || listLabProcNums.Count==0 ){//Not Canada or no labs to consider.
				return;
			}
			List<long> listOrigLabClaimProcNums=listClaimProcs.Where(x => listLabProcNums.Contains(x.ProcNum)).Select(x => x.ClaimProcNum).ToList();
			//Contains CPs we want to refresh and any that were added. Only look at estimates and cap estimates, see ClaimProcs.RefreshForTP(...)
			List<ClaimProc> listDbLabClaimProcs=ClaimProcs.RefreshForProcs(listLabProcNums)
				.Where(x => x.Status.In(ClaimProcStatus.Estimate,ClaimProcStatus.CapEstimate)).ToList();
			for(int i=0;i<listClaimProcs.Count;i++) {
				long claimProcNum=listClaimProcs[i].ClaimProcNum;
				if(!listOrigLabClaimProcNums.Contains(claimProcNum)) {
					continue;//listClaimProcs[i] is not associated to a lab
				}
				listClaimProcs[i]=ClaimProcs.GetFromList(listDbLabClaimProcs,claimProcNum);//Update listClaimProcs to reflect changed values.
			}
			//New estimates could have been added in ClaimProcs.CanadianLabBaseEstHelper(...).
			listClaimProcs.AddRange(listDbLabClaimProcs.Where(x => !listOrigLabClaimProcNums.Contains(x.ClaimProcNum)).ToList());
		}

		/// <summary>Returns in-memory TreatPlan representing the current treatplan. For displaying current treat-plan before saving it.</summary>
		private TreatPlan GetCurrentTPHelper() {
			TreatPlan retVal=new TreatPlan();
			retVal.Heading=Lan.g(this,"Proposed Treatment Plan");
			retVal.DateTP=DateTimeOD.Today;
			retVal.PatNum=PatCur.PatNum;
			retVal.Note=Preference.GetString(PreferenceName.TreatmentPlanNote);
			retVal.ListProcTPs=new List<ProcTP>();
			ProcTP procTP;
			Procedure proc;
			int itemNo=0;
			List<Procedure> procList=new List<Procedure>();
			if(gridMain.SelectedIndices.Length==0 || gridMain.SelectedIndices.All(x=>gridMain.Rows[x].Tag==null)) {
				gridMain.SetSelected(true);//either no rows selected, or only total rows selected.
			}
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				if(gridMain.Rows[gridMain.SelectedIndices[i]].Tag==null) {
					//user must have highlighted a subtotal row.
					continue;
				}
				proc=(Procedure)gridMain.Rows[gridMain.SelectedIndices[i]].Tag;
				procList.Add(proc);
				procTP=new ProcTP();
				//procTP.TreatPlanNum=tp.TreatPlanNum;
				procTP.PatNum=PatCur.PatNum;
				procTP.ProcNumOrig=proc.ProcNum;
				procTP.ItemOrder=itemNo;
				procTP.Priority=proc.Priority;
				procTP.ToothNumTP=Tooth.ToInternat(proc.ToothNum);
				if(ProcedureCodes.GetProcCode(proc.CodeNum).TreatArea==TreatmentArea.Surf) {
					procTP.Surf=Tooth.SurfTidyFromDbToDisplay(proc.Surf,proc.ToothNum);
				}
				else {
					procTP.Surf=proc.Surf;//for UR, L, etc.
				}
				procTP.ProcCode=ProcedureCodes.GetStringProcCode(proc.CodeNum);
				procTP.Descript=RowsMain[gridMain.SelectedIndices[i]].Description;
				if(checkShowFees.Checked) {
					procTP.FeeAmt=PIn.Double(RowsMain[gridMain.SelectedIndices[i]].Fee.ToString());
				}
				if(checkShowIns.Checked) {
					procTP.PriInsAmt=PIn.Double(RowsMain[gridMain.SelectedIndices[i]].PriIns.ToString());
					procTP.SecInsAmt=PIn.Double(RowsMain[gridMain.SelectedIndices[i]].SecIns.ToString());
				}
				if(checkShowDiscount.Checked) {
					procTP.Discount=PIn.Double(RowsMain[gridMain.SelectedIndices[i]].Discount.ToString());
				}
				procTP.PatAmt=PIn.Double(RowsMain[gridMain.SelectedIndices[i]].Pat.ToString());
				procTP.Prognosis=RowsMain[gridMain.SelectedIndices[i]].Prognosis;
				procTP.Dx=RowsMain[gridMain.SelectedIndices[i]].Dx;
				retVal.ListProcTPs.Add(procTP);
				//ProcTPs.InsertOrUpdate(procTP,true);
				itemNo++;
			}
			return retVal;
		}

		///<summary>Simply creates a new sheet from a given treatment plan and adds parameters to the sheet based on which checkboxes are checked.</summary>
		private Sheet TreatPlanToSheet(TreatPlan treatPlan) {
			Sheet sheetTP=SheetUtil.CreateSheet(SheetDefs.GetInternalOrCustom(SheetInternalType.TreatmentPlan),PatCur.PatNum);
			sheetTP.Parameters.Add(new SheetParameter(true,"TreatPlan") { ParamValue=treatPlan });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowDiscountNotAutomatic") { ParamValue=checkShowDiscountNotAutomatic });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowDiscount") { ParamValue=checkShowDiscount.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowMaxDed") { ParamValue=checkShowMaxDed.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowSubTotals") { ParamValue=checkShowSubtotals.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowTotals") { ParamValue=checkShowTotals.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowCompleted") { ParamValue=checkShowCompleted.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowFees") { ParamValue=checkShowFees.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowIns") { ParamValue=checkShowIns.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"toothChartImg") { ParamValue=SheetPrinting.GetToothChartHelper(PatCur.PatNum,checkShowCompleted.Checked,treatPlan) });
			//FormSheetFillEdit FormSFE=new FormSheetFillEdit(sheetTP);
			SheetFiller.FillFields(sheetTP);
			SheetUtil.CalculateHeights(sheetTP);
			return sheetTP;
		}

		private MigraDoc.DocumentObjectModel.Document CreateDocument(){
			MigraDoc.DocumentObjectModel.Document doc= new MigraDoc.DocumentObjectModel.Document();
			doc.DefaultPageSetup.PageWidth=Unit.FromInch(8.5);
			doc.DefaultPageSetup.PageHeight=Unit.FromInch(11);
			doc.DefaultPageSetup.TopMargin=Unit.FromInch(.5);
			doc.DefaultPageSetup.LeftMargin=Unit.FromInch(.5);
			doc.DefaultPageSetup.RightMargin=Unit.FromInch(.5);
			MigraDoc.DocumentObjectModel.Section section=doc.AddSection();
			string text;
			MigraDoc.DocumentObjectModel.Font headingFont=MigraDocHelper.CreateFont(13,true);
			MigraDoc.DocumentObjectModel.Font bodyFontx=MigraDocHelper.CreateFont(9,false);
			MigraDoc.DocumentObjectModel.Font nameFontx=MigraDocHelper.CreateFont(9,true);
			MigraDoc.DocumentObjectModel.Font totalFontx=MigraDocHelper.CreateFont(9,true);
			//Heading---------------------------------------------------------------------------------------------------------------
			#region printHeading
			Paragraph par=section.AddParagraph();
			ParagraphFormat parformat=new ParagraphFormat();
			parformat.Alignment=ParagraphAlignment.Center;
			parformat.Font=MigraDocHelper.CreateFont(10,true);
			par.Format=parformat;
			text=_listTreatPlans[gridPlans.SelectedIndices[0]].Heading;
			par.AddFormattedText(text,headingFont);
			par.AddLineBreak();

				Clinic clinic=Clinic.GetById(PatCur.ClinicNum);
				text=clinic.Description;
				par.AddText(text);
				par.AddLineBreak();
				text=clinic.Phone;
			
			if(text.Length==10 && Application.CurrentCulture.Name=="en-US") {
				text="("+text.Substring(0,3)+")"+text.Substring(3,3)+"-"+text.Substring(6);
			}
			par.AddText(text);
			par.AddLineBreak();
			text=PatCur.GetNameFLFormal()+", DOB "+PatCur.Birthdate.ToShortDateString();
			par.AddText(text);
			par.AddLineBreak();
			if(gridPlans.SelectedIndices[0]>0){//not the default plan
				if(_listTreatPlans[gridPlans.SelectedIndices[0]].ResponsParty!=0){
					text=Lan.g(this,"Responsible Party: ")
						+Patients.GetLim(_listTreatPlans[gridPlans.SelectedIndices[0]].ResponsParty).GetNameFL();
					par.AddText(text);
					par.AddLineBreak();
				}
			}
			if(new[] { TreatPlanStatus.Active,TreatPlanStatus.Inactive }.Contains(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus)) {//Active/Inactive TP
				text=DateTime.Today.ToShortDateString();
			}
			else {
				text=_listTreatPlans[gridPlans.SelectedIndices[0]].DateTP.ToShortDateString();
			}
			par.AddText(text);
			#endregion
			//Graphics---------------------------------------------------------------------------------------------------------------
			#region PrintGraphics
			TextFrame frame;
			int widthDoc=MigraDocHelper.GetDocWidth();
			if(!Clinic.GetById(Clinics.ClinicId).IsMedicalOnly)
			{	
				frame=MigraDocHelper.CreateContainer(section);
				MigraDocHelper.DrawString(frame,Lan.g(this,"Your")+"\r\n"+Lan.g(this,"Right"),bodyFontx,
					new RectangleF(widthDoc/2-toothChart.Width/2-50,toothChart.Height/2-10,50,100));
				MigraDocHelper.DrawBitmap(frame,chartBitmap,widthDoc/2-toothChart.Width/2,0);
				MigraDocHelper.DrawString(frame,Lan.g(this,"Your")+"\r\n"+Lan.g(this,"Left"),bodyFontx,
					new RectangleF(widthDoc/2+toothChart.Width/2+17,toothChart.Height/2-10,50,100));
				if(checkShowCompleted.Checked) {
					List<Definition> listDefs=Definition.GetByCategory(DefinitionCategory.ChartGraphicColors);
					float yPos=toothChart.Height+15;
					float xPos=225;
					MigraDocHelper.FillRectangle(frame,listDefs[3].Color,xPos,yPos,14,14);
					xPos+=16;
					MigraDocHelper.DrawString(frame,Lan.g(this,"Existing"),bodyFontx,xPos,yPos);
					Graphics g=this.CreateGraphics();//for measuring strings.
					xPos+=(int)g.MeasureString(Lan.g(this,"Existing"),bodyFont).Width+23;
					//The Complete work is actually a combination of EC and C. Usually same color.
					//But just in case they are different, this will show it.
					MigraDocHelper.FillRectangle(frame,listDefs[2].Color,xPos,yPos,7,14);
					xPos+=7;
					MigraDocHelper.FillRectangle(frame,listDefs[1].Color,xPos,yPos,7,14);
					xPos+=9;
					MigraDocHelper.DrawString(frame,Lan.g(this,"Complete"),bodyFontx,xPos,yPos);
					xPos+=(int)g.MeasureString(Lan.g(this,"Complete"),bodyFont).Width+23;
					MigraDocHelper.FillRectangle(frame,listDefs[4].Color,xPos,yPos,14,14);
					xPos+=16;
					MigraDocHelper.DrawString(frame,Lan.g(this,"Referred Out"),bodyFontx,xPos,yPos);
					xPos+=(int)g.MeasureString(Lan.g(this,"Referred Out"),bodyFont).Width+23;
					MigraDocHelper.FillRectangle(frame,listDefs[0].Color,xPos,yPos,14,14);
					xPos+=16;
					MigraDocHelper.DrawString(frame,Lan.g(this,"Treatment Planned"),bodyFontx,xPos,yPos);
					g.Dispose();
				}
			}	
			#endregion
			MigraDocHelper.InsertSpacer(section,10);
			if(!Preference.GetBool(PreferenceName.TreatPlanItemized)) {
				FillGridPrint();
				MigraDocHelper.DrawGrid(section,gridPrint);
				gridPrint.Visible=false;
				FillMainDisplay();
			}
			else {
				MigraDocHelper.DrawGrid(section,gridMain);
			}
			//Print benefits----------------------------------------------------------------------------------------------------
			#region printBenefits
			if(checkShowIns.Checked) {
				ODGrid gridFamIns=new ODGrid();
				this.Controls.Add(gridFamIns);
				gridFamIns.BeginUpdate();
				gridFamIns.Columns.Clear();
				ODGridColumn col=new ODGridColumn("",140);
				gridFamIns.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Primary"),70,HorizontalAlignment.Right);
				gridFamIns.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Secondary"),70,HorizontalAlignment.Right);
				gridFamIns.Columns.Add(col);
				gridFamIns.Rows.Clear();
				ODGridRow row;
				//Annual Family Max--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Family Maximum"));
				row.Cells.Add(POut.Double(userControlFamIns.FamPriMax));
				row.Cells.Add(POut.Double(userControlFamIns.FamSecMax));
				gridFamIns.Rows.Add(row);
				//Family Deductible--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Family Deductible"));
				row.Cells.Add(POut.Double(userControlFamIns.FamPriDed));
				row.Cells.Add(POut.Double(userControlFamIns.FamSecDed));
				gridFamIns.Rows.Add(row);
				//Print Family Insurance-----------------------
				MigraDocHelper.InsertSpacer(section,15);
				par=section.AddParagraph();
				par.Format.Alignment=ParagraphAlignment.Center;
				par.AddFormattedText(Lan.g(this,"Family Insurance Benefits"),totalFontx);
				MigraDocHelper.InsertSpacer(section,2);
				MigraDocHelper.DrawGrid(section,gridFamIns);
				gridFamIns.Dispose();
				//Individual Insurance---------------------
				ODGrid gridIns=new ODGrid();
				this.Controls.Add(gridIns);
				gridIns.BeginUpdate();
				gridIns.Columns.Clear();
				col=new ODGridColumn("",140);
				gridIns.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Primary"),70,HorizontalAlignment.Right);
				gridIns.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Secondary"),70,HorizontalAlignment.Right);
				gridIns.Columns.Add(col);
				gridIns.Rows.Clear();
				//Annual Max--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Annual Maximum"));
				row.Cells.Add(POut.Double(userControlIndIns.PriMax));
				row.Cells.Add(POut.Double(userControlIndIns.SecMax));
				gridIns.Rows.Add(row);
				//Deductible--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Deductible"));
				row.Cells.Add(POut.Double(userControlIndIns.PriDed));
				row.Cells.Add(POut.Double(userControlIndIns.SecDed));
				gridIns.Rows.Add(row);
				//Deductible Remaining--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Deductible Remaining"));
				row.Cells.Add(POut.Double(userControlIndIns.PriDedRem));
				row.Cells.Add(POut.Double(userControlIndIns.SecDedRem));
				gridIns.Rows.Add(row);
				//Insurance Used--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Insurance Used"));
				row.Cells.Add(POut.Double(userControlIndIns.PriUsed));
				row.Cells.Add(POut.Double(userControlIndIns.SecUsed));
				gridIns.Rows.Add(row);
				//Pending--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Pending"));
				row.Cells.Add(POut.Double(userControlIndIns.PriPend));
				row.Cells.Add(POut.Double(userControlIndIns.SecPend));
				gridIns.Rows.Add(row);
				//Remaining--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Remaining"));
				row.Cells.Add(POut.Double(userControlIndIns.PriRem));
				row.Cells.Add(POut.Double(userControlIndIns.SecRem));
				gridIns.Rows.Add(row);
				gridIns.EndUpdate();
				//Print Individual Insurance-------------------------
				MigraDocHelper.InsertSpacer(section,15);
				par=section.AddParagraph();
				par.Format.Alignment=ParagraphAlignment.Center;
				par.AddFormattedText(Lan.g(this,"Individual Insurance Benefits"),totalFontx);
				MigraDocHelper.InsertSpacer(section,2);
				MigraDocHelper.DrawGrid(section,gridIns);
				gridIns.Dispose();
			}
			#endregion
			//Note------------------------------------------------------------------------------------------------------------
			#region printNote
			string note="";
			if(gridPlans.SelectedIndices[0]==0) {//current TP
				note=Preference.GetString(PreferenceName.TreatmentPlanNote);
			}
			else {
				note=_listTreatPlans[gridPlans.SelectedIndices[0]].Note;
			}
			char nbsp='\u00A0';
			if(note!="") {
				//to prevent collapsing of multiple spaces to single spaces.  We only do double spaces to leave single spaces in place.
				note=note.Replace("  ",nbsp.ToString()+nbsp.ToString());
				MigraDocHelper.InsertSpacer(section,20);
				par=section.AddParagraph(note);
				par.Format.Font=bodyFontx;
				par.Format.Borders.Color=Colors.Gray;
				par.Format.Borders.DistanceFromLeft=Unit.FromInch(.05);
				par.Format.Borders.DistanceFromRight=Unit.FromInch(.05);
				par.Format.Borders.DistanceFromTop=Unit.FromInch(.05);
				par.Format.Borders.DistanceFromBottom=Unit.FromInch(.05);
			}
			#endregion
			//Signature-----------------------------------------------------------------------------------------------------------
			#region signature
			if(gridPlans.SelectedIndices[0]!=0//can't be default TP
				&& _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="")
			{
				TreatPlan treatPlan = _listTreatPlans[gridPlans.SelectedIndices[0]];
				List<ProcTP> proctpList = ProcTPs.RefreshForTP(_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum);
				System.Drawing.Bitmap sigBitmap = null;
				SignatureBoxWrapper sigBoxWrapper = new SignatureBoxWrapper();
				sigBoxWrapper.SignatureMode=SignatureBoxWrapper.SigMode.TreatPlan;
				string keyData = TreatPlans.GetKeyDataForSignatureHash(treatPlan,proctpList);
				sigBoxWrapper.FillSignature(treatPlan.SigIsTopaz,keyData,treatPlan.Signature);
				sigBitmap=sigBoxWrapper.GetSigImage();  //Previous tp code did not care if signature is valid or not.
				if(sigBitmap!=null) { 
					frame=MigraDocHelper.CreateContainer(section);
					MigraDocHelper.DrawBitmap(frame,sigBitmap,widthDoc/2-sigBitmap.Width/2,20);
				}
			}
			#endregion
			return doc;
		}

		///<summary>Just used for printing the 3D chart.</summary>
		private void ComputeProcListFiltered() {
			ProcListFiltered=new List<Procedure>();
			//first, add all completed work and conditions. C,EC,EO, and Referred
			for(int i=0;i<ProcList.Count;i++) {
				if(ProcList[i].ProcStatus==ProcStat.C
					|| ProcList[i].ProcStatus==ProcStat.EC
					|| ProcList[i].ProcStatus==ProcStat.EO) 
				{
					if(checkShowCompleted.Checked) {
						ProcListFiltered.Add(ProcList[i]);
					}
				}
				if(ProcList[i].ProcStatus==ProcStat.R) { //always show all referred
					ProcListFiltered.Add(ProcList[i]);
				}
				if(ProcList[i].ProcStatus==ProcStat.Cn) { //always show all conditions.
					ProcListFiltered.Add(ProcList[i]);
				}
			}
			//then add whatever is showing on the selected TP
			//Always select all procedures in TP.
			gridMain.SetSelected(true);
			if(new[] {TreatPlanStatus.Active,TreatPlanStatus.Inactive}.Contains(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus)) { //current plan
				ProcTPSelectList=gridMain.SelectedIndices.Where(x => gridMain.Rows[x].Tag!=null).Select(x => (ProcTP)gridMain.Rows[x].Tag).ToArray();
			}
			Procedure procDummy; //not a real procedure.  Just used to help display on graphical chart
			for(int i=0;i<ProcTPSelectList.Length;i++) {
				procDummy=new Procedure();
				//this next loop is a way to get missing fields like tooth range.  Could be improved.
				for(int j=0;j<ProcList.Count;j++) {
					if(ProcList[j].ProcNum==ProcTPSelectList[i].ProcNumOrig) {
						//but remember that even if the procedure is found, Status might have been altered
						procDummy=ProcList[j].Copy();
					}
				}
				if(Tooth.IsValidEntry(ProcTPSelectList[i].ToothNumTP)) {
					procDummy.ToothNum=Tooth.FromInternat(ProcTPSelectList[i].ToothNumTP);
				}
				if(ProcedureCodes.GetProcCode(ProcTPSelectList[i].ProcCode).TreatArea==TreatmentArea.Surf) {
					procDummy.Surf=Tooth.SurfTidyFromDisplayToDb(ProcTPSelectList[i].Surf,procDummy.ToothNum);
				}
				else {
					procDummy.Surf=ProcTPSelectList[i].Surf; //for quad, arch, etc.
				}
				if(procDummy.ToothRange==null) {
					procDummy.ToothRange="";
				}
				//procDummy.HideGraphical??
				procDummy.ProcStatus=ProcStat.TP;
				procDummy.CodeNum=ProcedureCodes.GetProcCode(ProcTPSelectList[i].ProcCode).CodeNum;
				ProcListFiltered.Add(procDummy);
			}
			ProcListFiltered.Sort(CompareProcListFiltered);
		}

		private int CompareProcListFiltered(Procedure proc1,Procedure proc2) {
			int dateFilter=proc1.ProcDate.CompareTo(proc2.ProcDate);
			if(dateFilter!=0) {
				return dateFilter;
			}
			//Dates are the same, filter by ProcStatus.
			int xIdx=GetProcStatusIdx(proc1.ProcStatus);
			int yIdx=GetProcStatusIdx(proc2.ProcStatus);
			return xIdx.CompareTo(yIdx);
		}

		private decimal ComputeAllowedAmount(Procedure proc,ClaimProc claimProcCur,List<SubstitutionLink> listSubLinks,Lookup<FeeKey2,Fee> lookupFees){
			//List<Fee> listFees) {
			decimal allowed=0;
			if(claimProcCur!=null) {				
				if(claimProcCur.AllowedOverride!=-1) {//check for allowed override
					allowed=(decimal)claimProcCur.AllowedOverride;
				}
				else {
					allowed=InsPlans.GetAllowedForProc(proc,claimProcCur,InsPlanList,listSubLinks,lookupFees);
					if(allowed==-1) {//Carrier does not have an allowed fee entered
						allowed=0;
					}
				}
			}
			return allowed;
		}

		///<summary>Returns index for sorting based on this order: Cn,TP,R,EO,EC,C,D</summary>
		private int GetProcStatusIdx(ProcStat procStat) {
			switch(procStat) {
				case ProcStat.Cn:
					return 0;
				case ProcStat.TPi:
				case ProcStat.TP:
					return 1;
				case ProcStat.R:
					return 2;
				case ProcStat.EO:
					return 3;
				case ProcStat.EC:
					return 4;
				case ProcStat.C:
					return 5;
				case ProcStat.D:
					return 6;
			}
			return 0;
		}

		private void DrawProcsGraphics() {
			Procedure proc;
			string[] teeth;
			System.Drawing.Color cLight=System.Drawing.Color.White;
			System.Drawing.Color cDark=System.Drawing.Color.White;
			List<Definition> listDefs=Definition.GetByCategory(DefinitionCategory.ChartGraphicColors);
			for(int i=0;i<ProcListFiltered.Count;i++) {
				proc=ProcListFiltered[i];
				//if(proc.ProcStatus!=procStat) {
				//  continue;
				//}
				if(proc.HideGraphics) {
					continue;
				}
				if(ProcedureCodes.GetProcCode(proc.CodeNum).PaintType==ToothPaintingType.Extraction && (
					proc.ProcStatus==ProcStat.C
					|| proc.ProcStatus==ProcStat.EC
					|| proc.ProcStatus==ProcStat.EO
					)) {
					continue;//prevents the red X. Missing teeth already handled.
				}
				if(ProcedureCodes.GetProcCode(proc.CodeNum).GraphicColor==System.Drawing.Color.FromArgb(0)) {
					switch(proc.ProcStatus) {
						case ProcStat.C:
							cDark=listDefs[1].Color;
							cLight=listDefs[6].Color;
							break;
						case ProcStat.TP:
							cDark=listDefs[0].Color;
							cLight=listDefs[5].Color;
							break;
						case ProcStat.EC:
							cDark=listDefs[2].Color;
							cLight=listDefs[7].Color;
							break;
						case ProcStat.EO:
							cDark=listDefs[3].Color;
							cLight=listDefs[8].Color;
							break;
						case ProcStat.R:
							cDark=listDefs[4].Color;
							cLight=listDefs[9].Color;
							break;
						case ProcStat.Cn:
							cDark=listDefs[16].Color;
							cLight=listDefs[17].Color;
							break;
					}
				}
				else {
					cDark=ProcedureCodes.GetProcCode(proc.CodeNum).GraphicColor;
					cLight=ProcedureCodes.GetProcCode(proc.CodeNum).GraphicColor;
				}
				switch(ProcedureCodes.GetProcCode(proc.CodeNum).PaintType) {
					case ToothPaintingType.BridgeDark:
						if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,proc.ToothNum)) {
							toothChart.SetPontic(proc.ToothNum,cDark);
						}
						else {
							toothChart.SetCrown(proc.ToothNum,cDark);
						}
						break;
					case ToothPaintingType.BridgeLight:
						if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,proc.ToothNum)) {
							toothChart.SetPontic(proc.ToothNum,cLight);
						}
						else {
							toothChart.SetCrown(proc.ToothNum,cLight);
						}
						break;
					case ToothPaintingType.CrownDark:
						toothChart.SetCrown(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.CrownLight:
						toothChart.SetCrown(proc.ToothNum,cLight);
						break;
					case ToothPaintingType.DentureDark:
						if(proc.Surf=="U") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+2).ToString();
							}
						}
						else if(proc.Surf=="L") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+18).ToString();
							}
						}
						else {
							teeth=proc.ToothRange.Split(new char[] { ',' });
						}
						for(int t=0;t<teeth.Length;t++) {
							if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,teeth[t])) {
								toothChart.SetPontic(teeth[t],cDark);
							}
							else {
								toothChart.SetCrown(teeth[t],cDark);
							}
						}
						break;
					case ToothPaintingType.DentureLight:
						if(proc.Surf=="U") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+2).ToString();
							}
						}
						else if(proc.Surf=="L") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+18).ToString();
							}
						}
						else {
							teeth=proc.ToothRange.Split(new char[] { ',' });
						}
						for(int t=0;t<teeth.Length;t++) {
							if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,teeth[t])) {
								toothChart.SetPontic(teeth[t],cLight);
							}
							else {
								toothChart.SetCrown(teeth[t],cLight);
							}
						}
						break;
					case ToothPaintingType.Extraction:
						toothChart.SetBigX(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.FillingDark:
						toothChart.SetSurfaceColors(proc.ToothNum,proc.Surf,cDark);
						break;
					case ToothPaintingType.FillingLight:
						toothChart.SetSurfaceColors(proc.ToothNum,proc.Surf,cLight);
						break;
					case ToothPaintingType.Implant:
						toothChart.SetImplant(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.PostBU:
						toothChart.SetBU(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.RCT:
						toothChart.SetRCT(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.Sealant:
						toothChart.SetSealant(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.Veneer:
						toothChart.SetVeneer(proc.ToothNum,cLight);
						break;
					case ToothPaintingType.Watch:
						toothChart.SetWatch(proc.ToothNum,cDark);
						break;
				}
			}
		}

		private void ToolBarMainUpdate_Click() {
			if(!new[] { TreatPlanStatus.Active,TreatPlanStatus.Inactive }.Contains(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus)) {
				MsgBox.Show(this,"The update fee utility only works on current treatment plans, not any saved plans.");
				return;
			}
			if(!MsgBox.Show(this,true,"Update all fees and insurance estimates on this treatment plan to the current fees for this patient?")) {
				return;
			}
			Procedure procCur;
			List<ClaimProc> claimProcList=ClaimProcs.RefreshForTP(PatCur.PatNum);
			List<ProcedureCode> listProcedureCodes=new List<ProcedureCode>();
			foreach(Procedure procedure in ProcListTP) {
				listProcedureCodes.Add(ProcedureCodes.GetProcCode(procedure.CodeNum));
			}
			List<Fee> listFees=Fees.GetListFromObjects(listProcedureCodes,ProcListTP.Select(x=>x.MedicalCode).ToList(),ProcListTP.Select(x=>x.ProvNum).ToList(),
				PatCur.PriProv,PatCur.SecProv,PatCur.FeeSched,InsPlanList,ProcListTP.Select(x=>x.ClinicNum).ToList(),null,//listAppts not needed because procs not based on appts
				_listSubstLinks,PatCur.DiscountPlanNum);
			for(int i=0;i<ProcListTP.Length;i++) {
				procCur=ProcListTP[i];
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")
					&& procCur.ProcNumLab!=0) 
				{
					continue;//The proc fee for a lab is derived from the lab fee on the parent procedure.
				}
				Procedure procOld=procCur.Copy(); //Get a copy of the old proc in case we need to update & to track the old procFee
				//Try to update the proc fee
				procCur.ProcFee=Procedures.GetProcFee(PatCur,PatPlanList,SubList,InsPlanList,procCur.CodeNum,procCur.ProvNum,procCur.ClinicNum,
					procCur.MedicalCode,listFees:listFees);
				Procedures.ComputeEstimates(procCur,PatCur.PatNum,claimProcList,false,InsPlanList,PatPlanList,BenefitList,PatCur.Age,SubList,listFees);
				if(AvaTax.CanProcedureBeTaxed(procCur)) { //If needed, update the sales tax amount as well (checks HQ)
					procCur.TaxAmt=(double)AvaTax.GetEstimate(procCur.CodeNum,procCur.PatNum,procCur.ProcFee);
				}
				//If the proc fee changed or the tax amt changed, update the procedurelog entry
				if((procOld.ProcFee!=procCur.ProcFee) || (procOld.TaxAmt!=procCur.TaxAmt)) {
					Procedures.Update(procCur,procOld);
				}
				//no recall synch required 
			}
			long tpNum=_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum;
			ModuleSelected(PatCur.PatNum);//refreshes TPs
			for(int i=0;i<_listTreatPlans.Count;i++) {
				if(_listTreatPlans[i].TreatPlanNum==tpNum) {
					gridPlans.SetSelected(i,true);
				}
			}
			FillMain();
		}

		private void ToolBarMainCreate_Click(){//Save TP
			//Cannot even click this button if user has not selected one of the treatment plans; Otherwise button is disabled.
			if(!new[]{TreatPlanStatus.Active,TreatPlanStatus.Inactive}.Contains(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus)){
			//if(gridPlans.SelectedIndices[0]!=0){
				MsgBox.Show(this,"An Active or Inactive TP must be selected before saving a TP.  You can highlight some procedures in the TP to save a TP with only those procedures in it.");
				return;
			}
			//Check for duplicate procedures on the appointment before sending the DFT to eCW.
			//if(Programs.UsingEcwTightOrFullMode() && Bridges.ECW.AptNum!=0) {
			//	List<Procedure> procs=Procedures.GetProcsForSingle(Bridges.ECW.AptNum,false);
			//	string duplicateProcs=ProcedureL.ProcsContainDuplicates(procs);
			//	if(duplicateProcs!="") {
			//		MessageBox.Show(duplicateProcs);
			//		return;
			//	}
			//}
			if(gridMain.SelectedIndices.Length==0){
				gridMain.SetSelected(true);//Select all if none selected.
			}
			List<TreatPlanAttach> listTreatPlanAttaches=TreatPlanAttaches.GetAllForTreatPlan(_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum);
			TreatPlan tp=new TreatPlan();
			tp.Heading=_listTreatPlans[gridPlans.SelectedIndices[0]].Heading;
			tp.DateTP=DateTimeOD.Today;
			tp.PatNum=PatCur.PatNum;
			tp.Note=_listTreatPlans[gridPlans.SelectedIndices[0]].Note;
			tp.ResponsParty=PatCur.ResponsParty;
			tp.UserNumPresenter=Security.CurrentUser.Id;
			tp.TPType=_listTreatPlans[gridPlans.SelectedIndices[0]].TPType;
			TreatPlans.Insert(tp);
			ProcTP procTP;
			Procedure proc;
			int itemNo=0;
			List<ProcTP> listSelectedProcTp=gridMain.SelectedIndices.Where(x => x>-1 && x<gridMain.Rows.Count)
				.Select(x => (ProcTP)gridMain.Rows[x].Tag).ToList();
			foreach(ProcTP selectedProcTP in listSelectedProcTp) {
				if(selectedProcTP==null){
					//user must have highlighted a subtotal row.
					continue;
				}
				proc=Procedures.GetOneProc(selectedProcTP.ProcNumOrig,true);
				procTP=new ProcTP();
				procTP.TreatPlanNum=tp.TreatPlanNum;
				procTP.PatNum=PatCur.PatNum;
				procTP.ProcNumOrig=proc.ProcNum;
				procTP.ItemOrder=itemNo;
				TreatPlanAttach tpAttach=listTreatPlanAttaches.FirstOrDefault(x=>x.ProcNum==proc.ProcNum);
				if(tpAttach==null) {
					//This could happen if another workstation completed this procedure just now.
					procTP.Priority=0;
				}
				else {
					procTP.Priority=tpAttach.Priority;
				}
				procTP.ToothNumTP=Tooth.ToInternat(proc.ToothNum);
				if(ProcedureCodes.GetProcCode(proc.CodeNum).TreatArea==TreatmentArea.Surf){
					procTP.Surf=Tooth.SurfTidyFromDbToDisplay(proc.Surf,proc.ToothNum);
				}
				else{
					procTP.Surf=proc.Surf;//for UR, L, etc.
				}
				procTP.ProcCode=ProcedureCodes.GetStringProcCode(proc.CodeNum);
				procTP.Descript=selectedProcTP.Descript;
				procTP.FeeAmt=selectedProcTP.FeeAmt;
				procTP.PriInsAmt=selectedProcTP.PriInsAmt;
				procTP.SecInsAmt=selectedProcTP.SecInsAmt;
				procTP.Discount=selectedProcTP.Discount;
				procTP.PatAmt=selectedProcTP.PatAmt;
				procTP.Prognosis=selectedProcTP.Prognosis;
				procTP.Dx=selectedProcTP.Dx;
				procTP.ProcAbbr=selectedProcTP.ProcAbbr;
				procTP.FeeAllowed=selectedProcTP.FeeAllowed;
				procTP.TaxAmt=selectedProcTP.TaxAmt;
				ProcTPs.InsertOrUpdate(procTP,true);
				itemNo++;
				#region Canadian Lab Fees
				/*
				proc=(Procedure)gridMain.Rows[gridMain.SelectedIndices[i]].Tag;
				procTP=new ProcTP();
				procTP.TreatPlanNum=tp.TreatPlanNum;
				procTP.PatNum=PatCur.PatNum;
				procTP.ProcNumOrig=proc.ProcNum;
				procTP.ItemOrder=itemNo;
				procTP.Priority=proc.Priority;
				procTP.ToothNumTP="";
				procTP.Surf="";
				procTP.Code=proc.LabProcCode;
				procTP.Descript=gridMain.Rows[gridMain.SelectedIndices[i]]
					.Cells[gridMain.Columns.GetIndex(Lan.g("TableTP","Description"))].Text;
				if(checkShowFees.Checked) {
					procTP.FeeAmt=PIn.PDouble(gridMain.Rows[gridMain.SelectedIndices[i]]
						.Cells[gridMain.Columns.GetIndex(Lan.g("TableTP","Fee"))].Text);
				}
				if(checkShowIns.Checked) {
					procTP.PriInsAmt=PIn.PDouble(gridMain.Rows[gridMain.SelectedIndices[i]]
						.Cells[gridMain.Columns.GetIndex(Lan.g("TableTP","Pri Ins"))].Text);
					procTP.SecInsAmt=PIn.PDouble(gridMain.Rows[gridMain.SelectedIndices[i]]
						.Cells[gridMain.Columns.GetIndex(Lan.g("TableTP","Sec Ins"))].Text);
					procTP.PatAmt=PIn.PDouble(gridMain.Rows[gridMain.SelectedIndices[i]]
						.Cells[gridMain.Columns.GetIndex(Lan.g("TableTP","Pat"))].Text);
				}
				ProcTPs.InsertOrUpdate(procTP,true);
				itemNo++;*/
				#endregion Canadian Lab Fees
			}
			ModuleSelected(PatCur.PatNum);
			for(int i=0;i<_listTreatPlans.Count;i++){
				if(_listTreatPlans[i].TreatPlanNum==tp.TreatPlanNum){
					gridPlans.SetSelected(i,true);
					FillMain();
				}
			}
		}

		private void ToolBarMainSign_Click() {
			if(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus!=TreatPlanStatus.Saved) {
				MsgBox.Show(this,"You may only sign a saved TP, not an Active or Inactive TP.");
				return;
			}
			//string patFolder=ImageStore.GetPatientFolder(PatCur,ImageStore.GetPreferredAtoZpath());
			if(Preference.GetBool(PreferenceName.TreatPlanSaveSignedToPdf) //preference enabled
			   && _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="" //and document is signed
			   && Documents.DocExists(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum)) //and file exists
			{
				MsgBox.Show(this,"Document already signed and saved to PDF. Unsign treatment plan from edit window to enable resigning.");
				Cursor=Cursors.WaitCursor;
				Documents.OpenDoc(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum);
				Cursor=Cursors.Default;
				return;//cannot re-sign document.
			}
			if(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum>0 && !Documents.DocExists(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum)) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Unable to open saved treatment plan. Would you like to recreate document using current information?")) {
					return;
				}
			}//TODO: Implement ODprintout pattern - MigraDoc
			FormTPsign FormT=new FormTPsign();
			if(DoPrintUsingSheets()) {
				TreatPlan treatPlan;
				treatPlan=_listTreatPlans[gridPlans.SelectedIndices[0]].Copy();
				treatPlan.ListProcTPs=ProcTPs.RefreshForTP(treatPlan.TreatPlanNum);
				FormT.SheetTP=TreatPlanToSheet(treatPlan);
				FormT.TotalPages=Sheets.CalculatePageCount(FormT.SheetTP,SheetPrinting.PrintMargin);
			}
			else {//Classic TPs
				PrepImageForPrinting();
				MigraDoc.DocumentObjectModel.Document doc=CreateDocument();
				MigraDocPrintDocument printdoc=new MigraDoc.Rendering.Printing.MigraDocPrintDocument();
				DocumentRenderer renderer=new MigraDoc.Rendering.DocumentRenderer(doc);
				renderer.PrepareDocument();
				printdoc.Renderer=renderer;
				FormT.Document=printdoc;
				FormT.TotalPages=renderer.FormattedDocument.PageCount;
			}
			FormT.SaveDocDelegate=SaveTPAsDocument;
			FormT.TPcur=_listTreatPlans[gridPlans.SelectedIndices[0]];
			FormT.DoPrintUsingSheets=DoPrintUsingSheets();
			FormT.ShowDialog();
			long tpNum=_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum;
			ModuleSelected(PatCur.PatNum);//refreshes TPs
			for(int i=0;i<_listTreatPlans.Count;i++) {
				if(_listTreatPlans[i].TreatPlanNum==tpNum) {
					gridPlans.SetSelected(i,true);
				}
			}
			FillMain();
		}

		///<summary>Saves TP as PDF in each image category defined as TP category. 
		/// If TreatPlanSaveSignedToPdf enabled, will default to first non-hidden category if no TP categories are explicitly defined.</summary>
		private List<Document> SaveTPAsDocument(bool isSigSave,Sheet sheet=null) {
			if(DoPrintUsingSheets() && sheet==null) {
				MsgBox.Show(this,"An error has occured with the Treatment Plans to sheets feature.  Please contact support.");
				return new List<Document>();
			}
			List<Document> retVal=new List<Document>();
			//Determine each of the document categories that this TP should be saved to.
			//"R"==TreatmentPlan; see FormDefEditImages.cs
			List<Definition> listImageCatDefs=Definition.GetByCategory(DefinitionCategory.ImageCats);
			List<long> categories= listImageCatDefs.Where(x => x.Value.Contains("R")).Select(x=>x.Id).ToList();
			if(isSigSave && categories.Count==0 && Preference.GetBool(PreferenceName.TreatPlanSaveSignedToPdf)) {
				//we must save at least one document, pick first non-hidden image category.
				Definition imgCat=listImageCatDefs.FirstOrDefault(x => !x.Hidden);
				if(imgCat==null) {
					MsgBox.Show(this,"Unable to save treatment plan because all image categories are hidden.");
					return new List<Document>();
				}
				categories.Add(imgCat.Id);
			}
			//Gauranteed to have at least one image category at this point.
			//Saving pdf to tempfile first simplifies this code, but can use extra bandwidth copying the file to and from the temp directory/Open Dent imgs.
			string tempFile=Preferences.GetRandomTempFile(".pdf");
			string rawBase64="";
			if(DoPrintUsingSheets()) {
				SheetPrinting.CreatePdf(sheet,tempFile,null);
				if(Preferences.AtoZfolderUsed!=DataStorageType.LocalAtoZ) {
					rawBase64=Convert.ToBase64String(System.IO.File.ReadAllBytes(tempFile));//Todo test this
				}
			}
			else {//classic TPs
				MigraDoc.Rendering.PdfDocumentRenderer pdfRenderer;
				pdfRenderer=new MigraDoc.Rendering.PdfDocumentRenderer(false,PdfFontEmbedding.Always);
				pdfRenderer.Document=CreateDocument();
				pdfRenderer.RenderDocument();
				pdfRenderer.Save(tempFile);
				if(Preferences.AtoZfolderUsed!=DataStorageType.LocalAtoZ) {
					using(MemoryStream stream=new MemoryStream()) {
						pdfRenderer.Save(stream,false);
						rawBase64=Convert.ToBase64String(stream.ToArray());
						stream.Close();
					}
				}
			}
			foreach(long docCategory in categories) {//usually only one, but do allow them to be saved once per image category.
				OpenDentBusiness.Document docSave=new Document();
				docSave.DocNum=Documents.Insert(docSave);
				string fileName="TPArchive"+docSave.DocNum;
				docSave.ImgType=ImageType.Document;
				docSave.DateCreated=DateTime.Now;
				docSave.PatNum=PatCur.PatNum;
				docSave.DocCategory=docCategory;
				docSave.Description=fileName;//no extension.
				docSave.RawBase64=rawBase64;//blank if using AtoZfolder

					string filePath=ImageStore.GetPatientFolder(PatCur);
					while(Storage.Default.FileExists(filePath+"\\"+fileName+".pdf")) {
						fileName+="x";
					}
                Storage.Default.CopyFile(tempFile,filePath+"\\"+fileName+".pdf");
				

				docSave.FileName=fileName+".pdf";//file extension used for both DB images and AtoZ images
				Documents.Update(docSave);
				retVal.Add(docSave);
			}
			try {
				File.Delete(tempFile); //cleanup the temp file.
			}
			catch {}
			return retVal;
		}

		///<summary>Returns true if the user has not checked 'Print using classic'.</summary>
		private bool DoPrintUsingSheets() {
			//If the Printing tab is visible and the print classic box is checked, then print classic.
			return (!tabShowSort.TabPages.Contains(tabPagePrint) || !checkPrintClassic.Checked);
		}

		///<summary>Similar method in Account</summary>
		private bool CheckClearinghouseDefaults() {
			if(Preference.GetLong(PreferenceName.ClearinghouseDefaultDent)==0) {
				MsgBox.Show(this,"No default dental clearinghouse defined.");
				return false;
			}
			if(Preference.GetBool(PreferenceName.ShowFeatureMedicalInsurance) && Preference.GetLong(PreferenceName.ClearinghouseDefaultMed)==0) {
				MsgBox.Show(this,"No default medical clearinghouse defined.");
				return false;
			}
			return true;
		}

		private void ToolBarMainPreAuth_Click() {
			if(gridPlans.SelectedIndices.Length==0) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.ClaimView)) {
				return;
			}
			if(!CheckClearinghouseDefaults()) {
				return;
			}
			if(!new[] { TreatPlanStatus.Active,TreatPlanStatus.Inactive }.Contains(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus)) {
				MsgBox.Show(this,"You can only send a preauth from a current TP, not a saved TP.");
				return;
			}
			if(gridMain.SelectedIndices.All(x => gridMain.Rows[x].Tag==null)) {
				MessageBox.Show(Lan.g(this,"Please select procedures first."));
				return;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canada
				List<int> selectedIndices=new List<int>(gridMain.SelectedIndices);
				if(gridMain.SelectedIndices.Length>7) {
					gridMain.SetSelected(false);
					int selectedLabCount=0;
					foreach(int selectedIdx in selectedIndices) {
						if(gridMain.Rows[selectedIdx].Tag==null) {
							continue;//subtotal row.
						}
						Procedure proc=(Procedures.GetOneProc(((ProcTP)gridMain.Rows[selectedIdx].Tag).ProcNumOrig,false));
						if(proc!=null && proc.ProcNumLab!=0) {
								selectedLabCount++;
						}
						gridMain.SetSelected(selectedIdx,true);
						if(gridMain.SelectedIndices.Length-selectedLabCount>=7) {
							break;//we have found seven procedures.
						}
					}
					if(selectedIndices.FindAll(x => gridMain.Rows[x].Tag!=null).Count-selectedLabCount>7) {//only if they selected more than 7 procedures, not 7 rows.
						MsgBox.Show(this,"Only the first 7 procedures will be selected.  You will need to create another preauth for the remaining procedures.");
					}
				}
			}
			Claim ClaimCur=new Claim();
      FormInsPlanSelect FormIPS=new FormInsPlanSelect(PatCur.PatNum); 
			FormIPS.ViewRelat=true;
			if(FormIPS.SelectedPlan==null) {//Won't be null if there is only one PatPlan
				FormIPS.ShowDialog();
				if(FormIPS.DialogResult!=DialogResult.OK) {
					return;
				}
			}
			ClaimCur.PatNum=PatCur.PatNum;
			ClaimCur.ClaimNote="";
			ClaimCur.ClaimStatus="W";
			ClaimCur.DateSent=DateTimeOD.Today;
			ClaimCur.DateSentOrig=DateTime.MinValue;
			ClaimCur.PlanNum=FormIPS.SelectedPlan.PlanNum;
			ClaimCur.InsSubNum=FormIPS.SelectedSub.InsSubNum;
			ClaimCur.ProvTreat=0;
			ClaimCur.ClaimForm=FormIPS.SelectedPlan.ClaimFormNum;
			List<Procedure> listProcsSelected=new List<Procedure>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				if(gridMain.Rows[gridMain.SelectedIndices[i]].Tag==null){
					continue;//skip any hightlighted subtotal lines
				}
				Procedure proc=Procedures.GetOneProc(((ProcTP)gridMain.Rows[gridMain.SelectedIndices[i]].Tag).ProcNumOrig,false);
				if(Procedures.NoBillIns(proc,ClaimProcList,ClaimCur.PlanNum)) {
					MsgBox.Show(this,"Not allowed to send procedures to insurance that are marked 'Do not bill to ins'.");
					return;
				}
				if(proc.ProcNumLab!=0) {
					continue;//Ignore Canadian labs. Labs are indirectly attached to the claim through a parent procedure
				}
				listProcsSelected.Add(proc);
				if(ClaimCur.ProvTreat==0){//makes sure that at least one prov is set
					ClaimCur.ProvTreat=proc.ProvNum;
				}
				if(!Provider.GetById(proc.ProvNum).IsSecondary) {
					ClaimCur.ProvTreat=proc.ProvNum;
				}
			}
			//Make sure that all procedures share the same place of service and clinic.
			long procClinicNum=-1;
			PlaceOfService placeService=PlaceOfService.Office;//Old behavior was to always use Office.
			for(int i=0;i<listProcsSelected.Count;i++) {
				if(i==0) {
					procClinicNum=listProcsSelected[i].ClinicNum;
					placeService=listProcsSelected[i].PlaceService;
					continue;
				}
				Procedure proc=listProcsSelected[i];
				if(procClinicNum!=proc.ClinicNum) {
					MsgBox.Show(this,"All procedures do not have the same clinic.");
					return;
				}
				if(!Preference.GetBool(PreferenceName.EasyHidePublicHealth) && proc.PlaceService!=placeService) {
					MsgBox.Show(this,"All procedures do not have the same place of service.");
					return;
				}
			}
			switch(PIn.Enum<ClaimZeroDollarProcBehavior>(Preference.GetInt(PreferenceName.ClaimZeroDollarProcBehavior))) {
				case ClaimZeroDollarProcBehavior.Warn:
					if(listProcsSelected.FirstOrDefault(x => x.ProcFee.IsZero())!=null
						&& !MsgBox.Show("ContrTreat",MsgBoxButtons.OKCancel,"You are about to make a claim that will include a $0 procedure.  Continue?"))
					{
						return;
					}
					break;
				case ClaimZeroDollarProcBehavior.Block:
					if(listProcsSelected.FirstOrDefault(x => x.ProcFee.IsZero())!=null) {
						MsgBox.Show("ContrTreat","You can't make a claim for a $0 procedure.");
						return;
					}
					break;
				case ClaimZeroDollarProcBehavior.Allow:
				default:
					break;
			}
			//Make the clinic on the claim match the clinic of the procedures.  Use the patients clinic if no procedures selected (shouldn't happen).
			ClaimCur.ClinicNum=(procClinicNum > -1) ? procClinicNum : PatCur.ClinicNum;
			if(Provider.GetById(ClaimCur.ProvTreat).IsSecondary){
				ClaimCur.ProvTreat=PatCur.PriProv;
				//OK if 0, because auto select first in list when open claim
			}
			ClaimCur.ProvBill=Providers.GetBillingProviderId(ClaimCur.ProvTreat,ClaimCur.ClinicNum);
			Provider prov=Provider.GetById(ClaimCur.ProvBill);
			if(prov.BillingOverrideProviderId.HasValue) {
				ClaimCur.ProvBill=prov.BillingOverrideProviderId.Value;
			}
			ClaimCur.EmployRelated=YN.No;
      ClaimCur.ClaimType="PreAuth";
			ClaimCur.AttachedFlags="Mail";
			//this could be a little better if we automate figuring out the patrelat
			//instead of making the user enter it:
			ClaimCur.PatRelat=FormIPS.PatRelat;
			ClaimCur.PlaceService=placeService;
			Claims.Insert(ClaimCur);
			ClaimProc ClaimProcCur;
			ClaimProc cpExisting;
			List<ClaimProc> listClaimProcs=new List<ClaimProc>();
			List<long> listCodeNums=new List<long>();//List of codeNums that have had their default note added to the claim.
			foreach(Procedure procCur in listProcsSelected){
				cpExisting=ClaimProcs.GetEstimate(ClaimProcList,procCur.ProcNum,FormIPS.SelectedPlan.PlanNum,FormIPS.SelectedSub.InsSubNum);
				double insPayEst=0;
				if(cpExisting!=null) {
					insPayEst=cpExisting.InsPayEst;
				}
				ProcedureCode procCodeCur=ProcedureCodes.GetProcCode(procCur.CodeNum);
				ClaimProcCur=new ClaimProc();
				ClaimProcs.CreateEst(ClaimProcCur,procCur,FormIPS.SelectedPlan,FormIPS.SelectedSub,0,insPayEst,false,true);//preauth est
        ClaimProcCur.ClaimNum=ClaimCur.ClaimNum;
				ClaimProcCur.NoBillIns=(cpExisting!=null) ? cpExisting.NoBillIns : false;
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && cpExisting!=null) {
					//ClaimProc.Percentage is typically set in ClaimProcs.ComputeBaseEst(...), not for pre-auths. 
					ClaimProcCur.Percentage=cpExisting.Percentage;//Used for Canadian preauths with lab procedures.
				}
				ClaimProcCur.FeeBilled=procCur.ProcFee;
				if(FormIPS.SelectedPlan.UseAltCode && (procCodeCur.AlternateCode1!="")){
					ClaimProcCur.CodeSent=procCodeCur.AlternateCode1;
				}
				else if(FormIPS.SelectedPlan.IsMedical && procCur.MedicalCode!=""){
					ClaimProcCur.CodeSent=procCur.MedicalCode;
				}
				else{
					ClaimProcCur.CodeSent=ProcedureCodes.GetStringProcCode(procCur.CodeNum);
					if(ClaimProcCur.CodeSent.Length>5 && ClaimProcCur.CodeSent.Substring(0,1)=="D"){
						ClaimProcCur.CodeSent=ClaimProcCur.CodeSent.Substring(0,5);
					}
					if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
						if(ClaimProcCur.CodeSent.Length>5) { //In Canadian electronic claims, codes can contain letters or numbers and cannot be longer than 5 characters.
							ClaimProcCur.CodeSent=ClaimProcCur.CodeSent.Substring(0,5);
						}
					}
				}
				listClaimProcs.Add(ClaimProcCur);
				if(ClaimCur.ClaimNote==null) {
					ClaimCur.ClaimNote="";
				}
				if(!listCodeNums.Contains(procCodeCur.CodeNum)) {
					if(ClaimCur.ClaimNote.Length > 0 && !string.IsNullOrEmpty(procCodeCur.DefaultClaimNote)) {
						ClaimCur.ClaimNote+="\n";
					}
					ClaimCur.ClaimNote+=procCodeCur.DefaultClaimNote;
					listCodeNums.Add(procCodeCur.CodeNum);
				}
				//ProcCur.Update(ProcOld);
			}
			for(int i=0;i<listClaimProcs.Count;i++) {
				listClaimProcs[i].LineNumber=(byte)(i+1);
				ClaimProcs.Insert(listClaimProcs[i]);
			}
			ProcList=Procedures.Refresh(PatCur.PatNum);
			//ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
			Claims.CalculateAndUpdate(ProcList,InsPlanList,ClaimCur,PatPlanList,BenefitList,PatCur,SubList);
			FormClaimEdit FormCE=new FormClaimEdit(ClaimCur,PatCur,FamCur);
			//FormCE.CalculateEstimates(
			FormCE.IsNew=true;//this causes it to delete the claim if cancelling.
			FormCE.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}

		private void ToolBarMainDiscount_Click() {
			if(!new[] { TreatPlanStatus.Active,TreatPlanStatus.Inactive }.Contains(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus)) {
				MsgBox.Show(this,"You can only create discounts from a current TP, not a saved TP.");
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				gridMain.SetSelected(true);
			}
			List<Procedure> listProcs=Procedures.GetManyProc(gridMain.SelectedIndices.ToList()
				.FindAll(x => gridMain.Rows[x].Tag!=null)
				.Select(x => ((ProcTP)gridMain.Rows[x].Tag).ProcNumOrig)
				.ToList(),false);
			if(listProcs.Count<=0) {
				MsgBox.Show(this,"There are no procedures selected in the treatment plan. Please add to, or select from, procedures attached to the treatment plan before applying a discount");
				return;
			}
			FormTreatmentPlanDiscount FormTPD=new FormTreatmentPlanDiscount(listProcs);
			FormTPD.ShowDialog();
			if(FormTPD.DialogResult==DialogResult.OK) {
				long tpNum=_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum;
				ModuleSelected(PatCur.PatNum);//refreshes TPs
				for(int i=0;i<_listTreatPlans.Count;i++) {
					if(_listTreatPlans[i].TreatPlanNum==tpNum) {
						gridPlans.SetSelected(i,true);
					}
				}
				FillMain();
			}
		}

		private void gridPreAuth_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.ClaimView)) {
				return;
			}
			Claim claim=Claims.GetClaim(((Claim)ALPreAuth[e.Row]).ClaimNum);//gets attached images.
			if(claim==null) {
				MsgBox.Show(this,"The pre authorization has been deleted.");
				ModuleSelected(PatCur.PatNum);
				return;
			}
			FormClaimEdit FormC=new FormClaimEdit(claim,PatCur,FamCur);
      //FormClaimEdit2.IsPreAuth=true;
			FormC.ShowDialog();
			if(FormC.DialogResult!=DialogResult.OK){
				return;
			}
			ModuleSelected(PatCur.PatNum);    
		}

		private void gridPreAuth_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(_listTreatPlans==null 
				|| _listTreatPlans.Count==0
				|| (_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus!=TreatPlanStatus.Active 
				&& _listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus!=TreatPlanStatus.Inactive))
			{
				return;
			}
			gridMain.SetSelected(false);
			Claim ClaimCur=(Claim)ALPreAuth[e.Row];
			List<ClaimProc> ClaimProcsForClaim=ClaimProcs.RefreshForClaim(ClaimCur.ClaimNum);
			for(int i=0;i<gridMain.Rows.Count;i++){//ProcListTP.Length;i++){
				if(gridMain.Rows[i].Tag==null){
					continue;//must be a subtotal row
				}
				ProcTP procTP=(ProcTP)gridMain.Rows[i].Tag;
				//proc=(Procedure)gridMain.Rows[i].Tag;
				for(int j=0;j<ClaimProcsForClaim.Count;j++){
					if(procTP.ProcNumOrig==ClaimProcsForClaim[j].ProcNum){
						gridMain.SetSelected(i,true);
						CanadianSelectedRowHelper(procTP);
						break;
					}
				}
			}
		}

		private void butInsRem_Click(object sender,EventArgs e) {
			if(PatCur==null) {
				MsgBox.Show(this,"Please select a patient before attempting to view insurance remaining.");
				return;
			}
			FormInsRemain FormIR=new FormInsRemain(PatCur.PatNum);
			FormIR.ShowDialog();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			//Update all unscheduled procs and claimprocs on the active TP to use the currently selected date as the ProcDate
			RefreshModuleScreen();
		}
		
		private void butPlannedAppt_Click(object sender,EventArgs e) {
			if(PatCur==null) {
				MsgBox.Show(this,"Please select a Patient.");
				return;
			}
			if(!gridPlans.GetSelectedIndex().Between(0,_listTreatPlans.Count-1)
				|| _listTreatPlans[gridPlans.GetSelectedIndex()].TPStatus!=TreatPlanStatus.Active || gridMain.SelectedIndices.Count()==0) 
			{
				MsgBox.Show(this,"Please select at least one procedure on an Active treatment plan.");
				return;
			}
			//We only care about ShowAppointments in the ChartModuleComponentsToLoad, reduces Db calls
			ChartModuleComponentsToLoad chartComponents=new ChartModuleComponentsToLoad(false);
			chartComponents.ShowAppointments=true;
			//Makes sure ChartModules.rawApt is filled before calling ChartModules.GetPlannedApt
			ChartModules.GetProgNotes(PatCur.PatNum,false,chartComponents);
			int itemOrder=ChartModules.GetPlannedApt(PatCur.PatNum).Rows.Count+1;
			List<long> listSelectedProcNums=gridMain.SelectedGridRows
				.FindAll(x => x.Tag!=null && x.Tag.GetType()==typeof(ProcTP))//ProcTP's only
				.Select(x => ((ProcTP)(x.Tag)).ProcNumOrig).ToList();//get ProcNums
			//mimic calls to CreatePlannedAppt in ContrChart, no need for FillPlanned() since no gridPlanned
			AppointmentL.CreatePlannedAppt(PatCur,itemOrder,listSelectedProcNums);
		}

		private void dateTimeTP_CloseUp(object sender,EventArgs e) {
			//Update all unscheduled procs and claimprocs on the active TP to use the currently selected date as the ProcDate
			RefreshModuleScreen();
		}

		private void textNote_Leave(object sender,EventArgs e) {
			UpdateTPNoteIfNeeded();
		}

		private void textNote_TextChanged(object sender,EventArgs e) {
			HasNoteChanged=true;
		}

		///<summary>Saves TP note to the database if changes were made</summary>
		public void UpdateTPNoteIfNeeded() {
			if(textNote.ReadOnly
				|| !HasNoteChanged
				|| gridPlans.SelectedIndices.Length==0
				|| _listTreatPlans.Count<=gridPlans.SelectedIndices[0]) 
			{
				return;
			}
			_listTreatPlans[gridPlans.SelectedIndices[0]].Note=PIn.String(textNote.Text);
			TreatPlans.Update(_listTreatPlans[gridPlans.SelectedIndices[0]]);
			HasNoteChanged=false;
		}

		private void radioTreatPlanSortTooth_MouseClick(object sender,MouseEventArgs e) {
			FormOpenDental.IsTreatPlanSortByTooth=radioTreatPlanSortTooth.Checked;
			FillMainData(); //need to do this so that the data is refreshed as the order of the treatment plan may have changed.
			FillMainDisplay();
		}

		/// <summary></summary>
		private void UpdateToolbarButtons() {
			if(PatCur!=null && _listTreatPlans.Count>0) {
				gridMain.Enabled=true;
				tabShowSort.Enabled=true;
				listSetPr.Enabled=true;
				//panelSide.Enabled=true;
				ToolBarMain.Buttons["Discount"].Enabled=true;
				ToolBarMain.Buttons["PreAuth"].Enabled=true;
				ToolBarMain.Buttons["Update"].Enabled=true;
				//ToolBarMain.Buttons["Create"].Enabled=true;
				ToolBarMain.Buttons["Print"].Enabled=true;
				ToolBarMain.Buttons["Email"].Enabled=true;
				ToolBarMain.Buttons["Sign"].Enabled=true;
				butSaveTP.Enabled=true;
				ToolBarMain.Invalidate();
				if(PatPlanList.Count==0) {//patient doesn't have insurance
					checkShowIns.Checked=false;
					checkShowMaxDed.Visible=false;
					if(PatCur.DiscountPlanNum!=0) {
						checkShowIns.Checked=true;
					}
				}
				else {//patient has insurance
					if(!Preference.GetBool(PreferenceName.EasyHideInsurance)) {//if insurance isn't hidden
						checkShowMaxDed.Visible=true;
						if(checkShowFees.Checked) {//if fees are showing
							if(!checkShowInsNotAutomatic) {
								checkShowIns.Checked=true;
							}
							InsSub sub=InsSubs.GetSub(PatPlanList[0].InsSubNum,SubList);
							InsPlan plan=InsPlans.GetPlan(sub.PlanNum,InsPlanList);
						}
					}
				}
			}
			else {
				gridMain.Enabled=false;
				tabShowSort.Enabled=false;
				listSetPr.Enabled=false;
				butSaveTP.Enabled=false;
				//panelSide.Enabled=false;
				ToolBarMain.Buttons["Discount"].Enabled=false;
				ToolBarMain.Buttons["PreAuth"].Enabled=false;
				ToolBarMain.Buttons["Update"].Enabled=false;
				//ToolBarMain.Buttons["Create"].Enabled=false;
				ToolBarMain.Buttons["Print"].Enabled=false;
				ToolBarMain.Buttons["Email"].Enabled=false;
				ToolBarMain.Buttons["Sign"].Enabled=false;
				ToolBarMain.Invalidate();
				//listPreAuth.Enabled=false;
			}
		}
	}



	public class TpRow {
		public string Done;
		public string Priority;
		public string Tth;
		public string Surf;
		public string Code;
		public string Description;
		public string Prognosis;
		public string Dx;
		public string ProcAbbr;
		public decimal Fee;
		public decimal PriIns;
		public decimal SecIns;
		public decimal Discount;
		public decimal Pat;
		public decimal FeeAllowed;
		public System.Drawing.Color ColorText;
		public System.Drawing.Color ColorLborder;
		public bool Bold;
		public object Tag;
		public decimal TaxEst;
	}
}
