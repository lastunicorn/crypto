using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.WatersArea.CreateWatersClientTlsCertificate;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.WatersArea;

[NamedCommand("create-waters-tls", Enabled = false)]
internal class CreateWatersClientTlsCertificateCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    public CreateWatersClientTlsCertificateCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        CreateWatersClientTlsCertificateRequest request = new();

        await mediator.Send(request);
    }
}