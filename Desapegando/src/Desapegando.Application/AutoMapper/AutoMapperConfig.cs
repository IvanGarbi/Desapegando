using AutoMapper;
using System.Net;
using Desapegando.Application.Models;
using Desapegando.Business.Models;

namespace Desapegando.Application.AutoMapper;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<CondominoRegisterViewModel, Condomino>().ReverseMap();
    }
}