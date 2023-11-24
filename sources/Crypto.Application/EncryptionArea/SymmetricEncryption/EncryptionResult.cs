namespace DustInTheWind.Crypto.Application.EncryptionArea.SymmetricEncryption;

public class EncryptionResult
{
    public EncryptionAlgorithmEnum EncryptionAlgorithm { get; set; }

    public string OriginalMessage { get; set; }

    public byte[] Key { get; set; }
    
    public byte[] IV;

    public byte[] EncryptedMessage { get; set; }
    
    public string DecryptedMessage { get; set; }
}