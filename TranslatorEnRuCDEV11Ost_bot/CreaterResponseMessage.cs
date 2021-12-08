using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TranslatorEnRuCDEV11Ost_bot.Commands;

namespace TranslatorEnRuCDEV11Ost_bot
{
    public class CreaterResponseMessage
    {
        private ITelegramBotClient botClient;
        private CommandStorage commandStorage;


        public CreaterResponseMessage(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            commandStorage = new CommandStorage();

            RegisterCommands();
        }

        private void RegisterCommands()
        {
            commandStorage.AddCommand(new AddWordCommand(botClient));
            commandStorage.AddCommand(new DeleteWordCommand());
            commandStorage.AddCommand(new TrainingCommand(botClient));
            commandStorage.AddCommand(new StopTrainingCommand());
        }

        public async Task MakeAnswer(Conversation chat, Update update)
        {
            var lastMessage = chat.GetLastMessage();

            if (chat.IsTraningInProcess && !commandStorage.IsTextCommand(lastMessage))
            {
                commandStorage.ContinueTraining(lastMessage, chat);

                return;
            }

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                commandStorage.HandleCallbackChat(update.CallbackQuery);
                return;
            }

            if (chat.IsAddingInProcess)
            {
                commandStorage.NextStage(lastMessage, chat);
                return;
            }

            if (commandStorage.IsMessageCommand(lastMessage))
            {
                await ExecCommand(chat, lastMessage, update);
            }
            else
            {
                var text = CreateTextMessage();

                await SendText(chat, text);
            }
        }

        public async Task ExecCommand(Conversation chat, string command, Update update)
        {
            if (commandStorage.IsTextCommand(command))
            {
                var text = commandStorage.GetMessageText(command, chat);

                await SendText(chat, text);
            }

            if (commandStorage.IsButtonCommand(command))
            {
                var keys = commandStorage.GetKeyBoard(command);
                var text = commandStorage.GetInformationalMeggase(command);
                commandStorage.AddCallbackChat(command, chat);

                await SendTextWithKeyBoard(chat, text, keys);

            }
           
            if(commandStorage.IsAddingCommand(command))
            {
                chat.IsAddingInProcess = true; 
                commandStorage.StartAddingWord(command, chat);
            }
        }

        public string CreateTextMessage()
        {
            var text = "Not a command";

            return text;
        }

        private async Task SendText(Conversation chat, string text)
        {

            await botClient.SendTextMessageAsync(
                chatId: chat.GetId(),
                text: text);
        }

        private async Task SendTextWithKeyBoard(Conversation chat,
            string text,
            InlineKeyboardMarkup keyboard)
        {
            await botClient.SendTextMessageAsync(
            chatId: chat.GetId(),
            text: text,
            replyMarkup: keyboard);
        }
    }
}
