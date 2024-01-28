using Finbuckle.MultiTenant;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using EndpointHandlers;
using Grpc.Net.Client;
using LivroRazao;

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/pocket-track-7edef-firebase-adminsdk-cf3ez-48f7387944.json");
Environment.SetEnvironmentVariable("FIREBASE_PROJECT_ID", "pocket-track-7edef");

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5116);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = $"https://securetoken.google.com/{Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID")}";
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID")}",
            ValidateAudience = true,
            ValidAudience = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID"),
            ValidateLifetime = true,
            RoleClaimType = "roles",
            NameClaimType = "name",   
        }; 
});
builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<FirebaseApp>(FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.GetApplicationDefault(),
        ProjectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID")
    }));
builder.Services.AddStackExchangeRedisCache(options => 
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddMultiTenant<TenantInfo>()
    .WithDistributedCacheStore()
    .WithDelegateStrategy(async context =>
    {
        var httpContext = context as HttpContext;
        if (!httpContext.User.Identity.IsAuthenticated)
        {
            return null;
        }
        if (httpContext.Request.Headers.Any(h => h.Key == "Authorization"))
        {
            var token = httpContext.Request.Headers
                .Where(h => h.Key == "Authorization")
                .Select(h => h.Value)
                .FirstOrDefault()
                .ToString()
                .Split(" ", 2)[1];

            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                return decodedToken.Uid;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        return null;
    });

builder.Services.AddScoped<CadastroEntidadesLivroRazaoServiceWrapper>(sp => 
{
    var tenantInfo = sp.GetRequiredService<ITenantInfo>();
    var httpHandler = new HttpClientHandler();
    httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    var channel = GrpcChannel.ForAddress(builder.Configuration.GetSection("GrpcServices:LivroRazao").Value!, new GrpcChannelOptions { HttpHandler = httpHandler });
    var grpcClient = new CadastroEntidadesLivroRazao.CadastroEntidadesLivroRazaoClient(channel);
    return new CadastroEntidadesLivroRazaoServiceWrapper(grpcClient, tenantInfo.Id!);
});


builder.Services.AddDbContext<MultiTenantDbContext>((serviceProvider, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));
}, ServiceLifetime.Scoped);

// IMPORTANT: This should be properly configured for production scenarios.
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseMultiTenant();
app.UseAuthorization();
app.MapEndpoints(Assembly.GetExecutingAssembly());
app.Run();
