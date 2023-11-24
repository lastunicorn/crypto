using MediatR;

namespace DustInTheWind.Crypto.Application.EncryptionArea.SymmetricEncryption;

public class SymmetricEncryptionRequest : IRequest<SymmetricEncryptionResponse>
{
    public string Text { get; set; }
}