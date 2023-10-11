using System;
using AutoMapper;
using FDex.Application.Contracts.Persistence;
using FDex.Application.Features.Users.Requests.Commands;
using MediatR;

namespace FDex.Application.Features.Users.Handlers.Commands
{
	public class UpdateReferredUserCommandHandler : IRequestHandler<UpdateReferredUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateReferredUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
            _unitOfWork = unitOfWork;
            _mapper = mapper;
		}

        public async Task Handle(UpdateReferredUserCommand request, CancellationToken cancellationToken)
        {
            var referringUser = await _unitOfWork.UserRepository.FindAsync(request.ReferringUser);
            referringUser.ReferredUserOf = request.ReferralUser;
            referringUser.Level = 0;
            _unitOfWork.UserRepository.Update(referringUser);
        }
    }
}

