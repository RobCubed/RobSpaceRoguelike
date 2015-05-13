using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RSS.CelestialObjects;

namespace RSS.Actions.WormholeActions
{
    class Jump : IRlKeyOption
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

        public Jump()
        {
            Key = RLKey.J;
            OptionText = "Jump";
        }


        public void Action(ICelestialObject ce)
        {
            ce.Jump();
        }
    }
}
