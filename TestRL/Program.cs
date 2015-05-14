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
using RSS.Menu;
using RSS.Tools;

namespace RSS
{
    class Program
    {
        public static readonly int ScreenWidth = 105;
        public static readonly int ScreenHeight = 90;
        public static Player Player;
        public static PopupMenu PopupMenu;
        public static bool ActiveMenu;
        private static RLRootConsole _rootConsole;
        private static string _statusText;
        private static ICelestialObject _currentCelestialObject;
        private static Sector _currentSectorMap;
        private static Cancel _cancel;
        public static Dictionary<int, Sector> SectorMap;
        public static int CurrentSector = 0;
        public static InformationLog<int, string> InfoLog;
        private static bool _gameOver;
        
        static void Main(string[] args)
        {
            _gameOver = false;
            _statusText = "";
            Player = new Player();

            InfoLog = new InformationLog<int, string>();
            SectorMap = new Dictionary<int, Sector>();
            SectorMap.Add(0, new Sector(CurrentSector));
            SectorMap.TryGetValue(0, out _currentSectorMap);
            _currentSectorMap.JoinSector();

            _cancel = new Cancel();
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
                if (ActiveMenu)
                {
                    foreach (IRlKeyOption rlko in PopupMenu.MenuItems)
                    {
                        if (keyPress.Key == rlko.Key)
                        {
                            rlko.Action(_currentCelestialObject);
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
                            PopupMenu = new PopupMenu(ce.Name, ce.Menu.Options, 10, 10);
                            ActiveMenu = true;
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
                            PopupMenu = new PopupMenu(ce.Name, ce.Menu.Options, 10, 10);
                            ActiveMenu = true;
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
                            PopupMenu = new PopupMenu(ce.Name, ce.Menu.Options, 10, 10);
                            ActiveMenu = true;
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
                            PopupMenu = new PopupMenu(ce.Name, ce.Menu.Options, 10, 10);
                            ActiveMenu = true;
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
                if (!_gameOver) InfoLog.AddEntry("Out of fuel! Game over. You ended with a score of " + Player.Score, true);
                _gameOver = true;
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

            DrawViewscreen();
            DrawShipLog();

            if (ActiveMenu)
            {
                PopupMenu.DrawMenu(_rootConsole);
            }

            _rootConsole.Draw();
        }

        private static void DrawShipLog()
        {
            _rootConsole.Print(0, ScreenHeight - 30, "ÚÄÄ SHIP'S LOG ÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄ¿", RLColor.White);
            for (int j = ScreenHeight - 29; j < 104; j++)
            {
                _rootConsole.Print(0, j, "³                                                                         ³", RLColor.White);
            }

            int startingPoint = InfoLog.Count - 27;
            int line = 0;
            for (int j = startingPoint; j <= InfoLog.Count; j++)
            {
                string currentLine = " ";
                InfoLog.TryGetValue(j, out currentLine);
                _rootConsole.Print(3, line + ScreenHeight - 29, currentLine, RLColor.Green);
                line++;
            }

            _rootConsole.Print(0, ScreenHeight - 1, "ÀÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÙ", RLColor.White);
        }

        private static void DrawViewscreen()
        {
            _rootConsole.Print(0, 0, "ÚÄÄ VIEWSCREEN ÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄ¿", RLColor.White);
            _rootConsole.Print(15, 0, " " + _currentSectorMap.Name + " ", RLColor.LightCyan);
            for (int j = 1; j < ScreenHeight - 31; j++)
            {
                _rootConsole.Print(0, j, "³", RLColor.White);
                _rootConsole.Print(ScreenWidth - 31, j, "³", RLColor.White);
            }
            _rootConsole.Print(0, ScreenHeight - 31, "ÀÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÄÙ", RLColor.White);
        }

        internal static void JumpToSector(int p)
        {
            if (!SectorMap.ContainsKey(p))
            {
                SectorMap.Add(p, new Sector(SectorMap.Where(x => x.Value == _currentSectorMap).Select(x => x.Key).FirstOrDefault()));
                Player.Score += 15;
            }
            int fromSector = SectorMap.Where(x => x.Value == _currentSectorMap).Select(x => x.Key).FirstOrDefault();
            SectorMap.TryGetValue(p, out _currentSectorMap);
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
                        SectorMap.TryGetValue(wh.JumpTo, out sectorCheck);
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
