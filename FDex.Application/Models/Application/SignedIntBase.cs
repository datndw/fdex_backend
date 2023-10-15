using System;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FDex.Application.Models.Application
{
    public class SignedIntBase
    {
        [Parameter("uint256", "sig", 1)]
        public virtual BigInteger Sig { get; set; }
        [Parameter("uint256", "abs", 2)]
        public virtual BigInteger Abs { get; set; }
    }
}

