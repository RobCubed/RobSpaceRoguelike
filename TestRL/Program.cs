using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using TestRL.Actions;
using TestRL.Actions.NebulaActions;
using TestRL.Actions.StationActions;
using TestRL.Actions.WreckageActions;
using TestRL.CelestialObjects;

namespace TestRL
{
    class Program
    {
        private static readonly int _screenWidth = 105;
        private static readonly int _screenHeight = 75;
        public static Player Player;
        private static RLRootConsole _rootConsole;
        private static IMap _map;
        private static string _statusText;
        private static List<ICelestialObject> _celestialObjects;
        private static ICelestialObject _currentCelestialObject;
        private static IRlMenu _menu;
        private static bool _menuActive;
        private static Cancel _cancel;
        private static List<IRlKeyOption> _currentOptions;
        
        static void Main(string[] args)
        {
            _statusText = "";
            Player = new Player();
            Player.X = 25;
            Player.Y = 25;
            Player.SensorRange = 25;
            Player.Fuel = 100;
            Player.MaxFuel = 100;
            Player.FuelProbes = 5;
            Player.Score = 0;
            Player.Credits = 0;
            Player.CargoHold = 0;
            Player.CargoHoldMax = 100;
            
            _cancel = new Cancel();
            _menuActive = false;
            _map = Map.Create(new BorderOnlyMapCreationStrategy<Map>(_screenWidth - 30, _screenHeight));
            GenerateObjects();
            string fontFileName = "terminal8x8.png";
            string consoleTitle = "RobSpaceRoguelike";
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);
            _rootConsole.Update += OnRootConsoleUpdate;
            _rootConsole.Render += OnRootConsoleRender;
            _rootConsole.Run();
        }

        private static Point GenerateRandomPoint(List<Point> takenPoints)
        {
            Random r = new Random();
            int X = r.Next(1, 75);
            int Y = r.Next(1, 75);
            bool check = true;
            Point finalPoint = new Point();
            while (check)
            {
                bool match = false;
                foreach (Point point in takenPoints)
                {
                    if (point.X == X && point.Y == Y)
                    {
                        X = r.Next(1, 75);
                        Y = r.Next(1, 75);
                        match = true;
                    }
                }
                if (!match)
                {
                    check = false;
                    finalPoint = new Point(X, Y);
                    takenPoints.Add(finalPoint);
                }
            }
            return finalPoint;
        }
        private static void GenerateObjects()
        {
            _celestialObjects = new List<ICelestialObject>();
            var takenPoints = new List<Point>();
            takenPoints.Add(new Point(Player.X, Player.Y));
            Random r = new Random();
            int runTimes = r.Next(2, 15);
            for (int i = 0; i < runTimes; i++)
            {
                Point point = GenerateRandomPoint(takenPoints);
                _celestialObjects.Add(new Nebula()
                {
                    Color = RLColor.LightRed,
                    Name = "Nebula " + i,
                    Symbol = '#',
                    TotalFuel = r.Next(0, 100),
                    X = point.X,
                    Y = point.Y,
                    Menu = new NebulaMenu()
                });
                point = GenerateRandomPoint(takenPoints);
                _celestialObjects.Add(new Wreckage()
                {
                    Color = RLColor.Yellow,
                    Name = "Wreckage " + i,
                    Symbol = 'W',
                    SalvageAvailable = r.Next(0, 50),
                    X = point.X,
                    Y = point.Y,
                    Menu = new WreckageMenu()
                });
           }

            runTimes = r.Next(2, 4);
            for (int i = 0; i < runTimes; i++)
            {
                Point point = GenerateRandomPoint(takenPoints);
                _celestialObjects.Add(new Station()
                {
                    Color = RLColor.LightBlue,
                    Name = "Nebula " + i,
                    Symbol = 'S',
                    X = point.X,
                    Y = point.Y,
                    Menu = new StationMenu()
                });
            }

            
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
                    if (_map.GetCell(Player.X, Player.Y - 1).IsWalkable)
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
                    if (_map.GetCell(Player.X, Player.Y + 1).IsWalkable)
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
                    if (_map.GetCell(Player.X - 1, Player.Y).IsWalkable)
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
                    if (_map.GetCell(Player.X + 1, Player.Y).IsWalkable)
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
            return _celestialObjects.FirstOrDefault(c => c.Y == y && c.X == x);
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
            
            foreach (ICelestialObject ce in _celestialObjects)
            {
                Cell cell = _map.GetCell(ce.X, ce.Y);
                _map.SetCellProperties(ce.X, ce.Y, false, false, cell.IsExplored);
            }

            _map.ComputeFov(Player.X, Player.Y, Player.SensorRange, true);
            
            foreach (var cell in _map.GetAllCells())
            {
                if (cell.IsInFov)
                {
                    _map.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                    if (cell.IsWalkable)
                    {
                        _rootConsole.Set(cell.X, cell.Y, RLColor.Gray, null, '.');
                    }
                    else
                    {
                        _rootConsole.Set(cell.X, cell.Y, RLColor.LightGray, null, ' ');
                        foreach (ICelestialObject ce in _celestialObjects)
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
                        foreach (ICelestialObject ce in _celestialObjects)
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
            _rootConsole.Print(78, 4, "Location : " + Player.X + "x" + Player.Y, RLColor.White);
            _rootConsole.Print(78, 5, "Sensor Strength : " + Player.SensorRange, RLColor.White);
            _rootConsole.Print(78, 6, "Fuel : " + Player.Fuel + " / " + Player.MaxFuel, RLColor.White);
            _rootConsole.Print(78, 7, "Fuel Probes : " + Player.FuelProbes, RLColor.White);
            _rootConsole.Print(78, 8, "Cargo : " + Player.CargoHold + " / " + Player.CargoHoldMax, RLColor.White);
            _rootConsole.Print(78, 9, "Credits : " + Player.Credits, RLColor.White);
            _rootConsole.Print(78, 10, "Score : " + Player.Score, RLColor.White);

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
    }
}
