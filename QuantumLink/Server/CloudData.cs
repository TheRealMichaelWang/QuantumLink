using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class CloudData
    {
        private Dictionary<string, string> database;

        public CloudData()
        {
            database = new Dictionary<string, string>();
        }

        public void Write(string api, string user,string keyname, string data)
        {
            database[user + "::" + api + "::" + keyname] = data;
        }

        public string Read(string api, string user, string keyname)
        {
            return database[user + "::" + api + "::" + keyname];
        }

        public void Save()
        {
            List<string> lines = new List<string>();
            foreach(string key in database.Keys)
            {
                lines.Add(key + "\t" + database[key]);
            }
            File.WriteAllLines(Environment.CurrentDirectory + "\\clouddata.txt",lines.ToArray());
        }

        public void Load()
        {
            if(!File.Exists(Environment.CurrentDirectory+"\\clouddata.txt"))
            {
                File.Create(Environment.CurrentDirectory + "\\clouddata.txt").Close();
            }
            string[] lines = File.ReadAllLines(Environment.CurrentDirectory + "\\clouddata.txt");
            foreach(string line in lines)
            {
                string[] largs = line.Split('\t');
                database[largs[0]] = largs[1];
            }
        }
    }
}
