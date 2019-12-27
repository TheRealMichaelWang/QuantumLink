using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace QuantumLink
{
    class MessageBoard
    {
        public Client client;
        public string boardname;
        public Thread newmessages;

        public MessageBoard(ref Client client, string boardname)
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: Error fetching message. Trying again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("[Input]: Press ANY key to continue...");
                    Console.ReadKey();
                }
            }
        }

        public void Start()
        {
            BigTextPrinter bigText = new BigTextPrinter();
            Message[] messages = client.GetMessageBoardMessages(boardname);
            bigText.PrintBigText(boardname, ConsoleColor.Yellow);
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
