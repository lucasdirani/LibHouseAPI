using System;
using System.ComponentModel.DataAnnotations;

namespace LibHouse.API.V1.ViewModels.Users
{
    /// <summary>
    /// Objeto que possui os dados necessários para realizar a confirmação do registro de um usuário.
    /// </summary>
    public class ConfirmUserRegistrationViewModel
    {
        /// <summary>
        /// O token de confirmação recebido no e-mail do usuário.
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string ConfirmationToken { get; set; }

        /// <summary>
        /// O endereço de e-mail utilizado no registro do usuário.
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(60, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 5)]
        [EmailAddress(ErrorMessage = "O campo {0} está em um formato inválido.")]
        public string UserEmail { get; set; }

        /// <summary>
        /// O identificador único do usuário retornado junto com o token de confirmação.
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid UserId { get; set; }
    }
}