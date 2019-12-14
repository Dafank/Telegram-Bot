using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace TelegramBotConsoleApp
{
    class ResourceCommand : Command
    {
        public override string Name => "/resource";

        public override string Example => "/resource";

        public override string[] Args { get; set; }

        public override int CountArgs => 0;

        //create 3 buttons which redirect to some sites
        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            try
            {
                var inlineKeybord = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithUrl("Розклад","http://desk.nuwm.edu.ua/cgi-bin/timetable.cgi")
                },
                new[]
                {
                    InlineKeyboardButton.WithUrl("Moodle","https://exam.nuwm.edu.ua/")
                },
                new[]
                {
                    InlineKeyboardButton.WithUrl("Кросс-плат програмування","https://iktmedia.moodlecloud.com/")
                }
            });
                string msg = $"{DateTime.Now}: initials - '{message.Chat.FirstName} {message.Chat.LastName} @{message.Chat.Username}', chatId - '{message.Chat.Id}', message - \"{message.Text}\"";
                File.AppendAllText("Message.log", $"{msg}\n");
                await client.SendTextMessageAsync(chatId, "Ресурси по воднику:\n", replyMarkup: inlineKeybord);
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
