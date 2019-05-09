using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading;

namespace CodeBase
{
    /// <summary>
    /// Contains all color information for the currently selected ODColorTheme
    /// </summary>
    public class ODColorTheme
    {
        static ODColorTheme()
        {
            SetSolidBrush(ref _gridTitleTopBrush, Color.White);
            SetSolidBrush(ref _gridTitleBottomBrush, Color.FromArgb(213, 213, 223));
            SetSolidBrush(ref _gridBackGrndBrush, Color.FromArgb(224, 223, 227));
            SetSolidBrush(ref _gridHeaderBackBrush, Color.FromArgb(210, 210, 210));
            SetSolidBrush(ref _gridTitleTextBrush, Color.Black);
            SetSolidBrush(ref _gridHeaderTextBrush, Color.Black);
            SetPen(ref _gridLinePen, Color.FromArgb(180, 180, 180));
            SetPen(ref _gridOutlinePen, Color.FromArgb(119, 119, 146));
            SetPen(ref _gridInnerLinePen, Color.FromArgb(102, 102, 122));
            SetPen(ref _gridColumnSeparatorPen, Color.FromArgb(120, 120, 120));
            _gridTitleFontOverride = null;//No override
            _gridHeaderFontOverride = null;//No override
                                           //toolbar buttons
            _toolBarTogglePushedTopColor = Color.FromArgb(248, 248, 248);
            _toolBarTogglePushedTopColorError = Color.FromArgb(255, 212, 212);
            _toolBarTogglePushedBottomColor = Color.FromArgb(248, 248, 248);
            _toolBarTogglePushedBottomColorError = Color.FromArgb(255, 118, 118);
            _toolBarHoverTopColor = Color.FromArgb(240, 240, 240);
            _toolBarHoverTopColorError = Color.FromArgb(255, 192, 192);
            _toolBarHoverBottomColor = Color.FromArgb(240, 240, 240);
            _toolBarHoverBottomColorError = Color.FromArgb(255, 98, 98);
            _toolBarTopColor = SystemColors.Control;
            _toolBarTopColorError = Color.FromArgb(255, 192, 192);//base color for other error colors (top)
            _toolBarBottomColor = SystemColors.Control;
            _toolBarBottomColorError = Color.FromArgb(255, 98, 98);//base color for other error colors (bottom)
            _toolBarPushedTopColor = Color.FromArgb(210, 210, 210);
            _toolBarPushedBottomColor = Color.FromArgb(210, 210, 210);
            _toolBarPushedTopColorError = Color.FromArgb(225, 162, 162);
            _toolBarPushedBottomColorError = Color.FromArgb(225, 68, 68);
            _colorNotify = Color.FromArgb(252, 178, 129);
            _colorNotifyDark = Color.FromArgb(182, 98, 44);
            SetSolidBrush(ref _toolBarTextForeBrush, Color.Black);
            SetSolidBrush(ref _toolBarTextForeBrushError, Color.Black);//derived from DefaultForeColor on ODToolBar.
            SetPen(ref _toolBarPenOutlinePen, Color.SlateGray);
            SetPen(ref _toolBarPenOutlinePenError, Color.SlateGray);
            SetSolidBrush(ref _toolBarTextDisabledBrush, SystemColors.GrayText);
            SetSolidBrush(ref _toolBarTextDisabledBrushError, SystemColors.GrayText);
            SetPen(ref _toolBarDividerPen, Color.FromArgb(180, 180, 180));
            SetPen(ref _toolBarDividerPenError, Color.FromArgb(180, 180, 180));
            SetSolidBrush(ref _toolBarDisabledOverlayBrush, Color.Empty);
            SetFont(ref _toolBarFont, FontFamily.GenericSansSerif, 8.25f, FontStyle.Regular);
            SetFont(ref _fontMenuItem, "Microsoft Sans Serif", 9, FontStyle.Regular);
            _butCornerRadiusOverride = 4;
            SetPen(ref _butBorderPen, Color.FromArgb(28, 81, 128));
            SetSolidBrush(ref _butDisabledTextBrush, Color.FromArgb(161, 161, 146));
            SetSolidBrush(ref _butDarkestBrush, Color.FromArgb(157, 164, 196));
            SetSolidBrush(ref _butLightestBrush, Color.FromArgb(255, 255, 255));
            SetSolidBrush(ref _butMainBrush, Color.FromArgb(223, 224, 235));
            SetSolidBrush(ref _butPressedDarkestBrush, Color.FromArgb(157, 164, 196));
            SetSolidBrush(ref _butPressedLightestBrush, Color.FromArgb(255, 255, 255));
            SetSolidBrush(ref _butPressedMainBrush, Color.FromArgb(223, 224, 235));
            SetSolidBrush(ref _butDefaultDarkBrush, Color.FromArgb(50, 70, 230));
            SetSolidBrush(ref _butHoverDarkBrush, Color.FromArgb(255, 190, 100));
            SetSolidBrush(ref _butHoverLightBrush, Color.FromArgb(255, 210, 130));
            SetSolidBrush(ref _butHoverMainBrush, Color.FromArgb(223, 224, 235));
            SetSolidBrush(ref _butTextBrush, Color.Black);
            SetSolidBrush(ref _butGlowBrush, Color.White);
            _butCenterColor = Color.FromArgb(255, 255, 255, 255);
            _butReflectionBotColor = Color.FromArgb(0, 0, 0, 0);
            _butReflectionTopColor = Color.FromArgb(50, 0, 0, 0);
            //outlook bar
            _outlookHoverCornerRadius = 4;
            SetSolidBrush(ref _outlookHotBrush, Color.FromArgb(235, 235, 235));
            SetSolidBrush(ref _outlookPressedBrush, Color.FromArgb(210, 210, 210));
            SetSolidBrush(ref _outlookSelectedBrush, Color.FromArgb(255, 255, 255));
            SetPen(ref _outlookOutlinePen, Color.FromArgb(28, 81, 128));
            SetSolidBrush(ref _outlookTextBrush, Color.Black);
            SetSolidBrush(ref _outlookBackBrush, SystemColors.Control);

            _isOutlookImageInverse = false;
            //odform
            _formBackColor = SystemColors.Control;
            //ButtonPanel
            SetSolidBrush(ref _buttonPanelLabelBackgroundBrush, Color.White);
            SetSolidBrush(ref _buttonPanelLabelBrush, Color.Black);
            SetSolidBrush(ref _buttonPanelBackgroundShadowBrush, Color.FromArgb(224, 223, 227));
            SetSolidBrush(ref _buttonPanelBackgroundBrush, Color.White);
            SetPen(ref _buttonPanelOutlinePen, Color.FromArgb(119, 119, 146));
        }


        #region Private Variables

        private static ConcurrentDictionary<string, Action> _dictOnThemeChangedActions = new ConcurrentDictionary<string, Action>();
        ///<summary>Locks objects so we don't half set them in the middle of changing themes.</summary>
        private static ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

        #region Private Grid Variables
        //odgrid
        ///<summary>ODGrid title top color.</summary>
        protected static SolidBrush _gridTitleTopBrush = null;
        ///<summary>ODGrid title bottom color.</summary>
        protected static SolidBrush _gridTitleBottomBrush = null;
        ///<summary>ODGrid background color (color of the grid itself, not the rows).</summary>
        protected static SolidBrush _gridBackGrndBrush = null;
        ///<summary>ODGrid column header color.</summary>
        protected static SolidBrush _gridHeaderBackBrush = null;
        ///<summary>ODGrid title text color.</summary>
        protected static SolidBrush _gridTitleTextBrush = null;
        ///<summary>ODGrid title text color.</summary>
        protected static SolidBrush _gridHeaderTextBrush = null;
        ///<summary>ODGrid outline color.</summary>
        protected static Pen _gridLinePen = null;
        ///<summary></summary>
        protected static Pen _gridOutlinePen = null;
        ///<summary>.</summary>
        protected static Pen _gridColumnSeparatorPen = null;
        ///<summary>.</summary>
        protected static Pen _gridInnerLinePen = null;
        ///<summary>ODGrid title font.</summary>
        protected static Font _gridTitleFontOverride = null;
        ///<summary>ODGrid column header font.</summary>
        protected static Font _gridHeaderFontOverride = null;
        #endregion
        #region Private Toolbar Variables
        //odtoolbar
        ///<summary>.</summary>
        protected static SolidBrush _toolBarTextForeBrush = null;
        ///<summary>.</summary>
        protected static SolidBrush _toolBarTextForeBrushError = null;
        ///<summary>.</summary>
        protected static Pen _toolBarPenOutlinePen = null;
        ///<summary>.</summary>
        protected static Pen _toolBarPenOutlinePenError = null;
        ///<summary>.</summary>
        protected static SolidBrush _toolBarTextDisabledBrush = null;
        ///<summary>.</summary>
        protected static SolidBrush _toolBarTextDisabledBrushError = null;
        ///<summary>.</summary>
        protected static Pen _toolBarDividerPen = null;
        ///<summary>.</summary>
        protected static Pen _toolBarDividerPenError = null;
        ///<summary></summary>
        protected static SolidBrush _toolBarDisabledOverlayBrush = null;
        ///<summary>Toolbar Button Text Font.</summary>
        protected static Font _toolBarFont = null;
        ///<summary></summary>
        protected static Color _toolBarTogglePushedTopColor;
        ///<summary></summary>
        protected static Color _toolBarTogglePushedTopColorError;
        ///<summary></summary>
        protected static Color _toolBarTogglePushedBottomColor;
        ///<summary></summary>
        protected static Color _toolBarTogglePushedBottomColorError;
        ///<summary></summary>
        protected static Color _toolBarHoverTopColor;
        ///<summary></summary>
        protected static Color _toolBarHoverTopColorError;
        ///<summary></summary>
        protected static Color _toolBarHoverBottomColor;
        ///<summary></summary>
        protected static Color _toolBarHoverBottomColorError;
        ///<summary></summary>
        protected static Color _toolBarTopColor;
        ///<summary></summary>
        protected static Color _toolBarTopColorError;
        ///<summary></summary>
        protected static Color _toolBarBottomColor;
        ///<summary></summary>
        protected static Color _toolBarBottomColorError;
        ///<summary></summary>
        protected static Color _toolBarPushedTopColor;
        ///<summary></summary>
        protected static Color _toolBarPushedTopColorError;
        ///<summary></summary>
        protected static Color _toolBarPushedBottomColor;
        ///<summary></summary>
        protected static Color _toolBarPushedBottomColorError;
        ///<summary></summary>
        protected static Color _colorNotify;
        ///<summary></summary>
        protected static Color _colorNotifyDark;
        #endregion
        #region Private Button Variables
        ///<summary>Button border color.</summary>
        protected static Pen _butBorderPen = null;
        ///<summary>Button disabled text color.</summary>
        protected static SolidBrush _butDisabledTextBrush = null;
        ///<summary>Button shading at lower and right borders.</summary>
        protected static SolidBrush _butDarkestBrush = null;
        ///<summary>Button overall main color. </summary>
        protected static SolidBrush _butMainBrush = null;
        ///<summary>Button light color. </summary>
        protected static SolidBrush _butLightestBrush = null;
        ///<summary>Button shading at lower and right borders. Button can gradient between three colors -- Darkest, Main, Lightest.</summary>
        protected static SolidBrush _butPressedDarkestBrush = null;
        ///<summary>Button overall main color. Button can gradient between three colors -- Darkest, Main, Lightest.</summary>
        protected static SolidBrush _butPressedMainBrush = null;
        ///<summary>Button light color.  Button can gradient between three colors -- Darkest, Main, Lightest.</summary>
        protected static SolidBrush _butPressedLightestBrush = null;
        ///<summary>Button default dark color.  Replaces DarkestColor when the button is the default, focused button.</summary>
        protected static SolidBrush _butDefaultDarkBrush = null;
        ///<summary>Button hover dark color.  Replaces DarkestColor when the button is hovered over.</summary>
        protected static SolidBrush _butHoverDarkBrush = null;//the outline when hovering
                                                              ///<summary>Button hover light color.  Replaces MainColor when the button is hovered over.</summary>
        protected static SolidBrush _butHoverMainBrush = null;
        ///<summary>Button hover light color.  Replaces LightestColor when the button is hovered over.</summary>
        protected static SolidBrush _butHoverLightBrush = null;
        ///<summary>Button text color.</summary>
        protected static SolidBrush _butTextBrush = null;
        ///<summary>A slight glow that text on buttons give off. Normally white.</summary>
        protected static SolidBrush _butGlowBrush = null;
        ///<summary>The "glare" of the button, near the center. Set to clear if you don't want a glare effect.</summary>
        protected static Color _butCenterColor;
        ///<summary>Overlays the bottom half of the button. The middle of the button will be set to _butReflectionTopColor, then gradient to this color.
        ///Set to clear if you don't want a gradient effect.</summary>
        protected static Color _butReflectionBotColor;
        ///<summary>Overlays the bottom half of the button. The middle of the button will be set to this color, then gradient to _butReflectionBotColor.
        ///Set to clear if you don't want a gradient effect.</summary>
        protected static Color _butReflectionTopColor;
        ///<summary></summary>
        protected static float _butCornerRadiusOverride;

        #endregion
        #region Private OutlookBar Variables
        //main modules outlook bar
        ///<summary>Newer themes are square, others are rounded.</summary>
        protected static float _outlookHoverCornerRadius = 0;
        ///<summary>When an item is hovered over.</summary>
        protected static SolidBrush _outlookHotBrush = null;
        ///<summary>When an item is pressed.</summary>
        protected static SolidBrush _outlookPressedBrush = null;
        ///<summary>When the item is currently selected.</summary>
        protected static SolidBrush _outlookSelectedBrush = null;
        ///<summary>Outline.</summary>
        protected static Pen _outlookOutlinePen = null;
        ///<summary>Text Color.</summary>
        protected static SolidBrush _outlookTextBrush = null;
        ///<summary>Background color.</summary>
        protected static SolidBrush _outlookBackBrush = null;
        ///<summary>.</summary>
        protected static int _outlookApptImageIndex = 0;
        ///<summary>.</summary>
        protected static int _outlookFamilyImageIndex = 1;
        ///<summary>.</summary>
        protected static int _outlookAcctImageIndex = 2;
        ///<summary>.</summary>
        protected static int _outlookTreatPlanImageIndex = 3;
        ///<summary>.</summary>
        protected static int _outlookChartImageIndex = 4;
        ///<summary>.</summary>
        protected static int _outlookImagesImageIndex = 5;
        ///<summary>.</summary>
        protected static int _outlookManageImageIndex = 6;
        ///<summary>.</summary>
        protected static int _outlookEcwTreatPlanImageIndex = 7;
        ///<summary>.</summary>
        protected static int _outlookEcwChartImageIndex = 8;
        ///<summary></summary>
        protected static bool _isOutlookImageInverse = false;
        #endregion
        #region Private ButtonPanel Variables
        ///<summary>Button panel brush for label text.</summary>
        protected static SolidBrush _buttonPanelLabelBrush = null;
        ///<summary>Button panel brush for label background.</summary>
        protected static SolidBrush _buttonPanelLabelBackgroundBrush = null;
        ///<summary>Button panel brush for panel background shadow.</summary>
        protected static SolidBrush _buttonPanelBackgroundShadowBrush = null;
        ///<summary>Button panel brush for panel background.</summary>
        protected static SolidBrush _buttonPanelBackgroundBrush = null;
        ///<summary>Button panel pen for control outline.</summary>
        protected static Pen _buttonPanelOutlinePen = null;
        #endregion

        #region ODForm
        ///<summary></summary>
        protected static Color _formBackColor;
        protected static Font _fontMenuItem;
        #endregion

        #endregion

        #region Getters

        private static T Get<T>(ref T obj)
        {
            _locker.EnterReadLock();
            try
            {
                return obj;
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        ///<summary>Orginally used for buttons.  The color of the outline of the button.</summary>
        public static Pen ButBorderPen
        {
            get { return Get(ref _butBorderPen); }
        }

        ///<summary>Orginally used for buttons.  The center color of the button. Needs to remain a color for now.</summary>
        [Obsolete]
        public static Color ButCenterColor
        {
            get { return Get(ref _butCenterColor); }
        }

        ///<summary>Orginally used for buttons.  -1 if no override. The roundness of the button corners.</summary>
        [Obsolete]
        public static float ButCornerRadiusOverride
        {
            get { return Get(ref _butCornerRadiusOverride); }
        }

        ///<summary>Orginally used for buttons.  The darkest color of the button.</summary>
        [Obsolete]
        public static SolidBrush ButDarkestBrush
        {
            get { return Get(ref _butDarkestBrush); }
        }

        ///<summary>Orginally used for buttons.  The default darkest color of the button.</summary>
        [Obsolete]
        public static SolidBrush ButDefaultDarkBrush
        {
            get { return Get(ref _butDefaultDarkBrush); }
        }

        ///<summary>Orginally used for buttons.  The text color on the button when it is disabled.</summary>
        [Obsolete]
        public static SolidBrush ButDisabledTextBrush
        {
            get { return Get(ref _butDisabledTextBrush); }
        }

        ///<summary>Orginally used for buttons.  The color of the glow for the button.</summary>
        [Obsolete]
        public static SolidBrush ButGlowBrush
        {
            get { return Get(ref _butGlowBrush); }
        }

        ///<summary>Orginally used for buttons.  The darkest color of the button when it is being hovered over.</summary>
        [Obsolete]
        public static SolidBrush ButHoverDarkBrush
        {
            get { return Get(ref _butHoverDarkBrush); }
        }

        ///<summary>Orginally used for buttons.  The lightest color of the button when it is being hovered over.</summary>
        [Obsolete]
        public static SolidBrush ButHoverLightBrush
        {
            get { return Get(ref _butHoverLightBrush); }
        }

        ///<summary>Orginally used for buttons.  The main color of the button when it is being hovered over. </summary>
        [Obsolete]
        public static SolidBrush ButHoverMainBrush
        {
            get { return Get(ref _butHoverMainBrush); }
        }

        ///<summary>Orginally used for buttons.  The lightest color of the button.</summary>
        [Obsolete]
        public static SolidBrush ButLightestBrush
        {
            get { return Get(ref _butLightestBrush); }
        }

        ///<summary>Orginally used for buttons.  The main color of the button.</summary>
        public static SolidBrush ButMainBrush
        {
            get { return Get(ref _butMainBrush); }
        }

        ///<summary>Orginally used for buttons.  The darkest color of the button when it is being pressed.</summary>
        [Obsolete]
        public static SolidBrush ButPressedDarkestBrush
        {
            get { return Get(ref _butPressedDarkestBrush); }
        }

        ///<summary>Orginally used for buttons.  The lightest color of the button when it is being pressed.</summary>
        [Obsolete]
        public static SolidBrush ButPressedLightestBrush
        {
            get { return Get(ref _butPressedLightestBrush); }
        }

        ///<summary>Orginally used for buttons.  The main color of the button when it is being pressed.</summary>
        [Obsolete]
        public static SolidBrush ButPressedMainBrush
        {
            get { return Get(ref _butPressedMainBrush); }
        }

        ///<summary>Orginally used for buttons.  The bottom color of the button reflection.</summary>
        [Obsolete]
        public static Color ButReflectionBottomColor
        {
            get { return Get(ref _butReflectionBotColor); }
        }

        ///<summary>Orginally used for buttons.  The top color of the button reflection. Used to make a linear gradient brush, just for color.</summary>
        [Obsolete]
        public static Color ButReflectionTopColor
        {
            get { return Get(ref _butReflectionTopColor); }
        }

        ///<summary>Orginally used for buttons.  The color of the text on the button.</summary>
        [Obsolete]
        public static SolidBrush ButTextColor
        {
            get { return Get(ref _butTextBrush); }
        }

        ///<summary>Usually set to the same font as the rest of the theme. This has been created specifically for menuItems since their font size 
        ///sometimes needs to be different. </summary>
        [Obsolete]
        public static Font FontMenuItem
        {
            get { return Get(ref _fontMenuItem); }
        }

        ///<summary>Originally from OdGrid.  The grid's background color when no rows are present.</summary>
        [Obsolete]
        public static SolidBrush GridBackGrndBrush
        {
            get { return Get(ref _gridBackGrndBrush); }
        }

        ///<summary>Originally from OdGrid.  The color of the vertical line that separates two columns in the grid.</summary>
        [Obsolete]
        public static Pen GridColumnSeparatorPen
        {
            get { return Get(ref _gridColumnSeparatorPen); }
        }

        ///<summary>Originally from OdGrid.  The color of the text for the main title bar.</summary>
        public static SolidBrush GridTextBrush
        {
            get { return Get(ref _gridTitleTextBrush); }
        }

        ///<summary>Originally from OdGrid.  The grid line(?).</summary>
        [Obsolete]
        public static Pen GridLinePen
        {
            get { return Get(ref _gridLinePen); }
        }

        /// <summary>Originally from OdGrid. The outline of the control.</summary>
        [Obsolete]
        public static Pen GridOutlinePen
        {
            get { return Get(ref _gridOutlinePen); }
        }

        ///<summary>Originally from OdGrid.  The bottom color of the main title bar.</summary>
        [Obsolete]
        public static SolidBrush TitleBottomBrush
        {
            get { return Get(ref _gridTitleBottomBrush); }
        }

        ///<summary>Originally from OdGrid.  The top color of the main title bar.</summary>
        [Obsolete]
        public static SolidBrush TitleTopBrush
        {
            get { return Get(ref _gridTitleTopBrush); }
        }

        ///<summary>Text of the label. Originally from panel in the chart module.</summary>
        [Obsolete]
        public static SolidBrush ButtonPanelLabelBrush
        {
            get { return Get(ref _buttonPanelLabelBrush); }
        }

        ///<summary>Background of the label. Originally from panel in the chart module.</summary>
        [Obsolete]
        public static SolidBrush ButtonPanelLabelBackgroundBrush
        {
            get { return Get(ref _buttonPanelLabelBackgroundBrush); }
        }

        ///<summary>Color of the shadow for entire panel. Originally from panel in the chart module.</summary>
        [Obsolete]
        public static SolidBrush ButtonPanelBackgroundShadowBrush
        {
            get { return Get(ref _buttonPanelBackgroundShadowBrush); }
        }

        ///<summary>Color of the entire panel. Originally from panel in the chart module.</summary>
        [Obsolete]
        public static SolidBrush ButtonPanelBackgroundBrush
        {
            get { return Get(ref _buttonPanelBackgroundBrush); }
        }

        ///<summary>Outline of the button panel. Orignally from panel in the chart module.</summary>
        [Obsolete]
        public static Pen ButtonPanelOutlinePen
        {
            get { return Get(ref _buttonPanelOutlinePen); }
        }

        #endregion Getters

        ///<summary>Disposes of the previous brush (do not pass in a system brush), then initializes the font using the given parameters.</summary>
        protected static void SetSolidBrush(ref SolidBrush brush, Color color)
        {
            if (brush == null)
            {
                brush = new SolidBrush(color);
            }
            else
            {
                brush.Color = color;
            }
        }

        ///<summary>Disposes of the previous pen (do not pass in a system pen), then initializes the font using the given parameters.</summary>
        protected static void SetPen(ref Pen pen, Color color)
        {
            Pen tempPen = pen;
            pen = new Pen(color);
            if (tempPen != null)
            {
                tempPen.Dispose();
                tempPen = null;
            }
        }

        ///<summary>Disposes of the previous font if it is not a system font, then initializes the font using the given parameters.</summary>
        protected static void SetFont(ref Font font, FontFamily family, float emSize, FontStyle style)
        {
            if (font != null && font.FontFamily == family && font.Size == emSize && font.Style == style)
            {
                return;//No changed needed.
            }
            Font tempFont = font;
            font = new Font(family, emSize, style);
            if (tempFont != null && !tempFont.IsSystemFont)
            {
                //For some reason disposing the font was causing errors when switching the theme.
                //Theme switching does not happen often, thus we are OK with leaving it floating in memory.
                //tempFont.Dispose();
                tempFont = null;
            }
        }

        ///<summary>Disposes of the previous font if it is not a system font, then initializes the font using the given parameters.</summary>
        protected static void SetFont(ref Font font, string familyName, float emSize, FontStyle style)
        {
            if (font != null && font.FontFamily.Name == familyName && font.Size == emSize && font.Style == style)
            {
                return;//No changed needed.
            }
            Font tempFont = font;
            font = new Font(familyName, emSize, style);
            if (tempFont != null && !tempFont.IsSystemFont)
            {
                //For some reason disposing the font was causing errors when switching the theme.
                //Theme switching does not happen often, thus we are OK with leaving it floating in memory.
                //tempFont.Dispose();
                tempFont = null;
            }
        }
    }
}