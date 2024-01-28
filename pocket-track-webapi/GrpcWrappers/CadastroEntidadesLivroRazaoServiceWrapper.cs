using Grpc.Core;
using LivroRazao;

public class CadastroEntidadesLivroRazaoServiceWrapper
{
    private readonly CadastroEntidadesLivroRazao.CadastroEntidadesLivroRazaoClient _client;
    private readonly string _tenantId;

    public CadastroEntidadesLivroRazaoServiceWrapper(CadastroEntidadesLivroRazao.CadastroEntidadesLivroRazaoClient client, string tenantId)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _tenantId = tenantId ?? throw new ArgumentNullException(nameof(tenantId));
    }

    public async Task<AdicionarEmpresaResponse> AdicionarNovaEmpresaAsync(AdicionarEmpresaRequest novaEmpresaRequest)
    {
        return await _client.AdicionarEmpresaAsync(novaEmpresaRequest, new Metadata {{ "tenantid", _tenantId }});
    }

    public async Task<AdicionarContaResponse> AdicionarContaAsync(CreateContaDto contaDto, Guid empresaId)
    {
        var request = new AdicionarContaRequest
        {
            EmpresaId = empresaId.ToString(),
            Conta = contaDto
        };
        return await _client.AdicionarContaAsync(request, new Metadata {{ "tenantid", _tenantId }});
    }

    public async Task<AdicionarRangeContasResponse> AdicionarRangeContas(List<LivroRazao.CreateContaDto> contasDto, Guid empresaId)
    {
        var request = new AdicionarRangeContasRequest
        {
            EmpresaId = empresaId.ToString()
        };
        request.Contas.AddRange(contasDto);
        return await _client.AdicionarRangeContasAsync(request, new Metadata {{ "tenantid", _tenantId }});
    }

    public async Task<ObterContasAninhadasResponse> ObterContasAninhadasAsync(Guid empresaId)
    {
        var obterContasRequest = new ObterContasAninhadasRequest { EmpresaId = empresaId.ToString() };
        return await _client.ObterContasAninhadasAsync(obterContasRequest, new Metadata {{ "tenantid", _tenantId }});
    }

    public async Task<List<ReadContaDto>> ObterContasPlanasAsync(Guid empresaId)
    {
        var request = new ObterContasPlanasRequest { EmpresaId = empresaId.ToString() };
        var response = await _client.ObterContasPlanasAsync(request, new Metadata {{ "tenantid", _tenantId }});
        return response.Contas.ToList();
    }

    public async Task<List<ReadGrupoContasDto>> CriarGruposContas(CriarGruposContasRequest criarGruposRequest)
    {
        var response = await _client.CriarGruposContasAsync(criarGruposRequest, new Metadata {{ "tenantid", _tenantId }});
        return response.Grupos.ToList();
    }

    public async Task<List<ReadGrupoContasDto>> ObterGruposContasAsync(Guid empresaId)
    {
        var obterGruposRequest = new ObterGruposContasRequest() { EmpresaId = empresaId.ToString() };
        var response = await _client.ObterGruposContasAsync(obterGruposRequest, new Metadata {{ "tenantid", _tenantId }});
        return response.Grupos.ToList();
    }
}