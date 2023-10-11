using System;
using AutoMapper;
using FDex.Application.Contracts.Persistence;
using FDex.Application.Features.Users.Requests.Queries;
using MediatR;

namespace FDex.Application.Features.Users.Handlers.Queries
{
	public class GetAccountStatusRequestHandler : IRequestHandler<GetAccountStatusRequest, bool>
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAccountStatusRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
            _unitOfWork = unitOfWork;
            _mapper = mapper;
		}

        public async Task<bool> Handle(GetAccountStatusRequest request, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.UserRepository.FindAsync(request.Wallet);
            return !String.IsNullOrEmpty(account.ReferredUserOf);
        }
    }
}

