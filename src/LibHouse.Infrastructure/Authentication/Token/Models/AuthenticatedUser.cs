using System;

namespace LibHouse.Infrastructure.Authentication.Token.Models
{
    public class AuthenticatedUser
    {
        public Guid Id { get; }
        public string FullName { get; }
        public DateTime BirthDate { get; }
        public string Gender { get; }
        public string Email { get; }
        public string UserType { get; }

        public AuthenticatedUser(
            Guid id,
            string fullName, 
            DateTime birthDate,
            string gender,
            string email, 
            string userType)
        {
            Id = id;
            FullName = fullName;
            BirthDate = birthDate;
            Gender = gender;
            Email = email;
            UserType = userType;
        }

        public override string ToString()
        {
            return $"Authenticated user {FullName}: {Id}";
        }
    }
}