using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Session
    {
        TcpClient client;
        Account current_account;
        bool logged_in;

        public Session(TcpClient client)
        {
            this.client = client;
            current_account = null;
            logged_in = false;
        }

        public static void Start(TcpClient client, ref List<Session> sessionlist)
        {
            Session session = new Session(client);
            sessionlist.Add(session);
            Thread thread = new Thread(new ThreadStart(session.doSessionLoop));
            thread.Start();
        }

        private string RunCommand(string command)
        {
            string[] args = command.Split('\t');
            if (args[0] == "login")
            {
                if (args.Length == 3 || logged_in)
                {
                    foreach (Account account in Program.accounts)
                    {
                        if (account.verify(args[1], args[2]))
                        {
                            current_account = account;
                            logged_in = true;
                            return "pass";
                        }
                    }
                    return "fail";
                }
                else
                {
                    return "fail";
                }
            }
            else if (args[0] == "signup")
            {
                if (args.Length == 3 && logged_in == false)
                {
                    if (Account.SignUp(args[1], args[2]))
                    {
                        return "pass";
                    }
                    return "fail";
                }
                else
                {
                    return "fail";
                }
            }
            else if (args[0] == "closeacc")
            {
                if (logged_in)
                {
                    RunCommand("delmsgs");
                    Program.accounts.Remove(current_account);
                    current_account = null;
                    logged_in = false;
                    Program.SaveAccounts();
                    return "pass";
                }
                else
                {
                    return "fail";
                }
            }
            else if (args[0] == "logout")
            {
                if(args.Length != 1 || logged_in == false)
                {
                    return "fail";
                }
                else
                {
                    current_account = null;
                    logged_in = false;
                    return "pass";
                }
            }
            else if(args[0] == "myinfo")
            {
                if(logged_in && args.Length == 2)
                {
                    if(args[1] == "username")
                    {
                        return current_account.username;
                    }
                    else if(args[1] == "unreadcount")
                    {
                        return Message.CountUnread(current_account.username).ToString();
                    }
                    else if(args[1] == "permissions")
                    {
                        if(current_account.permissions == Permissions.Admin)
                        {
                            return "admin";
                        }
                        else if(current_account.permissions == Permissions.Moderator)
                        {
                            return "moderator";
                        }
                        else
                        {
                            return "user";
                        }
                    }
                    else
                    {
                        return "fail";
                    }
                }
                else
                {
                    return "fail";
                }
            }
            else if(args[0] == "serverinfo")
            {
                if(args.Length == 2)
                {
                    if(args[1] == "connections")
                    {
                        return Program.sessions.Count.ToString();
                    }
                    else if(args[1] == "users")
                    {
                        return Program.accounts.Count.ToString();
                    }
                    else if(args[1] == "version")
                    {
                        return Program.version.ToString();
                    }
                    return "fail";
                }
                else
                {
                    return "fail";
                }
            }
            else if(args[0] == "msgcount")
            {
                if(logged_in)
                {
                    return Message.CountMessages(current_account.username).ToString();
                }
                else
                {
                    return "fail";
                }
            }
            else if(args[0] == "readmsg")
            {
                if (logged_in && args.Length == 2)
                {
                    if(Message.MarkRead(current_account.username,int.Parse(args[1])))
                    {
                        return "pass";
                    }
                    else
                    {
                        return "fail";
                    }
                }
                else
                {
                    return "fail";
                }
            }
            else if(args[0] == "getchmsg")
            {
                if(logged_in && args.Length == 2)
                {
                    try
                    {
                        Message msg= Message.GetchMessage(current_account.username,int.Parse(args[1]));
                        return msg.from + "\t" + msg.content + "\t" + msg.time+"\t"+msg.read;
                    }
                    catch
                    {
                        return "fail";
                    }
                }
                else
                {
                    return "fail";
                }
            }
            else if(args[0] == "msgcount")
            {
                if(logged_in)
                {

                }
                else
                {
                    return "fail";
                }
            }
            else if(args[0] == "delmsgs")
            {
                if(logged_in)
                {
                    Message.DeleteMessages(current_account.username);
                    return "pass";
                }
                else
                {
                    return "fail";
                }
            }
            else if(args[0] == "search")
            {
                if(args.Length == 3 || string.IsNullOrEmpty(args[2]))
                {
                    if(args[1] == "users")
                    {
                        string[] results = Search.SearchUsers(args[2], 7);
                        if(results.Length == 0)
                        {
                            return "fail";
                        }
                        return string.Join(" ", results);
                    }
                    else
                    {
                        return "fail";
                    }
                }
                else
                {
                    return "fail";
                }
            }
            else if(args[0] == "sendmsg")
            {
                if(args.Length == 3)
                {
                    if(logged_in)
                    {
                        if(Message.SendMessage(args[1],current_account.username,args[2]))
                        {
                            return "pass";
                        }
                        else
                        {
                            return "fail";
                        }
                    }
                    else
                    {
                        if (Message.SendMessage(args[1], "ANONYMOUS_USER", args[2]))
                        {
                            return "pass";
                        }
                        else
                        {
                            return "fail";
                        }
                    }
                }
                else
                {
                    return "fail";
                }
            }
            return "fail";
        }

        public void doSessionLoop()
        {
            NetworkStream stream = client.GetStream();
            DateTime last_response = DateTime.Now;
            while (true)
            {
                if(DateTime.Now - last_response > new TimeSpan(0,2,0))
                {
                    client.Close();
                    break;
                }
                if(client.Available != 0)
                {
                    last_response = DateTime.Now;
                    byte[] brecieve = new byte[client.Available];
                    stream.Read(brecieve, 0, brecieve.Length);
                    string recieve = ASCIIEncoding.Default.GetString(brecieve);
                    if(recieve == "close")
                    {
                        client.Close();
                        break;
                    }
                    byte[] send = ASCIIEncoding.Default.GetBytes(RunCommand(recieve));
                    stream.Write(send, 0, send.Length);
                }
            }
            Program.sessions.Remove(this);
        }
    }
}
