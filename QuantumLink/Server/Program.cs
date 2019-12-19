using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        public static List<Account> accounts;
        static TcpListener listener;
        public static List<Session> sessions;
        public static List<Message> messages;
        public static List<MessageBoard> messageBoards;
        public static List<Announcement> announcements;
        public static int version = 2;

        static void LoadAccounts()
        {
            accounts = new List<Account>();
            if (!File.Exists(Environment.CurrentDirectory + "\\accounts.txt"))
            {
                File.Create(Environment.CurrentDirectory + "\\accounts.txt").Close();
            }
            string[] lines = File.ReadAllLines(Environment.CurrentDirectory + "\\accounts.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split('\t');
                if (parts[2] == "USER")
                {
                    accounts.Add(new Account(parts[0], parts[1], Permissions.User));
                }
                else if (parts[2] == "MOD")
                {
                    accounts.Add(new Account(parts[0], parts[1], Permissions.Moderator));
                }
                else if (parts[2] == "ADMIN")
                {
                    accounts.Add(new Account(parts[0], parts[1], Permissions.Admin));
                }
            }
        }

        public static void LoadMessages()
        {
            messages = new List<Message>();
            if (!File.Exists(Environment.CurrentDirectory + "\\msglist.txt"))
            {
                File.Create(Environment.CurrentDirectory + "\\msglist.txt").Close();
            }
            string[] lines = File.ReadAllLines(Environment.CurrentDirectory + "\\msglist.txt");
            foreach(string line in lines)
            {
                string[] args = line.Split('\t');
                messages.Add(new Message(args[0], args[1], args[2], DateTime.Parse(args[3]),bool.Parse(args[4])));
            }
        }

        public static void LoadAnnouncements()
        {
            announcements = new List<Announcement>();
            if (!File.Exists(Environment.CurrentDirectory + "\\announcements.txt"))
            {
                File.Create(Environment.CurrentDirectory + "\\announcements.txt").Close();
            }
            string[] lines = File.ReadAllLines(Environment.CurrentDirectory + "\\announcements.txt");
            int i = 0;
            foreach (string line in lines)
            {
                string[] args = line.Split('\t');
                announcements.Add(new Announcement(args[0], args[1], DateTime.Parse(args[2]), i));
                i++;
            }
        }

        public static void LoadMessageBoards()
        {
            messageBoards = new List<MessageBoard>();
            if (!File.Exists(Environment.CurrentDirectory + "\\msgboards.txt"))
            {
                File.Create(Environment.CurrentDirectory + "\\msgboards.txt").Close();
            }
            string[] lines = File.ReadAllLines(Environment.CurrentDirectory + "\\msgboards.txt");
            foreach(string line in lines)
            {
                messageBoards.Add(new MessageBoard(line));
            }
        }

        public static void SaveAccounts()
        {
            List<string> lines = new List<string>();
            foreach (Account account in accounts)
            {
                if (account.permissions == Permissions.User)
                {
                    lines.Add(account.username + "\t" + account.password + "\tUSER");
                }
                else if (account.permissions == Permissions.Moderator)
                {
                    lines.Add(account.username + "\t" + account.password + "\tMOD");
                }
                else if (account.permissions == Permissions.Admin)
                {
                    lines.Add(account.username + "\t" + account.password + "\tADMIN");
                }
            }
            File.WriteAllLines(Environment.CurrentDirectory + "\\accounts.txt", lines);
        }

        public static void SaveMessages()
        {
            List<string> lines = new List<string>();
            foreach (Message message in messages)
            {
                lines.Add(message.from + "\t" + message.to + "\t" + message.content + "\t" + message.time + "\t" + message.read);
            }
            File.WriteAllLines(Environment.CurrentDirectory + "\\msglist.txt", lines);
        }

        public static void SaveAnnouncements()
        {
            List<string> lines = new List<string>();
            foreach(Announcement announcement in announcements)
            {
                lines.Add(announcement.content + "\t" + announcement.from + "\t" + announcement.posttime);
            }
            File.WriteAllLines(Environment.CurrentDirectory + "\\announcements.txt", lines);
        }

        public static void SaveMessageBoards()
        {
            List<string> lines = new List<string>();
            foreach (MessageBoard board in messageBoards)
            {
                lines.Add(board.name);
            }
            File.WriteAllLines(Environment.CurrentDirectory + "\\msgboards.txt", lines);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Loading Assets...");
            LoadAccounts();
            LoadMessages();
            LoadAnnouncements();
            LoadMessageBoards();
            sessions = new List<Session>();
            Console.WriteLine("Starting Server...");
            listener = new TcpListener(IPAddress.Any, 25565);
            listener.Start();
            while(true)
            {
                if(listener.Pending())
                {
                    Session.Start(listener.AcceptTcpClient(), ref sessions);
                }
            }
        }
    }
}
