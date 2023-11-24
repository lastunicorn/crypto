using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application;
using DustInTheWind.Crypto.Application.CertificateArea.ShowCertificate;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.CertificateArea;

/// <summary>
/// Call Example: crypto show -s "Dummy Root CA" -S Root -L LocalMachine
/// </summary>
[NamedCommand("show", Description = "Displays detailed information for the specified certificate from the store.")]
internal class ShowCertificateCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("store-location", ShortName = 'L', IsOptional = true)]
    public StoreLocation StoreLocation { get; set; }

    [NamedParameter("store-name", ShortName = 'S', IsOptional = true)]
    public StoreName StoreName { get; set; }

    [NamedParameter("subject-name", ShortName = 's')]
    public string SubjectName { get; set; }

    [NamedParameter("details", ShortName = 'd', IsOptional = true)]
    public CertificateDetailsType Details { get; set; }

    public ShowCertificateCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        ShowCertificateRequest request = new()
        {
            StoreLocation = StoreLocation,
            StoreName = StoreName,
            SubjectName = SubjectName,
            Details = Details
        };

        await mediator.Send(request);
    }
}