using LibHouse.Business.Entities.Users;
using System;

namespace LibHouse.Business.Entities.Residents
{
    public class Resident : User
    {
        public Resident(
            string name,
            string lastName,
            DateTime birthDate,
            Gender gender,
            string phone,
            string email,
            Cpf cpf)
            : this(name, lastName, birthDate, gender, phone, email, UserType.Resident)
        {
            CPF = cpf;
        }

        private Resident(
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
            $"Resident {Id}: {Name} {LastName}";
    }
}