using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.EncryptionArea.SymmetricEncryption;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.EncryptionArea.SymmetricEncryption;

[NamedCommand("encrypt", Description = "Encrypts and decrypts the provided text using symmetric encryption algorithms: AES, DES, TripleDES.")]
internal class SymmetricEncryptionCommand : IConsoleCommand<SymmetricEncryptionViewModel>
{
    private readonly IMediator mediator;

    [NamedParameter("text", ShortName = 't', IsOptional = true)]
    public string Text { get; set; }

    public SymmetricEncryptionCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<SymmetricEncryptionViewModel> Execute()
    {
        SymmetricEncryptionRequest request = new()
        {
            Text = Text
        };

        SymmetricEncryptionResponse response = await mediator.Send(request);

        return new SymmetricEncryptionViewModel
        {
            HashResults = response.EncryptionResults
        };
    }
}