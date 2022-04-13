using Microsoft.AspNetCore.Identity;
using System;

namespace LibHouse.Infrastructure.Authentication.Token.Models
{
    public class RefreshToken
    {
        public Guid Id { get; }
        public string UserId { get; }
        public string Token { get; }
        public string JwtId { get; }
        public bool IsUsed { get; private set; }
        public bool IsRevoked { get; }
        public DateTime CreatedAt { get; }
        public DateTime ExpiresIn { get; }
        public IdentityUser User { get; }

        public RefreshToken(
            string token, 
            string jwtId, 
            DateTime createdAt, 
            DateTime expiresIn,
            IdentityUser user,
            bool isUsed = false,
            bool isRevoked = false)
            : this(user.Id, token, jwtId, isUsed, isRevoked, createdAt, expiresIn)
        {
            Id = Guid.NewGuid();
            User = user;
        }

        private RefreshToken(
            string userId, 
            string token, 
            string jwtId, 
            bool isUsed, 
            bool isRevoked,
            DateTime createdAt,
            DateTime expiresIn)
        {
            UserId = userId;
            Token = token;
            JwtId = jwtId;
            IsUsed = isUsed;
            IsRevoked = isRevoked;
            CreatedAt = createdAt;
            ExpiresIn = expiresIn;
        }

        public void MarkAsUsed()
        {
            IsUsed = true;
        }

        public override string ToString()
        {
            return $"Refresh token {Token} pertencente ao usuário: {UserId}";
        }
    }
}