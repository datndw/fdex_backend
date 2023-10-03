using System;
namespace FDex.Domain.Entities
{
	public class Reporter
	{
        public string Wallet { get; set; }
        public long? ReportCount { get; set; }
        public DateTime? LastReportedDate { get; set; }
    }
}

