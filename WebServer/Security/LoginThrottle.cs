using System;
using System.Runtime.Caching;

namespace iRestaurant.Security
{
    /// <summary>
    /// Slows down password guessing: after too many failed attempts for the same account
    /// (or from the same client address) further attempts are rejected for a while.
    /// In-memory only, which is enough for a single web server.
    /// </summary>
    public static class LoginThrottle
    {
        private const int MaxFailuresPerAccount = 10;
        private const int MaxFailuresPerClient = 100; // generous: many users can share one NAT address
        private static readonly TimeSpan Window = TimeSpan.FromMinutes(15);
        private static readonly MemoryCache Cache = new MemoryCache("iRestaurant.LoginThrottle");

        /// <summary>Pass a null <paramref name="clientAddress"/> to only track the account key.</summary>
        public static bool IsBlocked(string account, string clientAddress)
        {
            return Count(AccountKey(account)) >= MaxFailuresPerAccount
                || (clientAddress != null && Count(ClientKey(clientAddress)) >= MaxFailuresPerClient);
        }

        public static void RegisterFailure(string account, string clientAddress)
        {
            Increment(AccountKey(account));
            if (clientAddress != null)
                Increment(ClientKey(clientAddress));
        }

        public static void RegisterSuccess(string account)
        {
            Cache.Remove(AccountKey(account));
        }

        private static string AccountKey(string account) => "a:" + (account ?? string.Empty).Trim().ToLowerInvariant();
        private static string ClientKey(string clientAddress) => "c:" + (clientAddress ?? string.Empty);

        private static int Count(string key)
        {
            var counter = Cache.Get(key) as Counter;
            return counter == null ? 0 : counter.Value;
        }

        private static void Increment(string key)
        {
            var counter = new Counter();
            var existing = Cache.AddOrGetExisting(key, counter, DateTimeOffset.UtcNow.Add(Window)) as Counter;
            System.Threading.Interlocked.Increment(ref (existing ?? counter).Value);
        }

        private sealed class Counter
        {
            public int Value;
        }
    }
}
