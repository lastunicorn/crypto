using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.PresentationAndUseCases.Steps;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands;

/// <summary>
/// Call Example: crypto remove -n "Dummy Root CA" -L LocalMachine -S Root
/// </summary>
[NamedCommand("remove", Description = "Removes the specified certificate from the store.")]
internal class RemoveCertificateCommand : ICommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;
    private readonly IFileSystem fileSystem;

    [NamedParameter("store-location", ShortName = 'L', IsOptional = true)]
    public StoreLocation StoreLocation { get; set; } = StoreLocation.CurrentUser;

    [NamedParameter("store-name", ShortName = 'S', IsOptional = true)]
    public StoreName StoreName { get; set; } = StoreName.My;

    [NamedParameter("name", ShortName = 'n')]
    public string Name { get; set; }

    public RemoveCertificateCommand(ILog log, ICertificateRepository certificateRepository, IFileSystem fileSystem)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public Task Execute()
    {
        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = new CertificateIdentifier
            {
                Name = Name,
                StoreLocation = StoreLocation,
                StoreName = StoreName
            }
        };

        findCertificateStep.Execute();

        if (findCertificateStep.FoundCertificates is { Count: not 0 })
        {
            RemoveCertificateFromStoreStep removeCertificateFromStoreStep = new(log, certificateRepository)
            {
                Certificates = findCertificateStep.FoundCertificates
            };

            removeCertificateFromStoreStep.Execute();

            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}