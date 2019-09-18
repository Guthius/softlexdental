namespace OpenDentBusiness
{
    /// <summary>
    /// AutoCode condition. Always attached to an AutoCodeItem, which is then, in turn, attached 
    /// to an autocode. There is usually only one or two conditions for a given AutoCodeItem.
    /// </summary>
    public class AutoCodeCond : DataRecord
    {
        public long AutoCodeItemId;

        public AutoCondition Condition;

        public AutoCodeCond Copy()
        {
            return (AutoCodeCond)MemberwiseClone();
        }
    }
}
