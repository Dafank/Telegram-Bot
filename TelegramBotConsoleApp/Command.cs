using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotConsoleApp
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract string[] Args { get; set; }
        public abstract void Execute(Message message, TelegramBotClient client);
        public abstract void OnError(Message message, TelegramBotClient client);
        public abstract int CountArgs { get; }
        public bool Cointains(string command)
        {
            var splits = command.Split(' ');
            Args = splits.Skip(1).Take(splits.Count()).ToArray();
            return command.Contains(this.Name);
        }
    }
}
