using MediatR;

namespace DustInTheWind.Crypto.Application.Hash;

public class HashRequest : IRequest
{
    public string Text { get; set; }

    public HashAlgorithmEnum Algorithm { get; set; }

}