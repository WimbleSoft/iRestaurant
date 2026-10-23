using System;

namespace iRestaurant.Security
{
    public static class SafeRedirect
    {
        /// <summary>
        /// Returns <paramref name="returnUrl"/> only if it is a local path inside the given
        /// controller (e.g. "/BranchAdmin/Menus"), otherwise the fallback. Prevents open redirects
        /// such as "//evil.example" or "/\evil.example".
        /// Plain action names ("Menus"), as sent by older links, are accepted as well.
        /// </summary>
        public static string Resolve(string returnUrl, string controller, string fallbackAction)
        {
            string fallback = "/" + controller + "/" + fallbackAction;
            if (string.IsNullOrWhiteSpace(returnUrl))
                return fallback;

            string candidate = returnUrl.Trim();
            if (IsSimpleName(candidate))
                return "/" + controller + "/" + candidate;

            bool local = candidate.StartsWith("/", StringComparison.Ordinal)
                         && !candidate.StartsWith("//", StringComparison.Ordinal)
                         && !candidate.StartsWith("/\\", StringComparison.Ordinal)
                         && candidate.IndexOf("://", StringComparison.Ordinal) < 0;
            bool sameController = candidate.StartsWith("/" + controller + "/", StringComparison.OrdinalIgnoreCase);
            return local && sameController ? candidate : fallback;
        }

        private static bool IsSimpleName(string value)
        {
            if (value.Length == 0 || value.Length > 64)
                return false;
            foreach (char c in value)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }
    }
}
