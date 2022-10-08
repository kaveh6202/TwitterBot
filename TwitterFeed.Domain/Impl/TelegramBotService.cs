using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TwitterFeed.Infra.Interface;

namespace TwitterFeed.Infra.Impl;

public class TelegramBotService : ITelegramBotService
{
    private readonly ITelegramBotClient _telegramBot;
    public async Task StartBot()
    {
        try
        {
            var m = _telegramBot.SendTextMessageAsync(1306163491, "Bot Is Starting").Result;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };
            _telegramBot.StartReceiving(updateHandler: HandleUpdateAsync, pollingErrorHandler: HandlePollingErrorAsync
                , receiverOptions: receiverOptions, cancellationToken: new CancellationToken());

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                // Only process Message updates: https://core.telegram.org/bots/api#message
                if (update.Message is not { } message)
                    return;
                // Only process text messages
                if (message.Text is not { } messageText)
                    return;

                var chatId = message.Chat.Id;

                if (update.Message.Text.ToUpper() == "/START")
                {
                    var welcomeText = $@"درود دوستان
توی این بات آخرین اطلاعات توییتر از کف خیابون ارسال میشه
لطفاً موارد امنیتی رو رعایت کنین قبل ارسال به دوستان
";


                    // Echo received message text
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: welcomeText,
                        cancellationToken: cancellationToken);
                }
                else if (update.Message.Text.ToUpper() == "/ADD")
                {

                }
                else if (update.Message.Text.ToUpper() == "/REMOVE")
                {

                }
            }

            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
