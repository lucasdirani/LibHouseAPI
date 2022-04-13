using System;
using System.Collections.Generic;

namespace LibHouse.API.V1.ViewModels.Users
{
    /// <summary>
    /// Representa os dados de autenticação de um usuário
    /// </summary>
    public class UserTokenViewModel
    {
        /// <summary>
        /// Os dados essenciais de cadastro do usuário (nome completo, tipo de usuário, etc.)
        /// </summary>
        public UserAuthenticatedViewModel User { get; }

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
        public IEnumerable<UserClaimViewModel> Claims { get; }

        public UserTokenViewModel(
            UserAuthenticatedViewModel user,
            string accessToken,
            DateTime expiresInAccessToken,
            string refreshToken,
            DateTime expiresInRefreshToken,
            IEnumerable<UserClaimViewModel> claims)
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