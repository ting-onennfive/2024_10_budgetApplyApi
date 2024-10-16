using budgetApplyApi.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace budgetApplyApi.Infrastructure.Models.Identity
{
    public class BlazorHeroUser : IdentityUser, IAuditableEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}