using DustInTheWind.Crypto.Application.CertificateArea.RemoveCertificate;
using DustInTheWind.Crypto.Application.Results;
using DustInTheWind.Crypto.Application.Sections;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;

namespace DustInTheWind.Crypto.Application.AlezArea.RemoveAlezCertificates;

internal class CertificateSearchAndRemoval
{
    private readonly CertificateIdentifier certificateIdentifier;
    private readonly ICertificateRepository certificateRepository;
    private CertificateSearchAndRemovalResult result;

    public CertificateSearchAndRemoval(CertificateIdentifier certificateIdentifier, ICertificateRepository certificateRepository)
    {
        this.certificateIdentifier = certificateIdentifier;
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public CertificateSearchAndRemovalResult Execute()
    {
        result = new CertificateSearchAndRemovalResult();

        List<GenericCertificate> foundCertificates = FindCertificates();

        foreach (GenericCertificate certificate in foundCertificates) 
            Remove(certificate);

        return result;
    }

    private List<GenericCertificate> FindCertificates()
    {
        List<GenericCertificate> foundCertificates = certificateRepository.Get(certificateIdentifier)
            .ToList();

        result.StoreLocation = certificateIdentifier.StoreLocation;
        result.StoreName = certificateIdentifier.StoreName;
        result.CertificateName = certificateIdentifier.Name;
        result.CertificateCount = foundCertificates.Count;

        return foundCertificates;
    }

    private void Remove(GenericCertificate certificate)
    {
        CertificateRemoval certificateRemoval = new(certificate, certificateRepository);
        CertificateRemovalResult removalResult = certificateRemoval.Execute();

        result.CertificateRemovalResults.Add(removalResult);
    }
}