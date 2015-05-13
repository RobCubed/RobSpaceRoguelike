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
            SectorMap = Map.Create(new BorderOnlyMapCreationStrategy<Map>(Program.ScreenWidth - 30, Program.ScreenHeight));
            CelestialObjects = new List<ICelestialObject>();
        }

        public void JoinSector()
        {
            if (!_generated) GenerateObjects();
            _generated = true;
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

        private void GenerateObjects()
        {
            var takenPoints = new List<Point>();
            takenPoints.Add(new Point(Program.Player.X, Program.Player.Y));
            Random r = new Random();
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

            int runTimes = r.Next(2, 4);
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


            runTimes = r.Next(2, 15);
            for (int i = 0; i < runTimes; i++)
            {
                Point point = GenerateRandomPoint(takenPoints);
                CelestialObjects.Add(new Nebula()
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
                CelestialObjects.Add(new Wreckage()
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
