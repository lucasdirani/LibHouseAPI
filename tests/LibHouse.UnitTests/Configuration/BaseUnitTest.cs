using LibHouse.Data.Context;
using LibHouse.Data.Extensions.Context;
using LibHouse.Infrastructure.Authentication.Context;
using Microsoft.EntityFrameworkCore;

namespace LibHouse.UnitTests.Configuration
{
    public abstract class BaseUnitTest
    {
        private DbContextOptions<LibHouseContext> _libHouseContextOptions;
        private LibHouseContext _libHouseContext;
        private DbContextOptions<AuthenticationContext> _authenticationContextOptions;
        private AuthenticationContext _authenticationContext;

        private DbContextOptions<LibHouseContext> LibHouseContextOptions
        {
            get => _libHouseContextOptions ??= new DbContextOptionsBuilder<LibHouseContext>().UseInMemoryDatabase("InMemoryLibHouse").Options;
        }

        protected LibHouseContext LibHouseContext
        {
            get => _libHouseContext ??= new LibHouseContext(LibHouseContextOptions);
        }

        private DbContextOptions<AuthenticationContext> AuthenticationContextOptions
        {
            get => _authenticationContextOptions ??= new DbContextOptionsBuilder<AuthenticationContext>().UseInMemoryDatabase("InMemoryAuthentication").Options;
        }

        protected AuthenticationContext AuthenticationContext
        {
            get => _authenticationContext ??= new AuthenticationContext(AuthenticationContextOptions);
        }

        protected void RestartLibHouseContext()
        {
            LibHouseContext.Database.EnsureDeletedAndCreated();
        }

        protected void RestartAuthenticationContext()
        {
            AuthenticationContext.Database.EnsureDeletedAndCreated();
        }
    }
}