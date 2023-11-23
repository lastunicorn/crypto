using MediatR;

namespace DustInTheWind.Crypto.Application.EncryptionArea.Hash;

public class HashRequest : IRequest<HashResponse>
{
    public string Text { get; set; }

    public HashAlgorithmEnum Algorithm { get; set; }

}