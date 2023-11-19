using System.Security.Cryptography;

namespace DustInTheWind.Crypto.Application;

internal static class OidExtensions
{
    public static string ToNiceString(this Oid oid)
    {
        if (oid == null)
            return "<null>";

        string oidFriendlyName = oid.FriendlyName ?? "<null>";
        string oidValue = oid.Value ?? "<null>";

        return $"{oidFriendlyName} ({oidValue})";
    }
}