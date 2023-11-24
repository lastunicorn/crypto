using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.AlezArea;
using DustInTheWind.Crypto.Application.AlezArea.GrantAccessToAlezCertificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.AlezArea;

[NamedCommand("alez-grant-access", Description = "Give read permissions to everyone for accessing the \"alez\" certificates.")]
internal class GrantAccessToAlezCertificatesCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; }

    [NamedParameter("filter", ShortName = 'f', IsOptional = true)]
    public string Filter { get; set; }

    public GrantAccessToAlezCertificatesCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        GrantAccessToAlezCertificatesRequest request = new()
        {
            CertificateType = CertificateType,
            Filter = Filter
        };

        await mediator.Send(request);
    }
}