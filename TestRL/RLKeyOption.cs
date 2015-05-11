using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using TestRL.CelestialObjects;

namespace TestRL
{
    public interface IRlKeyOption
    {
        RLKey Key { get; set; }
        string OptionText { get; set; }
        void Action(ICelestialObject ce);
    }
}
