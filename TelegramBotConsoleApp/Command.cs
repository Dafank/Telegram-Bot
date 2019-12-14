using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotConsoleApp
{
    /// <summary>
    /// Base class for all commands and has all needed properties for future command
    /// <code Cointains(string command)> Check is a command exist</code>
    /// </summary>
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract string Example { get; }
        public abstract string[] Args { get; set; } 
        public abstract void Execute(Message message, TelegramBotClient client);
        public abstract int CountArgs { get; }
        public virtual string Info { get; } = null;
        public bool Cointains(string command)
        {
            var splits = command.Split(' ');
            Args = splits.Skip(1).Take(splits.Count()).ToArray();
            return command.Contains(this.Name);
        }
    }
}
