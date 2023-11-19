using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.RemoveCertificate;
using MediatR;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands;

/// <summary>
/// Call Example: crypto remove -n "Dummy Root CA" -L LocalMachine -S Root
/// </summary>
[NamedCommand("remove", Description = "Removes the specified certificate from the store.")]
internal class RemoveCertificateCommand : IConsoleCommand
{
    private readonly IMediator mediator;

    [NamedParameter("store-location", ShortName = 'L', IsOptional = true)]
    public StoreLocation StoreLocation { get; set; } = StoreLocation.CurrentUser;

    [NamedParameter("store-name", ShortName = 'S', IsOptional = true)]
    public StoreName StoreName { get; set; } = StoreName.My;

    [NamedParameter("name", ShortName = 'n')]
    public string Name { get; set; }

    public RemoveCertificateCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        RemoveCertificateRequest request = new()
        {
            StoreLocation = StoreLocation,
            StoreName = StoreName,
            Name = Name
        };

        await mediator.Send(request);
    }
}