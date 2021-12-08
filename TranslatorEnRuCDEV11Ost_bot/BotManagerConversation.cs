using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TranslatorEnRuCDEV11Ost_bot
{
    public class BotManagerConversation
    {
        private ITelegramBotClient botClient;
        private CreaterResponseMessage createrResponseMessage;
        private Dictionary<long, Conversation> chatList;

        public BotManagerConversation (ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            createrResponseMessage = new CreaterResponseMessage(botClient);
            chatList = new Dictionary<long, Conversation>();
        }

        public async Task Response(Update update)
        {
            Message message = update.Message;

            if (update.Type == UpdateType.CallbackQuery)
            {
                message = update.CallbackQuery.Message;
                message.Text = update.CallbackQuery.Data;
            }

            if (message != null)
            {
                var id = message.Chat.Id;

                if (!chatList.ContainsKey(id))
                {
                    chatList.Add(id, new Conversation(message.Chat));
                }
                Conversation chat = chatList[id];

                chat.AddMessage(message);

                await SendResponseMessage(chat, update);
            }
        }

        private async Task SendResponseMessage(Conversation chat, Update update)
        {
            await createrResponseMessage.MakeAnswer(chat, update);
        }
    }
}