using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.SymmetricEncryption;
using MediatR;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands;

[NamedCommand("encrypt", Description = "Encrypts and decrypts the provided text using symmetric encryption algorithms: AES, DES, TripleDES.")]
internal class SymmetricEncryptionCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("text", ShortName = 't', IsOptional = true)]
    public string Text { get; set; }

    public SymmetricEncryptionCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        SymmetricEncryptionRequest request = new()
        {
            Text = Text
        };

        await mediator.Send(request);
    }
}