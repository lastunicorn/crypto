using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Results;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.FindCertificate;

public class FindCertificateRequest : IRequest<FindCertificateResponse>
{
    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string Name { get; set; }
}

public class FindCertificateResponse
{
    public FindCertificateResult FindCertificateResult { get; set; }
}