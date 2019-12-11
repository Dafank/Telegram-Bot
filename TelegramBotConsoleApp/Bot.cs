using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace TelegramBotConsoleApp
{
    class Bot
    {
        public static string Name { get; } = "eztest_bot";
        private static TelegramBotClient TelegramBot = new TelegramBotClient("829802200:AAFQC4_3itLYxsOyoYI-UUIrO3KdZSCATsU");
        private static List<Command> commandsList;

        public void BotInit() 
        {
            commandsList = new List<Command>();
            commandsList.Add(new HelloCommand());
            TelegramBot.OnUpdate += TelegramBot_OnUpdate;
            TelegramBot.StartReceiving();
        }

        private void TelegramBot_OnUpdate(object sender, Telegram.Bot.Args.UpdateEventArgs e)
        {
            var message = e.Update.Message;
            if(e.Update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text) 
            {
                foreach (var command in commandsList) 
                {
                    if (command.Cointains(message.Text))
                    {
                        command.Execute(message, TelegramBot);
                        break;
                    }
                    else 
                    {
                        command.OnError(message, TelegramBot);
                    }
                }
            }
        }
    }
}
