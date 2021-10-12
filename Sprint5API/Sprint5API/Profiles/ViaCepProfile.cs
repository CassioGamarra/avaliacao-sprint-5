using AutoMapper;
using Sprint5API.DTOs;
using Sprint5API.Models;

namespace Sprint5API.Profiles
{
    public class ViaCepProfile : Profile
    {
        public ViaCepProfile()
        {
            CreateMap<ViaCepDTO, ViaCep>();
            CreateMap<ViaCep, ViaCepDTO>();
        } 
    }
}
