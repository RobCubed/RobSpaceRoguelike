using RLNET;
using TestRL.CelestialObjects;

namespace TestRL.Actions.NebulaActions
{
    class Refuel : IRlKeyOption
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

        public Refuel()
        {
            Key = RLKey.R;
            OptionText = "Refuel";
        }

        public void Action(ICelestialObject ce)
        {
            ce.Refuel();
        }
    }
}
