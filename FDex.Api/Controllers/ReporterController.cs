using System;
using FDex.Application.DTOs.Reporter;
using FDex.Application.DTOs.Swap;
using FDex.Application.Features.Reporters.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FDex.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporterController : ControllerBase
	{
        private readonly IMediator _mediator;

        public ReporterController(IMediator mediator)
		{
            _mediator = mediator;
		}

        [HttpGet]
        public async Task<ActionResult<List<ReporterDTOView>>> Get()
        {
            var reporters = await _mediator.Send(new GetReportersRequest());
            return Ok(reporters);
        }
    }
}

