using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RLNET;
using RogueSharp;
using RSS.Actions;
using RSS.Actions.NebulaActions;
using RSS.Actions.RandomActions;
using RSS.Actions.StationActions;
using RSS.Actions.WreckageActions;
using RSS.CelestialObjects;
using RSS.Tools;

namespace RSS
{
    class Program
    {
        public static readonly int ScreenWidth = 105;
        public static readonly int ScreenHeight = 105;
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
        public static InformationLog<int, string> InfoLog;
        
        static void Main(string[] args)
        {
            _statusText = "";
            Player = new Player();

            InfoLog = new InformationLog<int, string>();
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
            
            // TESTING - Only uncomment this if you want to pre-generate (and travel through) a number of systems.
            //TestRun(50000);

            //TestLog();

            _rootConsole.Run();
        }

        private static void TestLog()
        {
            InfoLog.AddEntry("[00001] eorhkerpohkeroi ejoih erjoijh eroij herh eeorhkerpohkeroi ejoih erjoijh eroij herh eeorhkerpohkeroi ejoih erjoijh eroij herh e", true);
            InfoLog.AddEntry("[00002] 23123 2354 43 5 345 345 ", true);
            InfoLog.AddEntry("[00003] THIS IS A TEST. IT'S A TEST. IT'S JUST A TEST. TESTING. TESTING. TESTING.", true);
            InfoLog.AddEntry("[00004] 123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789", true);
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

            //Á : bottom-T
            //Â : top-T
            //Ã : left-T
            //Å : +

            //¿ : topright
            //Ù : bottomright

            //Ú : topleft
            //À : bottomleft

            //Ä : horizontal
            //³ : vertical


            _rootConsole.Print(0, 75, "ÚÄÄ SHIP'S LOG ÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄ¿", RLColor.White);
            for (int j = 76; j < 104; j++)
            {
                _rootConsole.Print(0, j, "³                                                                         ³", RLColor.White);
            }

            int startingPoint = InfoLog.Count - 27;
            //if (startingPoint < 0)
            //{
            //    for (int j = 0; j <= InfoLog.Count; j++)
            //    {
            //        string currentLine;
            //        InfoLog.TryGetValue(j, out currentLine);
            //        _rootConsole.Print(3, j + 76, currentLine, RLColor.Green);
            //    }
            //}
            //else
            //{
                int line = 0;
                for (int j = startingPoint; j <= InfoLog.Count; j++)
                {
                    string currentLine = " ";
                    InfoLog.TryGetValue(j, out currentLine);
                    _rootConsole.Print(3, line + 76, currentLine, RLColor.Green);
                    line++;
                }
            //}

            _rootConsole.Print(0, 104, "ÀÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÙ", RLColor.White);
            // Tell RLNET to draw the console that we set
            
            _rootConsole.Draw();
        }

        internal static void JumpToSector(int p)
        {
            if (!_sectorMap.ContainsKey(p))
            {
                _sectorMap.Add(p, new Sector(_sectorMap.Where(x => x.Value == _currentSectorMap).Select(x => x.Key).FirstOrDefault()));
                Player.Score += 15;
            }
            int fromSector = _sectorMap.Where(x => x.Value == _currentSectorMap).Select(x => x.Key).FirstOrDefault();
            _sectorMap.TryGetValue(p, out _currentSectorMap);
            _currentSectorMap.JoinSector(fromSector);
            InfoLog.AddEntry("Now in " + _currentSectorMap.Name, true);
        }

        private static void TestRun(int numSystems)
        {
            HashSet<int> ints = new HashSet<int>();
            for (int i = 0; i < 50000; i++)
            {
                foreach (ICelestialObject ce in _currentSectorMap.CelestialObjects)
                {
                    if (ce.GetType() == typeof (Wormhole))
                    {
                        Wormhole wh = (Wormhole) ce;
                        Sector sectorCheck;
                        _sectorMap.TryGetValue(wh.JumpTo, out sectorCheck);
                        if (sectorCheck == _currentSectorMap || ints.Contains(wh.JumpTo))
                        {
                            continue;
                        }
                        else
                        {
                            JumpToSector(wh.JumpTo);
                            ints.Add(wh.JumpTo);
                            Console.WriteLine("Now in " + _currentSectorMap.Name);
                            break;
                        }
                    }
                }
            }
        }
    }
}
