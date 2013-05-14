using System;

namespace SmplDotNet.Realization.Distributions
{
    [Serializable]
    public class ExponentialDistribution : IDistribution
    {
        private readonly Random random = new Random();

        public int Min { get; set; }
        public int Max { get; set; }

        public int Play()
        {
            return (int)Math.Round(-Max * Math.Log(random.NextDouble(), Math.E)) + Min;
        }
    }
}
