using LibHouse.Business.Entities.Shared;
using System;

namespace LibHouse.Business.Entities.Users
{
    public abstract class User : Entity
    {
        public string Name { get; protected set; }
        public string LastName { get; protected set; }
        public DateTime BirthDate { get; protected set; }
        public Gender Gender { get; protected set; }
        public string Phone { get; protected set; }
        public string Email { get; protected set; }
        public Cpf CPF { get; protected set; }
        public UserType UserType { get; protected set; }

        protected User(
            string name, 
            string lastName, 
            DateTime birthDate, 
            Gender gender, 
            string phone, 
            string email, 
            Cpf cpf, 
            UserType userType)
            : this(name, lastName, birthDate, gender, phone, email, userType)
        {
            CPF = cpf;
        }

        protected User(
            string name,
            string lastName,
            DateTime birthDate,
            Gender gender,
            string phone,
            string email,
            UserType userType)
        {
            Name = name;
            LastName = lastName;
            BirthDate = birthDate;
            Gender = gender;
            Phone = phone;
            Email = email;
            UserType = userType;
        }

        public override string ToString() =>
            $"User {Id}: {Name} {LastName}";
    }
}