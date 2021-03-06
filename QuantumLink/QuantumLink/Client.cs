﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace QuantumLink
{
    class Client
    {
        TcpClient client;
        NetworkStream stream;
        public bool LoggedIn
        {
            get;
            private set;
        }
        public bool Connected
        {
            get
            {
                return client.Connected;
            }
        }

        public Client()
        {
            
        }

        public bool Connect()
        {
            client = new TcpClient();
            int connectattempt = 0;
            while (true)
            {
                if(connectattempt == 4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: Could not connect. Exceeded 3  re-attempt limit.");
                    Console.ForegroundColor = ConsoleColor.White;
                    return false;
                }
                try
                {
                    connectattempt++;
                    Console.WriteLine("[Client]: Connecting...");
                    client.Connect(IPAddress.Parse("76.219.182.18"), 25565);
                    break;
                }
                catch
                {
                    if (connectattempt != 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[Error]: Connection faliure! Re-attempt no. " + (connectattempt) + ".");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            stream = client.GetStream();
            LoggedIn = false;
            return true;
        }

        public string runCommand(string command)
        {
            if(!Connected)
            {
                return "unconnected";
            }
            byte[] send = ASCIIEncoding.Default.GetBytes(command);
            stream.Write(send, 0, send.Length);
            while(client.Available == 0)
            {

            }
            byte[] brecieve = new byte[client.Available];
            stream.Read(brecieve, 0, brecieve.Length);
            return ASCIIEncoding.ASCII.GetString(brecieve);
        }

        public void MarkAsRead(int msgindex)
        {
            runCommand("readmsg\t" + msgindex);
        }

        public void DeleteMessages()
        {
            runCommand("delmsgs");
        }

        public bool SendMessage(string to,string msg)
        {
            string resp = runCommand("sendmsg\t" + to + "\t" + msg);
            return (resp != "fail");
        }

        public Message GetMessage(int index)
        {
            string resp = runCommand("getchmsg\t" + index);
            string[] args = resp.Split('\t');
            return new Message(args[0], args[1], DateTime.Parse(args[2]), bool.Parse(args[3]),index);
        }

        public Message GetchMessageBoardMessage(string name, int index)
        {
            string resp = runCommand("getchboardmessage\t" +name+"\t"+ index);
            string[] args = resp.Split('\t');
            return new Message(args[0], args[1], DateTime.Parse(args[2]), bool.Parse(args[3]), index);
        }

        public int CountUnreadMessages()
        {
            string resp = runCommand("myinfo\tunreadcount");
            if(resp == "fail")
            {
                return -1;
            }
            else
            {
                return int.Parse(resp);
            }
        }
        
        public Message[] GetMessageBoardMessages(string boardname)
        {
            string count_resp = runCommand("msgboardcount\t"+boardname);
            int count = int.Parse(count_resp);
            List<Message> message = new List<Message>();
            Thread.Sleep(100);
            for (int i = 0; i < count; i++)
            {
                message.Add(GetchMessageBoardMessage(boardname, i));
                Thread.Sleep(100);
            }
            return message.ToArray();
        }

        public Message[] GetAllMessages()
        {
            string count_resp = runCommand("msgcount");
            int count = int.Parse(count_resp);
            int start = 0;
            if(count > 30)
            {
                Console.WriteLine("[Client]: There are to many messages to fetch in a short peroid of time. How many messages do you want to fetch?");
                Console.Write("Messages>");
                start = int.Parse(Console.ReadLine());
            }
            List<Message> message = new List<Message>();
            Thread.Sleep(100);
            for (int i = start; i < count; i++)
            {
                message.Add(GetMessage(i));
                if(count>5)
                {
                    Console.WriteLine("[Client]: Getching message " + (i + 1) + " of " + count + ".");
                }
                Thread.Sleep(100);
            }
            return message.ToArray();
        }

        public Message[] GetUnreadMessages()
        {
            Message[] messages = GetAllMessages();
            List<Message> unread_messages = new List<Message>();
            foreach(Message message in messages)
            {
                if(message.read == false)
                {
                    unread_messages.Add(message);
                }
            }
            return unread_messages.ToArray();
        }

        public bool Announce(string what)
        {
            string resp = runCommand("announce\t"+what);
            return (resp != "fail");
        }

        public Announcement GetAnnouncement(int index)
        {
            string resp = runCommand("getchann\t" + index);
            string[] args = resp.Split('\t');
            return new Announcement(args[0], args[1], DateTime.Parse(args[2]));
        }

        public Announcement[] GetAnnouncements(TimeSpan span)
        {
            DateTime since_when = DateTime.Now - span;
            string resp = runCommand("anncount");
            int count = int.Parse(resp);
            List<Announcement> announcements = new List<Announcement>();
            for (int i = count-1; i > -1; i--)
            {
                Announcement announcement = GetAnnouncement(i);
                if (announcement.posttime - since_when > TimeSpan.Zero)
                {
                    announcements.Add(announcement);
                }
                else
                {
                    break;
                }
            }
            announcements.Reverse();
            return announcements.ToArray();
        }

        public Message[] SearchForMessage(string keyword)
        {
            Message[] messages = GetAllMessages();
            List<Message> message_results = new List<Message>();
            if(keyword == "today")
            {
                foreach(Message message in messages)
                {
                    if(message.time.Date == DateTime.Today)
                    {
                        message_results.Add(message);
                    }
                }
            }
            else
            {
                foreach (Message message in messages)
                {
                    if(message.from == keyword || message.content.Contains(keyword))
                    {
                        message_results.Add(message);
                    }
                }
            }
            return message_results.ToArray();
        }

        public string[] SearchForUsers(string keyword)
        {
            string resp = runCommand("search\tusers\t" + keyword);
            if(resp == "fail")
            {
                return new string[] { };
            }
            else
            {
                return resp.Split(' ');
            }
        }

        public string[] SearchForMessageBoards(string keyword)
        {
            string resp = runCommand("search\tmsgboards\t" + keyword);
            if (resp == "fail")
            {
                return new string[] { };
            }
            else
            {
                return resp.Split(' ');
            }
        }

        public bool ChangeUsername(string newusername)
        {
            string resp = runCommand("changeusername\t" + newusername);
            if (resp != "fail")
            {
                return true;
            }
            return false;
        }

        public bool CloseAccount()
        {
            if(LoggedIn)
            {
                runCommand("closeacc");
                LoggedIn = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Signup(string username, string password)
        {
            string resp = runCommand("signup\t" + username + "\t" + password);
            if (resp != "fail")
            {
                return true;
            }
            return false;
        }

        public bool Login(string username, string password)
        {
            string resp = runCommand("login\t"+username +"\t"+password);
            if(resp != "fail")
            {
                LoggedIn = true;
                return true;
            }
            return false;
        }

        public bool Logout()
        {
            string resp = runCommand("logout");
            if (resp != "fail")
            {
                LoggedIn = false;
                return true;
            }
            return false;
        }

        public void Disconnect()
        {
            byte[] send = ASCIIEncoding.Default.GetBytes("close");
            stream.Write(send, 0, send.Length);
        }
    }
}
