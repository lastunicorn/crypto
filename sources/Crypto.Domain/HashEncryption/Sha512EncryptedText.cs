using System.Security.Cryptography;
using System.Text;

namespace DustInTheWind.Crypto.Domain.HashEncryption;

public class Sha512EncryptedText
{
    private readonly byte[] encryptedMessage;

    public Sha512EncryptedText(string text)
    {
        byte[] originalBytes = Encoding.UTF8.GetBytes(text);

        using SHA512 sha512 = SHA512.Create();
        encryptedMessage = sha512.ComputeHash(originalBytes);
    }

    public static implicit operator byte[](Sha512EncryptedText encryptedText)
    {
        return encryptedText.encryptedMessage;
    }
}