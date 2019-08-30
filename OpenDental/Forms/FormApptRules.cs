/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormApptRules : FormBase
    {
        bool hasChanged;
        List<AppointmentRule> appointmentRuleList;

        public FormApptRules() => InitializeComponent();

        void LoadAppointmentRules()
        {
            AppointmentRules.RefreshCache();

            appointmentRuleList = AppointmentRules.GetDeepCopy();

            rulesGrid.BeginUpdate();
            rulesGrid.Columns.Clear();
            rulesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDescription, 200));
            rulesGrid.Columns.Add(new ODGridColumn("Start Code", 100));
            rulesGrid.Columns.Add(new ODGridColumn("End Code", 100));
            rulesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnEnabled, 50, HorizontalAlignment.Center));
            rulesGrid.Rows.Clear();

            foreach (var appointmentRule in appointmentRuleList)
            {
                var row = new ODGridRow();
                row.Cells.Add(appointmentRule.RuleDesc);
                row.Cells.Add(appointmentRule.CodeStart);
                row.Cells.Add(appointmentRule.CodeEnd);
                row.Cells.Add(appointmentRule.IsEnabled ? "X" : "");

                rulesGrid.Rows.Add(row);
            }

            rulesGrid.EndUpdate();
        }

        void FormApptRules_Load(object sender, EventArgs e) => LoadAppointmentRules();

        void FormApptRules_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hasChanged)
            {
                DataValid.SetInvalid(InvalidType.Views);
            }
        }

        void RulesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formApptRuleEdit = new FormApptRuleEdit(appointmentRuleList[e.Row]))
            {
                formApptRuleEdit.ShowDialog();

                LoadAppointmentRules();

                hasChanged = true;
            }
        }

        void AddButton_Click(object sender, EventArgs e)
        {
            AppointmentRule aptRule = new AppointmentRule
            {
                IsEnabled = true
            };

            using (var formApptRuleEdit = new FormApptRuleEdit(aptRule))
            {
                formApptRuleEdit.IsNew = true;

                if (formApptRuleEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadAppointmentRules();

                    hasChanged = true;
                }
            }
        }

        void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}