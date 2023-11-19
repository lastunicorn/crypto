using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.CertificateArea.ExportAsPem;
using MediatR;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands.CertificateArea;

[NamedCommand("export-pem", Description = "Exports a specific certificate from the store as a pem file.")]
internal class ExportAsPemCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("store-location", ShortName = 'L', IsOptional = true)]
    public StoreLocation StoreLocation { get; set; }

    [NamedParameter("store-name", ShortName = 'S', IsOptional = true)]
    public StoreName StoreName { get; set; }

    [NamedParameter("subject-name", ShortName = 's')]
    public string SubjectName { get; set; }

    public ExportAsPemCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        ExportAsPemRequest request = new()
        {
            StoreLocation = StoreLocation,
            StoreName = StoreName,
            SubjectName = SubjectName
        };

        await mediator.Send(request);
    }
}