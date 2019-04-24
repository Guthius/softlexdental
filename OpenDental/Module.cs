using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public class Module : UserControl
    {
        public event EventHandler<NavigationEventArgs> Navigating;

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class.
        /// </summary>
        public Module()
        {
            Font = new Font("Segoe UI", 9f);
        }

        /// <summary>
        /// Attempt to navigate to the specified target.
        /// </summary>
        /// <param name="target">The navigation target.</param>
        /// <param name="args">Arguments for the navigation.</param>
        /// <returns>True if the navigation was succesfull; otherwise, false.</returns>
        public bool Navigate(string target, params object[] args)
        {
            target = target.Trim().ToLower();
            if (target.Length == 0)
            {
                return false;
            }

            var eventArgs = new NavigationEventArgs(target, args);

            OnNavigating(eventArgs);

            return eventArgs.Handled;
        }

        /// <summary>
        /// Raises the <see cref="Navigating"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnNavigating(NavigationEventArgs e) => Navigating?.Invoke(this, e);
    }

    public class NavigationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the navigation target.
        /// </summary>
        public string Target { get; }

        /// <summary>
        /// Gets the navigation arguments.
        /// </summary>
        public object[] Arguments { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the navigation was handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigateEventArgs"/> class.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="args"></param>
        public NavigationEventArgs(string target, object[] args)
        {
            Target = target ?? "";
            Arguments = args ?? new object[0];
        }
    }

    /// <summary>
    /// Container class for common navigation targets for use in <see cref="FormOpenDental.Navigate"/>.
    /// </summary>
    public static class NavigationTargets
    {
        public const string Scheduler = "scheduler";
        public const string Claim = "claim";
        public const string Tasks = "tasks";
    }
}