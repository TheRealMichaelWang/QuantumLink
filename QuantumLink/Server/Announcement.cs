using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Announcement
    {
        public string from;
        public string content;
        public DateTime posttime;
        public int index;

        public Announcement(string content, string from, DateTime posttime, int index)
        {
            this.content = content;
            this.from = from;
            this.posttime = posttime;
            this.index = index;
        }
    }
}
