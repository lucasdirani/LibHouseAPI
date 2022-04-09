using System;
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
        /// A data e hora de expiração do token de acesso
        /// </summary>
        public DateTime ExpiresInAccessToken { get; }

        /// <summary>
        /// O token de renovação atrelado ao token de acesso do usuário
        /// </summary>
        public string RefreshToken { get; }

        /// <summary>
        /// A data e hora de expiração do refresh token
        /// </summary>
        public DateTime ExpiresInRefreshToken { get; }

        /// <summary>
        /// A lista de claims pertencentes ao usuário
        /// </summary>
        public IEnumerable<UserClaim> Claims { get; }

        public UserToken(
            AuthenticatedUser user, 
            string accessToken, 
            DateTime expiresInAccessToken, 
            string refreshToken,
            DateTime expiresInRefreshToken,
            IEnumerable<UserClaim> claims)
        {
            User = user;
            AccessToken = accessToken;
            ExpiresInAccessToken = expiresInAccessToken;
            RefreshToken = refreshToken;
            ExpiresInRefreshToken = expiresInRefreshToken;
            Claims = claims;
        }
    }
}