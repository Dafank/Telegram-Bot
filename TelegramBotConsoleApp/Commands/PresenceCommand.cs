using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace TelegramBotConsoleApp
{
    // присутність
    class PresenceCommand : Command
    {
        GroupPresence group;
        public PresenceCommand(GroupPresence group)
        {
            this.group = group;
        }
        public override string Name => "/presence";

        public override string[] Args { get; set; }

        public override int CountArgs => 1;

        private string subject = null;

        public override string Example => "/presence [subject]";

        /// <summary>
        /// if use /presence [subject] create list with buttons
        /// or use /presence create text with results
        /// </summary>
        public override async void Execute(Message message, TelegramBotClient client)
        {
            var inlineKeybord = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Присутній","Присутній"),
                    InlineKeyboardButton.WithCallbackData("Н/Б","Н/Б")
                }
            });
            var chatId = message.Chat.Id;
            if (group.Presence != null & group.Presence.Count != 11 & group.Presence.Count < 11)
            {
                group.Presence.Clear();

                try
                {
                    if (Args.Length > 2)
                        throw new Exception("Args out");
                    subject = Args[0];
                    await client.SendTextMessageAsync(chatId, $"{Args[0]}\n");
                    foreach (var classmate in group.Group)
                    {
                        await client.SendTextMessageAsync(chatId, $"{classmate}", replyMarkup: inlineKeybord);
                    }
                    string msg = $"{DateTime.Now}: initials - '{message.Chat.FirstName} {message.Chat.LastName} @{message.Chat.Username}', chatId - '{message.Chat.Id}', message - \"{message.Text}\"";
                    File.AppendAllText("Message.log", $"{msg}\n");
                }
                catch (Exception e)
                {
                    string errmsg = $"{DateTime.Now}: initials - '{message.Chat.FirstName} {message.Chat.LastName} @{message.Chat.Username}', chatId - '{message.Chat.Id}', message - \"{message.Text}\", error - '{e.Message}' path - '{e.StackTrace}'";
                    File.AppendAllText("Error.log", $"{errmsg}\n");
                    await client.SendTextMessageAsync(chatId, $"Enter Command properly '{this.Example}'");
                }
            }
            else
            {

                string s = null;
                s += $"{subject} - {DateTime.Now.ToShortDateString()}\n";
                for (int i = 0; i < group.Presence.Count; i++)
                {
                    s += $"{group.Group[i]} - {group.Presence[i]}\n";
                }
                try
                {
                    if (Args.Length > 2)
                        throw new Exception("Args out");
                    else if (group.Presence.Count < 11)
                        new Exception("Fill all column");
                    string msg = $"{DateTime.Now}: initials - '{message.Chat.FirstName} {message.Chat.LastName} @{message.Chat.Username}', chatId - '{message.Chat.Id}', message - \"{message.Text}\"";
                    File.AppendAllText("Message.log", $"{msg}\n");
                    await client.SendTextMessageAsync(chatId, s);
                }
                catch (Exception e)
                {
                    string errmsg = $"{DateTime.Now}: initials - '{message.Chat.FirstName} {message.Chat.LastName} @{message.Chat.Username}', chatId - '{message.Chat.Id}', message - \"{message.Text}\", error - '{e.Message}' path - '{e.StackTrace}'";
                    File.AppendAllText("Error.log", $"{errmsg}\n");
                    await client.SendTextMessageAsync(chatId, $"Something going wrong");
                }
            }
        }
    }
}
