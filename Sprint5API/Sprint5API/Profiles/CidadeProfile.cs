using AutoMapper;
using Sprint5API.DTOs;
using Sprint5API.Models;

namespace Sprint5API.Profiles
{
    public class CidadeProfile : Profile
    {
        public CidadeProfile()
        {
            CreateMap<CidadeDTO, Cidade>();
            CreateMap<Cidade, CidadeDTO>();
        }
    }
}
