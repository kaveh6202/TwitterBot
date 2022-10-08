namespace TwitterFeed.Infra.Models;

public class ConfigurationModel
{

    public TelegramConfigModel TelegramConfig { get; set; } = new TelegramConfigModel();
    public TwitterConfigModel TwitterConfig { get; set; } = new TwitterConfigModel();


    public class TwitterConfigModel
    {
        public string ApiKey { get; set; } = "eGGebfYneFew9f3cuTxtDq6qx";
        public string KeySecret { get; set; } = "z0nzWbQWs8WWC0t8A8l4py08HPDkNbVfIBrPQlZ0GLYIROU5nG";
        public string BearerKey { get; set; } = "AAAAAAAAAAAAAAAAAAAAAMcchwEAAAAA8hESSgCSqNZBiv%2B%2BH4B3HK8cYOU%3Dx79Yk5Xm84Hg1VxZwOtM3HXQtfhVArrrBlKQuGmjHCi8ljqFGv";
        public string AccessToken { get; set; } = "231389715-5Ug4N4BjjmOHu0SsQiKyJbhp9vALgJFL0xKjsa5y";
        public string AccessTokenSecret { get; set; } = "C0TvJIXtj4JUWiFxde2bvIbzBOQSOudrHxLMkI2pIMW1U";
    }
    public class TelegramConfigModel
    {
        public string ManagementBotApiKey { get; set; } = "5702498429:AAGDFi6bEJdM7KZ0uAjwQ6bYgRqhaczpytg";
        public long BaroadcastChannelId { get; set; } = -1001568486021;
        //public long BaroadcastChannelId { get; set; } = -1001843369099;
    }
}