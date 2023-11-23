namespace DustInTheWind.Crypto.Application.EncryptionArea.Hash;

public class EncryptionResult
{
    public HashAlgorithmEnum HashAlgorithm { get; set; }

    public string OriginalMessage { get; set; }
    
    public byte[] EncryptedMessage { get; set; }
}