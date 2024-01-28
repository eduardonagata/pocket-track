

using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class MultiTenantDbContext : DbContext, IMultiTenantDbContext
{

    public MultiTenantDbContext(TenantInfo tenantInfo, DbContextOptions<MultiTenantDbContext> options) 
        : base(options)
    {
        this.TenantInfo = tenantInfo;
        this.TenantMismatchMode = TenantMismatchMode.Overwrite;
        this.TenantNotSetMode = TenantNotSetMode.Overwrite;
    }

    public ITenantInfo TenantInfo { get; }
    public TenantMismatchMode TenantMismatchMode { get; }
    public TenantNotSetMode TenantNotSetMode { get; }
    public DbSet<ContaBancaria> ContasBancarias => Set<ContaBancaria>();
    public DbSet<CategoriaFinanceira> CategoriasFinanceiras => Set<CategoriaFinanceira>();


    private void ConfigureContasBancarias(ModelBuilder builder)
    {
        builder.Entity<ContaBancaria>()
            .HasKey(c => new { c.TenantId, c.Id });
        
        builder.Entity<ContaBancaria>()
            .Property(c => c.TenantId)
            .ValueGeneratedOnAdd();
        
        builder.Entity<ContaBancaria>()
            .IsMultiTenant();
    }

    private void ConfigureCategoriaFinanceira(ModelBuilder builder)
    {
        builder.Entity<CategoriaFinanceira>()
            .HasKey(c => new { c.TenantId, c.Id });
        
        builder.Entity<CategoriaFinanceira>()
            .Property(c => c.TenantId)
            .ValueGeneratedOnAdd();

        builder.Entity<CategoriaFinanceira>()
            .HasMany(c => c.Subcategorias)
            .WithOne()
            .HasForeignKey(c => new { c.TenantId, c.CategoriaSuperiorId });
        
        builder.Entity<CategoriaFinanceira>()
            .IsMultiTenant();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ConfigureCategoriaFinanceira(modelBuilder);
        ConfigureContasBancarias(modelBuilder);
    }

    public override int SaveChanges()
    {
        this.EnforceMultiTenant();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        this.EnforceMultiTenant();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.EnforceMultiTenant();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    } 
}