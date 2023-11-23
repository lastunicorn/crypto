using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.AlezArea;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Presentation.CommandsOld;

/// <summary>
/// Call Example: crypto alez-remove
/// Call Example: crypto alez-remove -t root
/// Call Example: crypto alez-remove -t intermediate
/// Call Example: crypto alez-remove -t normal
/// </summary>
[NamedCommand("alez-remove", Description = "Removes all three \"alez\" certificates.")]
internal class RemoveAlezCertificatesCommand : IConsoleCommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;
    private readonly IFileSystem fileSystem;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; } = CertificateType.All;

    public RemoveAlezCertificatesCommand(ILog log, ICertificateRepository certificateRepository, IFileSystem fileSystem)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public Task Execute()
    {
        AlezCertificateIdentifiers alezCertificateIdentifiers = new();

        IEnumerable<CertificateIdentifier> certificateIdentifiers = CertificateType switch
        {
            CertificateType.Root => alezCertificateIdentifiers.Where(x => x.StoreName == StoreName.Root),
            CertificateType.Intermediate => alezCertificateIdentifiers.Where(x => x.StoreName == StoreName.CertificateAuthority),
            CertificateType.Normal => alezCertificateIdentifiers.Where(x => x.StoreName == StoreName.My),
            _ => alezCertificateIdentifiers
        };

        SearchAndDestroy(certificateIdentifiers);

        return Task.CompletedTask;
    }

    private void SearchAndDestroy(IEnumerable<CertificateIdentifier> certificateIdentifiers)
    {
        foreach (CertificateIdentifier certificateIdentifier in certificateIdentifiers)
        {
            FindCertificateStep findCertificateStep = new(log, certificateRepository)
            {
                CertificateIdentifier = certificateIdentifier
            };
            findCertificateStep.Execute();

            List<GenericCertificate> foundCertificates = findCertificateStep.FoundCertificates;

            if (foundCertificates is { Count: > 0 })
                RemoveCertificates(foundCertificates);
        }
    }

    private void RemoveCertificates(IEnumerable<GenericCertificate> certificates)
    {
        RemoveCertificateFromStoreStep removeCertificateFromStoreStep = new(log, certificateRepository)
        {
            Certificates = certificates.ToList()
        };

        removeCertificateFromStoreStep.Execute();
    }
}