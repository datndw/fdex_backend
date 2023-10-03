using System;
using AutoMapper;
using FDex.Application.Contracts.Persistence;
using FDex.Application.DTOs.Reporter;
using FDex.Application.DTOs.Swap;
using FDex.Application.Features.Reporters.Requests.Queries;
using MediatR;

namespace FDex.Application.Features.Reporters.Handlers.Queries
{
	public class GetReportersRequestHandler : IRequestHandler<GetReportersRequest, List<ReporterDTOView>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetReportersRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ReporterDTOView>> Handle(GetReportersRequest request, CancellationToken cancellationToken)
        {
            var reporters = await _unitOfWork.ReporterRepository.GetAllAsync();
            List<ReporterDTOView> reporterDTOs = _mapper.Map<List<ReporterDTOView>>(reporters);
            return reporterDTOs;
        }
    }
}

