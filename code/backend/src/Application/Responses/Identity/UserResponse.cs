
namespace budgetApplyApi.Application.Responses.Identity
{
    public class UserResponse
    {
        public string Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}