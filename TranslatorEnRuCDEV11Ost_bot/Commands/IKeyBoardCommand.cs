using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TranslatorEnRuCDEV11Ost_bot.Commands
{    
    interface IKeyBoardCommand
    {
        InlineKeyboardMarkup ReturnKeyBoard();

        void AddCallbackChat(Conversation chat);

        void Bot_CallbackChat(CallbackQuery callbackQuery);

        string InformationalMessage();
    }
}
