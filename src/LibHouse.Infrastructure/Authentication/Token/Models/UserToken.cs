using System.Collections.Generic;

namespace LibHouse.Infrastructure.Authentication.Token.Models
{
    /// <summary>
    /// Representa os dados de autenticação de um usuário
    /// </summary>
    public class UserToken
    {
        /// <summary>
        /// Os dados essenciais de cadastro do usuário (nome completo, tipo de usuário, etc.)
        /// </summary>
        public AuthenticatedUser User { get; }

        /// <summary>
        /// O token de acesso atrelado ao usuário
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// O tempo de expiração em segundos do token de acesso
        /// </summary>
        public double ExpiresIn { get; }

        /// <summary>
        /// O token de renovação atrelado ao token de acesso do usuário
        /// </summary>
        public string RefreshToken { get; }

        /// <summary>
        /// A lista de claims pertencentes ao usuário
        /// </summary>
        public IEnumerable<UserClaim> Claims { get; }

        public UserToken(
            AuthenticatedUser user, 
            string accessToken, 
            double expiresIn, 
            string refreshToken,
            IEnumerable<UserClaim> claims)
        {
            User = user;
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
            Claims = claims;
        }
    }
}