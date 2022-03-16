using LibHouse.Data.Context;
using LibHouse.Data.Extensions.Context;
using Microsoft.EntityFrameworkCore;

namespace LibHouse.UnitTests.Configuration
{
    public abstract class BaseUnitTest
    {
        private DbContextOptions<LibHouseContext> _contextOptions;
        private LibHouseContext _libHouseContext;

        private DbContextOptions<LibHouseContext> ContextOptions
        {
            get => _contextOptions ??= new DbContextOptionsBuilder<LibHouseContext>().UseInMemoryDatabase("InMemoryLibHouse").Options;
        }

        protected LibHouseContext LibHouseContext
        {
            get => _libHouseContext ??= new LibHouseContext(ContextOptions);
        }

        protected void RestartLibHouseContext()
        {
            LibHouseContext.Database.EnsureDeletedAndCreated();
        }
    }
}