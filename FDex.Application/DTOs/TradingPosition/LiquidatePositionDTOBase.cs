using System;
using System.Numerics;
using FDex.Application.Enumerations;
using FDex.Application.Models.Application;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FDex.Application.DTOs.TradingPosition
{
    [Event("LiquidatePosition")]
    public class LiquidatePositionDTOBase : IEventDTO
    {
        [Parameter("bytes32", "key", 1, true)]
        public virtual byte[] Key { get; set; }

        [Parameter("address", "account", 2, false)]
        public virtual string Account { get; set; }

        [Parameter("address", "collateralToken", 3, false)]
        public virtual string CollateralToken { get; set; }

        [Parameter("address", "indexToken", 4, false)]
        public virtual string IndexToken { get; set; }

        [Parameter("uint8", "side", 5, false)]
        public virtual byte Side { get; set; }

        [Parameter("uint256", "size", 6, false)]
        public virtual BigInteger Size { get; set; }

        [Parameter("uint256", "collateralValue", 7, false)]
        public virtual BigInteger CollateralValue { get; set; }

        [Parameter("uint256", "reserveAmount", 8, false)]
        public virtual BigInteger ReserveAmount { get; set; }

        [Parameter("uint256", "indexPrice", 9, false)]
        public virtual BigInteger IndexPrice { get; set; }

        [Parameter("tuple", "pnl", 10, false)]
        public virtual SignedInt Pnl { get; set; }

        [Parameter("uint256", "feeValue", 11, false)]
        public virtual BigInteger FeeValue { get; set; }
    }
}

