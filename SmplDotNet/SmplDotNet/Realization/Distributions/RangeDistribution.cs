using System;

namespace SmplDotNet.Realization.Distributions
{
    [Serializable]
    public class RangeDistribution: IDistribution
    {
        private readonly Random random = new Random();

        public int Min { get; set; }
        public int Max { get; set; }

        public int Play()
        {
            return random.Next(Min, Max);
        }
    }
}
