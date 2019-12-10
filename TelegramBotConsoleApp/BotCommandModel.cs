using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBotConsoleApp
{
    class BotCommandModel
    {
        public string Command { get; set; }
        public string[] Args { get; set; }
    }
}
