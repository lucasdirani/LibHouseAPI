using LibHouse.Business.Entities.Shared;
using LibHouse.Business.Entities.Users;
using LibHouse.Business.Monads;
using LibHouse.Business.Notifiers;
using LibHouse.Business.Services.Shared;
using LibHouse.Business.Validations.Users;
using System;
using System.Threading.Tasks;

namespace LibHouse.Business.Services.Users
{
    public class UserRegistrationService : BaseService, IUserRegistrationService
    {
        private readonly UserRegistrationValidator _userRegistrationValidator;

        private readonly IUnitOfWork _unitOfWork;

        public UserRegistrationService(
            INotifier notifier,
            IUnitOfWork unitOfWork,
            UserRegistrationValidator userRegistrationValidator) 
            : base(notifier)
        {
            _unitOfWork = unitOfWork;
            _userRegistrationValidator = userRegistrationValidator;
        }

        public async Task<Result> ConfirmUserRegistrationAsync(Guid userId)
        {
            User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                Notify("Confirmar cadastro", "O usuário não foi encontrado.");

                return Result.Fail("O usuário não foi encontrado.");
            }

            user.Activate();

            bool isUserActivated = await _unitOfWork.CommitAsync();

            if (!isUserActivated)
            {
                Notify("Confirmar cadastro", "Erro ao ativar o cadastro do usuário.");

                return Result.Fail("Erro ao ativar o cadastro do usuário.");
            }

            return Result.Success();
        }

        public async Task<Result> RegisterUserAsync(User user)
        {
            if (!ExecuteValidation(_userRegistrationValidator, user))
            {
                return Result.Fail("O usuário já está registrado.");
            }

            user.Inactivate();

            await _unitOfWork.UserRepository.AddAsync(user);

            bool isUserRegistered = await _unitOfWork.CommitAsync();

            if (!isUserRegistered)
            {
                Notify("Cadastrar Usuário", "Erro ao armazenar os dados do usuário");

                return Result.Fail("Erro ao armazenar os dados do usuário");
            }

            return Result.Success();
        }

        public async Task<Result> ConfirmationExistingAccountAsync(Cpf cpf)
        {
            bool existUser = await _unitOfWork.UserRepository.CheckIfExistingAccountAsync(cpf);

            if (!existUser)
            {
                Notify("Resetar Senha", "O usuário não foi encontrado.");

                return Result.Fail("O usuário não foi encontrado.");
            }

            return Result.Success();
        }
    }
}