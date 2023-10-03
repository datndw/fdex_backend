using System;
using System.Numerics;

namespace FDex.Domain.Entities
{
	public class Swap
	{
        public string TxnHash { get; set; }
        public string Wallet { get; set; }
        public string TokenIn { get; set; }
        public string TokenOut { get; set; }
        public string AmountIn { get; set; }
        public string AmountOut { get; set; }
        public string Fee { get; set; }
        public DateTime Time { get; set; }

        public User User { get; set; }
    }
}