using System;
using FDex.Application.DTOs.Reporter;
using MediatR;

namespace FDex.Application.Features.Reporters.Requests.Queries
{
	public class GetReportersRequest : IRequest<List<ReporterDTOView>>
    {
	}
}

