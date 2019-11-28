using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Message
    {
        public string from;
        public string to;
        public string content;
        public bool read;
        public DateTime time;

        public Message(string from, string to, string content, DateTime time, bool read)
        {
            this.from = from;
            this.to = to;
            this.content = content;
            this.time = time;
            this.read = read;
        }

        public static int CountUnread(string username)
        {
            int unread = 0;
            foreach(Message message in Program.messages)
            {
                if(message.to == username)
                {
                    if(!message.read)
                    {
                        unread++;
                    }
                }
            }
            return unread;
        }

        public static void DeleteMessages(string username)
        {
            List<Message> todelete = new List<Message>();
            foreach (Message message in Program.messages)
            {
                if (message.to == username)
                {
                    todelete.Add(message);
                }
            }
            foreach(Message message in todelete)
            {
                Program.messages.Remove(message);
            }
            Program.SaveMessages();
        }

        public static int CountMessages(string username)
        {
            int count = 0;
            foreach(Message message in Program.messages)
            {
                if(message.to == username)
                {
                    count++;
                }
            }
            return count;
        }

        public static bool MarkRead(string username,int index)
        {
            int count = 0;
            foreach(Message message in Program.messages)
            {
                if(message.to == username)
                {
                    if(count == index)
                    {
                        message.read = true;
                        Program.SaveMessages();
                        return true;
                    }
                    count++;
                }
            }
            return false;
        }

        public static Message GetchMessage(string username,int index)
        {
            int count = 0;
            foreach (Message message in Program.messages)
            {
                if (message.to == username)
                {
                    if (count == index)
                    {
                        return message;
                    }
                    count++;
                }
            }
            throw new IndexOutOfRangeException();
        }

        public static bool SendMessage(string to, string from, string content)
        {
            foreach(Account account in Program.accounts)
            {
                if(account.username == to)
                {
                    Message tosend = new Message(from, to, content, DateTime.Now,false);
                    Program.messages.Add(tosend);
                    Program.SaveMessages();
                    return true;
                }
            }
            foreach(MessageBoard board in Program.messageBoards)
            {
                if(board.recieve_id == to)
                {
                    Message tosend = new Message(from, to, content, DateTime.Now, false);
                    Program.messages.Add(tosend);
                    Program.SaveMessages();
                    return true;
                }
            }
            return false;
        }
    }
}
