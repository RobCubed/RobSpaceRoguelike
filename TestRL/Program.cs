using System;
using System.Collections.Generic;
using System.Linq;
using RLNET;
using RogueSharp;
using RSS.Actions;
using RSS.Actions.NebulaActions;
using RSS.Actions.RandomActions;
using RSS.Actions.StationActions;
using RSS.Actions.WreckageActions;
using RSS.CelestialObjects;

namespace RSS
{
    class Program
    {
        public static readonly int ScreenWidth = 105;
        public static readonly int ScreenHeight = 75;
        public static Player Player;
        private static RLRootConsole _rootConsole;
        private static string _statusText;
        private static ICelestialObject _currentCelestialObject;
        private static Sector _currentSectorMap;
        private static IRlMenu _menu;
        private static bool _menuActive;
        private static Cancel _cancel;
        private static List<IRlKeyOption> _currentOptions;
        public static Dictionary<int, Sector> _sectorMap;
        public static int CurrentSector = 0;
        
        static void Main(string[] args)
        {
            _statusText = "";
            Player = new Player();
            Player.X = 25;
            Player.Y = 25;
            Player.SensorRange = 100;
            Player.Fuel = 100;
            Player.MaxFuel = 100;
            Player.FuelProbes = 5;
            Player.Score = 0;
            Player.Credits = 0;
            Player.CargoHold = 0;
            Player.CargoHoldMax = 100;

            _sectorMap = new Dictionary<int, Sector>();
            _sectorMap.Add(0, new Sector(CurrentSector));
            _sectorMap.TryGetValue(0, out _currentSectorMap);
            _currentSectorMap.JoinSector();

            _cancel = new Cancel();
            _menuActive = false;
            string fontFileName = "terminal8x8.png";
            string consoleTitle = "RobSpaceRoguelike";
            _rootConsole = new RLRootConsole(fontFileName, ScreenWidth, ScreenHeight, 8, 8, 1f, consoleTitle);
            _rootConsole.Update += OnRootConsoleUpdate;
            _rootConsole.Render += OnRootConsoleRender;
            _rootConsole.Run();
        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                if (_menuActive)
                {
                    foreach (IRlKeyOption rlko in _currentOptions)
                    {
                        if (keyPress.Key == rlko.Key)
                        {
                            rlko.Action(_currentCelestialObject);
                            _menuActive = !_menuActive;
                        }
                    }
                    return;
                }
                if (keyPress.Key == RLKey.Up)
                {
                    if (FuelCheck()) return;
                    RandomAction.Go();
                    if (_currentSectorMap.SectorMap.GetCell(Player.X, Player.Y - 1).IsWalkable)
                    {
                        // Update the player position
                        Player.Y--;
                        Player.Fuel--;
                    }
                    else
                    {
                        ICelestialObject ce = CheckObject(Player.X, Player.Y - 1);
                        if (ce != null)
                        {
                            _currentCelestialObject = ce;
                            _menu = ce.Menu;
                            _menuActive = true;
                        }
                    }
                }
                // Repeat for the other directions
                else if (keyPress.Key == RLKey.Down)
                {
                    if (FuelCheck()) return;
                    RandomAction.Go();
                    if (_currentSectorMap.SectorMap.GetCell(Player.X, Player.Y + 1).IsWalkable)
                    {
                        Player.Y++;
                        Player.Fuel--;
                    }
                    else
                    {
                        ICelestialObject ce = CheckObject(Player.X, Player.Y + 1);
                        if (ce != null)
                        {
                            _currentCelestialObject = ce;
                            _menu = ce.Menu;
                            _menuActive = true;
                        }
                    }
                }
                else if (keyPress.Key == RLKey.Left)
                {
                    if (FuelCheck()) return;
                    RandomAction.Go();
                    if (_currentSectorMap.SectorMap.GetCell(Player.X - 1, Player.Y).IsWalkable)
                    {
                        Player.X--;
                        Player.Fuel--;
                    }
                    else
                    {
                        ICelestialObject ce = CheckObject(Player.X - 1, Player.Y);
                        if (ce != null)
                        {
                            _currentCelestialObject = ce;
                            _menu = ce.Menu;
                            _menuActive = true;
                        }
                    }
                }
                else if (keyPress.Key == RLKey.Right)
                {
                    if (FuelCheck()) return;
                    RandomAction.Go();
                    if (_currentSectorMap.SectorMap.GetCell(Player.X + 1, Player.Y).IsWalkable)
                    {
                        Player.X++;
                        Player.Fuel--;
                    }
                    else
                    {
                        ICelestialObject ce = CheckObject(Player.X + 1, Player.Y);
                        if (ce != null)
                        {
                            _currentCelestialObject = ce;
                            _menu = ce.Menu;
                            _menuActive = true;
                        }
                    }
                }
            }
        }

        private static ICelestialObject CheckObject(int x, int y)
        {
            return _currentSectorMap.CelestialObjects.FirstOrDefault(c => c.Y == y && c.X == x);
        }

        private static bool FuelCheck()
        {
            if (Player.Fuel < 1)
            {
                _statusText = "Out of Fuel!";
                return true;
            }
            _statusText = "                      ";
            return false;
        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            _rootConsole.Clear();

            foreach (ICelestialObject ce in _currentSectorMap.CelestialObjects)
            {
                Cell cell = _currentSectorMap.SectorMap.GetCell(ce.X, ce.Y);
                _currentSectorMap.SectorMap.SetCellProperties(ce.X, ce.Y, false, false, cell.IsExplored);
            }

            _currentSectorMap.SectorMap.ComputeFov(Player.X, Player.Y, Player.SensorRange, true);

            foreach (var cell in _currentSectorMap.SectorMap.GetAllCells())
            {
                if (cell.IsInFov)
                {
                    _currentSectorMap.SectorMap.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                    if (cell.IsWalkable)
                    {
                        _rootConsole.Set(cell.X, cell.Y, RLColor.Gray, null, '.');
                    }
                    else
                    {
                        _rootConsole.Set(cell.X, cell.Y, RLColor.LightGray, null, ' ');
                        foreach (ICelestialObject ce in _currentSectorMap.CelestialObjects)
                        {
                            if (ce.X == cell.X && ce.Y == cell.Y)
                            {
                                _rootConsole.Set(ce.X, ce.Y, ce.Color, null, ce.Symbol);
                            }
                        }
                    }
                }
                else if (cell.IsExplored)
                {
                    if (cell.IsWalkable)
                    {
                        _rootConsole.Set(cell.X, cell.Y, new RLColor(30, 30, 30), null, '.');
                    }
                    else
                    {
                        _rootConsole.Set(cell.X, cell.Y, RLColor.Gray, null, ' ');
                        foreach (ICelestialObject ce in _currentSectorMap.CelestialObjects)
                        {
                            if (ce.X == cell.X && ce.Y == cell.Y)
                            {
                                _rootConsole.Set(ce.X, ce.Y, new RLColor(30, 30, 30), null, ce.Symbol);
                            }
                        }
                    }
                }
            }

            // Set the player's symbol after the map symbol to make sure it is draw
            _rootConsole.Set(Player.X, Player.Y, RLColor.LightGreen, null, '@');

            _rootConsole.Print(77, 1, "SpaceRL", RLColor.White);
            _rootConsole.Print(77, 3, "ShipName:", RLColor.White);
            _rootConsole.Print(78, 4, "Location : " + _currentSectorMap.Name, RLColor.White);
            _rootConsole.Print(78, 5, "Sensor Strength : " + Player.SensorRange, RLColor.White);
            _rootConsole.Print(78, 6, "Fuel : " + Player.Fuel + " / " + Player.MaxFuel, RLColor.White);
            _rootConsole.Print(78, 7, "Fuel Probes : " + Player.FuelProbes, RLColor.White);
            _rootConsole.Print(78, 8, "Cargo : " + Player.CargoHold + " / " + Player.CargoHoldMax, RLColor.White);
            _rootConsole.Print(78, 9, "Credits : " + Player.Credits, RLColor.White);
            _rootConsole.Print(78, 12, "Score : " + Player.Score, RLColor.Green);

            _rootConsole.Print(77, 40, _statusText, RLColor.Red);

            if (_menuActive)
            {
                _currentOptions = new List<IRlKeyOption>();
                _rootConsole.Print(18, 18, "========================================", RLColor.Green);
                _rootConsole.Print(20, 18, _currentCelestialObject.Name, RLColor.LightGreen);
                int i = 19;
                foreach (IRlKeyOption option in _menu.Options)
                {
                    _rootConsole.Print(18, i, "|                                      |", RLColor.Green);
                    _rootConsole.Print(20, i, option.Key.ToString() + " : " + option.OptionText, RLColor.Green);
                    _currentOptions.Add(option);
                    i++;
                }
                _rootConsole.Print(18, i++, "|                                      |", RLColor.Green);
                _rootConsole.Print(18, i, "|                                      |", RLColor.Green);
                _rootConsole.Print(20, i++, _cancel.Key.ToString() + " : " + _cancel.OptionText, RLColor.Green);
                _currentOptions.Add(_cancel);
                _rootConsole.Print(18, i, "========================================", RLColor.Green);
            }


            // Tell RLNET to draw the console that we set
            _rootConsole.Draw();
        }

        internal static void JumpToSector(int p)
        {
            if (!_sectorMap.ContainsKey(p))
                _sectorMap.Add(p, new Sector(_sectorMap.Where(x => x.Value == _currentSectorMap).Select(x => x.Key).FirstOrDefault()));
            int fromSector = _sectorMap.Where(x => x.Value == _currentSectorMap).Select(x => x.Key).FirstOrDefault();
            _sectorMap.TryGetValue(p, out _currentSectorMap);
            _currentSectorMap.JoinSector(fromSector);
        }
    }
}
