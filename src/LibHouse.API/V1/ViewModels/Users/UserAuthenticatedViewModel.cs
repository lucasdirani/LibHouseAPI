using System;

namespace LibHouse.API.V1.ViewModels.Users
{
    /// <summary>
    /// Representa os dados de um usuário autenticado
    /// </summary>
    public class UserAuthenticatedViewModel
    {
        /// <summary>
        /// O identificador único do usuário
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// O nome completo do usuário
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// A data de nascimento do usuário
        /// </summary>
        public DateTime BirthDate { get; }

        /// <summary>
        /// O gênero do usuário
        /// </summary>
        public string Gender { get; }

        /// <summary>
        /// O email do usuário
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// O tipo de cadastro do usuário
        /// </summary>
        public string UserType { get; }

        public UserAuthenticatedViewModel(
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
    }
}