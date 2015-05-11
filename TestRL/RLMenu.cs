using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRL
{
    public interface IRlMenu
    {
        List<IRlKeyOption> Options { get; set; }
    }
}
