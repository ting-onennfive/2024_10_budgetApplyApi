using AutoMapper;
using budgetApplyApi.Application.Responses.Identity;
using budgetApplyApi.Infrastructure.Models.Identity;

namespace budgetApplyApi.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, BlazorHeroUser>().ReverseMap();
        }
    }
}