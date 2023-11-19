using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.AlezArea;
using DustInTheWind.Crypto.Application.AlezArea.CreateAlezCertificates;
using MediatR;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands.AlezArea;

/// <summary>
/// Call Example: crypto alez-create
/// Call Example: crypto alez-create -t root
/// Call Example: crypto alez-create -t intermediate
/// Call Example: crypto alez-create -t normal
/// </summary>
[NamedCommand("alez-create", Description = "Creates the three \"alez\" certificates: one root, one intermediate and one normal.")]
internal class CreateAlezCertificatesCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; }

    public CreateAlezCertificatesCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        CreateAlezCertificatesRequest request = new()
        {
            CertificateType = CertificateType
        };

        await mediator.Send(request);
    }
}