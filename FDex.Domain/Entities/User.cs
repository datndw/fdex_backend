using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FDex.Domain.Entities
{
	public class User
	{
        public string Wallet { get; set; }
        public string? ReferalCode { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<Swap>? Swaps { get; set; }
    }
}

