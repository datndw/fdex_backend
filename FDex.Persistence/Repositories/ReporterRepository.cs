using System;
using FDex.Application.Contracts.Persistence;
using FDex.Domain.Entities;
using FDex.Persistence.DbContexts;
using NBitcoin.Secp256k1;

namespace FDex.Persistence.Repositories
{
	public class ReporterRepository : GenericRepository<Reporter>, IReporterRepository
	{
        public readonly FDexDbContext _context;
        public ReporterRepository(FDexDbContext context) : base(context)
        {
			_context = context;
		}
	}
}

