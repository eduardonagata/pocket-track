

public class ContaBancaria
{
    public string? TenantId { get; set; }
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string Banco { get; set; }
    public string? Agencia { get; set; }
    public string? Conta { get; set; }
}