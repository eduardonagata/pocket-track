using System.Data.Entity;
using AutoMapper;
using Finbuckle.MultiTenant;

namespace EndpointHandlers;

class CategoriaFinanceiraHandler : IEndpointRouteHandler
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categorias-financeiras/{catagoriaSuperiorId}", ObterCategorias)
            .RequireAuthorization(options =>
            {
                options.RequireAuthenticatedUser();
            });
        // app.MapGet("/api/categorias-financeiras/{id}", ObterCategoria);
        app.MapPost("/api/categorias-financeiras", CreateCategoria)
            .RequireAuthorization(options =>
            {
                options.RequireAuthenticatedUser();
            });
        // app.MapPut("/api/categorias-financeiras/{id}", UpdateCategoria);
        // app.MapDelete("/api/categorias-financeiras/{id}", DeleteCategoria);
    }

    private static async Task<IResult> ObterCategorias(MultiTenantDbContext dbContext, IMapper mapper, string? CategoriaSuperiorId = null)
    {
        // Carregando categorias e suas subcategorias de forma aninhada
        IQueryable<CategoriaFinanceira> query = dbContext.CategoriasFinanceiras;
        if (CategoriaSuperiorId != null)
        {
            // Filtrando por Categoria Superior, se fornecido
            query = query.Where(c => c.CategoriaSuperiorId == CategoriaSuperiorId);
        }
        var categoriasFinanceiras = await query
            .Include(c => c.Subcategorias) // Incluindo subcategorias
            .ToListAsync();
        var readCategoriasFinanceirasDto = mapper.Map<List<ReadCategoriaFinanceiraDto>>(categoriasFinanceiras);
        return Results.Ok(readCategoriasFinanceirasDto);
    }

    private static async Task<IResult> CreateCategoria(MultiTenantDbContext dbContext, IMapper mapper,
        CreateCategoriaFinanceiraDto createCategoriaFinanceiraDto)
    {
        var categoriaFinanceira = mapper.Map<CategoriaFinanceira>(createCategoriaFinanceiraDto);
        await dbContext.CategoriasFinanceiras.AddAsync(categoriaFinanceira);
        await dbContext.SaveChangesAsync();
        var readCategoriaFinanceiraDto = mapper.Map<ReadCategoriaFinanceiraDto>(categoriaFinanceira);
        return Results.Ok(readCategoriaFinanceiraDto);
    }
}