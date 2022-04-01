using System.ComponentModel.DataAnnotations;

namespace LibHouse.API.V1.ViewModels.Users
{
    /// <summary>
    /// Objeto que possui os dados necessários para obter um novo token de acesso
    /// </summary>
    public class UserRefreshTokenViewModel
    {
        /// <summary>
        /// O access token expirado que será renovado
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string AccessToken { get; set; }

        /// <summary>
        /// O refresh token que será utilizado para renovar o access token
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string RefreshToken { get; set; }
    }
}