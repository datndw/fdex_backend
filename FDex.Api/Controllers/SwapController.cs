﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FDex.Application.DTOs.Swap;
using FDex.Application.Features.Swaps.Requests.Queries;
using FDex.Application.Responses.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nethereum.RPC.Eth.DTOs;

namespace FDex.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SwapController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SwapController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        // ví, filter lọc search phân trang
        public async Task<ActionResult<List<SwapDTOView>>> Get()
        {
            var swaps = await _mediator.Send(new GetSwapsRequest());
            return Ok(swaps);
        }
    }
}

