using AutoMapper;
using Store.Catalogo.Application.Dtos;
using Store.Catalogo.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Catalogo.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Categoria, CategoriaDto>();
            CreateMap<Produto, ProdutoDto>()
                .ForMember(d => d.Altura, o => o.MapFrom(s => s.Dimensoes.Altura))
                .ForMember(d => d.Largura, o => o.MapFrom(s => s.Dimensoes.Largura))
                .ForMember(d => d.Profundidade, o => o.MapFrom(s => s.Dimensoes.Profundidade));

            CreateMap<CategoriaDto, Categoria>()
                .ConstructUsing(c => new Categoria(c.Nome, c.Codigo));

            CreateMap<ProdutoDto, Produto>()
                .ConstructUsing(p => new Produto(p.Nome, p.Descricao, p.Ativo,
                    p.Valor, p.CategoriaId, p.Imagem, p.DataCadastro,
                    new Dimensoes(p.Altura, p.Largura, p.Profundidade)));
        }
    }
}
