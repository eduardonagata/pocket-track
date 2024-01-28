

public class CategoriaFinanceira
{
    public string? TenantId { get; set; }
    public required string Id { get; set; }
    public required string Nome { get; set; }
    public TipoCategoriaFinanceira Tipo { get; set; }
    public virtual IEnumerable<CategoriaFinanceira>? Subcategorias { get; set; }
    public string? CategoriaSuperiorId { get; set; }
}