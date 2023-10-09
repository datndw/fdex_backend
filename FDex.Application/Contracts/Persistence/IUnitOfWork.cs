using System;
namespace FDex.Application.Contracts.Persistence
{
	public interface IUnitOfWork
	{
        ISwapRepository SwapRepository { get; }
        IUserRepository UserRepository { get; }
        IReporterRepository ReporterRepository { get; }
        IAddLiquidityRepository AddLiquidityRepository { get; }

        Task Save();
    }
}

