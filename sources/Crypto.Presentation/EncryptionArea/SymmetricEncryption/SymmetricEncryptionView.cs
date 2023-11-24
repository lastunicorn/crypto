using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.EncryptionArea.SymmetricEncryption;

namespace DustInTheWind.Crypto.Presentation.EncryptionArea.SymmetricEncryption;

internal class SymmetricEncryptionView : ViewBase<SymmetricEncryptionViewModel>
{
    public override void Display(SymmetricEncryptionViewModel viewModel)
    {
        foreach (EncryptionResult encryptionResult in viewModel.HashResults)
        {
            string fullTitle = ComputeTitle(encryptionResult.EncryptionAlgorithm);
            WriteTitle(fullTitle);

            WriteValue("Original Message", encryptionResult.OriginalMessage);
            WriteValue("Key", encryptionResult.Key);
            WriteValue("IV", encryptionResult.IV);

            CustomConsole.WriteLine();

            WriteValue("Encrypted Message", encryptionResult.EncryptedMessage);
            WriteValue("Decrypted Message", encryptionResult.DecryptedMessage);
        }
    }

    private static string ComputeTitle(EncryptionAlgorithmEnum encryptionAlgorithm)
    {
        return encryptionAlgorithm switch
        {
            EncryptionAlgorithmEnum.Aes => "AES",
            EncryptionAlgorithmEnum.Des => "DES",
            EncryptionAlgorithmEnum.TripleDes => "Triple DES",
            EncryptionAlgorithmEnum.All => string.Empty,
            _ => string.Empty
        };
    }
}