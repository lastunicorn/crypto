using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.ConsoleTools.Commando.Setup.Autofac;
using DustInTheWind.Crypto.PresentationAndUseCases.Commands.EncryptionArea;

namespace DustInTheWind.Crypto;

internal class Program
{
    public static async Task Main(string[] args)
    {
        ConsoleTools.Commando.Application application = ApplicationBuilder.Create()
            .ConfigureServices(ServiceConfiguration.Configure)
            .RegisterCommandsFrom(typeof(HashCommand).Assembly)
            .Build();

        ConfigureDefaults();

        await application.RunAsync(args);
    }

    private static void ConfigureDefaults()
    {
        //StepBase.FakeIt = true;
    }
}