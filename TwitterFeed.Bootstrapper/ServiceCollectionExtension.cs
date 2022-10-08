using Microsoft.Extensions.DependencyInjection;

namespace TwitterFeed.Bootstrapper
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddConsole(this IServiceCollection @this,IServiceCollectionConfigurationManager serviceCollectionConfigurationManager = null)
        {
            serviceCollectionConfigurationManager = serviceCollectionConfigurationManager == null ? new DefaultServiceCollectionConfigurationManager() : serviceCollectionConfigurationManager;
            serviceCollectionConfigurationManager.RegisterTwitterService(@this)
                                                 .RegisterRepositories(@this)
                                                 .RegisterConfigs(@this)
                                                 .RegisterTelegramService(@this);
            return @this;
        }

        public static IServiceCollection AddWindowsForm(this IServiceCollection @this, IServiceCollectionConfigurationManager serviceCollectionConfigurationManager = null)
        {
            serviceCollectionConfigurationManager = serviceCollectionConfigurationManager == null ? new DefaultServiceCollectionConfigurationManager() : serviceCollectionConfigurationManager;
            serviceCollectionConfigurationManager.RegisterTwitterService(@this)
                                                 .RegisterRepositories(@this)
                                                 .RegisterConfigs(@this)
                                                 .RegisterTelegramService(@this);
            return @this;
        }
    }
}