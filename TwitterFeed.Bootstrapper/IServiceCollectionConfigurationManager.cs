namespace TwitterFeed.Bootstrapper;

public interface IServiceCollectionConfigurationManager
{
    IServiceCollectionConfigurationManager RegisterTwitterService(IServiceCollection services);
    IServiceCollectionConfigurationManager RegisterTelegramService(IServiceCollection services);
    IServiceCollectionConfigurationManager RegisterRepositories(IServiceCollection services);
    IServiceCollectionConfigurationManager RegisterConfigs(IServiceCollection services);


}
