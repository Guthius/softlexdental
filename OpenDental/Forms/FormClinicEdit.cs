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
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormClinicEdit : FormBase
    {
        private readonly List<DefLink> specialityLinks = new List<DefLink>();
        private readonly List<DefLink> removedSpecialityLinks = new List<DefLink>();
        private List<Provider> providers;
        private long emailAddressId = 0;
        private readonly Clinic clinic;

        /// <summary>
        /// True if an HL7Def is enabled with the type HL7InternalType.MedLabv2_3, otherwise false.
        /// </summary>
        private bool medLabHL7DefEnabled;

        /// <summary>
        /// Gets or sets the ID of the selected default provider.
        /// </summary>
        private long? SelectedDefaultProviderId
        {
            get
            {
                if (defaultProviderComboBox.SelectedItem is Provider provider)
                {
                    return provider.ProvNum;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    foreach (Provider provider in defaultProviderComboBox.Items)
                    {
                        if (provider.ProvNum == value.Value)
                        {
                            defaultProviderComboBox.SelectedItem = provider;

                            return;
                        }
                    }
                }
                defaultProviderComboBox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Gets or sets the ID of the selected default provider for insurance billing.
        /// </summary>
        private long? SelectedInsuranceBillingProviderId
        {
            get
            {
                if (insBillingProviderSpecificRadioButton.Checked && 
                    insBillingProviderComboBox.SelectedItem is Provider provider)
                {
                    return provider.ProvNum;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    foreach (Provider provider in insBillingProviderComboBox.Items)
                    {
                        if (provider.ProvNum == value.Value)
                        {
                            insBillingProviderSpecificRadioButton.Checked = true;
                            insBillingProviderComboBox.SelectedItem = provider;

                            return;
                        }
                    }
                }
                insBillingProviderTreatRadioButton.Checked = true;
                insBillingProviderComboBox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Gets the ID of the selected region.
        /// </summary>
        public long? SelectedRegionId
        {
            get
            {
                if (regionComboBox.SelectedItem is Definition definition)
                {
                    return definition.Id;
                }
                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormClinicEdit"/> class.
        /// </summary>
        /// <param name="clinic">The clinic.</param>
        public FormClinicEdit(Clinic clinic)
        {
            InitializeComponent();

            this.clinic = clinic;
        }

        private void FormClinicEdit_Load(object sender, EventArgs e)
        {
            medicalOnlyCheckBox.Checked = clinic.IsMedicalOnly;
            idTextBox.Text = clinic.IsNew ? "New" : clinic.Id.ToString();
            descriptionTextBox.Text = clinic.Description;
            abbrTextBox.Text = clinic.Abbr;
            phoneTextBox.Text = TelephoneNumbers.ReFormat(clinic.Phone);
            faxTextBox.Text = TelephoneNumbers.ReFormat(clinic.Fax);
            useBillingAddressOnClaimsCheckBox.Checked = clinic.UseBillingAddressOnClaims;
            excludeFromInsVerifyListCheckBox.Checked = clinic.ExcludeFromInsuranceVerification;

            if (Preference.GetBool(PreferenceName.RxHasProc))
            {
                procedureCodeRequiredCheckBox.Enabled = true;
                procedureCodeRequiredCheckBox.Checked = (clinic.IsNew || clinic.RequireProcedureOnRx);
            }

            hiddenCheckBox.Checked = clinic.IsHidden;
            textAddress.Text = clinic.AddressLine1;
            textAddress2.Text = clinic.AddressLine2;
            textCity.Text = clinic.City;
            textState.Text = clinic.State;
            textZip.Text = clinic.Zip;
            textBillingAddress.Text = clinic.BillingAddressLine1;
            textBillingAddress2.Text = clinic.BillingAddressLine2;
            textBillingCity.Text = clinic.BillingCity;
            textBillingST.Text = clinic.BillingState;
            textBillingZip.Text = clinic.BillingZip;
            textPayToAddress.Text = clinic.PayToAddressLine1;
            textPayToAddress2.Text = clinic.PayToAddressLine2;
            textPayToCity.Text = clinic.PayToCity;
            textPayToST.Text = clinic.PayToState;
            textPayToZip.Text = clinic.PayToZip;
            bankNumberTextBox.Text = clinic.BankNumber;
            schedulingNoteTextBox.Text = clinic.SchedulingNote;
            placeOfServiceComboBox.Items.Clear();
            placeOfServiceComboBox.Items.AddRange(Enum.GetNames(typeof(PlaceOfService)));
            placeOfServiceComboBox.SelectedIndex = (int)clinic.DefaultPlaceOfService;
            //_selectedProvBillNum = Clinic.InsuranceBillingProviderId.GetValueOrDefault();
            SelectedDefaultProviderId = clinic.ProviderId;
            insBillingProviderComboBox.SelectedIndex = -1;

            if (!clinic.IsNew)
            {
                specialityLinks.AddRange(DefLinks.GetListByFKey(clinic.Id, DefLinkType.Clinic));
            }

            providers = Providers.GetProvsForClinic(clinic.Id);

            FillProvidersComboBoxes();
            FillSpecialty();
            FillRegions();

            if (clinic.InsuranceBillingProviderId.HasValue)
            {
                insBillingProviderSpecificRadioButton.Checked = true;
            }
            else
            {
                insBillingProviderTreatRadioButton.Checked = true;
            }

            if (clinic.EmailAddressId.HasValue)
            {
                var emailAddress = EmailAddress.GetById(clinic.EmailAddressId.Value);
                if (emailAddress != null)
                {
                    emailTextBox.Text = emailAddress.GetFrom();
                    emailNoneButton.Enabled = true;
                }
            }

            medLabHL7DefEnabled = HL7Defs.IsExistingHL7Enabled(0, true);
            if (medLabHL7DefEnabled)
            {
                medLabAccountNumberTextBox.Visible = true;
                medLabAccountNumberLabel.Visible = true;
                medLabAccountNumberTextBox.Text = clinic.MedLabAccountId;
            }
        }

        private void FillProvidersComboBoxes()
        {
            defaultProviderComboBox.Items.Clear();

            insBillingProviderComboBox.Items.Clear();

            foreach (var provider in providers)
            {
                defaultProviderComboBox.Items.Add(provider);
                if (provider.ProvNum == clinic.ProviderId)
                {
                    defaultProviderComboBox.SelectedItem = provider;
                }

                insBillingProviderComboBox.Items.Add(provider);
                if (clinic.InsuranceBillingProviderId.HasValue && clinic.InsuranceBillingProviderId.Value == provider.ProvNum)
                {
                    insBillingProviderComboBox.SelectedItem = provider;
                }
            }
        }

        private void FillSpecialty()
        {
            Dictionary<long, Definition> dictClinicDefs = Definition.GetByCategory(DefinitionCategory.ClinicSpecialty).ToDictionary(x => x.Id);

            gridSpecialty.BeginUpdate();
            gridSpecialty.Columns.Clear();
            gridSpecialty.Columns.Add(new ODGridColumn("Specialty", 100));
            gridSpecialty.Rows.Clear();

            string specialtyDescript;
            foreach (var defLink in specialityLinks)
            {
                var row = new ODGridRow();

                specialtyDescript = "";
                if (dictClinicDefs.TryGetValue(defLink.DefNum, out var definition))
                {
                    specialtyDescript = definition.Description + (definition.Hidden ? (" (hidden)") : "");
                }

                row.Cells.Add(specialtyDescript);
                row.Tag = defLink;

                gridSpecialty.Rows.Add(row);
            }

            gridSpecialty.EndUpdate();
        }

        private void FillRegions()
        {
            regionComboBox.Items.Clear();
            regionComboBox.Items.Add("None");
            regionComboBox.SelectedIndex = 0;

            var regions = Definition.GetByCategory(DefinitionCategory.Regions);

            foreach (var region in regions)
            {
                regionComboBox.Items.Add(region);
                if (region.Id == clinic.RegionId)
                {
                    regionComboBox.SelectedItem = region;
                }
            }
        }

        private void DefaultProviderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            defaultProviderNoneButton.Enabled = defaultProviderComboBox.SelectedIndex != -1;
        }

        private void DefaultProviderPickButton_Click(object sender, EventArgs e)
        {
            using (var formProviderPick = new FormProviderPick(providers))
            {
                formProviderPick.SelectedProvNum = SelectedDefaultProviderId.GetValueOrDefault(); // TODO: Pass null...
                if (formProviderPick.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                SelectedDefaultProviderId = formProviderPick.SelectedProvNum;
            }
        }

        private void DefaultProviderNoneButton_Click(object sender, EventArgs e)
        {
            SelectedDefaultProviderId = null;
        }

        private void EmailPickButton_Click(object sender, EventArgs e)
        {
            using (var formEmailAddresses = new FormEmailAddresses())
            {
                formEmailAddresses.IsSelectionMode = true;
                if (formEmailAddresses.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                emailAddressId = formEmailAddresses.EmailAddressId;
                emailTextBox.Text = EmailAddress.GetById(emailAddressId).GetFrom();
                emailNoneButton.Enabled = true;
            }
        }

        private void EmailNoneButton_Click(object sender, EventArgs e)
        {
            emailAddressId = 0;

            emailTextBox.Text = "";
            emailNoneButton.Enabled = false;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            using (var formDefinitionPicker = new FormDefinitionPicker(DefinitionCategory.ClinicSpecialty))
            {
                formDefinitionPicker.HasShowHiddenOption = false;
                formDefinitionPicker.IsMultiSelectionMode = true;

                if (formDefinitionPicker.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (var definition in formDefinitionPicker.ListSelectedDefs)
                    {
                        if (specialityLinks.Any(x => x.DefNum == definition.Id)) continue;

                        specialityLinks.Add(new DefLink
                        {
                            DefNum = definition.Id,
                            FKey = clinic.Id,
                            LinkType = DefLinkType.Clinic
                        });
                    }

                    FillSpecialty();
                }
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (gridSpecialty.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    "Please select a specialty first.", 
                    "Clinic", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            foreach (var rowIndex in gridSpecialty.SelectedIndices)
            {
                if (gridSpecialty.Rows[rowIndex].Tag is DefLink defLink)
                {
                    specialityLinks.Remove(defLink);

                    if (!clinic.IsNew)
                    {
                        removedSpecialityLinks.Add(defLink);
                    }
                }
            }

            FillSpecialty();
        }

        private void InsBillingProviderSpecificRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            insBillingProviderComboBox.Enabled = insBillingProviderSpecificRadioButton.Checked;
            insBillingProviderPickButton.Enabled = insBillingProviderSpecificRadioButton.Checked;
        }

        private void InsBillingProviderPickButton_Click(object sender, EventArgs e)
        {
            using (var formProviderPick = new FormProviderPick(providers))
            {
                formProviderPick.SelectedProvNum = SelectedInsuranceBillingProviderId.GetValueOrDefault(); // TODO: Pass null...
                if (formProviderPick.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                SelectedInsuranceBillingProviderId = formProviderPick.SelectedProvNum;
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            var description = descriptionTextBox.Text.Trim();
            if (description.Length == 0)
            {
                MessageBox.Show(
                    "Description cannot be blank.",
                    "Clinic",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var abbreviation = abbrTextBox.Text.Trim();
            if (abbreviation.Length == 0)
            {
                MessageBox.Show(
                    "Abbreviation cannot be blank.",
                    "Clinic",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (insBillingProviderSpecificRadioButton.Checked && !SelectedInsuranceBillingProviderId.HasValue)
            {
                MessageBox.Show(
                    "You must select the default insurance billing provider.",
                    "Clinic",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            string phone = phoneTextBox.Text.Trim();
            if (Application.CurrentCulture.Name == "en-US")
            {
                phone = phone.Replace("(", "");
                phone = phone.Replace(")", "");
                phone = phone.Replace(" ", "");
                phone = phone.Replace("-", "");

                if (phone.Length != 0 && phone.Length != 10)
                {
                    MessageBox.Show(
                        "Invalid phone",
                        "Clinic",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            string fax = faxTextBox.Text.Trim();
            if (Application.CurrentCulture.Name == "en-US")
            {
                fax = fax.Replace("(", "");
                fax = fax.Replace(")", "");
                fax = fax.Replace(" ", "");
                fax = fax.Replace("-", "");

                if (fax.Length != 0 && fax.Length != 10)
                {
                    MessageBox.Show(
                        "Invalid fax",
                        "Clinic",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            var medLabAccountId = medLabAccountNumberTextBox.Text.Trim();
            if (medLabHL7DefEnabled && !string.IsNullOrWhiteSpace(medLabAccountId))
            {
                if (Clinic.All().Where(x => x.Id != clinic.Id).Any(x => x.MedLabAccountId == medLabAccountId))
                {
                    MessageBox.Show(
                        "The MedLab Account Number entered is already in use by another clinic.",
                        "Clinic",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            if (hiddenCheckBox.Checked)
            {
                var restrictedUsers = User.GetUsersOnlyThisClinic(clinic.Id);

                if (restrictedUsers.Count > 0)
                {
                    MessageBox.Show(
                        "You may not hide this clinic as the following users are restricted to only this clinic: " + 
                        string.Join(", ", restrictedUsers.Select(user => user.UserName)), 
                        "Clinic", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return;
                }
            }

            if (medicalOnlyCheckBox.Checked) clinic.Options |= ClinicOptions.MedicalOnly;
            if (excludeFromInsVerifyListCheckBox.Checked) clinic.Options |= ClinicOptions.ExcludeFromInsuranceVerification;
            if (procedureCodeRequiredCheckBox.Checked) clinic.Options |= ClinicOptions.RequireProcedureOnRx;
            if (useBillingAddressOnClaimsCheckBox.Checked) clinic.Options |= ClinicOptions.UseBillingAddressOnClaims;

            clinic.Abbr = abbreviation;
            clinic.Description = description;
            clinic.Phone = phone;
            clinic.Fax = fax;
            clinic.AddressLine1 = textAddress.Text;
            clinic.AddressLine2 = textAddress2.Text;
            clinic.City = textCity.Text;
            clinic.State = textState.Text;
            clinic.Zip = textZip.Text;
            clinic.BillingAddressLine1 = textBillingAddress.Text;
            clinic.BillingAddressLine2 = textBillingAddress2.Text;
            clinic.BillingCity = textBillingCity.Text;
            clinic.BillingState = textBillingST.Text;
            clinic.BillingZip = textBillingZip.Text;
            clinic.PayToAddressLine1 = textPayToAddress.Text;
            clinic.PayToAddressLine2 = textPayToAddress2.Text;
            clinic.PayToCity = textPayToCity.Text;
            clinic.PayToState = textPayToST.Text;
            clinic.PayToZip = textPayToZip.Text;
            clinic.BankNumber = bankNumberTextBox.Text;
            clinic.DefaultPlaceOfService = (PlaceOfService)placeOfServiceComboBox.SelectedIndex;
            clinic.IsHidden = hiddenCheckBox.Checked;
            clinic.RegionId = SelectedRegionId;
            clinic.InsuranceBillingProviderId = SelectedInsuranceBillingProviderId;
            clinic.ProviderId = SelectedDefaultProviderId;

            if (medLabHL7DefEnabled)
            {
                clinic.MedLabAccountId = medLabAccountId;
            }

            clinic.SchedulingNote = schedulingNoteTextBox.Text;

            if (clinic.IsNew) Clinic.Insert(clinic);
            else
            {
                Clinic.Update(clinic);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
