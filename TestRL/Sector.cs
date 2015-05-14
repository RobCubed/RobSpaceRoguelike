using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using RSS.Actions.NebulaActions;
using RSS.Actions.StationActions;
using RSS.Actions.WormholeActions;
using RSS.Actions.WreckageActions;
using RSS.CelestialObjects;
using RSS.Stack;
using RSS.Tools;

namespace RSS
{
    public class Sector
    {
        public List<ICelestialObject> CelestialObjects { get; set; }
        public List<int> Links { get; set; }
        public string Name { get; set; }
        public IMap SectorMap { get; set; }
        private bool _generated;

        // on create
        public Sector(int fromLink)
        {
            Name = NameGenHelper.GenerateSystemName();
            Links = new List<int>()
            {
                fromLink
            };
            SectorMap = Map.Create(new BorderOnlyMapCreationStrategy<Map>(Program.ScreenWidth - 30, Program.ScreenHeight - 30));
            CelestialObjects = new List<ICelestialObject>();
        }

        public void JoinSector()
        {
            if (!_generated) GenerateObjects();
            _generated = true;
        }

        internal void JoinSector(int fromSector)
        {
            if (!_generated) GenerateObjects();
            _generated = true;

            foreach (ICelestialObject ce in CelestialObjects)
            {
                if (ce.GetType() == typeof(Wormhole))
                {
                    Wormhole wh = (Wormhole) ce;
                    if (Program._sectorMap.ContainsKey(wh.JumpTo))
                    {
                        Sector sector;
                        Program._sectorMap.TryGetValue(wh.JumpTo, out sector);
                        if (sector != null)
                        {
                            foreach (ICelestialObject nce in sector.CelestialObjects)
                            {
                                if (ce.GetType() == typeof (Wormhole))
                                {
                                    Wormhole nwh = (Wormhole) ce;
                                    nwh.Name = "Wormhole to " + sector.Name;
                                }
                            }
                        }
                    }
                    if (wh.JumpTo == fromSector)
                    {
                        Program.Player.X = wh.X;
                        Program.Player.Y = wh.Y;
                    }
                }
            }
        }

        private static Point GenerateRandomPoint(List<Point> takenPoints)
        {
            int X = Ran.dom.Next(2, 74);
            int Y = Ran.dom.Next(2, 74);
            bool check = true;
            Point finalPoint = new Point();
            while (check)
            {
                bool match = false;
                foreach (Point point in takenPoints)
                {
                    if (point.X == X && point.Y == Y)
                    {
                        X = Ran.dom.Next(2, 74);
                        Y = Ran.dom.Next(2, 74);
                        match = true;
                    }
                }
                if (!match)
                {
                    check = false;
                    finalPoint = new Point(X, Y);
                    Point finalPointX = new Point(X + 1, Y);
                    Point finalPointY = new Point(X, Y);
                    Point finalPointXY = new Point(X+1, Y+1);
                    takenPoints.Add(finalPoint);
                    takenPoints.Add(finalPointX);
                    takenPoints.Add(finalPointXY);
                }
            }
            return finalPoint;
        }

        private void GenerateObjects()
        {
            var takenPoints = new List<Point>();
            takenPoints.Add(new Point(Program.Player.X, Program.Player.Y));
            // Check to see if this is the first sector... if it is, we don't want to make a link to itself
            if (Program.CurrentSector > 0)
            {
                Point p = GenerateRandomPoint(takenPoints);
                CelestialObjects.Add(new Wormhole(Links[0])
                {
                    Color = RLColor.Blue,
                    Symbol = '@',
                    X = p.X,
                    Y = p.Y,
                    Menu = new WormholeMenu()
                });
            }

            int runTimes = Ran.dom.Next(2, 4);
            for (int i = 0; i < runTimes; i++)
            {
                Point point = GenerateRandomPoint(takenPoints);
                CelestialObjects.Add(new Wormhole(++Program.CurrentSector)
                {
                    Color = RLColor.Blue,
                    Symbol = '@',
                    X = point.X,
                    Y = point.Y,
                    Menu = new WormholeMenu()
                });
                Links.Add(Program.CurrentSector);
            }


            runTimes = Ran.dom.Next(2, 15);
            for (int i = 0; i < runTimes; i++)
            {
                Point point = GenerateRandomPoint(takenPoints);
                CelestialObjects.Add(new Nebula()
                {
                    Color = RLColor.LightRed,
                    Name = "Nebula " + i,
                    Symbol = '#',
                    TotalFuel = Ran.dom.Next(0, 100),
                    X = point.X,
                    Y = point.Y,
                    Menu = new NebulaMenu()
                });
                point = GenerateRandomPoint(takenPoints);
                CelestialObjects.Add(new Wreckage()
                {
                    Color = RLColor.Yellow,
                    Name = "Wreckage " + i,
                    Symbol = 'W',
                    SalvageAvailable = Ran.dom.Next(0, 50),
                    X = point.X,
                    Y = point.Y,
                    Menu = new WreckageMenu()
                });
            }

            runTimes = Ran.dom.Next(2, 4);
            for (int i = 0; i < runTimes; i++)
            {
                Point point = GenerateRandomPoint(takenPoints);
                CelestialObjects.Add(new Station()
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
    }
}
