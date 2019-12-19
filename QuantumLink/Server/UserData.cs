using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class UserData
    {
        private Dictionary<string, string> data;
        
        public UserData()
        {
            data = new Dictionary<string, string>();
            if(!File.Exists(Environment.CurrentDirectory+"\\userdata.txt"))
            {
                File.Create(Environment.CurrentDirectory + "\\userdata.txt").Close();
            }
            string[] lines = File.ReadAllLines(Environment.CurrentDirectory + "\\userdata.txt");
            foreach(string line in lines)
            {
                string[] args = line.Split('\t');
                data[args[0]] = args[1];
            }
        }

        public void Save()
        {
            List<string> lines = new List<string>();
            foreach(string key in data.Keys)
            {
                lines.Add(key + "\t"+data[key]);
            }
            File.WriteAllLines(Environment.CurrentDirectory + "\\userdata.txt", lines.ToArray());
        }

        public void WriteData(string username, string key, string value)
        {
            data[username + "::" + key] = value;
        }

        public string GetchData(string username, string key)
        {
            if(data.ContainsKey(username+"::"+key))
            {
                return data[username + "::" + key];
            }
            else
            {
                return "fail";
            }
        }
    }
}
