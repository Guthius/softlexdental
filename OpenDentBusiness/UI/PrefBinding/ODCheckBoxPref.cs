using OpenDentBusiness;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class ODCheckBoxPref : CheckBox, IPrefBinding
    {
        private PreferenceName _prefNameBinding = PreferenceName.NotApplicable;

        private bool _reverseValue;
        public bool DoAutoSave { get; set; }
        ///<summary>For all those "EasyHide" prefs etc. where we store the opposite value of what we mean.</summary>
        public bool ReverseValue
        {
            get
            {
                return _reverseValue;
            }
            set
            {
                _reverseValue = value;
                if (!DesignMode)
                { //VisualStudio designer will break if we try to make a db call here
                    Checked = _reverseValue ? !Preference.GetBool(PrefNameBinding) : Preference.GetBool(PrefNameBinding);
                }
            }
        }

        public PreferenceName PrefNameBinding
        {
            get
            {
                return _prefNameBinding;
            }
            set
            {
                _prefNameBinding = value;
                if (!DesignMode)
                { //VisualStudio designer will break if we try to make a db call here
                    Checked = ReverseValue ? !Preference.GetBool(value) : Preference.GetBool(value);
                }
            }
        }

        public ODCheckBoxPref()
        {
            InitializeComponent();
        }

        public bool Save()
        {
            return PrefNameBinding.Update(ReverseValue ? !Checked : Checked);
        }
    }
}