namespace RSS
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int SensorRange { get; set; }
        public int Fuel { get; set; }
        public int MaxFuel { get; set; }
        public int FuelProbes { get; set; }
        public int CargoHold { get; set; }
        public int CargoHoldMax { get; set; }
        public int Score { get; set; }
        public int Credits { get; set; }

        public Player()
        {
            X = 25;
            Y = 25;
            SensorRange = 30;
            Fuel = 1000;
            MaxFuel = 1000;
            FuelProbes = 5;
            Score = 0;
            Credits = 0;
            CargoHold = 0;
            CargoHoldMax = 100;
        }
    }
}
