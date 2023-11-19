using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.CertificateArea.CreateRootCertificate;
using MediatR;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands.CertificateArea;

/// <summary>
/// Call Example: crypto create-root -n "Dummy Root CA" -f "Dummy Root Certificate Authority"
/// Call Example: crypto create-root -n "Dummy Root CA" -f "Dummy Root Certificate Authority" -L LocalMachine -S Root
/// </summary>
[NamedCommand("create-root", Description = "Creates a new root certificate and installs it in the store.")]
internal class CreateRootCertificateCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("store-location", ShortName = 'L', IsOptional = true)]
    public StoreLocation StoreLocation { get; set; }

    [NamedParameter("store-name", ShortName = 'S', IsOptional = true)]
    public StoreName StoreName { get; set; }

    [NamedParameter("name", ShortName = 'n')]
    public string CertificateName { get; set; }

    [NamedParameter("friendly-name", ShortName = 'f', IsOptional = true)]
    public string FriendlyName { get; set; }

    public CreateRootCertificateCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        CreateRootCertificateRequest request = new()
        {
            StoreLocation = StoreLocation,
            StoreName = StoreName,
            CertificateName = CertificateName,
            FriendlyName = FriendlyName
        };

        await mediator.Send(request);
    }
}