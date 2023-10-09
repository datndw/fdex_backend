using System;
using FDex.Application.Contracts.Persistence;
using FDex.Domain.Entities;
using FDex.Persistence.DbContexts;

namespace FDex.Persistence.Repositories
{
	public class AddLiquidityRepository : GenericRepository<AddLiquidity>, IAddLiquidityRepository
	{
        public readonly FDexDbContext _context;
        public AddLiquidityRepository(FDexDbContext context) : base(context)
		{
			_context = context;
		}
	}
}

