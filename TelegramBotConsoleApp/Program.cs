using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramBotConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            bot.BotInit();
            Console.ReadKey();
        }
    }
}
