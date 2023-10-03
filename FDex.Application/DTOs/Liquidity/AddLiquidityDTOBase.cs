using System;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FDex.Application.DTOs.Liquidity
{
    [Event("AddLiquidity")]
    public class AddLiquidityDTOBase : IEventDTO
    {
        [Parameter("address", "wallet", 1, true)]
        public virtual string Wallet { get; set; }

        [Parameter("address", "asset", 2, false)]
        public virtual string Asset { get; set; }

        [Parameter("uint256", "amount", 3, false)]
        public virtual BigInteger Amount { get; set; }

        [Parameter("uint256", "fee", 4, false)]
        public virtual BigInteger Fee { get; set; }

        [Parameter("uint256", "markPriceIn", 5, false)]
        public virtual BigInteger MarkPriceIn { get; set; }
    }
}

