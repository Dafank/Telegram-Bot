using System;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace TelegramBotConsoleApp
{
    internal class InfoCommand : Command
    {
        public override string Name => "/info";

        public override string[] Args { get; set; }

        public override int CountArgs => 0;

        public override string Example => "/info";

        // Get all exist commands pattern
        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            try
            {

                string answer = null;
                for (int i = 0; i < Bot.Commands.Count; i++)
                {
                    if (Bot.Commands[i].Info != null)
                        answer += $"{Bot.Commands[i].Example} - {Bot.Commands[i].Info}\n";
                    else
                        answer += $"{Bot.Commands[i].Example}\n";
                }
                if (Args.Length > 0)
                    throw new Exception("Args is out");
                string msg = $"{DateTime.Now}: initials - '{message.Chat.FirstName} {message.Chat.LastName} @{message.Chat.Username}', chatId - '{message.Chat.Id}', message - \"{message.Text}\"";
                File.AppendAllText("Message.log", $"{msg}\n");
                await client.SendTextMessageAsync(chatId, $"There are all existing commands\n{answer}");
            }
            catch (Exception e)
            {
                string errmsg = $"{DateTime.Now}: initials - '{message.Chat.FirstName} {message.Chat.LastName} @{message.Chat.Username}', chatId - '{message.Chat.Id}', message - \"{message.Text}\", error - '{e.Message}' path - '{e.StackTrace}'";
                File.AppendAllText("Error.log", $"{errmsg}\n");
                await client.SendTextMessageAsync(chatId, $"Enter Command properly '{this.Example}'");
            }
        }
    }
}