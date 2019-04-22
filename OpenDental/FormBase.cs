using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OpenDental
{
    public class FormBase : ODForm
    {
        List<Tuple<string, PluginActionHandler>> pluginActions = new List<Tuple<string, PluginActionHandler>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormBase"/> class.
        /// </summary>
        public FormBase()
        {
            AutoScaleMode = AutoScaleMode.Inherit;
            Padding = new Padding(10, 16, 10, 10);
            Font = new Font("Segoe UI", 9f);
        }

        /// <summary>
        /// Paints the form.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var brush = new LinearGradientBrush(
                new Point(0, 0),
                new Point(ClientSize.Width - 1, 6),
                Color.FromArgb(40, 110, 240),
                Color.FromArgb(0, 70, 140)))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(0, 0, ClientSize.Width, 6));
            }
        }

        /// <summary>
        /// Registers a handler for a plugin action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="actionHandler">The handler to add.</param>
        protected void On(string action, PluginActionHandler actionHandler)
        {
            if (action == null || actionHandler == null) return;

            action = action.Trim().ToLower();
            if (action.Length == 0)
            {
                return;
            }

            if (Plugin.AddAction(action, actionHandler))
            {
                pluginActions.Add(
                    new Tuple<string, PluginActionHandler>(
                        action, 
                        actionHandler));
            }
        }

        /// <summary>
        /// When the form is closing we need to remove all the plugin action handlers registered by the form.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Only remove the actions when the close wasn't cancelled.
            if (!e.Cancel)
            {
                foreach (var tuple in pluginActions)
                {
                    Plugin.RemoveAction(
                        tuple.Item1, 
                        tuple.Item2);
                }
            }
        }
    }

    /// <summary>
    /// This should be used for forms that represent simple dialogs which require a header text.
    /// </summary>
    public class FormBaseDialog : FormBase
    {
        string headerText = string.Empty;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string HeaderText
        {
            get => headerText;
            set
            {
                if (value != headerText)
                {
                    headerText = value ?? "";
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormBaseDialog"/> class.
        /// </summary>
        public FormBaseDialog()
        {
            Padding = new Padding(10, 56, 10, 10);
        }

        /// <summary>
        /// Paints the form.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = 
                new LinearGradientBrush(
                    new Point(0, 6),
                    new Point(ClientSize.Width, 6),
                    Color.FromArgb(255, 255, 255),
                    Color.FromArgb(240, 240, 240)))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(0, 6, ClientSize.Width, 40));
            }

            if (!string.IsNullOrEmpty(HeaderText))
            {
                TextRenderer.DrawText(e.Graphics, HeaderText, Font, new Point(10, 10), Color.Black);
            }

            base.OnPaint(e);
        }
    }
}