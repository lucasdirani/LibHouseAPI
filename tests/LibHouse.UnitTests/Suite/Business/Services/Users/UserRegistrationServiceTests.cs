using LibHouse.Business.Entities.Residents;
using LibHouse.Business.Entities.Shared;
using LibHouse.Business.Entities.Users;
using LibHouse.Business.Monads;
using LibHouse.Business.Notifiers;
using LibHouse.Business.Services.Users;
using LibHouse.Business.Validations.Users;
using LibHouse.Data.Repositories.Users;
using LibHouse.Data.Transactions;
using LibHouse.UnitTests.Configuration;
using LibHouse.UnitTests.Setup.Suite.Business.Services.Users;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LibHouse.UnitTests.Suite.Business.Services.Users
{
    public class UserRegistrationServiceTests : BaseUnitTest, IDisposable
    {
        private readonly Notifier _notifier;
        private readonly UserRegistrationValidator _userRegistrationValidator;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserRegistrationServiceTests()
        {
            _notifier = new Notifier();
            _userRepository = new UserRepository(LibHouseContext);
            _userRegistrationValidator = new UserRegistrationValidator(_userRepository);
            _unitOfWork = new UnitOfWork(LibHouseContext);
        }

        [Fact]
        public async Task RegisterUserAsync_ValidUserResidentWithUnregisteredCpfAndEmail_ReturnsSuccess()
        {
            IUserRegistrationService userRegistrationService = new UserRegistrationService(_notifier, _unitOfWork, _userRegistrationValidator);

            SetupUserRegistrationServiceTests.SetupScenarioForValidUserResidentWithUnregisteredCpfAndEmail(LibHouseContext);

            User user = new Resident("Lucas", "Dirani", new DateTime(1998, 8, 12), Gender.Male, "11978192183", "lucas.dirani@gmail.com", Cpf.CreateFromDocument("95339604004"));

            Result userRegistrationResult = await userRegistrationService.RegisterUserAsync(user);

            Assert.True(userRegistrationResult.IsSuccess);
        }

        [Fact]
        public async Task RegisterUserAsync_InvalidUserResidentWithCpfAlreadyRegistered_ReturnsFailure()
        {
            IUserRegistrationService userRegistrationService = new UserRegistrationService(_notifier, _unitOfWork, _userRegistrationValidator);

            SetupUserRegistrationServiceTests.SetupScenarioForInvalidUserResidentWithCpfAlreadyRegistered(LibHouseContext);

            User user = new Resident("Lucas", "Dirani", new DateTime(1998, 8, 12), Gender.Male, "11978192183", "lucas@gmail.com", Cpf.CreateFromDocument("95339604004"));

            Result userRegistrationResult = await userRegistrationService.RegisterUserAsync(user);

            Assert.True(userRegistrationResult.Failure);
        }

        [Fact]
        public async Task RegisterUserAsync_InvalidUserResidentWithEmailAlreadyRegistered_ReturnsFailure()
        {
            IUserRegistrationService userRegistrationService = new UserRegistrationService(_notifier, _unitOfWork, _userRegistrationValidator);

            SetupUserRegistrationServiceTests.SetupScenarioForInvalidUserResidentWithEmailAlreadyRegistered(LibHouseContext);

            User user = new Resident("Lucas", "Dirani", new DateTime(1998, 8, 12), Gender.Male, "11978192183", "lucas.dirani@gmail.com", Cpf.CreateFromDocument("95339604004"));

            Result userRegistrationResult = await userRegistrationService.RegisterUserAsync(user);

            Assert.True(userRegistrationResult.Failure);
        }

        public void Dispose()
        {
            RestartLibHouseContext();
        }
    }
}