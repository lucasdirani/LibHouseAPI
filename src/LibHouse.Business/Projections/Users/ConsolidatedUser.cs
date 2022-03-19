using LibHouse.Business.Entities.Users;
using System;

namespace LibHouse.Business.Projections.Users
{
    public class ConsolidatedUser
    {
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime BirthDate { get; }
        public Gender Gender { get; }
        public string Email { get; }
        public UserType UserType { get; }

        public string FullName => $"{FirstName} {LastName}";

        public ConsolidatedUser(
            Guid id, 
            string firstName,
            string lastName,
            DateTime birthDate, 
            Gender gender, 
            string email, 
            UserType userType)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Gender = gender;
            Email = email;
            UserType = userType;
        }
    }
}