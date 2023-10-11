using System;
using FDex.Application.DTOs.Swap;
using FDex.Application.Features.Users.Requests.Commands;
using FDex.Application.Features.Users.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FDex.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<bool>> GetAccountStatus([FromBody] string wallet)
        {
            bool accountStatus = await _mediator.Send(new GetAccountStatusRequest() { Wallet = wallet});
            return Ok(accountStatus);
        }

        [HttpPost("[action]/{referralUser}")]
        public async Task<ActionResult> PostReferredUser([FromBody] string referringUser, string referralUser)
        {
            var command = new UpdateReferredUserCommand() { ReferringUser = referringUser, ReferralUser = referralUser };
            await _mediator.Send(command);
            return Ok();
        }
    }
}

