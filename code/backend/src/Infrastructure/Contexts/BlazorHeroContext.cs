using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Domain.Contracts;
using budgetApplyApi.Infrastructure.Models.Identity;
using budgetApplyApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace budgetApplyApi.Infrastructure.Contexts
{
    public class BlazorHeroContext : AuditableContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlazorHeroContext(
            DbContextOptions<BlazorHeroContext> options,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _currentUserService.UserId ?? "";
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId ?? "";
                        break;

                    case EntityState.Modified:
                        entry.Entity.CreatedBy = _currentUserService.UserId ?? "";
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId ?? "";
                        break;
                }
            }
            if (_currentUserService.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(
                    userId: _currentUserService.UserId ?? "",
                    ip: _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    cancellationToken: cancellationToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);

            builder.Entity<BlazorHeroUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            builder.Entity<BlazorHeroRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable(name: "RoleClaims");
            });

            builder.Entity<Budget>(entity =>
            {
                entity.ToTable("Budgets");
            });

            builder.Entity<BudgetDetail>(entity =>
            {
                entity.ToTable("BudgetDetails");
                entity.HasOne(d => d.Budget)
                .WithMany(p => p.BudgetDetails)
                .HasForeignKey(d => d.BudgetsId)
                .HasPrincipalKey(p => p.Id);
            });
        }
    }
}