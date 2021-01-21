namespace Client.Models
{
    public class HorseStats
    {
        public int HP { get; private set; }
        public int Stamina { get; private set; }
        public int Speed { get; private set; }
        public int Acceleration { get; private set; }

        public HorseStats(){}

        public HorseStats(int hp, int stamina, int speed, int acceleration)
        {
            HP = hp;
            Stamina = stamina;
            Speed = speed;
            Acceleration = acceleration;
        }
    }
}