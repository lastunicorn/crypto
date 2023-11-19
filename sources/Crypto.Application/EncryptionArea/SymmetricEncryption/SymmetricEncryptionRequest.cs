using MediatR;

namespace DustInTheWind.Crypto.Application.EncryptionArea.SymmetricEncryption;

public class SymmetricEncryptionRequest : IRequest
{
    public string Text { get; set; }
}