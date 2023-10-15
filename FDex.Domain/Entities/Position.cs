using System;
using System.Drawing;
using System.Numerics;

namespace FDex.Domain.Entities
{
	public class Position
	{
		public string Key { get; set; }
		public string Wallet { get; set; }
		public string CollateralToken { get; set; }
		public string IndexToken { get; set; }
        public string CollateralValue { get; set; }
        public string SizeChanged { get; set; }
        public string Side { get; set; }
        public string IndexPrice { get; set; }
        public string FeeValue { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }

        public User User { get; set; }
    }
}

