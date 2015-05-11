using System.Collections.Generic;

namespace RSS.Actions.StationActions
{
    public class StationMenu : IRlMenu
    {
        public List<IRlKeyOption> Options { get; set; }

        public StationMenu()
        {
            Options = new List<IRlKeyOption>();
            Options.Add(new SellSalvage());
            Options.Add(new BuyProbe());
        }
    }
}
