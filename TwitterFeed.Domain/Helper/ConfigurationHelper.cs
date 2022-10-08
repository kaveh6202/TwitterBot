using TwitterFeed.Infra.Models;

namespace TwitterFeed.Infra.Helper;

public static class ConfigurationHelper
{
    public static ConfigurationModel GetConfig()
    {
        return new ConfigurationModel()
        {
            TelegramConfig = new ConfigurationModel.TelegramConfigModel()
            {
                BaroadcastChannelId = -1001568486021,
                ManagementBotApiKey = "5702498429:AAGDFi6bEJdM7KZ0uAjwQ6bYgRqhaczpytg"
            },
            TwitterConfig = new ConfigurationModel.TwitterConfigModel()
            {
                AccessToken = "231389715-5Ug4N4BjjmOHu0SsQiKyJbhp9vALgJFL0xKjsa5y",
                AccessTokenSecret = "C0TvJIXtj4JUWiFxde2bvIbzBOQSOudrHxLMkI2pIMW1U",
                ApiKey = "eGGebfYneFew9f3cuTxtDq6qx",
                BearerKey = "AAAAAAAAAAAAAAAAAAAAAMcchwEAAAAA8hESSgCSqNZBiv%2B%2BH4B3HK8cYOU%3Dx79Yk5Xm84Hg1VxZwOtM3HXQtfhVArrrBlKQuGmjHCi8ljqFGv",
                KeySecret = "z0nzWbQWs8WWC0t8A8l4py08HPDkNbVfIBrPQlZ0GLYIROU5nG"
            }
        };
    }
}