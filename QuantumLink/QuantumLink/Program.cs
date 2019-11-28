using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantumLink
{
    class Program
    {
        static Client client;
        static MessageBoardManager boardManager;
        static Help help;
        static int version = 1;

        static bool YesNoPrompt(string prompt)
        {
            Console.Write(prompt + "(y/n)?");
            string inp = Console.ReadLine();
            if(inp == "y")
            {
                return true;
            }
            else if(inp == "n")
            {
                return false;
            }
            else
            {
                Console.WriteLine("[Error]: Unrecognized input. Enter y/n.");
                return YesNoPrompt(prompt);
            }
        }

        static void RunCommand(string command)
        {
            string[] args = command.Split(' ');
            if (command.StartsWith("help"))
            {
                if(args.Length == 1)
                {
                    help.printCommandList();
                }
                else
                {
                    help.printTopicHelp(args[1]);
                }
            }
            else if(command == "quit")
            {
                client.Disconnect();
                Process.GetCurrentProcess().Kill();
            }
            else if(command.StartsWith("search "))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Search]: Here are search results for \""+ command.TrimStart("search ".ToCharArray())+"\"");
                Console.ForegroundColor = ConsoleColor.White;
                if (client.LoggedIn)
                {
                    Console.WriteLine("Messages:");
                    Message[] message_results = client.SearchForMessage(command.TrimStart("search ".ToCharArray()));
                    foreach (Message message in message_results)
                    {
                        message.Print();
                    }
                    if (message_results.Length == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[Error]: No username matches keyword.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.WriteLine("Message Boards:");
                string[] board_results = client.SearchForMessageBoards(command.TrimStart("search ".ToCharArray()));
                foreach(string board in board_results)
                {
                    Console.WriteLine(board);
                }
                if (board_results.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: No message board matches keyword.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Users:");
                string[] user_results = client.SearchForUsers(command.TrimStart("search ".ToCharArray()));
                foreach(string user in user_results)
                {
                    Console.WriteLine(user);
                }
                if(user_results.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: No username matches keyword.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (command.StartsWith("login"))
            {
                if (args.Length == 3)
                {
                    if (client.Login(args[1], args[2]))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[Server]: You have succesfully logged in, " + args[1] + ".");
                        Console.ForegroundColor = ConsoleColor.White;
                        if(client.CountUnreadMessages() != 0 &&YesNoPrompt("Would you like to read your new messages?"))
                        {
                            RunCommand("unread");
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[Error]: Either your password or username is incorrect, or you are already logged in.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: Insufficient arguments.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (command.StartsWith("signup"))
            {
                if (args.Length == 3 && !client.LoggedIn)
                {
                    if (client.Signup(args[1], args[2]))
                    {
                        client.Login(args[1], args[2]);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[Server]: Succesfully signed up! Congratulations, you joined " + client.runCommand("serverinfo\tusers") + " other user(s)");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[Error]: That username is taken.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: Insufficient arguments or you are already logged in.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if(command == "closeaccount" && YesNoPrompt("Are you sure you want to close your QuantumLink account?"))
            {
                if(client.CloseAccount())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[Server]: Your account has successfuly been removed from QuantumLink servers. All messages sent to you have been deleted.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: You must log in to close your account.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (command == "logout")
            {
                if (client.Logout())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[Server]: You have succesfully logged out.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: You are not logged in");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if(args[0] == "delboard")
            {
                if (args.Length == 2 && client.LoggedIn)
                {
                    if (client.runCommand("delboard\t" + args[1]) == "pass")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[Server]: Successfuly deleted board.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[Error]: Board doesnt exists.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: Insufficient arguments or you have insufficient permissions.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if(args[0] == "newboard")
            {
                if(args.Length == 2 && client.LoggedIn)
                {
                    if(client.runCommand("newboard\t"+args[1]) == "pass")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[Server]: Successfuly created new board.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[Error]: Board already exists.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: Insufficient arguments or you have insufficient permissions.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if(args[0] == "clearmsgboard")
            {
                if (args.Length == 2 && client.LoggedIn)
                {
                    if (client.runCommand("clearboardmsgs\t" + args[1]) == "pass")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[Server]: Succesfully cleared all message board.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[Error]: Board doesn't exist.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: Insufficient arguments or you have insufficient permissions.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if(args[0] == "changeusername")
            {
                if(args.Length == 2)
                {
                    if (client.ChangeUsername(args[1]))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[Server]: Succesfully changed username");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[Error]: You must log in to change your username and you must select a unique one.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: Insufficient arguments.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if(args[0] == "join")
            {
                try
                {
                    boardManager = new MessageBoardManager(ref client, args[1]);
                    boardManager.Start();
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: No such message board exists.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if(command == "delmsgs")
            {
                if(client.LoggedIn)
                {
                    if(YesNoPrompt("Are you sure you want to delete all your messages?"))
                    {
                        client.DeleteMessages();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[Server]: All your messages have been deleted.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: You are not logged in");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (command == "sendmsg")
            {
                Console.Write("[Messenger]:To>");
                string to = Console.ReadLine();
                Console.Write("[Messenger]:Msg>");
                string msg = Console.ReadLine();
                if (client.SendMessage(to, msg))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[Server]: Message successfuly sent.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: No such username, \"" + to + "\".");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (command == "msglist")
            {
                if (client.LoggedIn)
                {
                    Message[] messages = client.GetAllMessages();
                    messages.Reverse();
                    Console.ForegroundColor = ConsoleColor.White;
                    foreach (Message message in messages)
                    {
                        message.Print();
                    }
                    if (messages.Length == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[Info]: You have no messages.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error]: You are not logged in");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (command == "unread")
            {
                Message[] messages = client.GetUnreadMessages();
                messages.Reverse();
                foreach (Message message in messages)
                {
                    message.Print();
                }
                if (messages.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[Info]: Congrats! You have no messages left to read!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if(YesNoPrompt("Would you like to mark these messages as read?"))
                {
                    foreach(Message message in messages)
                    {
                        client.MarkAsRead(message.server_index);
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error]: Unrecognized command. Type \"help\" for a comprehensive list of commands");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Quantum Link";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[Client]: Initializing");
            help = new Help();
            client = new Client();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("QUANTUM LINK");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Client Version: " + version);
            Console.WriteLine("Server Version: " + client.runCommand("serverinfo\tversion"));
            Console.WriteLine("Users: "+client.runCommand("serverinfo\tusers"));
            Console.WriteLine("Connections: " + client.runCommand("serverinfo\tconnections"));
            
            

            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                if(client.LoggedIn)
                {
                    Console.Write(client.runCommand("myinfo\tusername")+">");
                }
                else
                {
                    Console.Write("Guest>");
                }
                Console.ForegroundColor = ConsoleColor.White;
                string command = Console.ReadLine();
                RunCommand(command);
            }
        }
    }
}
