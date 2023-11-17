using System.Security.Cryptography;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.PresentationAndUseCases.Steps;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands;

[NamedCommand("encrypt", Description = "Encrypts and decrypts the provided text using symmetric encryption algorithms: AES, DES, TripleDES.")]
internal class SymmetricEncryptionCommand : ICommand
{
    private readonly ILog log;

    [NamedParameter("text", ShortName = 't', IsOptional = true)]
    public string Text { get; set; } = "Here is some data to encrypt!";

    public SymmetricEncryptionCommand(ILog log)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public Task Execute()
    {
        EncryptDecryptWithAes(Text);
        EncryptDecryptWithDes(Text);
        EncryptDecryptWithTripleDes(Text);

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