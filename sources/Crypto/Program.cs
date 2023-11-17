using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.ConsoleTools.Commando.Setup.Autofac;
using DustInTheWind.Crypto.PresentationAndUseCases.Commands;

namespace DustInTheWind.Crypto;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Application application = ApplicationBuilder.Create()
            .RegisterCommandsFrom(typeof(HashCommand).Assembly)
            .ConfigureServices(ServiceConfiguration.Configure)
            .Build();

        ConfigureDefaults();

        await application.RunAsync(args);
    }

    private static void ConfigureDefaults()
    {
        //StepBase.FakeIt = true;
    }
}