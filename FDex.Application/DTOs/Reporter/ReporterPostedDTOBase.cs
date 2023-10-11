using System;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FDex.Application.DTOs.Reporter
{
    [Event("ReporterPosted")]
    public class ReporterPostedDTOBase : IEventDTO
    {
        [Parameter("address", "wallet", 1, false)]
        public virtual string Wallet { get; set; }
    }
}

