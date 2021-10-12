using AutoMapper;
using Sprint5API.DTOs;
using Sprint5API.Models;

namespace Sprint5API.Profiles
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<ClienteDTO, Cliente>();
            CreateMap<Cliente, ClienteDTO>();
        }
    }
}
