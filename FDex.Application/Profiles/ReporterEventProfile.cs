using System;
using AutoMapper;
using FDex.Application.DTOs.Reporter;
using FDex.Domain.Entities;

namespace FDex.Application.Profiles
{
	public class ReporterEventProfile : Profile
	{
		public ReporterEventProfile()
		{
			CreateMap<Reporter, ReporterDTOView>()
				.ForMember(dest => dest.LastReportedDate, opt => opt.MapFrom(src => ((DateTimeOffset)src.LastReportedDate).ToUnixTimeSeconds()))
				.ReverseMap()
				.ForMember(dest => dest.LastReportedDate, opt => opt.MapFrom(src => src.LastReportedDate.HasValue ? DateTimeOffset.FromUnixTimeSeconds(src.LastReportedDate.Value).DateTime : (DateTime?)null));
        }
	}
}

