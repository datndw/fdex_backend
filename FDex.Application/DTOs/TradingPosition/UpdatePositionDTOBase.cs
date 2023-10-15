using System;
using System.Numerics;
using FDex.Application.Enumerations;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FDex.Application.DTOs.TradingPosition
{
	[Event("UpdatePosition")]
	public class UpdatePositionDTOBase : IEventDTO
	{
        [Parameter("bytes32", "key", 1, true)]
        public virtual string Key { get; set; }

        [Parameter("uint256", "size", 2, false)]
        public virtual BigInteger Size { get; set; }

        [Parameter("uint256", "collateralValue", 3, false)]
        public virtual BigInteger CollateralValue { get; set; }
        
        [Parameter("uint256", "entryPrice", 4, false)]
        public virtual BigInteger EntryPrice { get; set; }

        [Parameter("uint256", "entryInterestRate", 5, false)]
        public virtual BigInteger EntryInterestRate { get; set; }

        [Parameter("uint256", "reserveAmount", 6, false)]
        public virtual BigInteger ReserveAmount { get; set; }

        [Parameter("uint256", "indexPrice", 7, false)]
        public virtual BigInteger IndexPrice { get; set; }
    }
}

