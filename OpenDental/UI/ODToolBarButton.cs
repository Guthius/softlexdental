using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental.UI
{
    public class ODToolBarButton : System.ComponentModel.Component
    {
        public bool IsRed;

        /// <summary>
        /// A one or two character notification string which will show just above the dropdown arrow when dropDownMenu is not null.
        /// If null or empty, the dropdown arrow background will draw in the typical color and no text will show.
        /// Otherwise the dropdown rectangle will use the notification color background.
        /// </summary>
        public string NotificationText;
        
        /// <summary>
        /// DateTime of the last time this button was clicked. Used  to stop double clicking from firing 2 events.
        /// This must be public so that ODToolBar can access this.
        /// Must be a property or we will get a "marshall-by-reference" warning.
        /// </summary>
        public DateTime DateTimeLastClicked { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODToolBarButton"/> class.
        /// </summary>
        public ODToolBarButton()
        {
            Style = ODToolBarButtonStyle.PushButton;
        }

        ///<summary>Creates a new ODToolBarButton with the given text.  
        ///buttonTag will be a string for module specific buttons and will be a Program object for program link buttons.</summary>
        public ODToolBarButton(string buttonText, Image image, string buttonToolTip, Object buttonTag)
        {
            Style = ODToolBarButtonStyle.PushButton;
            Image = image;
            Text = buttonText;
            ToolTipText = buttonToolTip;
            Tag = buttonTag;
        }

        ///<summary>Creates a new separator style ODToolBarButton.</summary>
        public ODToolBarButton(ODToolBarButtonStyle buttonStyle)
        {
            Style = buttonStyle;
        }

        ///<summary>Creates a new PageNav ODToolBarButton.</summary>
        public ODToolBarButton(int pageVal, int pageMax, string buttonToolTip, object buttonTag)
        {
            Style = ODToolBarButtonStyle.PageNav;
            Text = "/";
            ToolTipText = buttonToolTip;
            Tag = buttonTag;
        }

        public int PageValue { get; set; }

        public int PageMax { get; set; }

        public Rectangle Bounds { get; set; }

        public ODToolBarButtonStyle Style { get; set; }

        public ToolBarButtonState State { get; set; } = ToolBarButtonState.Normal;

        public string Text { get; set; } = "";

        public string ToolTipText { get; set; } = "";

        public Image Image { get; set; }

        public bool Enabled { get; set; } = true;

        public Menu DropDownMenu { get; set; }

        /// <summary>
        /// Holds extra information about the button, so we can tell which button was clicked.
        /// E.g. Tag will be set to a string for module specific buttons and will be a Program object for program link buttons.
        /// </summary>
        public object Tag { get; set; } = "";

        ///<summary>Only used if style is ToggleButton.</summary>
        public bool Pushed { get; set; }
    }

    /// <summary>
    /// There are also pushed and enabled to worry about separately.
    /// </summary>
    public enum ToolBarButtonState
    {
        Normal,
        
        /// <summary>
        /// Mouse is hovering over the button and the mouse button is not pressed.
        /// </summary>
        Hover,
        
        /// <summary>
        /// Mouse was pressed over this button and is still down, even if it has moved off this button or off the toolbar.
        /// </summary>
        Pressed,
        
        /// <summary>
        /// In a dropdown button, only the dropdown portion is pressed. For hover, the entire button acts as one, 
        /// but for pressing, the dropdown can be pressed separately.
        /// </summary>
        DropPressed
    }

    /// <summary>
    /// Just like Forms.ToolBarButtonStyle, except includes some extras.
    /// </summary>
    public enum ODToolBarButtonStyle
    {
        DropDownButton,
        PushButton,
        Separator,
        ToggleButton,
        Label,
        PageNav,
    }
}