

using EndpointHandlers;
using Finbuckle.MultiTenant;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Auth.Multitenancy;

public class TenantHandler : IEndpointRouteHandler
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/tenants", CreateTenant);
    }

    private static async Task<IResult> CreateTenant(IMultiTenantStore<TenantInfo> tenantStore, FirebaseApp firebaseApp, 
        TenantCreateDto tenantCreateDto, IConfiguration configuration)
    {
        var defaultAuth = FirebaseAuth.GetAuth(firebaseApp);

        var gcipUser = new UserRecordArgs()
        {
            Email = tenantCreateDto.EmailAdquirente,
            DisplayName = tenantCreateDto.NomeAdquirente,
            EmailVerified = false,
            Password = tenantCreateDto.SenhaAdquirente,
            PhoneNumber = tenantCreateDto.NumeroCelular,
            Disabled = false
        };

        var userRecord = await defaultAuth.CreateUserAsync(new UserRecordArgs()
        {
            Email = tenantCreateDto.EmailAdquirente,
            DisplayName = tenantCreateDto.NomeAdquirente,
            EmailVerified = false,
            Password = tenantCreateDto.SenhaAdquirente,
            PhoneNumber = tenantCreateDto.NumeroCelular,
            Disabled = false
        });

        // Adiciona o locatário no tenant store.
        var novoTenant = new TenantInfo()
        {
            Id = userRecord.Uid,
            Identifier = userRecord.Uid,
            Name = userRecord.DisplayName
        };
        await tenantStore.TryAddAsync(novoTenant);

        return Results.Ok(new { TenantId = userRecord.Uid });
    }
}