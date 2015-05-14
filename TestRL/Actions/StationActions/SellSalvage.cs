using System;
using RLNET;
using RSS.CelestialObjects;

namespace RSS.Actions.StationActions
{
    public class SellSalvage : IRlKeyOption
    {
        public RLKey Key { get; set; }
        public string OptionText { get; set; }

        public SellSalvage()
        {
            Key = RLKey.S;
            OptionText = "Sell salvage";
        }

        public void Action(ICelestialObject ce)
        {
            ce.SellSalvage();
        }
    }
}
