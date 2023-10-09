using System;
using System.Numerics;
using AutoMapper;
using FDex.Application.DTOs.Swap;
using FDex.Domain.Entities;

namespace FDex.Application.Profiles
{
	public class SwapEventProfile : Profile
	{
        public SwapEventProfile()
        {
            CreateMap<SwapDTOAdd, Swap>()
                .ForMember(dest => dest.TxnHash, opt => opt.MapFrom(src => src.TxnHash))
                .ForMember(dest => dest.Wallet, opt => opt.MapFrom(src => src.Wallet))
                .ForMember(dest => dest.TokenIn, opt => opt.MapFrom(src => src.TokenIn))
                .ForMember(dest => dest.TokenOut, opt => opt.MapFrom(src => src.TokenOut))
                .ForMember(dest => dest.AmountIn, opt => opt.MapFrom(src => src.AmountIn.ToString()))
                .ForMember(dest => dest.AmountOut, opt => opt.MapFrom(src => src.AmountOut.ToString()))
                .ForMember(dest => dest.Fee, opt => opt.MapFrom(src => src.Fee.ToString()))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time))
                .ReverseMap()
                .ForMember(dest => dest.AmountIn, opt => opt.MapFrom(src => BigInteger.Parse(src.AmountIn)))
                .ForMember(dest => dest.AmountOut, opt => opt.MapFrom(src => BigInteger.Parse(src.AmountOut)))
                .ForMember(dest => dest.Fee, opt => opt.MapFrom(src => BigInteger.Parse(src.Fee)));

            CreateMap<Swap, SwapDTOView>()
                .ForMember(dest => dest.TokenIn, opt => opt.MapFrom(src => src.TokenIn))
                .ForMember(dest => dest.TokenOut, opt => opt.MapFrom(src => src.TokenOut))
                .ForMember(dest => dest.AmountIn, opt => opt.MapFrom(src => src.AmountIn.ToString()))
                .ForMember(dest => dest.AmountOut, opt => opt.MapFrom(src => src.AmountOut.ToString()))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time));
        }
	}
}

