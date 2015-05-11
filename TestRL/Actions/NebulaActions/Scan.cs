using RLNET;
using RSS.CelestialObjects;

namespace RSS.Actions.NebulaActions
{
    class Scan : IRlKeyOption
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

        public Scan()
        {
            Key = RLKey.S;
            OptionText = "Scan";
        }

        public void Action(ICelestialObject ce)
        {
            ce.Scan();
        }
    }
}
