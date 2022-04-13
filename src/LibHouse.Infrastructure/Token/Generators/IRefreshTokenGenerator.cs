using LibHouse.Infrastructure.Authentication.Token.Models;

namespace LibHouse.Infrastructure.Authentication.Token.Generators
{
    public interface IRefreshTokenGenerator
    {
        RefreshToken GenerateRefreshToken(string accessTokenId);
    }
}