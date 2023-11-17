using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain;

internal static class X509Certificate2Extensions
{
    public static List<X509Certificate2> ToList(this X509Certificate2 certificate)
    {
        List<X509Certificate2> list = new();

        if (certificate != null)
            list.Add(certificate);

        return list;
    }
}