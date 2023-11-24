using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application;
using DustInTheWind.Crypto.Application.WatersArea.ShowWatersCertificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.WatersArea;

[NamedCommand("waters-show", Description = "Displays detailed information for all the waters certificates.")]
internal class ShowWatersCertificatesCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; } = CertificateType.All;

    [NamedParameter("filter", ShortName = 'f', IsOptional = true)]
    public string Filter { get; set; }

    [NamedParameter("details", ShortName = 'd', IsOptional = true)]
    public CertificateDetailsType Details { get; set; }

    public ShowWatersCertificatesCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        ShowWatersCertificatesRequest request = new();

        await mediator.Send(request);
    }
}