using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.CertificateArea.CreateIntermediateCertificate;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.CertificateArea;

[NamedCommand("create-intermediate", Description = "Creates an intermediate certificate and installs it in the store.")]
internal class CreateIntermediateCertificateCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("parent-store-location", IsOptional = true)]
    public StoreLocation ParentStoreLocation { get; set; }

    [NamedParameter("parent-store-name", IsOptional = true)]
    public StoreName ParentStoreName { get; set; }

    [NamedParameter("parent-name", ShortName = 'p')]
    public string ParentName { get; set; }

    [NamedParameter("store-location", ShortName = 'L', IsOptional = true)]
    public StoreLocation StoreLocation { get; set; }

    [NamedParameter("store-name", ShortName = 'S', IsOptional = true)]
    public StoreName StoreName { get; set; }

    [NamedParameter("name", ShortName = 'n')]
    public string CertificateName { get; set; }

    public CreateIntermediateCertificateCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        CreateIntermediateCertificateRequest request = new()
        {
            ParentStoreLocation = ParentStoreLocation,
            ParentStoreName = ParentStoreName,
            ParentName = ParentName,
            StoreLocation = StoreLocation,
            StoreName = StoreName,
            CertificateName = CertificateName
        };

        await mediator.Send(request);
    }
}