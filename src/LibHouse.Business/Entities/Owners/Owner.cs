using LibHouse.Business.Entities.Users;
using System;

namespace LibHouse.Business.Entities.Owners
{
    public class Owner : User
    {
        public Owner(
            string name, 
            string lastName, 
            DateTime birthDate,
            Gender gender, 
            string phone, 
            string email, 
            Cpf cpf) 
            : this(name, lastName, birthDate, gender, phone, email, UserType.Owner)
        {
            CPF = cpf;
        }

        private Owner(
            string name,
            string lastName,
            DateTime birthDate,
            Gender gender,
            string phone,
            string email,
            UserType userType)
            : base(name, lastName, birthDate, gender, phone, email, userType)
        {
        }

        public override string ToString() =>
            $"Owner {Id}: {Name} {LastName}";
    }
}