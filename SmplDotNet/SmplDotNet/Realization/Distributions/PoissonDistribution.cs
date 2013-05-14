using System;

namespace SmplDotNet.Realization.Distributions
{
    [Serializable]
    public class PoissonDistribution: IDistribution
    {
        private readonly Random random = new Random();

        public int Min { get; set; }
        public int Max { get; set; }

        public int Play()
        {
            const double p = 0.1;
            const int accuracy = 4;

            int x = 0;
            for (int i = 1; i < Math.Round(Max / p) / accuracy; i++)
            {
                if (random.NextDouble() < p)
                    x += accuracy;
            }

            return x + Min;
        }
    }
}
