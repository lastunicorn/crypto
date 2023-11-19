using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.EncryptionArea.Hash;

internal class HashUseCase : IRequestHandler<HashRequest>
{
    private const string DefaultTextToEncrypt = "Here is some data to encrypt!";

    private readonly ILog log;

    public HashUseCase(ILog log)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public Task Handle(HashRequest request, CancellationToken cancellationToken)
    {
        string textToEncrypt = request.Text ?? DefaultTextToEncrypt;

        if (request.Algorithm is HashAlgorithmEnum.All or HashAlgorithmEnum.Md5)
            EncryptWithMd5(textToEncrypt);

        if (request.Algorithm is HashAlgorithmEnum.All or HashAlgorithmEnum.Sha1)
            EncryptWithSha1(textToEncrypt);

        if (request.Algorithm is HashAlgorithmEnum.All or HashAlgorithmEnum.Sha256)
            EncryptWithSha256(textToEncrypt);

        if (request.Algorithm is HashAlgorithmEnum.All or HashAlgorithmEnum.Sha384)
            EncryptWithSha384(textToEncrypt);

        if (request.Algorithm is HashAlgorithmEnum.All or HashAlgorithmEnum.Sha512)
            EncryptWithSha512(textToEncrypt);

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