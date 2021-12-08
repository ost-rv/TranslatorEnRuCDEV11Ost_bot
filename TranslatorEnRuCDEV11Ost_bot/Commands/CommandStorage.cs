using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TranslatorEnRuCDEV11Ost_bot.Commands
{
    public class CommandStorage
    {
        private List<IChatCommand> _commands;
        private AddingController addingController;

        public CommandStorage()
        {
            _commands = new List<IChatCommand>();
        }

        public void AddCommand(IChatCommand chatCommand)
        {
            _commands.Add(chatCommand);
            addingController = new AddingController();
        }

        public string GetMessageText(string message, Conversation chat)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as IChatTextCommand;

            return command.ReturnText();
        }

        #region KeyBoardCommand

        public bool IsButtonCommand(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message));

            return command is IKeyBoardCommand;
        }

        public InlineKeyboardMarkup GetKeyBoard(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;

            return command.ReturnKeyBoard();
        }

        public string GetInformationalMeggase(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;

            return command.InformationalMessage();
        }

        public void AddCallbackChat(string message, Conversation chat)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;
            command.AddCallbackChat(chat);
        }
        #endregion KeyBoardCommand

        public bool IsMessageCommand(string message)
        {
            return _commands.Exists(x => x.CheckMessage(message));
        }

        public bool IsTextCommand(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message));

            return command is IChatTextCommand;
        }

        public bool IsAddingCommand(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message));

            return command is AddWordCommand;
        }

        public void StartAddingWord(string message, Conversation chat)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as AddWordCommand;

            addingController.AddFirstState(chat);
            command.StartProcessAsync(chat);

        }

        public void NextStage(string message, Conversation chat)
        {
            var command = _commands.Find(x => x is AddWordCommand) as AddWordCommand;

            command.DoForStageAsync(addingController.GetStage(chat), chat, message);

            addingController.NextStage(message, chat);

        }

        public void ContinueTraining(string message, Conversation chat)
        {
            var command = _commands.Find(x => x is TrainingCommand) as TrainingCommand;

            command.NextStepAsync(chat, message);

        }

        public void HandleCallbackChat(CallbackQuery callbackQuery)
        {
            var command = _commands.Find(x => x is TrainingCommand) as TrainingCommand;
            command.Bot_CallbackChat(callbackQuery);

        }
    }
}
