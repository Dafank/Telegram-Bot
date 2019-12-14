using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Path = System.IO.Path;

namespace TelegramBotConsoleApp
{
    /// <summary>
    /// Simple list Initialization
    /// <code List<string> Group> list of groupmates</code>  
    /// <code List<string> Presence> list of present status which respond for each groupmates</code>
    /// </summary>
    class GroupPresence
    {
        public List<string> Group { get; set; }
        public List<string> Presence { get; set; }

        // Initials all lists and read groupmates from file
        public GroupPresence()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectpath = Directory.GetParent(Directory.GetParent(workingDirectory).Parent.FullName).FullName;
            string path = projectpath + @"\group.txt";
            string[] groupmates = File.ReadAllLines(path);
            Group = new List<string>();
            Presence = new List<string>();
            Group.AddRange(groupmates);
        }
    }
}
