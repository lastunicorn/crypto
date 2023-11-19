using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.AlezArea;
using DustInTheWind.Crypto.Application.AlezArea.ExportAlezAsPfx;
using MediatR;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands.AlezArea;

[NamedCommand("alez-export-pfx", Description = "Exports all \"alez\" certificates in pfx files.")]
internal class ExportAlezAsPfxCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; }

    public ExportAlezAsPfxCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        ExportAlezAsPfxRequest request = new()
        {
            CertificateType = CertificateType
        };

        await mediator.Send(request);
    }
}