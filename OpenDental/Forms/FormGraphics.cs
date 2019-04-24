using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using SparksToothChart;
using System;
using System.Drawing;
using System.Windows.Forms;
using Tao.Platform.Windows;

namespace OpenDental
{
    public partial class FormGraphics : FormBase
    {
        public ComputerPref ComputerPrefCur;

        OpenGLWinFormsControl.PixelFormatValue[] formats = new OpenGLWinFormsControl.PixelFormatValue[0];
        ToothChartDirectX.DirectXDeviceFormat[] xformats = new ToothChartDirectX.DirectXDeviceFormat[0];
        int selectedFormatNum = 0;
        string selectedDirectXFormat = "";
        bool isRemoteEdit;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormGraphics"/> class.
        /// </summary>
        public FormGraphics() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        async void FormGraphics_Load(object sender, EventArgs e)
        {
            if (ComputerPrefCur == null) ComputerPrefCur = ComputerPrefs.LocalComputer;
            else
            {
                if (ComputerPrefCur != ComputerPrefs.LocalComputer)
                {
                    isRemoteEdit = true;
                }

                MessageBox.Show(
                    "Warning, editing another computers graphical settings should be done from that computer to ensure the selected settings work. We do not recommend editing this way. If you make changes for another computer you should still verifiy that they work on that machine.",
                    "Graphics Preferences",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                SecurityLogs.MakeLogEntry(Permissions.GraphicsRemoteEdit, 0, "Edited graphical settings for " + ComputerPrefCur.ComputerName);
            }
            Text += " - " + ComputerPrefCur.ComputerName;

            // If we don't know the OS that the system is running we cannot reliably configure its graphical settings.
            if (ComputerPrefCur.ComputerOS == PlatformOD.Undefined)
            {
                MessageBox.Show(
                    "Selected computer needs to be updated before being able to remotely change its graphical settings.",
                    "Graphics Preferences",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                DialogResult = DialogResult.Cancel;
                return;
            }

            // Only the simple chart can be used in Linux systems.
            if (ComputerPrefCur.ComputerOS == PlatformOD.Unix)
            {
                MessageBox.Show(
                    "Linux users must use simple tooth chart.",
                    "Graphics Preferences",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                directXGraphicsRadioButton.Enabled = false;
                openGLGraphicsRadioButton.Enabled = false;
                formatsGroupBox.Enabled = false;
                return;
            }

            hardwareAccelerationCheckBox.Checked = ComputerPrefCur.GraphicsUseHardware;
            doubleBufferingCheckBox.Checked = ComputerPrefCur.GraphicsDoubleBuffering;
            selectedFormatNum = ComputerPrefCur.PreferredPixelFormatNum;
            selectedDirectXFormat = ComputerPrefCur.DirectXFormat;

            formatTextBox.Text = "";
            if (ComputerPrefCur.GraphicsSimple == DrawingMode.Simple2D)
            {
                simpleGraphicsRadioButton.Checked = true;
                formatsGroupBox.Enabled = false;
            }
            else if (ComputerPrefCur.GraphicsSimple == DrawingMode.DirectX)
            {
                directXGraphicsRadioButton.Checked = true;
                formatsGroupBox.Enabled = true;
            }
            else
            {
                openGLGraphicsRadioButton.Checked = true;
                formatsGroupBox.Enabled = true;
            }

            await RefreshFormats();
        }

        /// <summary>
        /// Populates the grid with the available formats.
        /// </summary>
        void FillGrid()
        {
            int selectionIndex = -1;

            formatsGrid.BeginUpdate();
            formatsGrid.Rows.Clear();

            if (directXGraphicsRadioButton.Checked)
            {
                formatTextBox.Text = "";
                formatsGrid.Columns.Clear();
                formatsGrid.Columns.Add(new ODGridColumn("#", 40));
                formatsGrid.Columns.Add(new ODGridColumn("Adapter", 60));
                formatsGrid.Columns.Add(new ODGridColumn("Accelerated", 80));
                formatsGrid.Columns.Add(new ODGridColumn("Buffered", 75));
                formatsGrid.Columns.Add(new ODGridColumn("Color Bits", 75));
                formatsGrid.Columns.Add(new ODGridColumn("Color Format", 75));
                formatsGrid.Columns.Add(new ODGridColumn("Depth Bits", 75));
                formatsGrid.Columns.Add(new ODGridColumn("Depth Format", 75));
                formatsGrid.Columns.Add(new ODGridColumn("Antialiasing", 75));

                for (int i = 0; i < xformats.Length; i++)
                {
                    var row = new ODGridRow();
                    row.Cells.Add((i + 1).ToString());
                    row.Cells.Add(xformats[i].adapterIndex.ToString());
                    row.Cells.Add(xformats[i].IsHardware ? "Yes" : "No");
                    row.Cells.Add("Yes");
                    row.Cells.Add(ToothChartDirectX.GetFormatBitCount(xformats[i].BackBufferFormat).ToString());
                    row.Cells.Add(xformats[i].BackBufferFormat);
                    row.Cells.Add(ToothChartDirectX.GetFormatBitCount(xformats[i].DepthStencilFormat).ToString());
                    row.Cells.Add(xformats[i].DepthStencilFormat);
                    row.Cells.Add(xformats[i].MultiSampleCount.ToString());
                    formatsGrid.Rows.Add(row);
                    if (xformats[i].ToString() == selectedDirectXFormat)
                    {
                        selectionIndex = i;
                        formatTextBox.Text = (i + 1).ToString();
                    }
                }
            }
            else if (openGLGraphicsRadioButton.Checked)
            {
                formatTextBox.Text = selectedFormatNum.ToString();
                formatsGrid.Columns.Clear();
                formatsGrid.Columns.Add(new ODGridColumn("#", 40));
                formatsGrid.Columns.Add(new ODGridColumn("OpenGL", 60));
                formatsGrid.Columns.Add(new ODGridColumn("Windowed", 80));
                formatsGrid.Columns.Add(new ODGridColumn("Bitmapped", 80));
                formatsGrid.Columns.Add(new ODGridColumn("Palette", 75));
                formatsGrid.Columns.Add(new ODGridColumn("Accelerated", 80));
                formatsGrid.Columns.Add(new ODGridColumn("Buffered", 75));
                formatsGrid.Columns.Add(new ODGridColumn("Color Bits", 75));
                formatsGrid.Columns.Add(new ODGridColumn("Depth Bits", 75));

                for (int i = 0; i < formats.Length; i++)
                {
                    var row = new ODGridRow();
                    row.Cells.Add(formats[i].formatNumber.ToString());
                    row.Cells.Add(OpenGLWinFormsControl.FormatSupportsOpenGL(formats[i].pfd) ? "Yes" : "No");
                    row.Cells.Add(OpenGLWinFormsControl.FormatSupportsWindow(formats[i].pfd) ? "Yes" : "No");
                    row.Cells.Add(OpenGLWinFormsControl.FormatSupportsBitmap(formats[i].pfd) ? "Yes" : "No");
                    row.Cells.Add(OpenGLWinFormsControl.FormatUsesPalette(formats[i].pfd) ? "Yes" : "No");
                    row.Cells.Add(OpenGLWinFormsControl.FormatSupportsAcceleration(formats[i].pfd) ? "Yes" : "No");
                    row.Cells.Add(OpenGLWinFormsControl.FormatSupportsDoubleBuffering(formats[i].pfd) ? "Yes" : "No");
                    row.Cells.Add(formats[i].pfd.cColorBits.ToString());
                    row.Cells.Add(formats[i].pfd.cDepthBits.ToString());
                    formatsGrid.Rows.Add(row);
                    if (formats[i].formatNumber == selectedFormatNum)
                    {
                        selectionIndex = i;
                    }
                }
            }

            formatsGrid.EndUpdate();
            if (selectionIndex >= 0)
            {
                formatsGrid.SetSelected(selectionIndex, true);
            }
        }

        /// <summary>
        /// Fetches the available graphics formats.
        /// </summary>
        async System.Threading.Tasks.Task RefreshFormats()
        {
            Cursor = Cursors.WaitCursor;

            formatsGrid.Rows.Clear();
            formatsGrid.Invalidate();
            formatTextBox.Text = "";

            await System.Threading.Tasks.Task.Run(() =>
            {
                if (directXGraphicsRadioButton.Checked)
                {
                    xformats = ToothChartDirectX.GetStandardDeviceFormats();
                }
                else if (openGLGraphicsRadioButton.Checked)
                {
                    OpenGLWinFormsControl contextFinder = new OpenGLWinFormsControl();

                    Gdi.PIXELFORMATDESCRIPTOR[] rawformats = OpenGLWinFormsControl.GetPixelFormats(contextFinder.GetHDC());
                    if (!hardwareAccelerationCheckBox.Checked && !doubleBufferingCheckBox.Checked)
                    {
                        formats = new OpenGLWinFormsControl.PixelFormatValue[rawformats.Length];
                        for (int i = 0; i < rawformats.Length; i++)
                        {
                            formats[i] = new OpenGLWinFormsControl.PixelFormatValue
                            {
                                formatNumber = i + 1,
                                pfd = rawformats[i]
                            };
                        }
                    }
                    else
                    {
                        formats = OpenGLWinFormsControl.PrioritizePixelFormats(rawformats,
                            doubleBufferingCheckBox.Checked,
                            hardwareAccelerationCheckBox.Checked);
                    }
                    contextFinder.Dispose();
                }
            });

            FillGrid();

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// If simple graphics is checked, disable the formats selection.
        /// </summary>
        void simpleGraphicsRadioButton_Click(object sender, EventArgs e) => formatsGroupBox.Enabled = false;
        
        /// <summary>
        /// When DirectX graphics are selected reload the list of formats.
        /// </summary>
        async void directXGraphicsRadioButton_Click(object sender, EventArgs e)
        {
            formatsGroupBox.Enabled = true;
            await RefreshFormats();
        }

        /// <summary>
        /// When OpenGL graphics are selected reload the list of formats.
        /// </summary>
        async void openGLGraphicsRadioButton_Click(object sender, EventArgs e)
        {
            formatsGroupBox.Enabled = true;
            await RefreshFormats();
        }

        /// <summary>
        /// When the 'Enable hardware accelaration' checkbox is checked or unchecked, refresh the formats list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void hardwareAccelerationCheckBox_Click(object sender, EventArgs e) => await RefreshFormats();

        /// <summary>
        /// When the 'Use double buffering' checkbox is checked or unchecked, refresh the formats list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void doubleBufferingCheckBox_Click(object sender, EventArgs e) => await RefreshFormats();
        
        /// <summary>
        /// Update the selected format when a format is selected from the grid.
        /// </summary>
        void gridFormats_CellClick(object sender, ODGridClickEventArgs e)
        {
            int formatNum = Convert.ToInt32(formatsGrid.Rows[e.Row].Cells[0].Text);

            formatTextBox.Text = formatNum.ToString();
            if (directXGraphicsRadioButton.Checked)
            {
                selectedDirectXFormat = xformats[formatNum - 1].ToString();
            }
            else if (this.openGLGraphicsRadioButton.Checked)
            {
                selectedFormatNum = formatNum;
            }
        }

        /// <summary>
        /// CAUTION: Runs slowly. May take minutes. Returns the string "invalid" if no suitable Direct X format can be found.
        /// </summary>
        public static string GetPreferredDirectXFormat(Form callingForm)
        {
            ToothChartDirectX.DirectXDeviceFormat[] possibleStandardFormats = ToothChartDirectX.GetStandardDeviceFormats();
            for (int i = 0; i < possibleStandardFormats.Length; i++)
            {
                if (TestDirectXFormat(callingForm, possibleStandardFormats[i].ToString()))
                {
                    return possibleStandardFormats[i].ToString();
                }
            }
            return "invalid";
        }

        /// <summary>
        /// Returns true if the given directXFormat works for a DirectX tooth chart on the local computer.
        /// </summary>
        static bool TestDirectXFormat(Form callingForm, string directXFormat)
        {
            ToothChartWrapper toothChartTest = new ToothChartWrapper();
            toothChartTest.Visible = false;
            // We add the invisible tooth chart to our form so that the device context will initialize properly
            // and our device creation test will then be accurate.
            callingForm.Controls.Add(toothChartTest);
            toothChartTest.DeviceFormat = new ToothChartDirectX.DirectXDeviceFormat(directXFormat);
            toothChartTest.DrawMode = DrawingMode.DirectX;//Creates the device.
            if (toothChartTest.DrawMode == DrawingMode.Simple2D)
            {
                // The chart is set back to 2D mode when there is an error initializing.
                callingForm.Controls.Remove(toothChartTest);
                toothChartTest.Dispose();
                return false;
            }

            // Now we check to be sure that one can draw and retrieve a screen shot from a DirectX control using the specified device format.
            try
            {
                Bitmap screenShot = toothChartTest.GetBitmap();
                screenShot.Dispose();
            }
            catch
            {
                callingForm.Controls.Remove(toothChartTest);
                toothChartTest.Dispose();
                return false;
            }
            callingForm.Controls.Remove(toothChartTest);
            toothChartTest.Dispose();
            return true;
        }

        /// <summary>
        /// Checks whether the selected format (if any) is valid and supported. Saves the settings and closes the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            var computerPrefOld = ComputerPrefCur.Copy();

            if (directXGraphicsRadioButton.Checked)
            {
                if (!isRemoteEdit && !TestDirectXFormat(this, selectedDirectXFormat))
                {
                    MessageBox.Show(
                        "Please choose a different device format, the selected device format will not support the DirectX 3D tooth chart on this computer",
                        "Graphics Preferences", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);

                    return;
                }
                ComputerPrefCur.GraphicsSimple = DrawingMode.DirectX;
                ComputerPrefCur.DirectXFormat = selectedDirectXFormat;
            }
            else if (simpleGraphicsRadioButton.Checked)
            {
                ComputerPrefCur.GraphicsSimple = DrawingMode.Simple2D;
            }
            else
            {
                using (var contextTester = new OpenGLWinFormsControl())
                {
                    try
                    {
                        if (!isRemoteEdit && contextTester.TaoInitializeContexts(selectedFormatNum) != selectedFormatNum)
                        {
                            throw new Exception("Could not initialize pixel format " + selectedFormatNum.ToString() + ".");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            "Please choose a different pixel format, the selected pixel format will not support the 3D tooth chart on this computer: " + ex.Message,
                            "Graphics Preferences",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        contextTester.Dispose();
                        return;
                    }
                }

                ComputerPrefCur.GraphicsUseHardware = hardwareAccelerationCheckBox.Checked;
                ComputerPrefCur.GraphicsDoubleBuffering = doubleBufferingCheckBox.Checked;
                ComputerPrefCur.PreferredPixelFormatNum = selectedFormatNum;
                ComputerPrefCur.GraphicsSimple = DrawingMode.OpenGL;
            }

            ComputerPrefs.Update(ComputerPrefCur, computerPrefOld);

            DialogResult = DialogResult.OK;
        }
    }
}