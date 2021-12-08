using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorEnRuCDEV11Ost_bot.Commands
{
    public class AbstractCommand : IChatCommand
    {
        public string CommandText;
        public virtual bool CheckMessage(string message)
        {
            return CommandText == message;
        }
    }
}
