using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS.Stack
{
    public static class NameGenHelper
    {
        private static List<string> _sectorNames = LoadSectorNames.Load();
        private static HashSet<string> _usedSectorNames = new HashSet<string>(); 
 
        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value between 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        public static string GenerateSystemName()
        {
            int currentCount = _usedSectorNames.Count;
            var r = new Random();
            string name = "ERROR";
            while (_usedSectorNames.Count == currentCount)
            {
                int randomName = r.Next(_sectorNames.Count);
                try
                {
                    name = (string) _sectorNames[randomName].First().ToString().ToUpper() +
                           String.Join("", ((string) _sectorNames[randomName]).Skip(1)) + " " + ToRoman(r.Next(1, 100));
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine(ioe.Message);
                    Console.WriteLine("....");
                }
                _usedSectorNames.Add(name);
            }

            return name;
        }
    }
}
