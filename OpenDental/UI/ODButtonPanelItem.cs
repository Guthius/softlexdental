using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace OpenDental.UI
{
    public class ODButtonPanelItem
    {
        /// <summary>
        /// Gets or sets the item text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Currently label or button, determines how item will be drawn.
        /// </summary>
        public ODButtonPanelItemType Type { get; set; } = ODButtonPanelItemType.Button;

        /// <summary>
        /// Gets or sets the state of the item.
        /// </summary>
        public ODButtonPanelItemState State { get; set; } = ODButtonPanelItemState.Normal;

        /// <summary>
        /// Gets or sets the row index of the button.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Zero based horizontal ordering of controls on the same row.
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// Used for attaching objects to this control. Potential uses: Images, procedures, delegate functions, etc. Will revisit later, maybe.
        /// </summary>
        public List<object> Tags { get; private set; }

        /// <summary>
        /// Gets the bounds of the item.
        /// </summary>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Initiailizes a new instance of the <see cref="ODButtonPanelItem"/> class.
        /// </summary>
        public ODButtonPanelItem()
        {
            Tags = new List<object>();
        }

        /// <summary>
        /// Updates the bounds of the item.
        /// </summary>
        /// <param name="bounds"></param>
        internal void SetBounds(Rectangle bounds) => Bounds = bounds;
    }

    public enum ODButtonPanelItemState
    {
        Normal,
        Hot
    }
}