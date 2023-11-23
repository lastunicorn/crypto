using DustInTheWind.Crypto.Domain.HashEncryption;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.EncryptionArea.Hash;

internal class HashUseCase : IRequestHandler<HashRequest, HashResponse>
{
    private const string DefaultTextToEncrypt = "Here is some data to encrypt!";

    private readonly ILog log;

    private HashResponse response;

    public HashUseCase(ILog log)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public Task<HashResponse> Handle(HashRequest request, CancellationToken cancellationToken)
    {
        response = new HashResponse();

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

        return Task.FromResult(response);
    }

    private void EncryptWithMd5(string textToEncrypt)
    {
        Md5EncryptedText encryptedText = new(textToEncrypt);

        EncryptionResult result = new()
        {
            HashAlgorithm = HashAlgorithmEnum.Md5,
            OriginalMessage = textToEncrypt,
            EncryptedMessage = encryptedText
        };

        response.EncryptionResults.Add(result);
    }

    private void EncryptWithSha1(string textToEncrypt)
    {
        Sha1EncryptedText encryptedText = new(textToEncrypt);

        EncryptionResult result = new()
        {
            HashAlgorithm = HashAlgorithmEnum.Sha1,
            OriginalMessage = textToEncrypt,
            EncryptedMessage = encryptedText
        };

        response.EncryptionResults.Add(result);
    }

    private void EncryptWithSha256(string textToEncrypt)
    {
        Sha256EncryptedText encryptedText = new(textToEncrypt);

        EncryptionResult result = new()
        {
            HashAlgorithm = HashAlgorithmEnum.Sha256,
            OriginalMessage = textToEncrypt,
            EncryptedMessage = encryptedText
        };

        response.EncryptionResults.Add(result);
    }

    private void EncryptWithSha384(string textToEncrypt)
    {
        Sha384EncryptedText encryptedText = new(textToEncrypt);

        EncryptionResult result = new()
        {
            HashAlgorithm = HashAlgorithmEnum.Sha384,
            OriginalMessage = textToEncrypt,
            EncryptedMessage = encryptedText
        };

        response.EncryptionResults.Add(result);
    }

    private void EncryptWithSha512(string textToEncrypt)
    {
        Sha512EncryptedText encryptedText = new(textToEncrypt);

        EncryptionResult result = new()
        {
            HashAlgorithm = HashAlgorithmEnum.Sha512,
            OriginalMessage = textToEncrypt,
            EncryptedMessage = encryptedText
        };

        response.EncryptionResults.Add(result);
    }
}