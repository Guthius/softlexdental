namespace OpenDentBusiness
{
    public class ClinicPref
    {
        public long ClinicPrefNum;
        public long ClinicNum;
        public PreferenceName PrefName;
        public string ValueString;

        public ClinicPref()
        {
        }

        public ClinicPref(long clinicNum, PreferenceName prefName, string valueString)
        {
            ClinicNum = clinicNum;
            PrefName = prefName;
            ValueString = valueString;
        }

        public ClinicPref(long clinicNum, PreferenceName prefName, bool valueBool) : this(clinicNum, prefName, valueBool.ToString())
        {
        }

        public ClinicPref Clone()
        {
            return (ClinicPref)this.MemberwiseClone();
        }
    }
}
