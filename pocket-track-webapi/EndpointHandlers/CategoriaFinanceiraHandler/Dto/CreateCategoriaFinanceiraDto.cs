
namespace EndpointHandlers;

public class CreateCategoriaFinanceiraDto
{
    public required string Id { get; set; }
    public required string Nome { get; set; }
    public TipoCategoriaFinanceira Tipo { get; set; }
    public string? CategoriaSuperiorId { get; set; }
}