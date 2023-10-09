using System;
using System.Numerics;
using FDex.Application.Contracts.Persistence;
using FDex.Domain.Entities;
using FDex.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace FDex.Persistence.Repositories
{
	public class UserRepository : GenericRepository<User>, IUserRepository
	{
        public readonly FDexDbContext _context;

        public UserRepository(FDexDbContext context) : base(context)
        {
			_context = context;
		}

        public async Task<object> GetDashboardItemDatas()
        {
            var totalUser = _context.Users;
            BigInteger accuredFees = 0;
            BigInteger accuredFeesChange = 0;
            BigInteger assetsUnderManagement = 0;
            BigInteger assetsUnderManagementChange = 0;
            BigInteger totalTradingVolumn = 0;
            BigInteger totalTradingVolumnChange = 0;
            BigInteger totalUserCount = totalUser.Count();
            BigInteger totalUserCountChange = await totalUser.Where(u => u.CreatedDate.Date > DateTime.Now.AddDays(-1).Date).CountAsync();
            List<Swap> swaps = _context.Swaps.ToList();
            List<AddLiquidity> addLiquidities = _context.AddLiquidities.ToList();
            foreach (var swap in swaps)
            {
                accuredFees += BigInteger.Parse(swap.Fee);
                if(swap.Time.Date > DateTime.Now.AddDays(-1).Date)
                {
                    accuredFeesChange += BigInteger.Parse(swap.Fee);
                }
            }
            foreach(var al in addLiquidities)
            {
                accuredFees += BigInteger.Parse(al.Fee);
                if (al.DateAdded.Date > DateTime.Now.AddDays(-1).Date)
                {
                    accuredFeesChange += BigInteger.Parse(al.Fee);
                }
            }    
            var dashboardItemDatas = new
            {
                TotalUserCount = totalUserCount.ToString(),
                TotalUserCountChange = totalUserCountChange.ToString(),
                AccuredFees = accuredFees.ToString(),
                AccuredFeesChange = accuredFeesChange.ToString(),
                TotalTradingVolumn = totalTradingVolumn.ToString(),
                TotalTradingVolumnChange = totalTradingVolumnChange.ToString(),
                AssetsUnderManagement = assetsUnderManagement.ToString(),
                AssetsUnderManagementChange = assetsUnderManagementChange.ToString()
            };
            return dashboardItemDatas;
        }
    }
}

