using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRL.Actions.StationActions
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
