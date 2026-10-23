using System;
using System.Security.Cryptography;
using System.Text;

namespace iRestaurant.Security
{
    /// <summary>
    /// Salted PBKDF2 password hashing.
    /// Stored format: PBKDF2$&lt;iterations&gt;$&lt;base64 salt&gt;$&lt;base64 hash&gt;
    /// Older accounts still hold an unsalted SHA-256 hex digest. Those are accepted once and
    /// should be re-hashed by the caller when <c>needsRehash</c> comes back true.
    /// </summary>
    public static class PasswordHasher
    {
        private const string Prefix = "PBKDF2";
        private const int Iterations = 100000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public const int MinimumLength = 8;
        public const int MaximumLength = 128;

        public static bool IsAcceptablePassword(string password)
        {
            return password != null && password.Length >= MinimumLength && password.Length <= MaximumLength;
        }

        public static string Hash(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            byte[] salt = SecureRandom.Bytes(SaltSize);
            byte[] hash = Derive(password, salt, Iterations);
            return string.Join("$", Prefix, Iterations.ToString(), Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public static bool Verify(string password, string storedHash, out bool needsRehash)
        {
            needsRehash = false;
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                return false;

            string[] parts = storedHash.Split('$');
            if (parts.Length == 4 && parts[0] == Prefix)
            {
                int iterations;
                if (!int.TryParse(parts[1], out iterations) || iterations <= 0)
                    return false;

                byte[] salt, expected;
                try
                {
                    salt = Convert.FromBase64String(parts[2]);
                    expected = Convert.FromBase64String(parts[3]);
                }
                catch (FormatException)
                {
                    return false;
                }

                byte[] actual = Derive(password, salt, iterations, expected.Length);
                bool ok = FixedTimeEquals(actual, expected);
                needsRehash = ok && iterations < Iterations;
                return ok;
            }

            // Legacy: unsalted SHA-256, upper-case hex (64 chars).
            if (storedHash.Length == 64)
            {
                string legacy = LegacySha256(password);
                bool ok = FixedTimeEquals(Encoding.ASCII.GetBytes(legacy), Encoding.ASCII.GetBytes(storedHash.ToUpperInvariant()));
                needsRehash = ok;
                return ok;
            }

            return false;
        }

        public static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }

        public static bool FixedTimeEquals(string a, string b)
        {
            if (a == null || b == null)
                return false;
            return FixedTimeEquals(Encoding.UTF8.GetBytes(a), Encoding.UTF8.GetBytes(b));
        }

        private static byte[] Derive(string password, byte[] salt, int iterations, int length = HashSize)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), salt, iterations))
                return pbkdf2.GetBytes(length);
        }

        private static string LegacySha256(string password)
        {
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                var sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                    sb.Append(b.ToString("X2"));
                return sb.ToString();
            }
        }
    }
}
