using AutoMapper;
using LibHouse.API.V1.ViewModels.Users;
using LibHouse.Business.Entities.Owners;
using LibHouse.Business.Entities.Residents;
using LibHouse.Business.Entities.Users;
using LibHouse.Infrastructure.Authentication.Login.Models;
using System.Linq;

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

            CreateMap<AuthenticatedUser, UserTokenViewModel>().ConstructUsing((authenticatedUser, ctx) =>
            {
                return new UserTokenViewModel(
                    user: new UserAuthenticatedViewModel(
                        id: authenticatedUser.Profile.Id,
                        fullName: authenticatedUser.Profile.FullName,
                        birthDate: authenticatedUser.Profile.BirthDate,
                        gender: authenticatedUser.Profile.Gender,
                        email: authenticatedUser.Profile.Email,
                        userType: authenticatedUser.Profile.UserType
                    ),
                    accessToken: authenticatedUser.AccessToken.Value,
                    expiresInAccessToken: authenticatedUser.AccessToken.ExpiresIn,
                    refreshToken: authenticatedUser.AccessToken.RefreshToken.Token,
                    expiresInRefreshToken: authenticatedUser.AccessToken.RefreshToken.ExpiresIn,
                    claims: authenticatedUser.AccessToken.Claims.Select(c => new UserClaimViewModel(c.Value, c.Type))
                );
            });
        }
    }
}