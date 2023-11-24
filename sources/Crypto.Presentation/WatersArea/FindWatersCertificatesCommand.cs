using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.WatersArea.FindWatersCertificates;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.WatersArea;

[NamedCommand("waters-find", Description = "Search in the store for the \"waters\" certificates.")]
internal class FindWatersCertificatesCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    public FindWatersCertificatesCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        FindWatersCertificatesRequest request = new();

        await mediator.Send(request);
    }
}