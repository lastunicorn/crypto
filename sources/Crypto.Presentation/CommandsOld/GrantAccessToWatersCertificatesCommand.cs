using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.AlezArea;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.CommandsOld;

[NamedCommand("waters-grant-access", Description = "Give read permissions to everyone for accessing the \"waters\" certificates.")]
internal class GrantAccessToWatersCertificatesCommand : IConsoleCommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; } = CertificateType.All;

    [NamedParameter("filter", ShortName = 'f', IsOptional = true)]
    public string Filter { get; set; }

    public GrantAccessToWatersCertificatesCommand(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Execute()
    {
        WatersCertificateIdentifiers watersCertificateIdentifiers = new();

        IEnumerable<CertificateIdentifier> certificateIdentifiers = CertificateType switch
        {
            CertificateType.Root => watersCertificateIdentifiers.Where(x => x.StoreName == StoreName.Root),
            CertificateType.Intermediate => watersCertificateIdentifiers.Where(x => x.StoreName == StoreName.CertificateAuthority),
            CertificateType.Normal => watersCertificateIdentifiers.Where(x => x.StoreName == StoreName.My),
            _ => watersCertificateIdentifiers
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