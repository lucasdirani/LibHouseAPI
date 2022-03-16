using LibHouse.Business.Entities.Residents;
using LibHouse.Business.Entities.Users;
using LibHouse.Data.Context;
using System;

namespace LibHouse.UnitTests.Setup.Suite.Business.Services.Users
{
    internal static class SetupUserRegistrationServiceTests
    {
        public static void SetupScenarioForValidUserResidentWithUnregisteredCpfAndEmail(
            LibHouseContext libHouseContext)
        {
            libHouseContext.Users.AddRange(
                new Resident("Victor", "Almeida", DateTime.Now.AddYears(-20), Gender.Male, "11982561092", "victor@gmail.com", Cpf.CreateFromDocument("69588646030")),
                new Resident("Leandro", "Kramer", DateTime.Now.AddYears(-30), Gender.Male, "11942469092", "leandro@gmail.com", Cpf.CreateFromDocument("70841447063"))
            );

            libHouseContext.SaveChanges();
        }

        public static void SetupScenarioForInvalidUserResidentWithCpfAlreadyRegistered(
            LibHouseContext libHouseContext)
        {
            libHouseContext.Users.AddRange(
                new Resident("Victor", "Almeida", DateTime.Now.AddYears(-20), Gender.Male, "11982561092", "victor@gmail.com", Cpf.CreateFromDocument("69588646030")),
                new Resident("Lucas", "Dirani", new DateTime(1998, 8, 12), Gender.Male, "11978192183", "lucas.dirani@gmail.com", Cpf.CreateFromDocument("95339604004"))
            );

            libHouseContext.SaveChanges();
        }

        public static void SetupScenarioForInvalidUserResidentWithEmailAlreadyRegistered(
            LibHouseContext libHouseContext)
        {
            libHouseContext.Users.AddRange(
                new Resident("Luan", "Santos", DateTime.Now.AddYears(-10), Gender.Male, "11983541191", "luan-santos@gmail.com", Cpf.CreateFromDocument("69588646030")),
                new Resident("Lucas", "Dirani", new DateTime(1998, 8, 12), Gender.Male, "11978192183", "lucas.dirani@gmail.com", Cpf.CreateFromDocument("95339604004"))
            );

            libHouseContext.SaveChanges();
        }
    }
}