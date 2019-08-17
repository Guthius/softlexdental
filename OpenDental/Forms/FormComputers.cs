using OpenDental.Properties;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormComputers : FormBase
    {
        bool changed;
        List<Computer> computersList;

        /// <summary>
        /// Initializes a new instance of the <cee cref="FormComputers"/> class.
        /// </summary>
        public FormComputers() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormComputers_Load(object sender, EventArgs e)
        {
            List<string> serviceList = Computer.GetServiceInfo();
            serverNameTextBox.Text = serviceList[2];
            serviceNameTextBox.Text = serviceList[0];
            serviceVersionTextBox.Text = serviceList[3];
            serviceCommentTextBox.Text = serviceList[1];
            currentComputerTextBox.Text = Environment.MachineName;

            FillList();
            if (!Security.IsAuthorized(Permissions.GraphicsEdit, true))
            {
                setSimpleGraphicsButton.Enabled = false;
            }
        }

        /// <summary>
        /// Invalidate the computer data when the form closes and changes were made.
        /// </summary>
        void FormComputers_Closing(object sender, CancelEventArgs e)
        {
            if (changed)
            {
                DataValid.SetInvalid(InvalidType.Computers);
            }
        }

        /// <summary>
        /// Fill the computers list.
        /// </summary>
        void FillList()
        {
            CacheManager.Invalidate<Computer>();

            // Fill the computer list.
            computersList = Computer.All();
            computerListBox.Items.Clear();
            for (int i = 0; i < computersList.Count; i++)
            {
                computerListBox.Items.Add(computersList[i].Name);
                if (computersList[i].Name == currentComputerTextBox.Text)
                {
                    computerListBox.SelectedIndex = i;
                }
            }
        }

        /// <summary>
        /// Open the graphics settings form when the user double clicks on a computer in the list.
        /// </summary>
        void ComputerListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.GraphicsEdit)) return;

            if (computerListBox.SelectedIndex != -1)
            {
                using (var formGraphics = new FormGraphics())
                {
                    formGraphics.ComputerPrefCur = ComputerPrefs.GetForComputer(computersList[computerListBox.SelectedIndex].Name);
                    formGraphics.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Draws each item in the computers list.
        /// </summary>
        void ComputerListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;

            var text = computerListBox.Items[e.Index].ToString();

            DrawListBoxItem(Resources.IconComputer, text, e);
        }

        /// <summary>
        /// Draws a list item.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="text"></param>
        /// <param name="e"></param>
        void DrawListBoxItem(Image image, string text, DrawItemEventArgs e)
        {
            var textColor = SystemColors.ControlText;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                using (var brush = new LinearGradientBrush(e.Bounds, Color.FromArgb(40, 110, 240), Color.FromArgb(0, 70, 140), LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }

                textColor = Color.White;
            }
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
            }

            TextRenderer.DrawText(
                e.Graphics, text, Font,
                Rectangle.FromLTRB(
                    e.Bounds.Left + (image == null ? 4 : 24), 
                    e.Bounds.Top, 
                    e.Bounds.Right - 4, 
                    e.Bounds.Bottom),
                textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            if (image != null)
            {
                e.Graphics.DrawImage(
                    image, 
                    new Point(
                        e.Bounds.Left + 4,
                        e.Bounds.Top + (e.Bounds.Height - image.Height) / 2));
            }
        }

        /// <summary>
        /// Set graphics for selected computer to simple.
        /// </summary>
        void SetSimpleGraphicsButton_Click(object sender, EventArgs e)
        {
            if (computerListBox.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "You must select a computer name first.", 
                    "Computers", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            ComputerPrefs.SetToSimpleGraphics(computersList[computerListBox.SelectedIndex].Name);

            MessageBox.Show(
                "Done",
                "Computers", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);

            SecurityLogs.MakeLogEntry(Permissions.GraphicsEdit, 0, "Set the graphics for computer " + computersList[computerListBox.SelectedIndex].Name + " to simple");
        }

        /// <summary>
        /// Deletes the selected computer from the list.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (computerListBox.SelectedIndex != -1)
            {
                var result =
                    MessageBox.Show(
                        "Are you sure you want to delete the selected computer?", 
                        "Computers", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question, 
                        MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No) return;

                Computer.Delete(computersList[computerListBox.SelectedIndex]);

                changed = true;

                FillList();
            }
        }
    }
}