using AutoMapper;
using Iprox.Application.Common.Dtos;
using Iprox.Domain.Entities;

namespace Iprox.Application.TvShowsApi.Configurations;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Base, BaseDto>().ReverseMap();
        CreateMap<TvShow, TvShowDto>().ReverseMap();
        CreateMap<TvShow, CreateTvShowDto>().ReverseMap();
        CreateMap<TvShow, PatchTvShowDto>().ReverseMap();
        CreateMap<Genre, GenreDto>().ReverseMap();
    }
}
