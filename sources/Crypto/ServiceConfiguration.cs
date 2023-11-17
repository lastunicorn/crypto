using Autofac;
using DustInTheWind.Crypto.Adapters.CertificateAccess;
using DustInTheWind.Crypto.Adapters.FileAccess;
using DustInTheWind.Crypto.Adapters.LogAccess;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto;

internal static class ServiceConfiguration
{
    public static void Configure(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<FileSystem>().As<IFileSystem>();
        containerBuilder.RegisterType<CertificateRepository>().As<ICertificateRepository>();

        containerBuilder
            .Register(context => new ColorfulConsoleLog
            {
                BinaryFormat = BinaryDisplayFormat.Hexadecimal,
                BinaryMaxLength = 20
            })
            .As<ILog>();
    }
}