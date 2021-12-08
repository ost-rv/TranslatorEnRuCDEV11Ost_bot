using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TranslatorEnRuCDEV11Ost_bot
{
    public class BotWatcher
    {
        private ITelegramBotClient botClient;
        private BotManagerConversation managerConversation;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationToken cancellationToken;
        


        public void Initialize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
            cts = new CancellationTokenSource();
            cancellationToken = cts.Token;
            managerConversation = new BotManagerConversation(botClient);
            
        }

        public void Start()
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[] {  } // receive all update types
            };
            try
            {
                botClient.StartReceiving(
                    HandleUpdateAsync,
                    HandleErrorAsync,
                    receiverOptions,
                    cancellationToken
                );
            }
            catch (OperationCanceledException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public void Stop()
        {
            cts.Cancel();
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await managerConversation.Response(update);
        }

        async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ApiRequestException apiRequestException)
            {
                await botClient.SendTextMessageAsync(123, apiRequestException.ToString());
            }
        }
    }
}
