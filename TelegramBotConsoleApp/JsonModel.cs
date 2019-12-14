using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace TelegramBotConsoleApp
{
    /// <summary>
    /// All this classes are objects which based on JSON responce
    /// <seealso cref="Subject"/>
    /// <seealso cref="Day"/>
    /// <seealso cref="Schedule"/>
    /// <seealso cref="Response"/>
    /// <seealso cref="RootObject"/>
    /// </summary>
    public class Subject
    {
        public string lecturer { get; set; }
        public string subgroup { get; set; }
        public string streams_type { get; set; }
        public int lessonNum { get; set; }
        public string time { get; set; }
        public string classroom { get; set; }
        public string subject { get; set; }
        public string type { get; set; }
    }

    public class Day
    {
        public List<Subject> subjects { get; set; }
        public string day { get; set; }
        public string dayname { get; set; }
        public int day_of_week { get; set; }
        public int day_of_year { get; set; }
    }

    public class Schedule
    {
        public List<Day> days { get; set; }
        public int weeknum { get; set; }
        public string weekstart { get; set; }
        public string weekend { get; set; }
    }

    public class Response
    {
        public List<Schedule> schedule { get; set; }
    }

    public class RootObject
    {
        public int code { get; set; }
        public bool cache { get; set; }
        public object error { get; set; }
        public Response response { get; set; }
    }

    /// <summary>
    /// Get make object model which based on JSON responce
    /// </summary>
    class JsonModel
    {
        RootObject root;
        public JsonModel(string str)
        {
            root =new RootObject();
            root = JsonConvert.DeserializeObject<RootObject>(str);
        }

        // Return list of day, which contain all info about subjects
        public IEnumerable<Day> GetSubjectsInfo() 
        {
            var query = from sched in root.response.schedule
                        from days in sched.days
                        select days;
            return query;
        }
    }
}
