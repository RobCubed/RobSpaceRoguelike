using System.Collections.Generic;
using RSS.Actions.NebulaActions;

namespace RSS.Actions.WreckageActions
{
    public class WreckageMenu : IRlMenu
    {
        public List<IRlKeyOption> Options { get; set; }

        public WreckageMenu()
        {
            Options = new List<IRlKeyOption>();
            Options.Add(new Salvage());
            Options.Add(new Scan());
            Options.Add(new Cancel());
        }
    }
}
