using System;

namespace Foxy.Flipside
{

    public class Rng
    {

        private readonly Random rand;


        public Rng(int seed) =>
            rand = new Random(seed);


        public static Rng NewFromCurrentTime() =>
            new Rng(DateTime.Now.GetHashCode());


        public int InRange(int min, int max) =>
            rand.Next(min, max + 1);

        public float InRange(float min, float max) =>
            min + (float)rand.NextDouble() * (max - min);

        public bool CoinToss() =>
            rand.NextDouble() <= .5;

    }

}