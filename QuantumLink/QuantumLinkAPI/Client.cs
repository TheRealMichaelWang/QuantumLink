using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace QuantumLinkAPI
{
    public class Client
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
                if (connectattempt == 4)
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
            if (!Connected)
            {
                return "unconnected";
            }
            byte[] send = ASCIIEncoding.Default.GetBytes(command);
            stream.Write(send, 0, send.Length);
            while (client.Available == 0)
            {

            }
            byte[] brecieve = new byte[client.Available];
            stream.Read(brecieve, 0, brecieve.Length);
            return ASCIIEncoding.ASCII.GetString(brecieve);
        }

        public bool SendMessage(string to, string msg)
        {
            string resp = runCommand("sendmsg\t" + to + "\t" + msg);
            return (resp != "fail");
        }

        
        public bool Login(string username, string password)
        {
            string resp = runCommand("login\t" + username + "\t" + password);
            if (resp != "fail")
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
