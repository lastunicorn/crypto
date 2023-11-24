using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.EncryptionArea.Hash;

namespace DustInTheWind.Crypto.Presentation.EncryptionArea.Hash;

internal class HashView : ViewBase<HashViewModel>
{
    public override void Display(HashViewModel viewModel)
    {
        foreach (EncryptionResult encryptionResult in viewModel.HashResults)
        {
            string fullTitle = ComputeTitle(encryptionResult.HashAlgorithm);
            WriteTitle(fullTitle);

            WriteValue("Original Message", encryptionResult.OriginalMessage);
            WriteValue("Hash", encryptionResult.EncryptedMessage);
        }
    }

    private static string ComputeTitle(HashAlgorithmEnum hashAlgorithm)
    {
        return hashAlgorithm switch
        {
            HashAlgorithmEnum.Md5 => "MD5 (Encrypt)",
            HashAlgorithmEnum.Sha1 => "SHA1 (Encrypt)",
            HashAlgorithmEnum.Sha256 => "SHA256 (Encrypt)",
            HashAlgorithmEnum.Sha384 => "SHA384 (Encrypt)",
            HashAlgorithmEnum.Sha512 => "SHA512 (Encrypt)",
            HashAlgorithmEnum.All => string.Empty,
            _ => string.Empty
        };
    }
}