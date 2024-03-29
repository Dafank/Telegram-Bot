﻿using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace TelegramBotConsoleApp
{
    class HelloCommand : Command
    {
        public override string Name => "/hello";

        public override string Example => "/hello";
        public override int CountArgs => 0;
        public override string[] Args { get; set; }

        // Send a reply message which contain Hello!
        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            try
            {
                if (Args.Length > 0)
                    throw new Exception("Args is out");
                string msg = $"{DateTime.Now}: initials - '{message.Chat.FirstName} {message.Chat.LastName} @{message.Chat.Username}', chatId - '{message.Chat.Id}', message - \"{message.Text}\"";
                File.AppendAllText("Message.log", $"{msg}\n");
                await client.SendTextMessageAsync(chatId, "Hello!", replyToMessageId: messageId);
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
