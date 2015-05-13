using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS.Stack
{
    public static class LoadSectorNames
    {
        public static List<string> Load()
        {
            var file = File.ReadAllLines(@"Resources\sectornames.txt");
            return new List<string>(file);
        }
    }
}
