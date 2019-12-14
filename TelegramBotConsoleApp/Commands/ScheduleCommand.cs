using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace TelegramBotConsoleApp
{
    internal class ScheduleCommand : Command
    {
        public override string Name => "/schedule";
        JsonModel model;

        public override string[] Args { get; set; } = null;

        public override int CountArgs => 2;
        public override string Example => "/schedule [startdate] [enddate] or /schedule";
        /// <summary>
        /// if use /schedule [startdate] [enddate] get subjects between startdate and enddate
        /// or use /schedule get subjects on today
        /// </summary>
        public override async void Execute(Message message, TelegramBotClient client)
        {
            string answer = null;
            var chatId = message.Chat.Id;
            if (Args.Length != 0)
            {
                GetResponse(Args[0], Args[1]);
                if (model != null)
                {
                    var days = model.GetSubjectsInfo();
                    foreach (var day in days)
                    {
                        answer += $"{day.day} - {day.dayname}\n";
                        foreach (var subject in day.subjects)
                        {
                            answer += $"{subject.time}\n{subject.lecturer}\n {subject.subject}\n {subject.type}, {subject.classroom} ауд.\n";
                        }
                        answer += new string('-', 10) + "\n";
                    }
                }
                else
                {
                    answer += "Пар немає!!!! ЮХУУУУУУУ!!!";
                }

            }
            else
            {
                GetResponse(DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString());
                if (model != null)
                {
                    var days = model.GetSubjectsInfo();
                    foreach (var day in days)
                    {
                        answer += $"{day.day} - {day.dayname}\n";
                        foreach (var subject in day.subjects)
                        {
                            answer += $"{subject.time}\n{subject.lecturer}\n {subject.subject}\n {subject.type}, {subject.classroom} ауд.\n";
                        }
                        answer += new string('-', 10) + "\n";
                    }
                }
                else
                {
                    answer += "Сьогодні пар немає!!!! ЮХУУУУУУУ!!!";
                }

            }


            try
            {
                if (Args.Length > 3)
                    throw new Exception("Args out");
                string msg = $"{DateTime.Now}: initials - '{message.Chat.FirstName} {message.Chat.LastName} @{message.Chat.Username}', chatId - '{message.Chat.Id}', message - \"{message.Text}\"";
                File.AppendAllText("Message.log", $"{msg}\n");
                await client.SendTextMessageAsync(chatId, answer);
            }
            catch (Exception e)
            {
                string errmsg = $"{DateTime.Now}: initials - '{message.Chat.FirstName} {message.Chat.LastName} @{message.Chat.Username}', chatId - '{message.Chat.Id}', message - \"{message.Text}\", error - '{e.Message}' path - '{e.StackTrace}'";
                File.AppendAllText("Error.log", $"{errmsg}\n");
                await client.SendTextMessageAsync(chatId, $"Something going wrong");
            }

        }


        private void GetResponse(string start, string end)
        {
            var pattern = new Regex("^([0]?[0-9]|[12][0-9]|[3][01])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$");
            try
            {
                if (pattern.IsMatch(start) & pattern.IsMatch(end))
                {
                    string URL = $"http://calc.nuwm.edu.ua:3002/api/sched?group=%D0%9A%D0%9D-41%D1%96%D0%BD%D1%82&sdate={start}&edate={end}&type=weeks";
                    var webRequest = WebRequest.Create(URL) as HttpWebRequest;
                    if (webRequest == null)
                    {
                        return;
                    }

                    webRequest.ContentType = "application/json";
                    webRequest.UserAgent = "Nothing";

                    using (var s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (var sr = new StreamReader(s))
                        {
                            var ShcheduleJson = sr.ReadToEnd();
                            model = new JsonModel(ShcheduleJson);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}