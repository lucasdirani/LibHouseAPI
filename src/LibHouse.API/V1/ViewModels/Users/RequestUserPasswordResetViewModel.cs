using LibHouse.API.Attributes.Documents;
using System.ComponentModel.DataAnnotations;

namespace LibHouse.API.V1.ViewModels.Users
{
    /// <summary>
    /// Representa os dados necessários para que um usuário solicite a redefinição da sua senha
    /// </summary>
    public class RequestUserPasswordResetViewModel
    {
        /// <summary>
        /// O número de cpf do usuário cadastrado.
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(11, ErrorMessage = "O campo {0} deve ter {1} caracteres.", MinimumLength = 11)]
        [Cpf(ErrorMessage = "O campo {0} está em um formato inválido.")]
        public string Cpf { get; set; }
    }
}