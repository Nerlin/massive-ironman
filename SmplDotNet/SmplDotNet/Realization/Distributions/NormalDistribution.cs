using System;

namespace SmplDotNet.Realization.Distributions
{
    [Serializable]
    public class NormalDistribution: IDistribution
    {
        private readonly Random random = new Random();

        public int Mx { get; set; }
        public int Sigma { get; set; }

        public int Play()
        {
            double r, x, y;
            do
            {
                x = 2 * random.NextDouble() - 1;
                y = 2 * random.NextDouble() - 1;
                r = x * x + y * y;

            } while (r > 0 && r <= 1);

            double z0 = x * Math.Sqrt(-2 * Math.Log(r, Math.E) / r);
            return (int)Math.Round(Mx + Sigma * z0);
        }
    }
}
