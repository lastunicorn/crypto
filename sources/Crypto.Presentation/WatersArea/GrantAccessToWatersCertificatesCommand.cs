using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.WatersArea.GrantAccessToWatersCertificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.WatersArea;

[NamedCommand("waters-grant-access", Description = "Give read permissions to everyone for accessing the \"waters\" certificates.")]
internal class GrantAccessToWatersCertificatesCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; }

    [NamedParameter("filter", ShortName = 'f', IsOptional = true)]
    public string Filter { get; set; }

    public GrantAccessToWatersCertificatesCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        GrantAccessToWatersCertificatesRequest request = new()
        {
            CertificateType = CertificateType,
            Filter = Filter
        };

        await mediator.Send(request);
    }
}