using System;
using MediatR;

namespace FDex.Application.Features.Users.Requests.Queries
{
	public class GetAccountStatusRequest : IRequest<bool>
    {
        public string Wallet { get; set; }
    }
}

