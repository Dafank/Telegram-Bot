using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotConsoleApp
{
    class HelloCommand : Command
    {
        public override string Name => "/hello";

        public string Example { get; set; }
        public override int CountArgs => 0;
        public override string[] Args { get; set; }
        
        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatID = message.Chat.Id;
            var messageId = message.MessageId;
            await client.SendTextMessageAsync(chatID, "Hello!", replyToMessageId: messageId);
        }

        public override async void OnError(Message message, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(message.Chat.Id,"Something going wrong");
        }
    }
}
