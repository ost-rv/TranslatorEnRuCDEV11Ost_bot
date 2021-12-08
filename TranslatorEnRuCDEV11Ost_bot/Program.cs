using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TranslatorEnRuCDEV11Ost_bot
{
    class Program
    {

        static void Main(string[] args)
        {
            BotWatcher bot = new BotWatcher();

            bot.Initialize();
            bot.Start();

            Console.WriteLine("Введите stop для прекращения работы");

            string command;

            do
            {
                command = Console.ReadLine();
            }
            while (command != "stop");

            bot.Stop(); 
        }
    }
}
