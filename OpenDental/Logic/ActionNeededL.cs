namespace OpenDental
{
    public class ActionNeededEventArgs
    {
        public ActionNeededEventArgs(ActionNeededTypes actionType)
        {
            ActionType = actionType;
        }

        public ActionNeededTypes ActionType { get; }
    }

    public enum ActionNeededTypes
    {
        RadiologyProcedures,
    }

    public delegate void ActionNeededEventHandler(object sender, ActionNeededEventArgs e);
}