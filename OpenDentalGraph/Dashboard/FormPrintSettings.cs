using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OpenDentBusiness;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Media.Imaging;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;

namespace OpenDentalGraph
{
    public partial class FormPrintSettings : Form
    {
        private readonly Chart chart;
        private readonly Legend legend;
        private bool isLoading = true;
        private int marginWidth;
        private int marginHeight;
        private int _xPos;
        private int _yPos;
        private int _pageWidth;
        private int _pageHeight;
        private Bitmap bitmap;
        //private XGraphics _xg;
        //private Graphics _g;

        public FormPrintSettings(Chart chart, Legend legend)
        {
            this.chart = new Chart();

            using (var memoryStream = new MemoryStream())
            {
                chart.Serializer.Save(memoryStream);

                this.chart.Serializer.Load(memoryStream);
            }

            this.legend = legend.PrintCopy();

            InitializeComponent();
        }

        private void FormPrintSettings_Load(object sender, EventArgs e)
        {
            chart.Printing.PrintDocument = new PrintDocument();
            chart.Printing.PrintDocument.PrintPage += new PrintPageEventHandler(ChartGenericFormat_PrintPage);
            chart.Printing.PrintDocument.OriginAtMargins = true;
            chart.Dock = DockStyle.None;

            printPreviewControl.Document = new PrintDocument();

            legend.Height = 30;

            textWidth.Text = "800";
            textHeight.Text = "600";
            textMarginHeight.Text = "0";
            textMarginWidth.Text = "0";
            textXPos.Text = "150";
            textYPos.Text = "130";

            isLoading = false;

            MakePage();
        }

        /// <summary>
        /// Draw to the chart's built-in PrintDocument, then set the PrintPreviewController's 
        /// document to that PrintDocument. Besides keeping everything synchronized, this makes it
        /// easier to display the print preview and show the printer settings window when the user 
        /// clicks Print.
        /// </summary>
        private void MakePage()
        {
            FillDimensions();

            chart.Printing.PrintDocument.DefaultPageSettings.Landscape = checkLandscape.Checked;
            chart.Printing.PrintDocument.DefaultPageSettings.Margins = 
                new System.Drawing.Printing.Margins(
                    marginWidth, marginWidth, marginHeight, marginHeight);

            if (bitmap != null) bitmap.Dispose();

            bitmap = new Bitmap(_pageWidth, _pageHeight);

            using (var graphics = Graphics.FromImage(bitmap))
            using (var x = XGraphics.FromGraphics(graphics, new XSize(bitmap.Width, bitmap.Height)))
            {
                using (var chartBitmap = new Bitmap(chart.Width, chart.Height))
                using (var legendBitmap = new Bitmap(legend.Width, legend.Height))
                {
                    chart.DrawToBitmap(chartBitmap, new Rectangle(0, 0, chart.Width, chart.Height));
                    legend.DrawToBitmap(legendBitmap, new Rectangle(0, 0, legend.Width, legend.Height));

                    x.DrawImage(legendBitmap, _xPos + marginWidth, _yPos + marginHeight + chartBitmap.Height, legend.Width, legend.Height);
                    x.DrawImage(chartBitmap, _xPos + marginWidth, _yPos + marginHeight, chart.Width, chart.Height);
                }
            }

            printPreviewControl.Document = chart.Printing.PrintDocument;
        }

        /// <summary>
        /// Called from MakePage to fill the private variables of this form when anything changes.
        /// </summary>
        private void FillDimensions()
        {
            chart.Width = 1;
            if (int.TryParse(textWidth.Text, out var chartWidth))
            {
                if (chartWidth <= 0) chartWidth = 1;
                else if (chartWidth > 1200)
                {
                    chartWidth = 1200;
                }

                chart.Width = chartWidth;
            }
            legend.Width = chart.Width;

            chart.Height = 1;
            if (int.TryParse(textHeight.Text, out var chartHeight))
            {
                if (chartHeight <= 0) chartHeight = 1;
                else if (chartHeight > 1200)
                {
                    chartHeight = 1200;
                }
                chart.Height = chartHeight;
            }

            //margin width, default to 0.
            try
            {
                marginWidth = PIn.Int(textMarginWidth.Text);
            }
            catch
            {
                marginWidth = 0;
            }
            if (marginWidth < 0)
            {
                marginWidth = 0;
            }
            if (marginWidth > 1200)
            {
                marginWidth = 1200;
            }

            //margin height, default to 0.
            try
            {
                marginHeight = PIn.Int(textMarginHeight.Text);
            }
            catch
            {
                marginHeight = 0;
            }
            if (marginHeight < 0)
            {
                marginHeight = 0;
            }
            if (marginHeight > 1200)
            {
                marginHeight = 1200;
            }

            //chart X position, default to 0
            try
            {
                _xPos = PIn.Int(textXPos.Text);
            }
            catch
            {
                _xPos = 0;
            }
            if (_xPos < 0)
            {
                _xPos = 0;
            }
            if (_xPos > 1200)
            {
                _xPos = 1200;
            }

            //chart Y position, default to 0
            try
            {
                _yPos = PIn.Int(textYPos.Text);
            }
            catch
            {
                _yPos = 0;
            }
            if (_yPos < 0)
            {
                _yPos = 0;
            }
            if (_yPos > 1200)
            {
                _yPos = 1200;
            }
            //printpreviewcontrol's document's width and height are always stored as if the page were in portrait mode.
            //this dynamically stores the width and height of the page, with width always being horizontal and height always being vertical.
            //makes the code cleaner as we don't have to put if-checks everywhere that we need the paper size.
            if (checkLandscape.Checked)
            {
                _pageWidth = printPreviewControl.Document.DefaultPageSettings.PaperSize.Height;
                _pageHeight = printPreviewControl.Document.DefaultPageSettings.PaperSize.Width;
            }
            else
            {
                _pageWidth = printPreviewControl.Document.DefaultPageSettings.PaperSize.Width;
                _pageHeight = printPreviewControl.Document.DefaultPageSettings.PaperSize.Height;
            }
        }

        private void ChartGenericFormat_PrintPage(object sender, PrintPageEventArgs ev) =>
            ev.Graphics.DrawImage(bitmap, 0, 0, _pageWidth, _pageHeight);

        private void ExportButton_Click(object sender, EventArgs e)
        {
            string fileName;

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF (*.pdf)|*.pdf";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                fileName = saveFileDialog.FileName;
            }

            using (var document = new PdfDocument())
            {
                try
                {
                    var page = new PdfPage();

                    document.Pages.Add(page);

                    page.Orientation = 
                        checkLandscape.Checked ?
                            PdfSharp.PageOrientation.Landscape : 
                            PdfSharp.PageOrientation.Portrait;

                    using (var graphics = XGraphics.FromPdfPage(page))
                    {
                        graphics.DrawImage(bitmap, 0, 0, page.Width, page.Height);
                    }

                    document.Save(fileName);

                    MessageBox.Show(
                        "Chart saved.", 
                        "Chart", 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        "Chart not saved." + "\r\n" + exception.Source + "\r\n" + exception.Message + "\r\n" + exception.StackTrace,
                        "Chart", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
        }

        private void PrintButton_Click(object sender, EventArgs e) => chart.Printing.Print(true);

        private void CloseButton_Click(object sender, EventArgs e) => Close();

        private void Refresh_Event(object sender, EventArgs e)
        {
            if (isLoading) return;
            
            refreshTimer.Stop();
            refreshTimer.Start();
        }

        private void LandscapeCheckBox_CheckedChanged(object sender, EventArgs e) => MakePage();

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            refreshTimer.Stop();

            MakePage();
        }
    }
}
