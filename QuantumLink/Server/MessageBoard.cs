using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class MessageBoard
    {
        public string name
        {
            get;
            private set;
        }

        public string recieve_id
        { 
            get
            {
                return "msgboard::" + name;
            }
        }

        public Message[] messages
        { 
            get
            {
                List<Message> messages = new List<Message>();
                foreach(Message message in Program.messages)
                {
                    if(message.to == recieve_id)
                    {
                        messages.Add(message);
                    }
                }
                return messages.ToArray();
            }
        }

        public MessageBoard(string name)
        {
            this.name = name;
        }

        public static MessageBoard GetMessageBoard(string name)
        {
            foreach(MessageBoard board in Program.messageBoards)
            {
                if(board.name == name)
                {
                    return board;
                }
            }
            return null;
        }

        public static void ClearMessages(MessageBoard board)
        {
            List<Message> toremove = new List<Message>();
            foreach(Message message in Program.messages)
            {
                if(board.recieve_id == message.to)
                {
                    toremove.Add(message);
                }
            }
            foreach(Message message in toremove)
            {
                Program.messages.Remove(message);
            }
            Program.SaveMessages();
        }

        public static bool Delete(string name)
        {
            foreach(MessageBoard board in Program.messageBoards)
            {
                if(board.name == name)
                {
                    Program.messageBoards.Remove(board);
                    Program.SaveMessageBoards();
                    ClearMessages(board);
                    return true;
                }
            }
            return false;
        }

        public static bool Create(string name)
        {
            foreach(MessageBoard board in Program.messageBoards)
            {
                if(board.name == name)
                {
                    return false;
                }
            }
            MessageBoard board_toadd = new MessageBoard(name);
            Program.messageBoards.Add(board_toadd);
            Message.SendMessage(board_toadd.recieve_id, "POSTMASTER", "Welcome to this message board! It was created on "+DateTime.Now+".");
            Program.SaveMessageBoards();
            return true;
        }
    }
}
