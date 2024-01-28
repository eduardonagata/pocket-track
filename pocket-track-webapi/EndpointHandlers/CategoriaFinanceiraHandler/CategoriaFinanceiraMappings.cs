using AutoMapper;

namespace EndpointHandlers;

class CategoriaFinanceiraMappings : Profile
{
    public CategoriaFinanceiraMappings()
    {
        CreateMap<CreateCategoriaFinanceiraDto, CategoriaFinanceira>();
        CreateMap<CategoriaFinanceira, ReadCategoriaFinanceiraDto>();
    }
}