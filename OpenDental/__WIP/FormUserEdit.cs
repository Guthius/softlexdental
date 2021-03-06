using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using System.Collections.Generic;
using System.DirectoryServices;
using CodeBase;

namespace OpenDental
{
    /// <summary>
    /// Summary description for FormBasicTemplate.
    /// </summary>
    public class FormUserEdit : ODForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        ///<summary></summary>
        public bool IsNew;
        ///<summary></summary>
        public User UserCur;
        private TabControl tabControl1;
        private TabPage tabUser;
        private TabPage tabClinics;
        private UI.Button butCancel;
        private UI.Button butOK;
        private UI.Button butPassword;
        private Label labelClinic;
        private ListBox listClinic;
        private Label label1;
        private Label label3;
        private ListBox listUserGroup;
        private TextBox textUserName;
        private Label label2;
        private ListBox listEmployee;
        private Label label4;
        private Label label5;
        private ListBox listProv;
        private CheckBox checkIsHidden;
        private Label label27;
        private TextBox textUserNum;
        private ListBox listClinicMulti;
        private Label label6;
        private CheckBox checkClinicIsRestricted;
        private List<UserGroup> _listUserGroups;
        private TabPage tabAlertSubs;
        private ListBox listAlertSubMulti;
        private Label label7;
        private List<AlertSubscription> _listUserAlertTypesOld;
        private Label labelAlertClinic;
        private ListBox listAlertSubsClinicsMulti;
        private List<Clinic> _listClinics;
        private UI.Button butUnlock;
        private TextBox textDomainUser;
        private Label labelDomainUser;
        private UI.Button butPickDomainUser;
        ///<summary>The password that was entered in FormUserPassword.</summary>
        private string _passwordTyped;
        private CheckBox checkRequireReset;
        private TextBox textDoseSpotUserID;
        private Label label8;
        private UI.Button butDoseSpotAdditional;

        ///<summary>The alert categories that are available to be selected. Some alert types will not be displayed if this is not OD HQ.</summary>
        private List<AlertCategory> _listAlertCategories;
        ///<summary>The UserOdPref for DoseSpot User ID.</summary>
        private UserPreference _doseSpotUserPrefDefault;
        private List<Employee> _listEmployees;
        private List<Provider> _listProviders;
        private bool _isFromAddUser;
        private List<UserPreference> _listDoseSpotUserPrefOld;
        private List<UserPreference> _listDoseSpotUserPrefNew;

        ///<summary></summary>
        public FormUserEdit(User userCur, bool isFromAddUser = false)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            UserCur = userCur.Copy();
            _isFromAddUser = isFromAddUser;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserEdit));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabUser = new System.Windows.Forms.TabPage();
            this.butDoseSpotAdditional = new OpenDental.UI.Button();
            this.textDoseSpotUserID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkRequireReset = new System.Windows.Forms.CheckBox();
            this.butPickDomainUser = new OpenDental.UI.Button();
            this.textDomainUser = new System.Windows.Forms.TextBox();
            this.labelDomainUser = new System.Windows.Forms.Label();
            this.textUserNum = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.checkIsHidden = new System.Windows.Forms.CheckBox();
            this.listProv = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listEmployee = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textUserName = new System.Windows.Forms.TextBox();
            this.listUserGroup = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabClinics = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.listClinicMulti = new System.Windows.Forms.ListBox();
            this.checkClinicIsRestricted = new System.Windows.Forms.CheckBox();
            this.listClinic = new System.Windows.Forms.ListBox();
            this.labelClinic = new System.Windows.Forms.Label();
            this.tabAlertSubs = new System.Windows.Forms.TabPage();
            this.labelAlertClinic = new System.Windows.Forms.Label();
            this.listAlertSubsClinicsMulti = new System.Windows.Forms.ListBox();
            this.listAlertSubMulti = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.butOK = new OpenDental.UI.Button();
            this.butCancel = new OpenDental.UI.Button();
            this.butPassword = new OpenDental.UI.Button();
            this.butUnlock = new OpenDental.UI.Button();
            this.tabControl1.SuspendLayout();
            this.tabUser.SuspendLayout();
            this.tabClinics.SuspendLayout();
            this.tabAlertSubs.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabUser);
            this.tabControl1.Controls.Add(this.tabClinics);
            this.tabControl1.Controls.Add(this.tabAlertSubs);
            this.tabControl1.Location = new System.Drawing.Point(12, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(614, 399);
            this.tabControl1.TabIndex = 0;
            // 
            // tabUser
            // 
            this.tabUser.BackColor = System.Drawing.SystemColors.Control;
            this.tabUser.Controls.Add(this.butDoseSpotAdditional);
            this.tabUser.Controls.Add(this.textDoseSpotUserID);
            this.tabUser.Controls.Add(this.label8);
            this.tabUser.Controls.Add(this.checkRequireReset);
            this.tabUser.Controls.Add(this.butPickDomainUser);
            this.tabUser.Controls.Add(this.textDomainUser);
            this.tabUser.Controls.Add(this.labelDomainUser);
            this.tabUser.Controls.Add(this.textUserNum);
            this.tabUser.Controls.Add(this.label27);
            this.tabUser.Controls.Add(this.checkIsHidden);
            this.tabUser.Controls.Add(this.listProv);
            this.tabUser.Controls.Add(this.label5);
            this.tabUser.Controls.Add(this.label4);
            this.tabUser.Controls.Add(this.listEmployee);
            this.tabUser.Controls.Add(this.label2);
            this.tabUser.Controls.Add(this.textUserName);
            this.tabUser.Controls.Add(this.listUserGroup);
            this.tabUser.Controls.Add(this.label3);
            this.tabUser.Controls.Add(this.label1);
            this.tabUser.Location = new System.Drawing.Point(4, 22);
            this.tabUser.Name = "tabUser";
            this.tabUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabUser.Size = new System.Drawing.Size(606, 373);
            this.tabUser.TabIndex = 0;
            this.tabUser.Text = "User";
            // 
            // butDoseSpotAdditional
            // 
            this.butDoseSpotAdditional.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.butDoseSpotAdditional.Autosize = false;
            this.butDoseSpotAdditional.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
            this.butDoseSpotAdditional.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
            this.butDoseSpotAdditional.CornerRadius = 4F;
            this.butDoseSpotAdditional.Location = new System.Drawing.Point(407, 67);
            this.butDoseSpotAdditional.Name = "butDoseSpotAdditional";
            this.butDoseSpotAdditional.Size = new System.Drawing.Size(23, 21);
            this.butDoseSpotAdditional.TabIndex = 173;
            this.butDoseSpotAdditional.Text = "...";
            this.butDoseSpotAdditional.Click += new System.EventHandler(this.butDoseSpotAdditional_Click);
            // 
            // textDoseSpotUserID
            // 
            this.textDoseSpotUserID.Location = new System.Drawing.Point(224, 68);
            this.textDoseSpotUserID.Name = "textDoseSpotUserID";
            this.textDoseSpotUserID.Size = new System.Drawing.Size(180, 20);
            this.textDoseSpotUserID.TabIndex = 172;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(224, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(177, 20);
            this.label8.TabIndex = 171;
            this.label8.Text = "DoseSpot User ID";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkRequireReset
            // 
            this.checkRequireReset.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkRequireReset.Location = new System.Drawing.Point(407, 48);
            this.checkRequireReset.Name = "checkRequireReset";
            this.checkRequireReset.Size = new System.Drawing.Size(192, 20);
            this.checkRequireReset.TabIndex = 170;
            this.checkRequireReset.Text = "Require Password Reset";
            this.checkRequireReset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkRequireReset.UseVisualStyleBackColor = true;
            // 
            // butPickDomainUser
            // 
            this.butPickDomainUser.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.butPickDomainUser.Autosize = false;
            this.butPickDomainUser.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
            this.butPickDomainUser.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
            this.butPickDomainUser.CornerRadius = 4F;
            this.butPickDomainUser.Location = new System.Drawing.Point(407, 25);
            this.butPickDomainUser.Name = "butPickDomainUser";
            this.butPickDomainUser.Size = new System.Drawing.Size(23, 21);
            this.butPickDomainUser.TabIndex = 169;
            this.butPickDomainUser.Text = "...";
            this.butPickDomainUser.Click += new System.EventHandler(this.butPickDomainUser_Click);
            // 
            // textDomainUser
            // 
            this.textDomainUser.Location = new System.Drawing.Point(224, 26);
            this.textDomainUser.Name = "textDomainUser";
            this.textDomainUser.ReadOnly = true;
            this.textDomainUser.Size = new System.Drawing.Size(180, 20);
            this.textDomainUser.TabIndex = 168;
            // 
            // labelDomainUser
            // 
            this.labelDomainUser.Location = new System.Drawing.Point(224, 6);
            this.labelDomainUser.Name = "labelDomainUser";
            this.labelDomainUser.Size = new System.Drawing.Size(177, 20);
            this.labelDomainUser.TabIndex = 167;
            this.labelDomainUser.Text = "Domain User";
            this.labelDomainUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textUserNum
            // 
            this.textUserNum.BackColor = System.Drawing.SystemColors.Control;
            this.textUserNum.Location = new System.Drawing.Point(31, 27);
            this.textUserNum.Name = "textUserNum";
            this.textUserNum.ReadOnly = true;
            this.textUserNum.Size = new System.Drawing.Size(182, 20);
            this.textUserNum.TabIndex = 165;
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(31, 9);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(72, 17);
            this.label27.TabIndex = 166;
            this.label27.Text = "User ID";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkIsHidden
            // 
            this.checkIsHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkIsHidden.Location = new System.Drawing.Point(495, 72);
            this.checkIsHidden.Name = "checkIsHidden";
            this.checkIsHidden.Size = new System.Drawing.Size(104, 16);
            this.checkIsHidden.TabIndex = 163;
            this.checkIsHidden.Text = "Is Hidden";
            this.checkIsHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkIsHidden.UseVisualStyleBackColor = true;
            // 
            // listProv
            // 
            this.listProv.Location = new System.Drawing.Point(414, 110);
            this.listProv.Name = "listProv";
            this.listProv.Size = new System.Drawing.Size(185, 225);
            this.listProv.TabIndex = 160;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(414, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 20);
            this.label5.TabIndex = 159;
            this.label5.Text = "Provider";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(232, 340);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(344, 23);
            this.label4.TabIndex = 158;
            this.label4.Text = "Setting employee or provider is entirely optional";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // listEmployee
            // 
            this.listEmployee.Location = new System.Drawing.Point(221, 110);
            this.listEmployee.Name = "listEmployee";
            this.listEmployee.Size = new System.Drawing.Size(185, 225);
            this.listEmployee.TabIndex = 157;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(221, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 20);
            this.label2.TabIndex = 156;
            this.label2.Text = "Employee (for timecards)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textUserName
            // 
            this.textUserName.Location = new System.Drawing.Point(31, 68);
            this.textUserName.Name = "textUserName";
            this.textUserName.Size = new System.Drawing.Size(182, 20);
            this.textUserName.TabIndex = 152;
            // 
            // listUserGroup
            // 
            this.listUserGroup.Location = new System.Drawing.Point(28, 110);
            this.listUserGroup.Name = "listUserGroup";
            this.listUserGroup.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listUserGroup.Size = new System.Drawing.Size(185, 225);
            this.listUserGroup.TabIndex = 154;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(28, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 20);
            this.label3.TabIndex = 153;
            this.label3.Text = "User Group";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(31, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 20);
            this.label1.TabIndex = 151;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabClinics
            // 
            this.tabClinics.BackColor = System.Drawing.SystemColors.Control;
            this.tabClinics.Controls.Add(this.label6);
            this.tabClinics.Controls.Add(this.listClinicMulti);
            this.tabClinics.Controls.Add(this.checkClinicIsRestricted);
            this.tabClinics.Controls.Add(this.listClinic);
            this.tabClinics.Controls.Add(this.labelClinic);
            this.tabClinics.Location = new System.Drawing.Point(4, 22);
            this.tabClinics.Name = "tabClinics";
            this.tabClinics.Padding = new System.Windows.Forms.Padding(3);
            this.tabClinics.Size = new System.Drawing.Size(606, 373);
            this.tabClinics.TabIndex = 1;
            this.tabClinics.Text = "Clinics";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(329, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 20);
            this.label6.TabIndex = 169;
            this.label6.Text = "User Restricted Clinics";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listClinicMulti
            // 
            this.listClinicMulti.Location = new System.Drawing.Point(329, 66);
            this.listClinicMulti.Name = "listClinicMulti";
            this.listClinicMulti.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listClinicMulti.Size = new System.Drawing.Size(250, 225);
            this.listClinicMulti.TabIndex = 168;
            // 
            // checkClinicIsRestricted
            // 
            this.checkClinicIsRestricted.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkClinicIsRestricted.Location = new System.Drawing.Point(329, 297);
            this.checkClinicIsRestricted.Name = "checkClinicIsRestricted";
            this.checkClinicIsRestricted.Size = new System.Drawing.Size(250, 52);
            this.checkClinicIsRestricted.TabIndex = 167;
            this.checkClinicIsRestricted.Text = "Restrict user to only see these clinics";
            this.checkClinicIsRestricted.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkClinicIsRestricted.UseVisualStyleBackColor = true;
            // 
            // listClinic
            // 
            this.listClinic.Location = new System.Drawing.Point(28, 66);
            this.listClinic.Name = "listClinic";
            this.listClinic.Size = new System.Drawing.Size(250, 225);
            this.listClinic.TabIndex = 166;
            this.listClinic.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listClinic_MouseClick);
            // 
            // labelClinic
            // 
            this.labelClinic.Location = new System.Drawing.Point(28, 43);
            this.labelClinic.Name = "labelClinic";
            this.labelClinic.Size = new System.Drawing.Size(150, 20);
            this.labelClinic.TabIndex = 165;
            this.labelClinic.Text = "User Default Clinic";
            this.labelClinic.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tabAlertSubs
            // 
            this.tabAlertSubs.BackColor = System.Drawing.SystemColors.Control;
            this.tabAlertSubs.Controls.Add(this.labelAlertClinic);
            this.tabAlertSubs.Controls.Add(this.listAlertSubsClinicsMulti);
            this.tabAlertSubs.Controls.Add(this.listAlertSubMulti);
            this.tabAlertSubs.Controls.Add(this.label7);
            this.tabAlertSubs.Location = new System.Drawing.Point(4, 22);
            this.tabAlertSubs.Name = "tabAlertSubs";
            this.tabAlertSubs.Size = new System.Drawing.Size(606, 373);
            this.tabAlertSubs.TabIndex = 2;
            this.tabAlertSubs.Text = "Alert Subs";
            // 
            // labelAlertClinic
            // 
            this.labelAlertClinic.Location = new System.Drawing.Point(329, 43);
            this.labelAlertClinic.Name = "labelAlertClinic";
            this.labelAlertClinic.Size = new System.Drawing.Size(150, 20);
            this.labelAlertClinic.TabIndex = 171;
            this.labelAlertClinic.Text = "Clinics Subscribed";
            this.labelAlertClinic.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listAlertSubsClinicsMulti
            // 
            this.listAlertSubsClinicsMulti.Location = new System.Drawing.Point(329, 66);
            this.listAlertSubsClinicsMulti.Name = "listAlertSubsClinicsMulti";
            this.listAlertSubsClinicsMulti.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listAlertSubsClinicsMulti.Size = new System.Drawing.Size(250, 225);
            this.listAlertSubsClinicsMulti.TabIndex = 170;
            // 
            // listAlertSubMulti
            // 
            this.listAlertSubMulti.Location = new System.Drawing.Point(28, 66);
            this.listAlertSubMulti.Name = "listAlertSubMulti";
            this.listAlertSubMulti.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listAlertSubMulti.Size = new System.Drawing.Size(250, 225);
            this.listAlertSubMulti.TabIndex = 168;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(28, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(167, 20);
            this.label7.TabIndex = 167;
            this.label7.Text = "User Alert Subscriptions";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // butOK
            // 
            this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Autosize = true;
            this.butOK.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
            this.butOK.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
            this.butOK.CornerRadius = 4F;
            this.butOK.Location = new System.Drawing.Point(470, 421);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 26);
            this.butOK.TabIndex = 150;
            this.butOK.Text = "&OK";
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Autosize = true;
            this.butCancel.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
            this.butCancel.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
            this.butCancel.CornerRadius = 4F;
            this.butCancel.Location = new System.Drawing.Point(551, 421);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 26);
            this.butCancel.TabIndex = 149;
            this.butCancel.Text = "&Cancel";
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butPassword
            // 
            this.butPassword.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.butPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butPassword.Autosize = true;
            this.butPassword.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
            this.butPassword.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
            this.butPassword.CornerRadius = 4F;
            this.butPassword.Location = new System.Drawing.Point(16, 421);
            this.butPassword.Name = "butPassword";
            this.butPassword.Size = new System.Drawing.Size(103, 26);
            this.butPassword.TabIndex = 155;
            this.butPassword.Text = "Change Password";
            this.butPassword.Click += new System.EventHandler(this.butPassword_Click);
            // 
            // butUnlock
            // 
            this.butUnlock.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.butUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butUnlock.Autosize = true;
            this.butUnlock.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
            this.butUnlock.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
            this.butUnlock.CornerRadius = 4F;
            this.butUnlock.Location = new System.Drawing.Point(142, 421);
            this.butUnlock.Name = "butUnlock";
            this.butUnlock.Size = new System.Drawing.Size(103, 26);
            this.butUnlock.TabIndex = 168;
            this.butUnlock.Text = "Unlock Account";
            this.butUnlock.Click += new System.EventHandler(this.butUnlock_Click);
            // 
            // FormUserEdit
            // 
            this.ClientSize = new System.Drawing.Size(638, 452);
            this.Controls.Add(this.butUnlock);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butPassword);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUserEdit";
            this.ShowInTaskbar = false;
            this.Text = "User Edit";
            this.Load += new System.EventHandler(this.FormUserEdit_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabUser.ResumeLayout(false);
            this.tabUser.PerformLayout();
            this.tabClinics.ResumeLayout(false);
            this.tabAlertSubs.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void FormUserEdit_Load(object sender, System.EventArgs e)
        {
            checkIsHidden.Checked = UserCur.Hidden;
            if (UserCur.Id != 0)
            {
                textUserNum.Text = UserCur.Id.ToString();
            }
            textUserName.Text = UserCur.UserName;
            textDomainUser.Text = UserCur.DomainUser;
            if (!Preference.GetBool(PreferenceName.DomainLoginEnabled))
            {
                labelDomainUser.Visible = false;
                textDomainUser.Visible = false;
                butPickDomainUser.Visible = false;
            }
            checkRequireReset.Checked = UserCur.PasswordResetRequired;
            _listUserGroups = UserGroup.All();
            for (int i = 0; i < _listUserGroups.Count; i++)
            {
                listUserGroup.Items.Add(new ODBoxItem<UserGroup>(_listUserGroups[i].Description, _listUserGroups[i]));
                if (!_isFromAddUser && UserCur.IsInUserGroup(_listUserGroups[i].Id))
                {
                    listUserGroup.SetSelected(i, true);
                }
                if (_isFromAddUser && _listUserGroups[i].Id == Preference.GetLong(PreferenceName.DefaultUserGroup))
                {
                    listUserGroup.SetSelected(i, true);
                }
            }
            if (listUserGroup.SelectedIndex == -1)
            {//never allowed to delete last group, so this won't fail
                listUserGroup.SelectedIndex = 0;
            }
            listEmployee.Items.Clear();
            listEmployee.Items.Add(Lan.g(this, "none"));
            listEmployee.SelectedIndex = 0;
            _listEmployees = Employee.All();
            for (int i = 0; i < _listEmployees.Count; i++)
            {
                listEmployee.Items.Add(Employee.GetNameFL(_listEmployees[i]));
                if (UserCur.EmployeeId == _listEmployees[i].Id)
                {
                    listEmployee.SelectedIndex = i + 1;
                }
            }
            listProv.Items.Clear();
            listProv.Items.Add("none");
            listProv.SelectedIndex = 0;
            _listProviders = Provider.All().ToList();
            for (int i = 0; i < _listProviders.Count; i++)
            {
                listProv.Items.Add(_listProviders[i].GetLongDesc());
                if (UserCur.ProviderId == _listProviders[i].Id)
                {
                    listProv.SelectedIndex = i + 1;
                }
            }
            _listClinics = Clinic.All().ToList();
            _listUserAlertTypesOld = AlertSubscription.GetByUser(UserCur.Id);
            List<long> listSubscribedClinics = _listUserAlertTypesOld.Where(x => x.ClinicId.HasValue).Select(x => x.ClinicId.Value).Distinct().ToList();
            List<long> listAlertCatNums = _listUserAlertTypesOld.Select(x => x.AlertCategoryId).Distinct().ToList();
            bool isAllClinicsSubscribed = listSubscribedClinics.Count == _listClinics.Count + 1;//Plus 1 for HQ
            listAlertSubMulti.Items.Clear();
            _listAlertCategories = AlertCategory.All();
            List<long> listUserAlertCatNums = _listUserAlertTypesOld.Select(x => x.AlertCategoryId).ToList();
            foreach (AlertCategory cat in _listAlertCategories)
            {
                int index = listAlertSubMulti.Items.Add(Lan.g(this, cat.Description));
                listAlertSubMulti.SetSelected(index, listUserAlertCatNums.Contains(cat.Id));
            }

            listClinic.Items.Clear();
            listClinic.Items.Add(Lan.g(this, "All"));
            listAlertSubsClinicsMulti.Items.Add(Lan.g(this, "All"));
            listAlertSubsClinicsMulti.Items.Add(Lan.g(this, "Headquarters"));
            if (UserCur.ClinicId == 0)
            {//Unrestricted
                listClinic.SetSelected(0, true);
                checkClinicIsRestricted.Enabled = false;//We don't really need this checkbox any more but it's probably better for users to keep it....
            }
            if (isAllClinicsSubscribed)
            {//They are subscribed to all clinics
                listAlertSubsClinicsMulti.SetSelected(0, true);
            }
            else if (listSubscribedClinics.Contains(0))
            {//They are subscribed to Headquarters
                listAlertSubsClinicsMulti.SetSelected(1, true);
            }
            List<ClinicUser> listUserClinics = ClinicUser.GetForUser(UserCur.Id).ToList();
            for (int i = 0; i < _listClinics.Count; i++)
            {
                listClinic.Items.Add(_listClinics[i].Abbr);
                listClinicMulti.Items.Add(_listClinics[i].Abbr);
                listAlertSubsClinicsMulti.Items.Add(_listClinics[i].Abbr);
                if (UserCur.ClinicId == _listClinics[i].Id)
                {
                    listClinic.SetSelected(i + 1, true);
                }
                if (UserCur.ClinicId != 0 && listUserClinics.Exists(x => x.ClinicId == _listClinics[i].Id))
                {
                    listClinicMulti.SetSelected(i, true);//No "All" option, don't select i+1
                }
                if (!isAllClinicsSubscribed && _listUserAlertTypesOld.Exists(x => x.ClinicId == _listClinics[i].Id))
                {
                    listAlertSubsClinicsMulti.SetSelected(i + 2, true);//All+HQ
                }
            }
            checkClinicIsRestricted.Checked = UserCur.ClinicRestricted;

            if (string.IsNullOrEmpty(UserCur.PasswordHash))
            {
                butPassword.Text = Lan.g(this, "Create Password");
            }
            if (IsNew)
            {
                butUnlock.Visible = false;
            }
            //_listDoseSpotUserPrefOld=UserOdPrefs.GetByUserAndFkeyAndFkeyType(UserCur.UserNum,
            //	Programs.GetCur(ProgramName.eRx).ProgramNum,UserOdFkeyType.Program,
            //	Clinics.GetForUserod(Security.CurUser,true).Select(x => x.ClinicNum)
            //	.Union(new List<long>() { 0 })//Always include 0 clinic, this is the default, NOT a headquarters only value.
            //	.Distinct()
            //	.ToList());
            //_listDoseSpotUserPrefNew=_listDoseSpotUserPrefOld.Select(x => x.Clone()).ToList();
            //_doseSpotUserPrefDefault=_listDoseSpotUserPrefNew.Find(x => x.ClinicNum==0);
            //if(_doseSpotUserPrefDefault==null) {
            //	_doseSpotUserPrefDefault=DoseSpot.GetDoseSpotUserIdFromPref(UserCur.UserNum,0);
            //	_listDoseSpotUserPrefNew.Add(_doseSpotUserPrefDefault);
            //}
            //textDoseSpotUserID.Text=_doseSpotUserPrefDefault.ValueString;
            if (_isFromAddUser && !Security.IsAuthorized(Permissions.SecurityAdmin, true))
            {
                butPassword.Visible = false;
                checkRequireReset.Checked = true;
                checkRequireReset.Enabled = false;
                butUnlock.Visible = false;
            }
        }

        private void butPickDomainUser_Click(object sender, EventArgs e)
        {
            //DirectoryEntry does recognize an empty string as a valid LDAP entry and will just return all logins from all available domains
            //But all logins should be on the same domain, so this field is required
            if (string.IsNullOrWhiteSpace(Preference.GetString(PreferenceName.DomainLoginPath)))
            {
                MsgBox.Show(this, "DomainLoginPath is missing in security settings. DomainLoginPath is required before assigning domain logins to user accounts.");
                return;
            }
            //Try to access the specified DomainLoginPath
            try
            {
                DirectoryEntry.Exists(Preference.GetString(PreferenceName.DomainLoginPath));
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lan.g(this, "An error occurred while attempting to access the provided DomainLoginPath:") + " " + ex.Message);
                return;
            }
            FormDomainUserPick FormDU = new FormDomainUserPick();
            FormDU.ShowDialog();
            if (FormDU.DialogResult == DialogResult.OK && FormDU.SelectedDomainName != null)
            { //only check for null, as empty string should clear the field
                UserCur.DomainUser = FormDU.SelectedDomainName;
                textDomainUser.Text = UserCur.DomainUser;
            }
        }

        private void listClinic_MouseClick(object sender, MouseEventArgs e)
        {
            int idx = listClinic.IndexFromPoint(e.Location);
            if (idx == -1)
            {
                return;
            }
            if (idx == 0)
            {//all
                checkClinicIsRestricted.Checked = false;
                checkClinicIsRestricted.Enabled = false;
            }
            else
            {
                checkClinicIsRestricted.Enabled = true;
            }
        }

        private void butPassword_Click(object sender, System.EventArgs e)
        {
            bool isCreate = string.IsNullOrEmpty(UserCur.PasswordHash);
            FormUserPassword FormU = new FormUserPassword(isCreate, UserCur.UserName);
            FormU.IsInSecurityWindow = true;
            FormU.ShowDialog();
            if (FormU.DialogResult == DialogResult.Cancel)
            {
                return;
            }
            UserCur.Password = FormU.LoginDetails;
            UserCur.PasswordIsStrong = FormU.PasswordIsStrong;
            _passwordTyped = FormU.Password;
            if (string.IsNullOrEmpty(UserCur.PasswordHash))
            {
                butPassword.Text = Lan.g(this, "Create Password");
            }
            else
            {
                butPassword.Text = Lan.g(this, "Change Password");
            }
        }

        private void butUnlock_Click(object sender, EventArgs e)
        {
            if (!MsgBox.Show(this, true, "Users can become locked when invalid credentials have been entered several times in a row.\r\n"
                + "Unlock this user so that more log in attempts can be made?"))
            {
                return;
            }
            UserCur.FailedDateTime = DateTime.MinValue;
            UserCur.FailedAttempts = 0;
            try
            {
                User.Update(UserCur);
                MsgBox.Show(this, "User has been unlocked.");
            }
            catch (Exception)
            {
                MsgBox.Show(this, "There was a problem unlocking this user.  Please call support or wait the allotted lock time.");
            }
        }

        private void butDoseSpotAdditional_Click(object sender, EventArgs e)
        {
            _doseSpotUserPrefDefault.Value = textDoseSpotUserID.Text;
            FormUserPrefAdditional FormUP = new FormUserPrefAdditional(_listDoseSpotUserPrefNew, UserCur);
            FormUP.ShowDialog();
            if (FormUP.DialogResult == DialogResult.OK)
            {
                _listDoseSpotUserPrefNew = FormUP.ListUserPrefOut;
                _doseSpotUserPrefDefault = _listDoseSpotUserPrefNew.Find(x => x.ClinicId == 0);
                textDoseSpotUserID.Text = _doseSpotUserPrefDefault.Value;
            }
        }

        private void butOK_Click(object sender, System.EventArgs e)
        {
            if (textUserName.Text == "")
            {
                MsgBox.Show(this, "Please enter a username.");
                return;
            }
            if (IsNew && Preference.GetBool(PreferenceName.PasswordsMustBeStrong) && string.IsNullOrWhiteSpace(_passwordTyped))
            {
                MsgBox.Show(this, "Password may not be blank when the strong password feature is turned on.");
                return;
            }
            if (listClinic.SelectedIndex == -1)
            {
                MsgBox.Show(this, "This user does not have a User Default Clinic set.  Please choose one to continue.");
                return;
            }
            if (listUserGroup.SelectedIndices.Count == 0)
            {
                MsgBox.Show(this, "Users must have at least one user group associated. Please select a user group to continue.");
                return;
            }
            if (_isFromAddUser && !Security.IsAuthorized(Permissions.SecurityAdmin, true)
                && (listUserGroup.SelectedItems.Count != 1 || listUserGroup.SelectedTag<UserGroup>().Id != Preference.GetLong(PreferenceName.DefaultUserGroup)))
            {
                MsgBox.Show(this, "This user must be assigned to the default user group.");
                for (int i = 0; i < listUserGroup.Items.Count; i++)
                {
                    if (((ODBoxItem<UserGroup>)listUserGroup.Items[i]).Tag.Id == Preference.GetLong(PreferenceName.DefaultUserGroup))
                    {
                        listUserGroup.SetSelected(i, true);
                    }
                    else
                    {
                        listUserGroup.SetSelected(i, false);
                    }
                }
                return;
            }
            List<ClinicUser> listUserClinics = new List<ClinicUser>();
            if (checkClinicIsRestricted.Checked)
            {//They want to restrict the user to certain clinics or clinics are enabled.  
                for (int i = 0; i < listClinicMulti.SelectedIndices.Count; i++)
                {
                    listUserClinics.Add(new ClinicUser(_listClinics[listClinicMulti.SelectedIndices[i]].Id, UserCur.Id));
                }
                //If they set the user up with a default clinic and it's not in the restricted list, return.
                if (!listUserClinics.Exists(x => x.ClinicId == _listClinics[listClinic.SelectedIndex - 1].Id))
                {
                    MsgBox.Show(this, "User cannot have a default clinic that they are not restricted to.");
                    return;
                }
            }
            // TODO: Fix me...
            //if(UserClinic.Sync(listUserClinics,UserCur.Id)) {//Either syncs new list, or clears old list if no longer restricted.
            //	DataValid.SetInvalid(InvalidType.UserClinics);
            //}
            if (listClinic.SelectedIndex == 0)
            {
                UserCur.ClinicId = 0;
            }
            else
            {
                UserCur.ClinicId = _listClinics[listClinic.SelectedIndex - 1].Id;
            }
            //UserCur.ClinicRestricted=checkClinicIsRestricted.Checked;//This is kept in sync with their choice of "All".
            UserCur.Hidden = checkIsHidden.Checked;
            UserCur.PasswordResetRequired = checkRequireReset.Checked;
            UserCur.UserName = textUserName.Text;
            if (listEmployee.SelectedIndex == 0)
            {
                UserCur.EmployeeId = 0;
            }
            else
            {
                UserCur.EmployeeId = _listEmployees[listEmployee.SelectedIndex - 1].Id;
            }

            if (listProv.SelectedIndex == 0)
            {
                UserCur.ProviderId = null;
            }
            else
            {
                UserCur.ProviderId = _listProviders[listProv.SelectedIndex - 1].Id;
            }

            try
            {
                if (IsNew)
                {
                    User.Insert(UserCur, listUserGroup.SelectedItems.OfType<ODBoxItem<UserGroup>>().Select(x => x.Tag.Id).ToList());
                    //Set the userodprefs to the new user's UserNum that was just retreived from the database.
                    _listDoseSpotUserPrefNew.ForEach(x => x.UserId = UserCur.Id);
                    SecurityLog.Write(null, SecurityLogEvents.AddNewUser, "New user '" + UserCur.UserName + "' added");
                }
                else
                {
                    List<UserGroup> listNewUserGroups = listUserGroup.SelectedItems.OfType<ODBoxItem<UserGroup>>().Select(x => x.Tag).ToList();
                    List<UserGroup> listOldUserGroups = UserCur.GetGroups();
                    User.Update(UserCur, listNewUserGroups.Select(x => x.Id).ToList());
                    //if this is the current user, update the user, credentials, etc.
                    if (UserCur.Id == Security.CurrentUser.Id)
                    {
                        Security.CurrentUser = UserCur.Copy();
                        if (_passwordTyped != null)
                        {
                            Security.PasswordTyped = _passwordTyped; //update the password typed for middle tier refresh
                        }
                    }
                    //Log changes to the User's UserGroups.
                    Func<List<UserGroup>, List<UserGroup>, List<UserGroup>> funcGetMissing = (listCur, listCompare) =>
                    {
                        List<UserGroup> retVal = new List<UserGroup>();
                        foreach (UserGroup group in listCur)
                        {
                            if (listCompare.Exists(x => x.Id == group.Id))
                            {
                                continue;
                            }
                            retVal.Add(group);
                        }
                        return retVal;
                    };
                    List<UserGroup> listRemovedGroups = funcGetMissing(listOldUserGroups, listNewUserGroups);
                    List<UserGroup> listAddedGroups = funcGetMissing(listNewUserGroups, listOldUserGroups);
                    if (listRemovedGroups.Count > 0)
                    {//Only log if there are items in the list
                        SecurityLog.Write(null, SecurityLogEvents.SecurityAdmin, "User " + UserCur.UserName +
                            " removed from User group(s): " + string.Join(", ", listRemovedGroups.Select(x => x.Description).ToArray()) + " by: " + Security.CurrentUser.UserName);
                    }
                    if (listAddedGroups.Count > 0)
                    {//Only log if there are items in the list.
                        SecurityLog.Write(null, SecurityLogEvents.SecurityAdmin, "User " + UserCur.UserName +
                            " added to User group(s): " + string.Join(", ", listAddedGroups.Select(x => x.Description).ToArray()) + " by: " + Security.CurrentUser.UserName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            //DoseSpot User ID Insert/Update/Delete
            //if(_doseSpotUserPrefDefault.Value!=textDoseSpotUserID.Text) {
            //	if(string.IsNullOrWhiteSpace(textDoseSpotUserID.Text)) {
            //		UserOdPrefs.DeleteMany(_doseSpotUserPrefDefault.UserId,_doseSpotUserPrefDefault.Fkey,UserPreferenceName.Program);
            //	}
            //	else {
            //		_doseSpotUserPrefDefault.Value=textDoseSpotUserID.Text;
            //		UserOdPrefs.Upsert(_doseSpotUserPrefDefault);
            //	}
            //}
            DataValid.SetInvalid(InvalidType.Security);
            //List of AlertTypes that are selected.
            List<long> listUserAlertCats = new List<long>();
            foreach (int index in listAlertSubMulti.SelectedIndices)
            {
                listUserAlertCats.Add(_listAlertCategories[index].Id);
            }
            List<long> listClinics = new List<long>();
            foreach (int index in listAlertSubsClinicsMulti.SelectedIndices)
            {
                if (index == 0)
                {//All
                    listClinics.Add(0);//Add HQ
                    foreach (Clinic clinicCur in _listClinics)
                    {
                        listClinics.Add(clinicCur.Id);
                    }
                    break;
                }
                if (index == 1)
                {//HQ
                    listClinics.Add(0);
                    continue;
                }
                Clinic clinic = _listClinics[index - 2];//Subtract 2 for 'All' and 'HQ'
                listClinics.Add(clinic.Id);
            }
            List<AlertSubscription> _listUserAlertTypesNew = new List<AlertSubscription>(_listUserAlertTypesOld);
            //Remove AlertTypes that have been deselected through either deslecting the type or clinic.
            _listUserAlertTypesNew.RemoveAll(x => !listUserAlertCats.Contains(x.AlertCategoryId));

            _listUserAlertTypesNew.RemoveAll(x => !listClinics.Contains(x.ClinicId.GetValueOrDefault()));

            foreach (long alertCatNum in listUserAlertCats)
            {
                foreach (long clinicNumCur in listClinics)
                {
                    if (!_listUserAlertTypesOld.Exists(x => x.ClinicId == clinicNumCur && x.AlertCategoryId == alertCatNum))
                    {//Was not subscribed to type.
                        _listUserAlertTypesNew.Add(new AlertSubscription(UserCur.Id, clinicNumCur, alertCatNum));
                        continue;
                    }
                }

            }

            // TODO: AlertSubs.Sync(_listUserAlertTypesNew,_listUserAlertTypesOld);
            //UserOdPrefs.Sync(_listDoseSpotUserPrefNew,_listDoseSpotUserPrefOld);
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
