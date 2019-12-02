/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormOperatoryEdit : FormBase
    {
        private readonly Operatory operatory;
        private List<Provider> providers;

        /// <summary>
        /// Gets or sets the ID of the selected provider.
        /// </summary>
        private long? ProviderId
        {
            get
            {
                if (providerComboBox.SelectedItem is Provider provider)
                {
                    return provider.Id;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    for (int i = 1; i < providerComboBox.Items.Count; i++)
                    {
                        if (providerComboBox.Items[i] is Provider provider && provider.Id == value.Value)
                        {
                            providerComboBox.SelectedItem = provider;

                            return;
                        }
                    }
                }
                providerComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Gets or sets the ID of the selected hygienist.
        /// </summary>
        private long? HygienistId
        {
            get
            {
                if (hygienistComboBox.SelectedItem is Provider provider)
                {
                    return provider.Id;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    for (int i = 1; i < hygienistComboBox.Items.Count; i++)
                    {
                        if (hygienistComboBox.Items[i] is Provider provider && provider.Id == value.Value)
                        {
                            hygienistComboBox.SelectedItem = provider;

                            return;
                        }
                    }
                }
                providerComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Gets or sets the ID of the selected clinic.
        /// </summary>
        private long SelectedClinicId
        {
            get
            {
                if (clinicComboBox.SelectedItem is Clinic clinic)
                {
                    return clinic.Id;
                }

                return 0;
            }
            set
            {
                for (int i = 1; i < clinicComboBox.Items.Count; i++)
                {
                    if (clinicComboBox.Items[i] is Clinic clinic && clinic.Id == value)
                    {
                        clinicComboBox.SelectedIndex = i;

                        return;
                    }
                }

                clinicComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormOperatoryEdit"/> class.
        /// </summary>
        /// <param name="operatory">The operatory to edit.</param>
		public FormOperatoryEdit(Operatory operatory)
        {
            this.operatory = operatory;

            InitializeComponent();
        }

        private void FormOperatoryEdit_Load(object sender, EventArgs e)
        {
            nameTextBox.Text = operatory.Description;
            abbrevTextBox.Text = operatory.Abbr;
            hiddenCheckBox.Checked = operatory.IsHidden;

            LoadClinics();
            LoadProviders();

            hygieneCheckBox.Checked = operatory.IsHygiene;
            prospectiveCheckBox.Checked = operatory.IsProspective;
        }

        private void LoadClinics()
        {
            clinicComboBox.Items.Clear();

            foreach (var clinic in Clinic.GetByUser(Security.CurrentUser))
            {
                clinicComboBox.Items.Add(clinic);
                if (clinic.Id == operatory.ClinicId)
                {
                    clinicComboBox.SelectedItem = clinic;
                }
            }

            if (clinicComboBox.SelectedItem == null && 
                clinicComboBox.Items.Count > 0)
            {
                clinicComboBox.SelectedIndex = 0;
            }
        }

        private void LoadProviders()
        {
            providers = Provider.GetByClinic(SelectedClinicId).ToList();

            providerComboBox.Items.Add("None");

            hygienistComboBox.Items.Add("None");

            foreach (var provider in providers)
            {
                providerComboBox.Items.Add(provider);
                if (provider.Id == operatory.ProvDentistId)
                {
                    providerComboBox.SelectedItem = provider;
                }

                hygienistComboBox.Items.Add(provider);
                if (provider.Id == operatory.ProvHygienistId)
                {
                    hygienistComboBox.SelectedItem = provider;
                }
            }

            if (providerComboBox.SelectedItem == null)
                providerComboBox.SelectedIndex = 0;

            if (hygienistComboBox.SelectedItem == null)
                hygienistComboBox.SelectedIndex = 0;
        }

        private void PickClinicButton_Click(object sender, EventArgs e)
        {
            using (var formClinics = new FormClinics())
            {
                formClinics.IsSelectionMode = true;
                if (formClinics.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                SelectedClinicId = formClinics.SelectedClinicId;
            }
        }

        private void PickProviderButton_Click(object sender, EventArgs e)
        {
            using (var formProviderPick = new FormProviderPick(providers))
            { 
                formProviderPick.SelectedProvNum = ProviderId.GetValueOrDefault();
                if (formProviderPick.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                ProviderId = formProviderPick.SelectedProvNum;
            }
        }

        private void PickHygienistButton_Click(object sender, EventArgs e)
        {
            using (var formProviderPick = new FormProviderPick(providers))
            {
                formProviderPick.SelectedProvNum = HygienistId.GetValueOrDefault();
                if (formProviderPick.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                HygienistId = formProviderPick.SelectedProvNum;
            }
        }

        private void ClinicComboBox_SelectedIndexChanged(object sender, EventArgs e) => LoadProviders();

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            var name = nameTextBox.Text.Trim();
            if (name.Length == 0)
            {
                MessageBox.Show(
                    "Operatory name cannot be blank.",
                    "Operatories", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (hiddenCheckBox.Checked == true && Operatory.HasFutureApts(operatory.Id, ApptStatus.UnschedList))
            {
                MessageBox.Show(
                    "Can not hide an operatory with future appointments.", 
                    "Operatories", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                hiddenCheckBox.Checked = false;
                return;
            }

            operatory.Description = nameTextBox.Text;
            operatory.Abbr = abbrevTextBox.Text;
            operatory.IsHidden = hiddenCheckBox.Checked;
            operatory.ClinicId = SelectedClinicId;
            operatory.ProvDentistId = ProviderId;
            operatory.ProvHygienistId = HygienistId;
            operatory.IsHygiene = hygieneCheckBox.Checked;
            operatory.IsProspective = prospectiveCheckBox.Checked;

            if (operatory.IsNew)
            {
                Operatory.Insert(operatory);
            }
            else
            {
                Operatory.Update(operatory);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
