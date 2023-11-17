using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.PresentationAndUseCases.Steps;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands;

[NamedCommand("grant-access", Description = "Give read permissions to everyone for accessing the specified certificate.")]
internal class GrantAccessToCertificateCommand : ICommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    [NamedParameter("store-location", ShortName = 'L', IsOptional = true)]
    public StoreLocation StoreLocation { get; set; } = StoreLocation.CurrentUser;

    [NamedParameter("store-name", ShortName = 'S', IsOptional = true)]
    public StoreName StoreName { get; set; } = StoreName.My;

    [NamedParameter("subject-name", ShortName = 's')]
    public string SubjectName { get; set; }

    public GrantAccessToCertificateCommand(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Execute()
    {
        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = new CertificateIdentifier
            {
                Name = SubjectName,
                StoreLocation = StoreLocation,
                StoreName = StoreName
            }
        };

        findCertificateStep.Execute();

        GrantReadAccessToCertificateStep grantReadAccessToCertificateStep = new(log)
        {
            Certificate = findCertificateStep.FoundCertificates?.FirstOrDefault()
        };

        grantReadAccessToCertificateStep.Execute();

        return Task.CompletedTask;
    }
}