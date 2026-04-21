using System.Security.Cryptography;
using DustInTheWind.Crypto.Domain.SymetricEncryption;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.EncryptionArea.SymmetricEncryption;

internal class SymmetricEncryptionUseCase : IRequestHandler<SymmetricEncryptionRequest, SymmetricEncryptionResponse>
{
    private const string DefaultTextToEncrypt = "Here is some data to encrypt!";

    private readonly ILog log;
    private SymmetricEncryptionResponse response;

    public SymmetricEncryptionUseCase(ILog log)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public Task<SymmetricEncryptionResponse> Handle(SymmetricEncryptionRequest request, CancellationToken cancellationToken)
    {
        response = new SymmetricEncryptionResponse();

        string textToEncrypt = request.Text ?? DefaultTextToEncrypt;

        EncryptDecryptWithAes(textToEncrypt);
        EncryptDecryptWithDes(textToEncrypt);
        EncryptDecryptWithTripleDes(textToEncrypt);

        return Task.FromResult(response);
    }

    private void EncryptDecryptWithAes(string textToEncrypt)
    {
        EncryptionResult result = new()
        {
            EncryptionAlgorithm = EncryptionAlgorithmEnum.Aes,
            OriginalMessage = textToEncrypt
        };

        using Aes myAes = Aes.Create();

        byte[] key = myAes.Key;
        byte[] iv = myAes.IV;

        result.Key = key;
        result.IV = iv;

        AesEncryptedText encryptedText1 = new(textToEncrypt, key, iv);
        result.EncryptedMessage = encryptedText1.EncryptedValue;

        AesEncryptedText encryptedText2 = new(result.EncryptedMessage, key, iv);
        result.DecryptedMessage = encryptedText2.DecryptedValue;

        response.EncryptionResults.Add(result);
    }

    private void EncryptDecryptWithDes(string textToEncrypt)
    {
        EncryptionResult result = new()
        {
            EncryptionAlgorithm = EncryptionAlgorithmEnum.Des,
            OriginalMessage = textToEncrypt
        };

        using DES myDes = DES.Create();

        byte[] key = myDes.Key;
        byte[] iv = myDes.IV;

        result.Key = key;
        result.IV = iv;

        DesEncryptedText encryptedText1 = new(textToEncrypt, key, iv);
        result.EncryptedMessage = encryptedText1.EncryptedValue;

        DesEncryptedText encryptedText2 = new(result.EncryptedMessage, key, iv);
        result.DecryptedMessage = encryptedText2.DecryptedValue;

        response.EncryptionResults.Add(result);
    }

    private void EncryptDecryptWithTripleDes(string textToEncrypt)
    {
        EncryptionResult result = new()
        {
            EncryptionAlgorithm = EncryptionAlgorithmEnum.TripleDes,
            OriginalMessage = textToEncrypt
        };

        using TripleDES myTripleDes = TripleDES.Create();

        byte[] key = myTripleDes.Key;
        byte[] iv = myTripleDes.IV;

        result.Key = key;
        result.IV = iv;

        TripleDesEncryptedText encryptedText1 = new(textToEncrypt, key, iv);
        result.EncryptedMessage = encryptedText1.EncryptedValue;

        TripleDesEncryptedText encryptedText2 = new(result.EncryptedMessage, key, iv);
        result.DecryptedMessage = encryptedText2.DecryptedValue;

        response.EncryptionResults.Add(result);
    }
}