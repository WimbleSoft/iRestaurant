using System;
using System.Runtime.Caching;

namespace iRestaurant.Security
{
    /// <summary>
    /// Short-lived, single-use tokens that let the POS desktop app open the cashier / branch
    /// admin panels in its embedded browser without putting credentials in the URL.
    /// </summary>
    public static class OneTimeLoginTokens
    {
        private static readonly TimeSpan Lifetime = TimeSpan.FromMinutes(2);
        private static readonly MemoryCache Cache = new MemoryCache("iRestaurant.OneTimeLoginTokens");

        public static string Issue(int employeeId)
        {
            string token = SecureRandom.HexToken(32);
            Cache.Set(token, employeeId, DateTimeOffset.UtcNow.Add(Lifetime));
            return token;
        }

        /// <summary>Consumes the token. Returns the employee id, or 0 if the token is unknown or expired.</summary>
        public static int Redeem(string token)
        {
            if (string.IsNullOrEmpty(token))
                return 0;
            object value = Cache.Remove(token);
            return value is int ? (int)value : 0;
        }
    }
}
