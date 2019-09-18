using DaceloRex.WebApplication.DataAccess;
using DaceloRex.WebApplication.DataAccess.Mappings;
using DaceloRex.WebApplication.Domain;
using DaceloRex.WebApplication.SeedWork;
using DaceloRex.WebApplication.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DaceloRex.DataAccess
{
    public class DaceloRexContext : DbContext
    {
        private readonly IUserContextService userContextService;
        private readonly ITenantContextService tenantContextService;

        public DbSet<Student> Students { get; set; }

        public DbSet<Grade> Grades { get; set; }

        public DaceloRexContext(DbContextOptions options, IUserContextService userContextService, ITenantContextService tenantContextService) : base(options)
        {
            this.userContextService = userContextService;
            this.tenantContextService = tenantContextService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyShadowProperties()
                .ApplyConfiguration(new StudentMapping())
                .ApplyConfiguration(new GradeMapping());

            ApplyQueryFilter(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.DetectChanges();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditableEntity)
                {
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property("modifiedDate").CurrentValue = DateTime.UtcNow;
                        entry.Property("modifiedBy").CurrentValue = userContextService.GetUserId();
                    }

                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("createdDate").CurrentValue = DateTime.UtcNow;
                        entry.Property("createdBy").CurrentValue = userContextService.GetUserId();
                    }
                }

                if (entry.Entity is ITenancyEntity && entry.State == EntityState.Added)
                {
                    entry.Property("TenantId").CurrentValue = tenantContextService.GetTenantId();
                }
                
                if (entry.Entity is ISoftDeletableEntity)
                {
                    if (entry.State == EntityState.Deleted)
                    {
                        entry.State = EntityState.Modified;
                        entry.Property("isDeleted").CurrentValue = true;
                    }

                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("isDeleted").CurrentValue = false;
                    }
                }

            }
            return base.SaveChangesAsync(cancellationToken);

        }

        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes().Select(x => x.ClrType))
            {
                var method = ApplyQueryFilterMethodInfo.MakeGenericMethod(type);
                method.Invoke(this, new object[] { modelBuilder });
            }
        }

        private static readonly MethodInfo ApplyQueryFilterMethodInfo = typeof(DaceloRexContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Single(t => t.IsGenericMethod && t.Name == nameof(DaceloRexContext.ApplyQueryFilter));

        public void ApplyQueryFilter<T>(ModelBuilder builder) where T : class
        {
            // ciekawa sprawa ApplyQueryFilter jest zaaplikowany raz podczas budowania modelu
            // jednak z racji ze wstrzyknęliśmy tenantContextService, framework nie ma problemu
            // z uzyskiwaniem TenantId z niego, kazdorazowo
            builder.Entity<T>().HasQueryFilter(item =>
                    !EF.Property<bool>(item, "isDeleted") &&
                    EF.Property<Guid>(item, "TenantId") == tenantContextService.GetTenantId());
        }
    }

}
