using System.ComponentModel.DataAnnotations;

namespace LibHouse.API.V1.ViewModels.Users
{
    /// <summary>
    /// Objeto que possui os dados necessários para realizar o logout do usuário
    /// </summary>
    public class LogoutUserViewModel
    {
        /// <summary>
        /// O endereço de e-mail do usuário que está fazendo o logout
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(60, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 5)]
        [EmailAddress(ErrorMessage = "O campo {0} está em um formato inválido.")]
        public string Email { get; set; }

        /// <summary>
        /// O refresh token que será revogado
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string RefreshToken { get; set; }
    }
}