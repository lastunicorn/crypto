using System.Security.Cryptography;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.SymmetricEncryption;

internal class SymmetricEncryptionUseCase : IRequestHandler<SymmetricEncryptionRequest>
{
    private const string DefaultTextToEncrypt = "Here is some data to encrypt!";

    private readonly ILog log;

    public SymmetricEncryptionUseCase(ILog log)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public Task Handle(SymmetricEncryptionRequest request, CancellationToken cancellationToken)
    {
        string textToEncrypt = request.Text ?? DefaultTextToEncrypt;

        EncryptDecryptWithAes(textToEncrypt);
        EncryptDecryptWithDes(textToEncrypt);
        EncryptDecryptWithTripleDes(textToEncrypt);

        return Task.CompletedTask;
    }

    private void EncryptDecryptWithAes(string textToEncrypt)
    {
        using Aes myAes = Aes.Create();

        // ----------------------------------------------------------------------------------------------------
        // AES Encrypt
        // ----------------------------------------------------------------------------------------------------

        AesEncryptStep aesEncryptStep = new(log)
        {
            Message = textToEncrypt,
            Key = myAes.Key,
            IV = myAes.IV
        };

        aesEncryptStep.Execute();

        // ----------------------------------------------------------------------------------------------------
        // AES Decrypt
        // ----------------------------------------------------------------------------------------------------

        AesDecryptStep aesDecryptStep = new(log)
        {
            EncryptedMessage = aesEncryptStep.EncryptedMessage,
            Key = myAes.Key,
            IV = myAes.IV
        };

        aesDecryptStep.Execute();
    }

    private void EncryptDecryptWithDes(string textToEncrypt)
    {
        using DES myDes = DES.Create();

        // ----------------------------------------------------------------------------------------------------
        // DES Encrypt
        // ----------------------------------------------------------------------------------------------------

        DesEncryptStep desEncryptStep = new(log)
        {
            Message = textToEncrypt,
            Key = myDes.Key,
            IV = myDes.IV
        };

        desEncryptStep.Execute();

        // ----------------------------------------------------------------------------------------------------
        // DES Decrypt
        // ----------------------------------------------------------------------------------------------------

        DesDecryptStep desDecryptStep = new(log)
        {
            EncryptedMessage = desEncryptStep.EncryptedMessage,
            Key = myDes.Key,
            IV = myDes.IV
        };

        desDecryptStep.Execute();
    }

    private void EncryptDecryptWithTripleDes(string textToEncrypt)
    {
        using TripleDES myTripleDes = TripleDES.Create();

        TripleDesEncryptStep tripleDesEncryptStep = new(log)
        {
            Message = textToEncrypt,
            Key = myTripleDes.Key,
            IV = myTripleDes.IV
        };

        tripleDesEncryptStep.Execute();

        TripleDesDecryptStep tripleDesDecryptStep = new(log)
        {
            EncryptedMessage = tripleDesEncryptStep.EncryptedMessage,
            Key = myTripleDes.Key,
            IV = myTripleDes.IV
        };

        tripleDesDecryptStep.Execute();
    }
}