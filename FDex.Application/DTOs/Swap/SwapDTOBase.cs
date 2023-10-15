using System;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FDex.Application.DTOs.Swap
{
    [Event("Swap")]
    public class SwapDTOBase : IEventDTO
    {
        [Parameter("address", "sender", 1, true)]
        public virtual string Wallet { get; set; }

        [Parameter("address", "tokenIn", 2, false)]
        public virtual string TokenIn { get; set; }

        [Parameter("address", "tokenOut", 3, false)]
        public virtual string TokenOut { get; set; }

        [Parameter("uint256", "amountIn", 4, false)]
        public virtual BigInteger AmountIn { get; set; }

        [Parameter("uint256", "amountOut", 5, false)]
        public virtual BigInteger AmountOut { get; set; }

        [Parameter("uint256", "fee", 6, false)]
        public virtual BigInteger Fee { get; set; }

        [Parameter("uint256", "markPrice", 7, false)]
        public virtual BigInteger MarkPrice { get; set; }
    }
}

