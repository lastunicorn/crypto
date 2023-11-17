using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.PresentationAndUseCases.Steps;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands;

[NamedCommand("waters-find", Description = "Search in the store for the \"waters\" certificates.")]
internal class FindWatersCertificatesCommand : ICommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    public FindWatersCertificatesCommand(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Execute()
    {
        WatersCertificateIdentifiers certificateIdentifiers = new();

        foreach (CertificateIdentifier certificateIdentifier in certificateIdentifiers)
        {
            FindCertificateStep findCertificateStep = new(log, certificateRepository)
            {
                CertificateIdentifier = certificateIdentifier
            };
            findCertificateStep.Execute();
        }

        return Task.CompletedTask;
    }
}