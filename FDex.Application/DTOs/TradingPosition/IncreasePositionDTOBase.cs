using System;
using System.Drawing;
using System.Numerics;
using FDex.Application.Enumerations;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FDex.Application.DTOs.TradingPosition
{
    [Event("IncreasePosition")]
    public class IncreasePositionDTOBase : IEventDTO
    {
        [Parameter("bytes32", "key", 1, true)]
        public virtual byte[] Key { get; set; }

        [Parameter("address", "wallet", 2, false)]
        public virtual string Wallet { get; set; }

        [Parameter("address", "collateralToken", 3, false)]
        public virtual string CollateralToken { get; set; }

        [Parameter("address", "indexToken", 4, false)]
        public virtual string IndexToken { get; set; }

        [Parameter("uint256", "collateralValue", 5, false)]
        public virtual BigInteger CollateralValue { get; set; }

        [Parameter("uint256", "sizeChanged", 6, false)]
        public virtual BigInteger SizeChanged { get; set; }

        [Parameter("uint8", "side", 7, false)]
        public virtual byte Side { get; set; }

        [Parameter("uint256", "indexPrice", 8, false)]
        public virtual BigInteger IndexPrice { get; set; }

        [Parameter("uint256", "feeValue", 9, false)]
        public virtual BigInteger FeeValue { get; set; }
    }
}

