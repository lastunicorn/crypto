using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.AlezArea;
using DustInTheWind.Crypto.Application.AlezArea.ExportAlezAsPem;
using DustInTheWind.Crypto.Domain.CertificateModel;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.AlezArea;

[NamedCommand("alez-export-pem", Description = "Exports all \"alez\" certificates in pem files.")]
internal class ExportAlezAsPemCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; }

    public ExportAlezAsPemCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        ExportAlezAsPemRequest request = new()
        {
            CertificateType = CertificateType
        };

        await mediator.Send(request);
    }
}