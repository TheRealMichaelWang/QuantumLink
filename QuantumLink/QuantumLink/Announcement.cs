using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantumLink
{
    class Announcement
    {
        public string from;
        public string content;
        public DateTime posttime;

        public Announcement(string content, string from, DateTime posttime)
        {
            this.content = content;
            this.from = from;
            this.posttime = posttime;
        }

        public void Print()
        {
            if (posttime.Date == DateTime.Now.Date)
            {
                Console.WriteLine("[" + posttime.TimeOfDay + ",From: " + from + "]" + content);
            }
            else
            {
                Console.WriteLine("[" + posttime.Month + "/" + posttime.Day + "/" + posttime.Year + ",From: " + from + "]" + content);
            }
        }
    }
}
