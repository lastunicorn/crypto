using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.EncryptionArea.Hash;
using MediatR;

namespace DustInTheWind.Crypto.Presentation.EncryptionArea.Hash;

[NamedCommand("hash", Description = "Encrypts the provided text using hashing algorithms: MD5, SHA1, SHA256, SHA384, SHA512.")]
public class HashCommand : IConsoleCommand<HashViewModel>
{
    private readonly IMediator mediator;

    [NamedParameter("text", ShortName = 't', IsOptional = true)]
    public string Text { get; set; }

    [NamedParameter("algorithm", ShortName = 'a', IsOptional = true)]
    public HashAlgorithmEnum Algorithm { get; set; }

    public HashCommand(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<HashViewModel> Execute()
    {
        HashRequest request = new()
        {
            Text = Text,
            Algorithm = Algorithm
        };

        HashResponse response = await mediator.Send(request);

        return new HashViewModel
        {
            HashResults = response.EncryptionResults
        };
    }
}