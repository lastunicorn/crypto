using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.PresentationAndUseCases.Steps;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands;

[NamedCommand("hash", Description = "Encrypts the provided text using hashing algorithms: MD5, SHA1, SHA256, SHA384, SHA512.")]
public class HashCommand : ICommand
{
    private readonly ILog log;

    [NamedParameter("text", ShortName = 't', IsOptional = true)]
    public string Text { get; set; } = "Here is some data to encrypt!";

    [NamedParameter("algorithm", ShortName = 'a', IsOptional = true)]
    public HashAlgorithmEnum Algorithm { get; set; } = HashAlgorithmEnum.All;

    public HashCommand(ILog log)
    {
        this.log = log;
    }

    public Task Execute()
    {
        if (Algorithm is HashAlgorithmEnum.All or HashAlgorithmEnum.Md5)
            EncryptWithMd5(Text);

        if (Algorithm is HashAlgorithmEnum.All or HashAlgorithmEnum.Sha1)
            EncryptWithSha1(Text);

        if (Algorithm is HashAlgorithmEnum.All or HashAlgorithmEnum.Sha256)
            EncryptWithSha256(Text);

        if (Algorithm is HashAlgorithmEnum.All or HashAlgorithmEnum.Sha384)
            EncryptWithSha384(Text);

        if (Algorithm is HashAlgorithmEnum.All or HashAlgorithmEnum.Sha512)
            EncryptWithSha512(Text);

        return Task.CompletedTask;
    }

    private void EncryptWithMd5(string textToEncrypt)
    {
        Md5EncryptStep md5EncryptStep = new(log)
        {
            Message = textToEncrypt
        };

        md5EncryptStep.Execute();
    }

    private void EncryptWithSha1(string textToEncrypt)
    {
        Sha1EncryptStep sha1EncryptStep = new(log)
        {
            Message = textToEncrypt
        };

        sha1EncryptStep.Execute();
    }

    private void EncryptWithSha256(string textToEncrypt)
    {
        Sha256EncryptStep sha256EncryptStep = new(log)
        {
            Message = textToEncrypt
        };

        sha256EncryptStep.Execute();
    }

    private void EncryptWithSha384(string textToEncrypt)
    {
        Sha384EncryptStep sha384EncryptStep = new(log)
        {
            Message = textToEncrypt
        };

        sha384EncryptStep.Execute();
    }

    private void EncryptWithSha512(string textToEncrypt)
    {
        Sha512EncryptStep sha512EncryptStep = new(log)
        {
            Message = textToEncrypt
        };

        sha512EncryptStep.Execute();
    }
}