using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.Ports.UserAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.WatersArea.ShowWatersCertificates;

internal class ShowWatersCertificatesUseCase : IRequestHandler<ShowWatersCertificatesRequest, ShowWatersCertificatesResponse>
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;
    private readonly IUserInterface userInterface;

    public ShowWatersCertificatesUseCase(ILog log, ICertificateRepository certificateRepository, IUserInterface userInterface)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
        this.userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
    }

    public Task<ShowWatersCertificatesResponse> Handle(ShowWatersCertificatesRequest request, CancellationToken cancellationToken)
    {
        WatersCertificateIdentifiers watersCertificateIdentifiers = new();

        IEnumerable<CertificateIdentifier> certificateIdentifiers = request.CertificateType switch
        {
            CertificateType.Root => watersCertificateIdentifiers.Where(x => x.StoreName == StoreName.Root),
            CertificateType.Intermediate => watersCertificateIdentifiers.Where(x => x.StoreName == StoreName.CertificateAuthority),
            CertificateType.Normal => watersCertificateIdentifiers.Where(x => x.StoreName == StoreName.My),
            _ => watersCertificateIdentifiers
        };

        if (request.Filter != null)
            certificateIdentifiers = certificateIdentifiers.Where(x => x.Name.Contains(request.Filter));

        foreach (CertificateIdentifier watersCertificateIdentifier in certificateIdentifiers)
            Show(watersCertificateIdentifier, request.Details);

        ShowWatersCertificatesResponse response = new();

        return Task.FromResult(response);
    }

    private void Show(CertificateIdentifier certificateIdentifier, CertificateDetailsType detailsType)
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
                ShowCertificate(certificate, index, detailsType);
                index++;
            }
        }
    }

    private void ShowCertificate(GenericCertificate certificate, int index, CertificateDetailsType detailsType)
    {
        switch (detailsType)
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
        ShowCertificateOverviewStep showCertificateOverviewStep = new(log, userInterface)
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
        ShowCertificateLocationStep showCertificateLocationStep = new(log, userInterface)
        {
            Subtitle = index.ToString(),
            Certificate = certificate
        };

        showCertificateLocationStep.Execute();
    }
}