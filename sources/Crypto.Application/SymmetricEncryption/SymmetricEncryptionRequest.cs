using MediatR;

namespace DustInTheWind.Crypto.Application.SymmetricEncryption;

public class SymmetricEncryptionRequest : IRequest
{
    public string Text { get; set; }
}