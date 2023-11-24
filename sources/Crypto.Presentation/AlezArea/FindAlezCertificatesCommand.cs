using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.AlezArea.FindAlezCertificates;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.AlezArea;

[NamedCommand("alez-find", Description = "Search in the store for the \"alez\" certificates.")]
internal class FindAlezCertificatesCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    public FindAlezCertificatesCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        FindAlezCertificatesRequest request = new();
        await mediator.Send(request);
    }
}