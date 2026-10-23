using System.Net;
using System.Net.Sockets;

namespace iRestaurant.Security
{
    /// <summary>
    /// Thermal printers are reached over raw TCP (port 9100). The address comes from data that
    /// tenants can edit, so it is restricted to literal private-network IPv4 addresses; otherwise
    /// the server could be used to send arbitrary bytes to internal or external hosts (SSRF).
    /// </summary>
    public static class PrinterAddress
    {
        public static bool IsAllowed(string address)
        {
            IPAddress ip;
            if (string.IsNullOrWhiteSpace(address) || !IPAddress.TryParse(address.Trim(), out ip) || ip.AddressFamily != AddressFamily.InterNetwork)
                return false;

            byte[] b = ip.GetAddressBytes();
            return b[0] == 10
                || (b[0] == 172 && b[1] >= 16 && b[1] <= 31)
                || (b[0] == 192 && b[1] == 168);
        }

        /// <summary>Accepts an empty value (no printer configured) or an allowed address.</summary>
        public static bool IsEmptyOrAllowed(string address) => string.IsNullOrWhiteSpace(address) || IsAllowed(address);
    }
}
