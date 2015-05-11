using RLNET;
using RSS.CelestialObjects;

namespace RSS.Actions.WreckageActions
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
