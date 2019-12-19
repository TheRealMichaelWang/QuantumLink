using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantumLink
{
    class Message
    {
        public string from;
        public string content;
        public DateTime time;
        public bool read;
        public int server_index;

        public Message(string from , string content, DateTime time, bool read, int server_index)
        {
            this.from = from;
            this.content = content;
            this.time = time;
            this.read = read;
            this.server_index = server_index;
        }

        public void Print()
        {
            if(time.Date == DateTime.Now.Date)
            {
                Console.WriteLine("[" + time.TimeOfDay + ",From: " + from + "]" + content);
            }
            else
            {
                Console.WriteLine("[" + time.Month+"/"+time.Day+"/"+time.Year + ",From: " + from + "]" + content);
            }
        }
    }
}
