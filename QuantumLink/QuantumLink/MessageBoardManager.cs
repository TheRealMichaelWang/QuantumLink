using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantumLink
{
    class MessageBoardManager
    {
        public Client client;
        public string boardname;
        public Thread newmessages;

        public MessageBoardManager(ref Client client, string boardname)
        {
            this.client = client;
            this.boardname = boardname;
            if(client.runCommand("msgboardcount\t"+boardname) == "fail")
            {
                throw new Exception();
            }
            newmessages = new Thread(new ThreadStart(getch_new_messages));
        }

        public void getch_new_messages()
        {
            int msgcount = int.Parse(client.runCommand("msgboardcount\t"+boardname));
            while(true)
            {
                Thread.Sleep(100);
                try
                {
                    int fetch = int.Parse(client.runCommand("msgboardcount\t" + boardname));
                    if(fetch != msgcount)
                    {
                        Message message = client.GetchMessageBoardMessage(boardname, fetch - 1);
                        message.Print();
                        msgcount = fetch;
                    }
                }
                catch
                {

                }
            }
        }

        public void Start()
        {
            Message[] messages = client.GetMessageBoardMessages(boardname);
            Console.WriteLine("QLink MessageBoard System");
            Console.WriteLine("[MessageBoard]: " + boardname);
            foreach(Message message in messages)
            {
                message.Print();
            }
            newmessages.Start();
            while(true)
            {
                string inp = Console.ReadLine();
                if(inp == "quit")
                {
                    newmessages.Abort();
                    break;
                }
                else
                {
                    if(!client.SendMessage("msgboard::" + boardname, inp))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[Server]: Message send failiure!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }
    }
}
