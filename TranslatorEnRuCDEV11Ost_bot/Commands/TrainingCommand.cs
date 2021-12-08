using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using TranslatorEnRuCDEV11Ost_bot.Enumeration;

namespace TranslatorEnRuCDEV11Ost_bot.Commands
{
    public class TrainingCommand : AbstractCommand, IKeyBoardCommand
    {

        private ITelegramBotClient botClient;

        private Dictionary<long, TrainingType> training;

        private Dictionary<long, Conversation> trainingChats;

        private Dictionary<long, string> activeWord;

        public TrainingCommand(ITelegramBotClient botClient)
        {
            CommandText = "/training";

            this.botClient = botClient;

            training = new Dictionary<long, TrainingType>();
            trainingChats = new Dictionary<long, Conversation>();
            activeWord = new Dictionary<long, string>();
        }

        public InlineKeyboardMarkup ReturnKeyBoard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton("С русского на английский")
                {
                    CallbackData = "rustoeng"
                },

                new InlineKeyboardButton("С английского на русский")
                {
                    CallbackData = "engtorus"
                }
            };

            var keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }

        public string InformationalMessage()
        {
            return "Выберите тип тренировки. Для окончания тренировки введите команду /stop";
        }

        public void AddCallbackChat(Conversation chat)
        {
            trainingChats.Add(chat.GetId(), chat);
        }

        public async void Bot_CallbackChat(CallbackQuery callbackQuery)
        {
            var text = "";

            var id = callbackQuery.Message.Chat.Id;

            var chat = trainingChats[id];
            try
            {
                switch (callbackQuery.Data)
                {
                    case "rustoeng":
                        training.Add(id, TrainingType.RusToEng);

                        text = chat.GetTrainingWord(TrainingType.RusToEng);

                        break;
                    case "engtorus":
                        training.Add(id, TrainingType.EngToRus);

                        text = chat.GetTrainingWord(TrainingType.EngToRus);
                        break;
                    default:
                        break;
                }
                chat.IsTraningInProcess = true;
                activeWord.Add(id, text);

                if (trainingChats.ContainsKey(id))
                {
                    trainingChats.Remove(id);
                }
            }
            catch(Exception ex)
            {
                text = ex.Message;
                chat.IsTraningInProcess = false;
            }



            await botClient.SendTextMessageAsync(id, text);
            //await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        }

        public async void NextStepAsync(Conversation chat, string message)
        {
            var type = training[chat.GetId()];
            var word = activeWord[chat.GetId()];

            var check = chat.CheckWord(type, word, message);

            var text = "";

            if (check)
            {
                text = "Правильно!";
            }
            else
            {
                text = "Неправильно!";
            }

            text = text + " Следующее слово: ";

            var newword = chat.GetTrainingWord(type);

            text = text + newword;

            activeWord[chat.GetId()] = newword;


            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }
    }
}