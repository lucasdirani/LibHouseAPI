using System.ComponentModel.DataAnnotations;

namespace LibHouse.API.V1.ViewModels.Users
{
    /// <summary>
    /// Objeto que possui os dados necessários para realizar a redefinição de senha de um usuário.
    /// </summary>
    public class ConfirmUserPasswordResetViewModel
    {
        /// <summary>
        /// O token de redefinição de senha recebido no e-mail do usuário.
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string PasswordResetToken { get; set; }

        /// <summary>
        /// O endereço de e-mail utilizado no registro do usuário.
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(60, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 5)]
        [EmailAddress(ErrorMessage = "O campo {0} está em um formato inválido.")]
        public string UserEmail { get; set; }

        /// <summary>
        /// A nova senha do usuário cadastrado. A senha deve ter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caracter especial. Mínimo de dez, e máximo de trinta caracteres.
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(30, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 10)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{10,30}$", ErrorMessage = "A senha deve ter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caracter especial.")]
        public string Password { get; set; }

        /// <summary>
        /// A confirmação da nova senha do usuário cadastrado.
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Compare("Password", ErrorMessage = "As senhas são diferentes.")]
        public string ConfirmPassword { get; set; }
    }
}