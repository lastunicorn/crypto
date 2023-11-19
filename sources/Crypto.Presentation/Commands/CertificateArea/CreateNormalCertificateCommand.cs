using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.CertificateArea.CreateNormalCertificate;
using MediatR;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands.CertificateArea;

[NamedCommand("create-normal")]
internal class CreateNormalCertificateCommand : IConsoleCommand
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

    public CreateNormalCertificateCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        CreateNormalCertificateRequest request = new()
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