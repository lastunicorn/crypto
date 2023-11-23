using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application;
using DustInTheWind.Crypto.Application.AlezArea;
using DustInTheWind.Crypto.Application.AlezArea.ShowAlezCertificates;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.Commands.AlezArea;

[NamedCommand("alez-show", Description = "Displays detailed information for all three \"alez\" certificates.")]
internal class ShowAlezCertificatesCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; } = CertificateType.All;

    [NamedParameter("subject", ShortName = 's', IsOptional = true)]
    public string Subject { get; set; }

    [NamedParameter("details", ShortName = 'd', IsOptional = true)]
    public CertificateDetailsType Details { get; set; }

    public ShowAlezCertificatesCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        ShowAlezCertificatesRequest request = new()
        {
            CertificateType = CertificateType,
            Subject = Subject,
            Details = Details
        };

        await mediator.Send(request);
    }
}