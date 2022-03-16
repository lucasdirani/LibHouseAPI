using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LibHouse.Data.Extensions.Context
{
    public static class DatabaseExtensions
    {
        public static bool EnsureDeletedAndCreated(this DatabaseFacade database)
        {
            return database.EnsureDeleted() && database.EnsureCreated();
        }
    }
}