// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Design.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class DesignTimeServicesBuilder
    {
        private readonly Assembly _startupAssembly;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public DesignTimeServicesBuilder([NotNull] Assembly startupAssembly)
        {
            _startupAssembly = startupAssembly;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual IServiceProvider Build([NotNull] DbContext context)
        {
            Check.NotNull(context, nameof(context));

            var services = ConfigureServices(new ServiceCollection());

            var contextServices = ((IInfrastructure<IServiceProvider>)context).Instance;
            ConfigureContextServices(((IInfrastructure<IServiceProvider>)context).Instance, services);

            ConfigureProviderServices(contextServices.GetRequiredService<IDatabaseProvider>().InvariantName, services);

            ConfigureUserServices(services);

            return services.BuildServiceProvider();
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual IServiceProvider Build([NotNull] string provider)
            => ConfigureUserServices(
                    ConfigureProviderServices(
                        Check.NotEmpty(provider, nameof(provider)),
                        ConfigureServices(new ServiceCollection()), throwOnError: true))
                .BuildServiceProvider();

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual IServiceCollection ConfigureServices([NotNull] IServiceCollection services)
            => services
                .AddSingleton<CSharpHelper>()
                .AddSingleton<CSharpMigrationOperationGenerator>()
                .AddSingleton<CSharpSnapshotGenerator>()
                .AddSingleton<MigrationsCodeGenerator, CSharpMigrationsGenerator>()
                .AddScaffolding()
                .AddLogging();

        private IServiceCollection ConfigureProviderServices(string provider, IServiceCollection services, bool throwOnError = false)
            => ConfigureDesignTimeServices(GetProviderDesignTimeServices(provider, throwOnError), services);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual IServiceCollection ConfigureContextServices(
                [NotNull] IServiceProvider contextServices,
                [NotNull] IServiceCollection services)
            => services
                .AddTransient<MigrationsScaffolder>()
                .AddTransient(_ => contextServices.GetService<ILoggingOptions>())
                .AddTransient(_ => contextServices.GetService(typeof(IDiagnosticsLogger<>)))
                .AddTransient(_ => contextServices.GetService<DiagnosticSource>())
                .AddTransient(_ => contextServices.GetService<ICurrentDbContext>())
                .AddTransient(_ => contextServices.GetService<IDatabaseProvider>())
                .AddTransient(_ => contextServices.GetService<IDbContextOptions>())
                .AddTransient(_ => contextServices.GetService<IHistoryRepository>())
                .AddTransient(_ => contextServices.GetService<ILoggerFactory>())
                .AddTransient(_ => contextServices.GetService<IMigrationsAssembly>())
                .AddTransient(_ => contextServices.GetService<IMigrationsIdGenerator>())
                .AddTransient(_ => contextServices.GetService<IMigrationsModelDiffer>())
                .AddTransient(_ => contextServices.GetService<IMigrator>())
                .AddTransient(_ => contextServices.GetService<IModel>());

        private IServiceCollection ConfigureUserServices(IServiceCollection services)
        {
            var designTimeServicesType = _startupAssembly.GetLoadableDefinedTypes()
                .Where(t => typeof(IDesignTimeServices).GetTypeInfo().IsAssignableFrom(t)).Select(t => t.AsType())
                .FirstOrDefault();

            return ConfigureDesignTimeServices(designTimeServicesType, services);
        }

        private Type GetProviderDesignTimeServices(string provider, bool throwOnError)
        {
            Assembly providerAssembly;
            try
            {
                providerAssembly = Assembly.Load(new AssemblyName(provider));
            }
            catch (Exception ex)
            {
                if (!throwOnError)
                {
                    return null;
                }

                throw new OperationException(DesignStrings.CannotFindRuntimeProviderAssembly(provider), ex);
            }

            var providerServicesAttribute = providerAssembly.GetCustomAttribute<DesignTimeProviderServicesAttribute>();
            if (providerServicesAttribute == null)
            {
                if (!throwOnError)
                {
                    return null;
                }

                throw new InvalidOperationException(
                    DesignStrings.CannotFindDesignTimeProviderAssemblyAttribute(
                        nameof(DesignTimeProviderServicesAttribute),
                        provider));
            }

            Assembly designTimeProviderAssembly;
            try
            {
                designTimeProviderAssembly = Assembly.Load(new AssemblyName(providerServicesAttribute.AssemblyName));
            }
            catch (Exception ex)
                when (ex is FileNotFoundException || ex is FileLoadException || ex is BadImageFormatException)
            {
                if (!throwOnError)
                {
                    return null;
                }

                throw new OperationException(
                    DesignStrings.CannotFindDesignTimeProviderAssembly(providerServicesAttribute.PackageName), ex);
            }

            return designTimeProviderAssembly.GetType(
                providerServicesAttribute.TypeName,
                throwOnError: true,
                ignoreCase: false);
        }

        private static IServiceCollection ConfigureDesignTimeServices(
            Type designTimeServicesType,
            IServiceCollection services)
        {
            if (designTimeServicesType != null)
            {
                var designTimeServices = (IDesignTimeServices)Activator.CreateInstance(designTimeServicesType);
                designTimeServices.ConfigureDesignTimeServices(services);
            }

            return services;
        }
    }
}
