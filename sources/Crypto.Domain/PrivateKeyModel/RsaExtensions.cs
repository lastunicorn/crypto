using System.Security.Cryptography;

namespace DustInTheWind.Crypto.Domain.PrivateKeyModel;

public static class RsaExtensions
{
    public static string GetUniqueName(this RSA rsa)
    {
        return rsa switch
        {
            RSACryptoServiceProvider rsaCryptoServiceProvider => rsaCryptoServiceProvider.CspKeyContainerInfo.UniqueKeyContainerName,
            RSACng rsaCng => rsaCng.Key.UniqueName,
            _ => throw new Exception("Unknown key type")
        };
    }
}