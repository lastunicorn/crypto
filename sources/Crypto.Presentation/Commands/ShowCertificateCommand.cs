using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.PresentationAndUseCases.Steps;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands;

/// <summary>
/// Call Example: crypto show -s "Dummy Root CA" -S Root -L LocalMachine
/// </summary>
[NamedCommand("show", Description = "Displays detailed information for the specified certificate from the store.")]
internal class ShowCertificateCommand : ICommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    [NamedParameter("store-location", ShortName = 'L', IsOptional = true)]
    public StoreLocation StoreLocation { get; set; } = StoreLocation.CurrentUser;

    [NamedParameter("store-name", ShortName = 'S', IsOptional = true)]
    public StoreName StoreName { get; set; } = StoreName.My;

    [NamedParameter("subject-name", ShortName = 's')]
    public string SubjectName { get; set; }

    [NamedParameter("details", ShortName = 'd', IsOptional = true)]
    public CertificateDetailsType Details { get; set; }

    public ShowCertificateCommand(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Execute()
    {
        List<GenericCertificate> foundCertificates = FindCertificates();

        if (foundCertificates.Count != 0)
        {
            int index = 0;

            foreach (GenericCertificate certificate in foundCertificates)
            {
                ShowCertificate(certificate, index);
                index++;
            }
        }

        return Task.CompletedTask;
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

    private List<GenericCertificate> FindCertificates()
    {
        FindCertificateBySubjectStep findCertificateBySubjectStep = new(log, certificateRepository)
        {
            SubjectName = SubjectName,
            StoreLocation = StoreLocation,
            StoreName = StoreName
        };

        findCertificateBySubjectStep.Execute();

        return findCertificateBySubjectStep.FoundCertificates;
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