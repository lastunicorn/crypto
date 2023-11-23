using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.AlezArea;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Presentation.CommandsOld;

[NamedCommand("alez-grant-access", Description = "Give read permissions to everyone for accessing the \"alez\" certificates.")]
internal class GrantAccessToAlezCertificatesCommand : IConsoleCommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; } = CertificateType.All;

    [NamedParameter("filter", ShortName = 'f', IsOptional = true)]
    public string Filter { get; set; }

    public GrantAccessToAlezCertificatesCommand(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
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

        if (Filter != null)
            certificateIdentifiers = certificateIdentifiers.Where(x => x.Name.Contains(Filter));

        foreach (CertificateIdentifier watersCertificateIdentifier in certificateIdentifiers)
            GrantAccessTo(watersCertificateIdentifier);

        return Task.CompletedTask;
    }

    private void GrantAccessTo(CertificateIdentifier certificateIdentifier)
    {
        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = certificateIdentifier
        };
        findCertificateStep.Execute();

        GrantReadAccessToCertificateStep grantReadAccessToCertificateStep = new(log)
        {
            Certificate = findCertificateStep.FoundCertificates?.FirstOrDefault()
        };

        grantReadAccessToCertificateStep.Execute();
    }
}