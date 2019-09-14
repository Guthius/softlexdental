using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormPrinterSetup : FormBaseDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormPrinterSetup"/> class.
        /// </summary>
        public FormPrinterSetup() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormPrinterSetup_Load(object sender, EventArgs e)
        {
            List<string> installedPrinters = null;
            try
            {
                installedPrinters = new List<string>(PrinterSettings.InstalledPrinters.AsEnumerable<string>());
            }
            catch (Exception ex)
            {
                FormFriendlyException.Show("Unable to access installed printers.", ex);

                DialogResult = DialogResult.Cancel;
                return;
            }

            simpleCheckBox.Checked = Preference.GetBool(PreferenceName.EasyHidePrinters);

            SetControls(this, installedPrinters);
        }

        /// <summary>
        /// Sets the state all combobox and checkbox controls that are part of the specified control.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="printerNames">The list of available printers.</param>
        void SetControls(Control container, List<string> printerNames)
        {
            foreach (Control control in container.Controls)
            {
                SetControls(control, printerNames);
                if (control == simpleCheckBox)
                {
                    continue;
                }

                if (control.GetType() == typeof(ComboBox)) FillComboBox((ComboBox)control, printerNames);
                if (control.GetType() == typeof(CheckBox)) FillCheckBox((CheckBox)control);
            }
        }

        /// <summary>
        /// Fills the specified combobox with the list of available printers.
        /// </summary>
        /// <param name="comboBox">The combobox.</param>
        /// <param name="printerNames">The list of available printers.</param>
        void FillComboBox(ComboBox comboBox, List<string> printerNames)
        {
            var printer = Printers.GetForSit(GetPrintSituation(comboBox));

            // Get the name of the printer.
            string printerName = printer != null ? printer.PrinterName : "";

            // Add the 'default' option.
            comboBox.Items.Clear();
            if (comboBox == defaultComboBox)
            {
                comboBox.Items.Add("System Default");
            }
            else
            {
                comboBox.Items.Add("Default");
            }

            for (int i = 0; i < printerNames.Count; i++)
            {
                comboBox.Items.Add(printerNames[i]);
                if (printerName == printerNames[i])
                {
                    comboBox.SelectedIndex = i + 1;
                }
            }

            // If no printer was selected, selected the first option (use default printer).
            if (comboBox.SelectedIndex == -1) comboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Determines the checked state of the specified checkbox.
        /// </summary>
        /// <param name="checkBox">The checkbox.</param>
        void FillCheckBox(CheckBox checkBox)
        {
            var printer = Printers.GetForSit(GetPrintSituation(checkBox));

            if (printer == null)
            {
                checkBox.Checked = false;
                return;
            }

            checkBox.Checked = printer.DisplayPrompt;
        }

        /// <summary>
        /// Gets the print situation from a control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        PrintSituation GetPrintSituation(Control control)
        {
            var printSituation = PrintSituation.Default;
            switch (control.Name)
            {
                default:
                    MessageBox.Show("error. " + control.Name);
                    break;
                case "defaultComboBox":
                case "checkDefault":
                    printSituation = PrintSituation.Default;
                    break;
                case "comboAppointments":
                case "checkAppointments":
                    printSituation = PrintSituation.Appointments;
                    break;
                case "comboClaim":
                case "checkClaim":
                    printSituation = PrintSituation.Claim;
                    break;
                case "comboLabelSheet":
                case "checkLabelSheet":
                    printSituation = PrintSituation.LabelSheet;
                    break;
                case "comboLabelSingle":
                case "checkLabelSingle":
                    printSituation = PrintSituation.LabelSingle;
                    break;
                case "comboPostcard":
                case "checkPostcard":
                    printSituation = PrintSituation.Postcard;
                    break;
                case "comboRx":
                case "checkRx":
                    printSituation = PrintSituation.Rx;
                    break;
                case "comboRxControlled":
                case "checkRxControlled":
                    printSituation = PrintSituation.RxControlled;
                    break;
                case "comboRxMulti":
                case "checkRxMulti":
                    printSituation = PrintSituation.RxMulti;
                    break;
                case "comboStatement":
                case "checkStatement":
                    printSituation = PrintSituation.Statement;
                    break;
                case "comboTPPerio":
                case "checkTPPerio":
                    printSituation = PrintSituation.TPPerio;
                    break;
                case "comboReceipt":
                case "checkReceipt":
                    printSituation = PrintSituation.Receipt;
                    break;
            }
            return printSituation;
        }

        /// <summary>
        /// Toggles visibility of the simple panel.
        /// </summary>
        void simpleCheckBox_CheckedChanged(object sender, EventArgs e) => simplePanel.Visible = !simpleCheckBox.Checked;

        /// <summary>
        /// Saves the settings and closes the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            string computerName = Environment.MachineName;
            if (simpleCheckBox.Checked && !Preference.GetBool(PreferenceName.EasyHidePrinters))
            {
                var result =
                    MessageBox.Show(
                        "Warning! You have selected the easy view option. This will clear all printing preferences for all computers. Are you sure you wish to continue?",
                        "Printer Setup",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                Printers.ClearAll();
                Printers.RefreshCache();

                string defaultPrinterName =
                    defaultComboBox.SelectedIndex > 0 ?
                        defaultComboBox.SelectedItem.ToString() :
                        "";

                Printers.PutForSit(PrintSituation.Default, computerName, defaultPrinterName, true);
            }
            else
            {
                // Get the name of the default printer.
                string defaultPrinterName =
                    defaultComboBox.SelectedIndex > 0 ?
                        defaultComboBox.SelectedItem.ToString() :
                        "";

                // Update the printer for each situation.
                for (int i = 0; i < Enum.GetValues(typeof(PrintSituation)).Length; i++)
                {
                    bool isChecked = false;
                    string printerName = "";

                    // Find the checkbox and combobox for this situation and get their values.
                    foreach (Control control in simplePanel.Controls)
                    {
                        if (control.GetType() != typeof(ComboBox) && 
                            control.GetType() != typeof(CheckBox))
                            continue;

                        // So only two controls out of all will be used in each Enum loop
                        if (GetPrintSituation(control) != (PrintSituation)i) continue;
                        
                        if (control is ComboBox comboBox)
                        {
                            printerName =
                                comboBox.SelectedIndex > 0 ?
                                    comboBox.SelectedItem.ToString() :
                                    defaultPrinterName;
                        }
                        else isChecked = ((CheckBox)control).Checked;
                    }
                    Printers.PutForSit((PrintSituation)i, computerName, printerName, isChecked);
                }
            }

            DataValid.SetInvalid(InvalidType.Computers);
            if (simpleCheckBox.Checked != Preference.GetBool(PreferenceName.EasyHidePrinters))
            {
                Preference.Update(PreferenceName.EasyHidePrinters, simpleCheckBox.Checked);
                DataValid.SetInvalid(InvalidType.Prefs);
            }

            Printers.RefreshCache();

            DialogResult = DialogResult.OK;
        }
    }
}