using System.Security.Cryptography;
using System.Text;

namespace DustInTheWind.Crypto.Domain.HashEncryption;

public class Sha256EncryptedText
{
    private readonly byte[] encryptedMessage;

    public Sha256EncryptedText(string text)
    {
        byte[] originalBytes = Encoding.UTF8.GetBytes(text);

        using SHA256 sha256 = SHA256.Create();
        encryptedMessage = sha256.ComputeHash(originalBytes);
    }

    public static implicit operator byte[](Sha256EncryptedText encryptedText)
    {
        return encryptedText.encryptedMessage;
    }
}