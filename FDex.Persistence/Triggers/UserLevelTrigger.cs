using System;
using EntityFrameworkCore.Triggered;
using FDex.Application.Contracts.Persistence;
using FDex.Domain.Entities;

namespace FDex.Persistence.Triggers
{
	public class UserLevelTrigger : IAfterSaveAsyncTrigger<User>
    {
        private readonly IUnitOfWork _unitOfWork;
		public UserLevelTrigger(IUnitOfWork unitOfWork)
		{
            _unitOfWork = unitOfWork;
		}

        public async Task AfterSaveAsync(ITriggerContext<User> context, CancellationToken cancellationToken)
        {
            if(context.Entity.TradePoint.HasValue && context.Entity.ReferralPoint.HasValue)
            {
                if (context.Entity.TradePoint >= 1000 && context.Entity.ReferralPoint >= 1000)
                {
                    context.Entity.Level = 1;
                }
                else if (context.Entity.TradePoint >= 2000 && context.Entity.ReferralPoint >= 2000)
                {
                    context.Entity.Level = 2;
                }
                else if (context.Entity.TradePoint >= 3000 && context.Entity.ReferralPoint >= 3000)
                {
                    context.Entity.Level = 3;
                }    
                else
                {
                    context.Entity.Level = 0;
                }
            }
            else
            {
                context.Entity.Level = null;
            }
            await _unitOfWork.SaveAsync();
        }
    }
}

