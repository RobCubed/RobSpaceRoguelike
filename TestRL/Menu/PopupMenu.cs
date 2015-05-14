using System.Collections.Generic;
using System.ComponentModel;
using RLNET;
using RSS.Actions;
using RSS.CelestialObjects;

namespace RSS.Menu
{
    public class PopupMenu
    {
        private string _menuName;
        private PopupMenu _parentMenu;
        public List<IRlKeyOption> MenuItems;
        private int _x;
        private int _y;
        private int _width;

        public PopupMenu(string menuName, List<IRlKeyOption> menuItems, int x, int y)
        {
            Initialize(menuName, menuItems, x, y);
        }

        public PopupMenu(string menuName, List<IRlKeyOption> menuItems, int x, int y, PopupMenu parentMenu)
        {
            Initialize(menuName, menuItems, x, y);
            _parentMenu = parentMenu;
        }

        private void Initialize(string menuName, List<IRlKeyOption> menuItems, int x, int y)
        {
            _menuName = menuName;
            MenuItems = menuItems;
            _x = x;
            _y = y;
        }

        public void DrawMenu(RLRootConsole rootConsole)
        {
            _width = 0;
            foreach (IRlKeyOption option in MenuItems)
            {
                string opt = (option.Key.ToString() + " : " + option.OptionText);
                if (opt.Length > _width)
                    _width = opt.Length;
            }

            if (_menuName.Length > _width) _width = _menuName.Length;

            _width += 2;

            string bar = "";
            string spaces = "";
            for (int j = 0; j < _width; j++)
            {
                bar += "Ä";
                spaces += " ";
            }

            rootConsole.Print(_x, _y, "Ú" + bar + "¿", RLColor.Green);
            rootConsole.Print(_x + 2, _y, _menuName, RLColor.LightGreen);
            int i = _y + 1;
            foreach (IRlKeyOption option in MenuItems)
            {
                rootConsole.Print(_x, i, "³" + spaces + "³", RLColor.Green);
                rootConsole.Print(_x + 2, i, option.Key.ToString() + " : " + option.OptionText, RLColor.LightCyan);
                i++;
            }
            //rootConsole.Print(_x, i++, "³" + spaces + "³", RLColor.Green);
            rootConsole.Print(_x, i, "À" + bar + "Ù", RLColor.Green);
        }

        public PopupMenu Kill()
        {
            if (_parentMenu != null) return _parentMenu;
            return null;
        }

        internal void FallBack()
        {
            if (_parentMenu == null)
            {
                Program.ActiveMenu = false;
            }
            else
            {
                Program.PopupMenu = _parentMenu;
            }
        }
    }
}
