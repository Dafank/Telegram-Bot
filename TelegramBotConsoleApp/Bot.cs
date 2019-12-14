using System;
using System.Collections.Generic;
using Telegram.Bot;
using System.Linq;
using File = System.IO.File;
using System.IO;

namespace TelegramBotConsoleApp
{
    /// <summary>
    /// Class which is a bot concept
    /// </summary>
    class Bot
    {
        public static string Name { get; } = "eztest_bot";// Bot name
        private readonly TelegramBotClient TelegramBot;// Create bot client
        private static List<Command> commandsList;
        public static IReadOnlyList<Command> Commands;
        GroupPresence group; // Group of classmates
        // Initial group of classmates and create bot client
        public Bot()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectpath = Directory.GetParent(Directory.GetParent(workingDirectory).Parent.FullName).FullName;
            string path = projectpath + @"\token.txt";
            string token = File.ReadAllText(path);
            TelegramBot = new TelegramBotClient(token);
            group = new GroupPresence();
        }

        /// <summary>
        /// Add command in list, add event precessing and starting bot
        /// </summary>
        public void BotInit()
        {
            commandsList = new List<Command>();
            commandsList.Add(new HelloCommand());
            commandsList.Add(new InfoCommand());
            commandsList.Add(new ScheduleCommand());
            commandsList.Add(new PresenceCommand(group));
            commandsList.Add(new ResourceCommand());
            Commands = commandsList.AsReadOnly();
            TelegramBot.OnMessage += TelegramBot_OnMessage; ;
            TelegramBot.OnCallbackQuery += TelegramBot_OnCallbackQuery;
            TelegramBot.StartReceiving();
        }

        // Get all messages which bot receiving
        private void TelegramBot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            try
            {
                if (e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                    throw new Exception("Message is not a text");
                var result = commandsList.Single(s => s.Cointains(message.Text) == true);
                result.Execute(message, TelegramBot);
            }
            catch (Exception exep)
            {
                string errmsg = $"{DateTime.Now}: initials - '{message.Chat.FirstName} {message.Chat.LastName} @{message.Chat.Username}', chatId - '{message.Chat.Id}', message - \"{message.Text}\", error - '{exep.Message}' path - '{exep.StackTrace}'";
                File.AppendAllText("Error.log", $"{errmsg}\n");
                TelegramBot.SendTextMessageAsync(message.Chat.Id, "Please enter text or command not found!");
            }

        }

        // Get some info from CallBack buttons and add status for Presence list
        private void TelegramBot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var message = e.CallbackQuery.Message;
            try
            {
                group.Presence.Add(e.CallbackQuery.Data);
            }
            catch (Exception exep)
            {
                string errmsg = $"{DateTime.Now}: initials - '{message.Chat.FirstName} {message.Chat.LastName} @{message.Chat.Username}', chatId - '{message.Chat.Id}', message - \"{message.Text}\", error - '{exep.Message}' path - '{exep.StackTrace}'";
                File.AppendAllText("Error.log", $"{errmsg}\n");
            }

        }
    }
}
