using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.Core
{
    public static class GameRandom
    {
        private static Random random;
        public static int NextInt(int a, int b)
        {
            if (random == null)
                random = new Random();
            return random.Next(a, b);
        }
        public static double NextDouble(double a, double b)
        {
            if (random == null)
                random = new Random();
            return random.NextDouble() * (b - a) + a;
        }
    }
}
