using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.CommandsOld;

[NamedCommand("waters-show", Description = "Displays detailed information for all the waters certificates.")]
internal class ShowWatersCertificatesCommand : IConsoleCommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; } = CertificateType.All;

    [NamedParameter("filter", ShortName = 'f', IsOptional = true)]
    public string Filter { get; set; }

    [NamedParameter("details", ShortName = 'd', IsOptional = true)]
    public CertificateDetailsType Details { get; set; }

    public ShowWatersCertificatesCommand(ILog log, ICertificateRepository certificateRepository)
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
            Show(watersCertificateIdentifier);

        return Task.CompletedTask;
    }

    private void Show(CertificateIdentifier certificateIdentifier)
    {
        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = certificateIdentifier
        };
        findCertificateStep.Execute();

        List<GenericCertificate> foundCertificates = findCertificateStep.FoundCertificates;

        if (foundCertificates == null || foundCertificates.Count == 0)
            return;

        if (foundCertificates.Count != 0)
        {
            int index = 0;

            foreach (GenericCertificate certificate in foundCertificates)
            {
                ShowCertificate(certificate, index);
                index++;
            }
        }
    }

    private void ShowCertificate(GenericCertificate certificate, int index)
    {
        switch (Details)
        {
            case CertificateDetailsType.Full:
                ShowOverview(certificate, index);
                ShowKeys(certificate, index);
                ShowExtensions(certificate, index);
                break;

            case CertificateDetailsType.Overview:
                ShowOverview(certificate, index);
                break;

            case CertificateDetailsType.Keys:
                ShowKeys(certificate, index);
                break;

            case CertificateDetailsType.Extensions:
                ShowExtensions(certificate, index);
                break;

            case CertificateDetailsType.KeyLocation:
                ShowKeyLocation(certificate, index);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ShowOverview(GenericCertificate certificate, int index)
    {
        ShowCertificateOverviewStep showCertificateOverviewStep = new(log)
        {
            Subtitle = index.ToString(),
            Certificate = certificate
        };

        showCertificateOverviewStep.Execute();
    }

    private void ShowKeys(GenericCertificate certificate, int index)
    {
        ShowCertificateKeysStep certificateKeysStep = new(log)
        {
            Subtitle = index.ToString(),
            Certificate = certificate
        };

        certificateKeysStep.Execute();
    }

    private void ShowExtensions(GenericCertificate certificate, int index)
    {
        ShowCertificateExtensionsStep showCertificateExtensionsStep = new(log)
        {
            Subtitle = index.ToString(),
            Certificate = certificate
        };

        showCertificateExtensionsStep.Execute();
    }

    private void ShowKeyLocation(GenericCertificate certificate, int index)
    {
        ShowCertificateLocationStep showCertificateLocationStep = new(log)
        {
            Subtitle = index.ToString(),
            Certificate = certificate
        };

        showCertificateLocationStep.Execute();
    }
}