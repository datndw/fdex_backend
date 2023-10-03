using System;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FDex.Application.DTOs.Reporter
{
    [Event("ReporterPosted")]
    public class ReporterPostedDTOBase : IEventDTO
    {
        [Parameter("address", "wallet", 1, true)]
        public virtual string Wallet { get; set; }
    }
}

