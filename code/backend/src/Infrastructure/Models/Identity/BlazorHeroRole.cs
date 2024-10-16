using Microsoft.AspNetCore.Identity;
using budgetApplyApi.Domain.Contracts;

namespace budgetApplyApi.Infrastructure.Models.Identity
{
    public class BlazorHeroRole : IdentityRole, IAuditableEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}