using Autofac;
using DustInTheWind.Crypto.Adapters.CertificateAccess;
using DustInTheWind.Crypto.Adapters.FileAccess;
using DustInTheWind.Crypto.Adapters.LogAccess;
using DustInTheWind.Crypto.Adapters.UserAccess;
using DustInTheWind.Crypto.Application.EncryptionArea.SymmetricEncryption;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.Ports.UserAccess;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

namespace DustInTheWind.Crypto;

internal static class ServiceConfiguration
{
    public static void Configure(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<FileSystem>().As<IFileSystem>();
        containerBuilder.RegisterType<CertificateRepository>().As<ICertificateRepository>();
        containerBuilder.RegisterType<UserInterface>().As<IUserInterface>();

        containerBuilder
            .Register(context => new ColorfulConsoleLog
            {
                BinaryFormat = Ports.LogAccess.BinaryDisplayFormat.Hexadecimal,
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