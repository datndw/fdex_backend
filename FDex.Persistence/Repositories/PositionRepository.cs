using System;
using FDex.Application.Contracts.Persistence;
using FDex.Domain.Entities;
using FDex.Persistence.DbContexts;

namespace FDex.Persistence.Repositories
{
	public class PositionRepository : GenericRepository<Position>, IPositionRepository
	{
		private readonly FDexDbContext _context;
		public PositionRepository(FDexDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
