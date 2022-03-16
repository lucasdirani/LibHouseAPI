using AutoMapper;
using LibHouse.API.V1.ViewModels.Users;
using LibHouse.Business.Entities.Owners;
using LibHouse.Business.Entities.Residents;
using LibHouse.Business.Entities.Users;

namespace LibHouse.API.V1.Profiles.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserViewModel, User>().ConstructUsing((user, ctx) => 
            {
                return user.UserType is UserType.Resident 
                    ? new Resident(user.Name, user.LastName, user.BirthDate, user.Gender, user.Phone, user.Email, Cpf.CreateFromDocument(user.Cpf)) 
                    : new Owner(user.Name, user.LastName, user.BirthDate, user.Gender, user.Phone, user.Email, Cpf.CreateFromDocument(user.Cpf));
            });
        }
    }
}