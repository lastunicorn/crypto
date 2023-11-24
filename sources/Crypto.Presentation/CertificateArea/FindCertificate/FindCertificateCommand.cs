using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.CertificateArea.FindCertificate;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.CertificateArea.FindCertificate;

[NamedCommand("find", Description = "Search in the store for the specified certificate.")]
internal class FindCertificateCommand : IConsoleCommand<FindCertificateViewModel>
{
    private readonly IMediator mediator;

    [NamedParameter("store-location", ShortName = 'L', IsOptional = true)]
    public StoreLocation StoreLocation { get; set; }

    [NamedParameter("store-name", ShortName = 'S', IsOptional = true)]
    public StoreName StoreName { get; set; }

    [NamedParameter("subject-name", ShortName = 's')]
    public string Name { get; set; }

    public FindCertificateCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<FindCertificateViewModel> Execute()
    {
        FindCertificateRequest request = new()
        {
            StoreLocation = StoreLocation,
            StoreName = StoreName,
            Name = Name
        };

        FindCertificateResponse response = await mediator.Send(request);

        return new FindCertificateViewModel
        {
            FindCertificateResult = response.FindCertificateResult
        };
    }
}