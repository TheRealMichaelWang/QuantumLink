using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantumLink
{
    class Help
    {
        private Dictionary<string, string> arguments;
        private Dictionary<string, string> short_descriptions;
        private Dictionary<string, string> descriptions;

        public Help()
        {
            arguments = new Dictionary<string, string>();
            short_descriptions = new Dictionary<string, string>();
            descriptions = new Dictionary<string, string>();
            arguments.Add("login","[username] [password]");
            arguments.Add("signup", "[username] [password]");
            arguments.Add("closeaccount", "");
            arguments.Add("logout","");
            arguments.Add("search","[keyword]");
            arguments.Add("newboard", "[boardname]");
            arguments.Add("delboard", "[boardname]");
            arguments.Add("join", "[boardname]");
            arguments.Add("delmsgs","");
            arguments.Add("sendmsg", "");
            arguments.Add("msglist", "");
            arguments.Add("announcements", "[since_when]");
            arguments.Add("announce", "[msg]");
            arguments.Add("unread", "");
            arguments.Add("quit", "");
            short_descriptions.Add("login","Logs the user into the server.");
            short_descriptions.Add("signup", "Register a Quantum Link account.");
            short_descriptions.Add("search", "Searches for users, messages, and message boards.");
            short_descriptions.Add("newboard", "Creates a new message board.");
            short_descriptions.Add("delboard", "Deletes a existing message board.");
            short_descriptions.Add("join", "Joins a message board.");
            short_descriptions.Add("closeaccount", "Close your Quantum Link account.");
            short_descriptions.Add("logout", "Logs out of the server.");
            short_descriptions.Add("delmsgs", "Deletes your message list.");
            short_descriptions.Add("sendmsg", "Sends a message.");
            short_descriptions.Add("msglist", "Fetches and prints your message list.");
            short_descriptions.Add("unread", "Fetches and displays unread messages.");
            short_descriptions.Add("announcements", "Fetches and prints announcements.");
            short_descriptions.Add("announce", "Announces a message.");
            short_descriptions.Add("quit", "Quits the app.");
            descriptions.Add("login","Logs the user into the server.");
            descriptions.Add("signup", "Register a Quantum Link account. The username must be unique.");
            descriptions.Add("search", "Search for users, message boards, or messages you recieved. Messages are displayed on top of users.");
            descriptions.Add("newboard", "You must have an admin or moderator account to create a message board.");
            descriptions.Add("delboard", "You must have an admin or moderator account to delete a message board.");
            descriptions.Add("join", "Joins a message board. You dont need to sign in to perform this function.");
            descriptions.Add("closeaccount", "You will be asked to confirm whether you want to close your account before actually doing it.");
            descriptions.Add("delmsgs", "All the messages in your message list will be deleted. This may speed up the client if you have too many messages.");
            descriptions.Add("sendmsg", "Sends a message to a person registered on Quantum Link. You don't have to login to perform this function.");
            descriptions.Add("msglist","Displays all the messages people have sent you. Note, this may take a while depending on the size of your message list.");
            descriptions.Add("unread", "Displays unread messages sent to you. Note, this may take a while depending on the size of your message list.");
            descriptions.Add("quit", "Disconnects from the QuantumLink server and closes the application. Please use this command instead of closing the window because your client will still be connected to QuantumLink servers otherwise.");
            descriptions.Add("announcements", "Fetches all announcements since the time given in the argument. If there is no time give, then it will fetch todays announcements only. Valid time arguments are \"yestarday\", \"week\", \"month\", and \"year\".");
            descriptions.Add("announce", "Announces a message. You must log in to perform this function.");
        }

        public void printTopicHelp(string topic)
        {
            if(arguments.ContainsKey(topic))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(topic);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Command Usage: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\""+topic +" " + arguments[topic]+"\"");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(descriptions[topic]);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error]: Topic not found.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void printCommandList()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[Help]: For more comprehendive help type \"help [topic]\" where topic the topic.");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (string key in arguments.Keys)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(key.PadRight(20));
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(short_descriptions[key]);
            }
        }
    }
}
