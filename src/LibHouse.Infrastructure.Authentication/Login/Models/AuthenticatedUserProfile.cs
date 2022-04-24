using LibHouse.Business.Entities.Users;
using System;

namespace LibHouse.Infrastructure.Authentication.Login.Models
{
    public class AuthenticatedUserProfile
    {
        public Guid Id { get; }
        public string FullName { get; }
        public DateTime BirthDate { get; }
        public string Gender { get; }
        public string Email { get; }
        public string UserType { get; }

        public AuthenticatedUserProfile(
            Guid id,
            string firstName,
            string lastName,
            DateTime birthDate,
            Gender gender,
            string email,
            UserType userType)
        {
            Id = id;
            FullName = string.Concat(firstName, " ", lastName);
            BirthDate = birthDate;
            Gender = gender.ToString();
            Email = email;
            UserType = userType.ToString();
        }
    }
}