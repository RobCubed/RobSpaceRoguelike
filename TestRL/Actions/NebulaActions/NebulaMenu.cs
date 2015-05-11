using System.Collections.Generic;

namespace TestRL.Actions.NebulaActions
{
    class NebulaMenu : IRlMenu
    {
        public List<IRlKeyOption> Options { get; set; }

        public NebulaMenu()
        {
            Options = new List<IRlKeyOption>();
            Options.Add(new Refuel());
            Options.Add(new Scan());
        }
    }
}
