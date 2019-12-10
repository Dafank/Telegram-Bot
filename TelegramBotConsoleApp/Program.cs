using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramBotConsoleApp
{
    class Program
    {
        static TelegramBotClient bot;

        static List<BotCommand> commands = new List<BotCommand>();

        static void Main(string[] args)
        {
            bot = new TelegramBotClient("829802200:AAFQC4_3itLYxsOyoYI-UUIrO3KdZSCATsU");

            commands.Add(new BotCommand
            {

                Command = "/hello",
                CountArgs = 0,
                Example = "/hello",
                Execute = async (model, update) =>
                {
                    await bot.SendTextMessageAsync(update.Message.From.Id, "Hello");
                },
                OnError = async (model, update) =>
                {
                    await bot.SendTextMessageAsync(update.Message.From.Id, "Something going wrong");

                }
            }
            );

            Run().Wait();
            Console.ReadKey();
        }



        static async Task Run()
        {
            var offset = 0;

            while (true)
            {
                var updates = await bot.GetUpdatesAsync(offset);

                foreach (var update in updates)
                {
                    if (update.Message.Type == MessageType.Text)
                    {
                        var model = BotCommand.Parse(update.Message.Text);

                        if (model != null)
                        {
                            foreach (var cmd in commands)
                            {
                                if (cmd.Command == model.Command)
                                {
                                    cmd.Execute?.Invoke(model, update);
                                }
                                else
                                {
                                    cmd.OnError?.Invoke(model, update);
                                }
                            }
                        }
                        else
                        {
                            await bot.SendTextMessageAsync(update.Message.From.Id, "That is not a command");
                        }

                    }
                    offset = update.Id + 1;
                }
                Task.Delay(500).Wait();
            }
        }
    }
}
