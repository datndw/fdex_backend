using System;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FDex.Application.DTOs.Reporter
{
    [Event("ReporterRemoved")]
    public class ReporterRemovedDTOBase : IEventDTO
    {
        [Parameter("address", "wallet", 1, true)]
        public virtual string Wallet { get; set; }
    }
}

