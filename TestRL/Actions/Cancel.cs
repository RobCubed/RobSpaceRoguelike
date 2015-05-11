using RLNET;
using TestRL.CelestialObjects;

namespace TestRL.Actions
{
    class Cancel : IRlKeyOption
    {
        private RLKey _key;
        private string _optionText;

        public RLKey Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public string OptionText
        {
            get { return _optionText; }
            set { _optionText = value; }
        }

        public Cancel()
        {
            Key = RLKey.C;
            OptionText = "Cancel";
        }

        public void Action(ICelestialObject ce)
        {
            
        }
    }
}
