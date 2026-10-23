using System.Security.Cryptography;
using System.Text;

namespace iRestaurant.Security
{
    /// <summary>Cryptographically secure random values for tokens, salts and codes.</summary>
    public static class SecureRandom
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        public static byte[] Bytes(int count)
        {
            var buffer = new byte[count];
            lock (Rng)
                Rng.GetBytes(buffer);
            return buffer;
        }

        /// <summary>Returns a lower-case hex token with <paramref name="byteCount"/> bytes of entropy.</summary>
        public static string HexToken(int byteCount = 16)
        {
            byte[] bytes = Bytes(byteCount);
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
