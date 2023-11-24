using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.AlezArea;
using DustInTheWind.Crypto.Application.AlezArea.RemoveAlezCertificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.AlezArea;

/// <summary>
/// Call Example: crypto alez-remove
/// Call Example: crypto alez-remove -t root
/// Call Example: crypto alez-remove -t intermediate
/// Call Example: crypto alez-remove -t normal
/// </summary>
[NamedCommand("alez-remove", Description = "Removes all three \"alez\" certificates.")]
internal class RemoveAlezCertificatesCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; }

    public RemoveAlezCertificatesCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        RemoveAlezCertificatesRequest request = new()
        {
            CertificateType = CertificateType
        };

        await mediator.Send(request);
    }
}