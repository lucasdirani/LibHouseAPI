using System;
using System.Linq;

namespace LibHouse.Infrastructure.Authentication.Helpers.String
{
    internal static class RandomStringGenerator
    {
        const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        internal static string GenerateRandomString(int length)
        {
            return new string(Enumerable.Repeat(Characters, length).Select(x => x[new Random().Next(x.Length)]).ToArray());
        }
    }
}