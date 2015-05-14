using System.Collections.Generic;

namespace RSS.Actions.WormholeActions
{
    public class WormholeMenu : IRlMenu
    {
        public List<IRlKeyOption> Options { get; set; }

        public WormholeMenu()
        {
            Options = new List<IRlKeyOption>();
            Options.Add(new Scan());
            Options.Add(new Jump());
            Options.Add(new Cancel());
        }
    }
}
