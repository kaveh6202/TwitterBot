using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using TwitterFeed.Infra.Interface;
using TwitterFeed.Infra.Models;

namespace TwitterFeed.Infra.Impl;

public class TwitterSyncer : ITwitterSyncer
{
    private readonly ITwitterBaseService _twitter;
    private readonly ConfigurationModel _config;
    private readonly ITelegramBotClient _telegramBot;
    private readonly ILogger<TwitterSyncer> _logger;

    public TwitterSyncer(ITwitterBaseService twitter, ILogger<TwitterSyncer> logger, ITelegramBotClient telegramBot, ConfigurationModel config)
    {
        _twitter = twitter;
        _logger = logger;
        _telegramBot = telegramBot;
        _config = config;
    }

    public async Task SyncTwitterTimelineToTelegram()
    {
        while (true)
        {
            _logger.LogInformation($"{DateTime.Now} Starting Loop");
            var nextTime = DateTime.Now.AddSeconds(90);
            try
            {
                var tweetDetails = await _twitter.GetTimeline();

                foreach (var tweetDetail in tweetDetails)
                {
                    switch (tweetDetail.Type)
                    {
                        case CustomTwitterModel.TweetType.Text:
                            await _telegramBot.SendTextMessageAsync(_config.TelegramConfig.BaroadcastChannelId, tweetDetail.TweetText, parseMode: ParseMode.Html, disableWebPagePreview: true);
                            break;
                        case CustomTwitterModel.TweetType.Photo:
                            var photo = new InputOnlineFile(tweetDetail.MediaLink);
                            await _telegramBot.SendPhotoAsync(_config.TelegramConfig.BaroadcastChannelId, photo, tweetDetail.TweetText, parseMode: ParseMode.Html);
                            break;
                        case CustomTwitterModel.TweetType.Video:
                            var video = new InputOnlineFile(tweetDetail.MediaLink);
                            await _telegramBot.SendVideoAsync(_config.TelegramConfig.BaroadcastChannelId, video, caption: tweetDetail.TweetText, parseMode: ParseMode.Html);
                            break;
                    }
                    await Task.Delay(5000);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Syncing Twitter Timeline to telegram channel");
            }
            finally
            {
                if (DateTime.Now < nextTime)
                {
                    await Task.Delay((int)nextTime.Subtract(DateTime.Now).TotalMilliseconds);
                }
            }
        }
    }
}