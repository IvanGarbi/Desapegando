﻿using AutoMapper;
using System.Net;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Models;

namespace Desapegando.Application.AutoMapper;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<CondominoRegisterViewModel, Condomino>().ReverseMap();
        CreateMap<CondominoInativoViewModel, Condomino>().ReverseMap();
        CreateMap<CondominoViewModel, Condomino>().ReverseMap();
        CreateMap<ProdutoViewModel, Produto>().ReverseMap();
        CreateMap<GetProdutoViewModel, Produto>().ForMember(x => x.ProdutoImagens, z => z.MapFrom(a => a.ProdutoImagemViewModels)).ReverseMap();
        CreateMap<ProdutoImagemViewModel, ProdutoImagem>().ReverseMap();
        CreateMap<UpdateProdutoViewModel, Produto>().ReverseMap();
        CreateMap<CampanhaViewModel, Campanha>().ReverseMap();
        CreateMap<GetCampanhaViewModel, Campanha>().ForMember(x => x.CampanhaImagens, z => z.MapFrom(a => a.CampanhaImagemViewModels)).ReverseMap();
        CreateMap<CampanhaImagemViewModel, CampanhaImagem>().ReverseMap();
        CreateMap<UpdateCampanhaViewModel, Campanha>().ReverseMap();
    }
}