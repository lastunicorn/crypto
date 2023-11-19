using Autofac;
using DustInTheWind.ConsoleTools.Commando.Setup.Autofac;
using DustInTheWind.Crypto.Adapters.CertificateAccess;
using DustInTheWind.Crypto.Adapters.FileAccess;
using DustInTheWind.Crypto.Adapters.LogAccess;
using DustInTheWind.Crypto.Application.SymmetricEncryption;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

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

        MediatRConfiguration mediatRConfiguration = MediatRConfigurationBuilder
            .Create(typeof(SymmetricEncryptionRequest).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        containerBuilder.RegisterMediatR(mediatRConfiguration);
    }
}