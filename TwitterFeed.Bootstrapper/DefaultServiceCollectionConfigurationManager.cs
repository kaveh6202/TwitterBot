using Telegram.Bot;

namespace TwitterFeed.Bootstrapper
{
    internal class DefaultServiceCollectionConfigurationManager : IServiceCollectionConfigurationManager
    {
        public IServiceCollectionConfigurationManager RegisterConfigs(IServiceCollection services)
        {

            services.AddTransient((item) => GetTweetFilterConfig());
            services.AddTransient((item) => GetConfig());
            return this;
        }

        public IServiceCollectionConfigurationManager RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IRepository, Repository>();
            return this;
        }

        public IServiceCollectionConfigurationManager RegisterTwitterService(IServiceCollection services)
        {
            services.AddTransient<ITwitterBaseService, TwitterBaseService>();
            services.AddTransient<ITwitterSyncer, TwitterSyncer>();

            var cnf = GetConfig();
            services.AddTransient<ITwitterClient, TwitterClient>((item) => new TwitterClient(cnf.TwitterConfig.ApiKey, cnf.TwitterConfig.KeySecret, cnf.TwitterConfig.AccessToken, cnf.TwitterConfig.AccessTokenSecret));
            return this;
        }

        public IServiceCollectionConfigurationManager RegisterTelegramService(IServiceCollection services)
        {
            var cnf = GetConfig();
            services.AddTransient<ITelegramBotClient, TelegramBotClient>((item) => new TelegramBotClient(cnf.TelegramConfig.ManagementBotApiKey));
            return this;
        }

        private ConfigurationModel GetConfig()
        {
            var authConfigRaw = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Files", "Credential.json"));
            var authConfig = JsonConvert.DeserializeObject<ConfigurationModel>(authConfigRaw);
            return authConfig;
        }

        private FilterTweetsModel GetTweetFilterConfig()
        {
            var tweetFilterRaw = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Files", "FilterTwitter.json"));
            var tweetFilter = JsonConvert.DeserializeObject<FilterTweetsModel>(tweetFilterRaw);
            tweetFilter.ContainsAccounts = tweetFilter.ContainsAccounts.Select(i => i.ToUpper());
            tweetFilter.ExcludeAccounts = tweetFilter.ExcludeAccounts.Select(i => i.ToUpper());
            return tweetFilter;
        }
    }
}
