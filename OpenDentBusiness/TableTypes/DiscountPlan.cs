namespace OpenDentBusiness
{
    ///<summary>Discount plans will automatically create adjustments when procedures are completed.
    ///The fee schedule associated to the discount plan will be used with the UCR fee schedule in order to determine the "discount".
    ///The associated DefNum will be the adjustment type that is used so that users can quickly query adjustments to see discount plan usage.</summary>
    public class DiscountPlan
    {
        public long DiscountPlanNum;

        ///<summary>Description of this discount plan</summary>
        public string Description;

        ///<summary>FK to feesched.FeeSchedNum</summary>
        public long FeeSchedNum;

        ///<summary>FK to definition.DefNum.  Represents the adjustment type of the feesched plan.</summary>
        public long DefNum;

        ///<summary>Set true to hide in Discount Plan list.</summary>
        public bool IsHidden;

        public DiscountPlan Copy()
        {
            return (DiscountPlan)MemberwiseClone();
        }
    }
}
