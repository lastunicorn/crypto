using System.Security.Cryptography;
using System.Text;

namespace DustInTheWind.Crypto.Domain.HashEncryption;

public class Sha384EncryptedText
{
    private readonly byte[] encryptedMessage;

    public Sha384EncryptedText(string text)
    {
        byte[] originalBytes = Encoding.UTF8.GetBytes(text);

        using SHA384 sha384 = SHA384.Create();
        encryptedMessage = sha384.ComputeHash(originalBytes);
    }

    public static implicit operator byte[](Sha384EncryptedText encryptedText)
    {
        return encryptedText.encryptedMessage;
    }
}