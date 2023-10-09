using System;
namespace FDex.Application.DTOs.Swap
{
    public partial class SwapDTOAdd : SwapDTOBase {
        public string TxnHash { get; set; }
        public DateTime Time { get; set; }
    }
}

