using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using TestRL.CelestialObjects;

namespace TestRL.Actions.WreckageActions
{
    public class Salvage : IRlKeyOption {

        public RLKey Key { get; set; }

        public string OptionText { get; set; }

        public Salvage()
        {
            Key = RLKey.W;
            OptionText = "Salvage";
        }

        public void Action(ICelestialObject ce)
        {
            ce.Salvage();
        }
    }
}
