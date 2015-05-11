using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRL.Actions.NebulaActions;

namespace TestRL.Actions.WreckageActions
{
    public class WreckageMenu : IRlMenu
    {
        public List<IRlKeyOption> Options { get; set; }

        public WreckageMenu()
        {
            Options = new List<IRlKeyOption>();
            Options.Add(new Salvage());
            Options.Add(new Scan());
        }
    }
}
