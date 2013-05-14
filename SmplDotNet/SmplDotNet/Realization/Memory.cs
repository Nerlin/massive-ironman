using System;

namespace SmplDotNet.Realization
{
    [Serializable]
    struct Memory
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int Elements { get; set; }
        public ITransaction Transaction { get; set; }
    }
}