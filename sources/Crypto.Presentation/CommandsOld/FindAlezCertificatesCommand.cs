using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.CommandsOld;

[NamedCommand("alez-find", Description = "Search in the store for the \"alez\" certificates.")]
internal class FindAlezCertificatesCommand : IConsoleCommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    public FindAlezCertificatesCommand(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Execute()
    {
        AlezCertificateIdentifiers alezCertificateIdentifiers = new();

        foreach (CertificateIdentifier alezCertificateIdentifier in alezCertificateIdentifiers)
        {
            FindCertificateStep findCertificateStep = new(log, certificateRepository)
            {
                CertificateIdentifier = alezCertificateIdentifier
            };
            findCertificateStep.Execute();
        }

        return Task.CompletedTask;
    }
}