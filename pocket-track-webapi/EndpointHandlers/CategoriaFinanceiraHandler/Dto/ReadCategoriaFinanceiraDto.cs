

namespace EndpointHandlers;

public class ReadCategoriaFinanceiraDto
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public TipoCategoriaFinanceira Tipo { get; set; }
    public IEnumerable<ReadCategoriaFinanceiraDto>? Subcategorias { get; set; }
}