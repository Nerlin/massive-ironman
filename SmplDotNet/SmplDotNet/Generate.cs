using System;

namespace SmplDotNet
{
    static class Generate
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// Возвращает случайное число согласно экспоненциальному закону распределения.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int Exponential(int m)
        {
            return (int) Math.Round(-m * Math.Log(Random.NextDouble(), Math.E));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public static int Normal(int mx, int sigma)
        {
            double r, x, y;
            do
            {
                x = 2 * Random.NextDouble() - 1;
                y = 2 * Random.NextDouble() - 1;
                r = x*x + y*y;

            } while (r > 0 && r <= 1);

            double z0 = x*Math.Sqrt(-2*Math.Log(r, Math.E)/r);
            return (int) Math.Round(mx+sigma*z0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int Poisson(int m)
        {
            const double p = 0.1;
            const int accuracy = 4;

            int x = 0;
            for (int i = 1; i < Math.Round(m/p)/accuracy; i++)
            {
                if (Random.NextDouble() < p)
                    x += accuracy;
            }

            return x;
        }
    }
}
