using System;
namespace FDex.Application.DTOs.Reporter
{
	public class ReporterDTOView
	{
        public string Wallet { get; set; }
        public long? ReportCount { get; set; }
        public long? LastReportedDate { get; set; }
    }
}

