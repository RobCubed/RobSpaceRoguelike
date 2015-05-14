using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RSS.Tools
{
    public class InformationLog<TKey, TValue> : Dictionary<int, string>
    {
        private int _line = 1;
 
        public void AddEntry(string toAdd, bool first)
        {
            if (first) toAdd = "[" + _line++ + "] " + toAdd;
            if (toAdd.Length > 65)
            {
                StringBuilder sb = new StringBuilder();
                string[] spaces = toAdd.Split(' ');
                foreach (string space in spaces)
                {
                    if ((sb.ToString() + space).Length > 65) break;
                    sb.Append(space + " ");
                }
                string toAddBase = sb.ToString();

                string toAddRecursive = "     " + toAdd.Substring(toAddBase.Length);
                Add(Count + 1, toAddBase);
                AddEntry(toAddRecursive, false);
            }
            else
            {
                Add(Count + 1, toAdd);
            }
        }
    }
}
