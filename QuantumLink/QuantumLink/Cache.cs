using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantumLink
{
    static class Cache
    {
        static public Dictionary<string, string> values;

        static public void Save()
        {
            List<string> lines = new List<string>();
            foreach(string key in values.Keys)
            {
                lines.Add(key + "\t" + values[key]);
            }
            File.WriteAllLines(Environment.CurrentDirectory + "\\cache", lines.ToArray());
        }

        static public void Load()
        { 
            values = new Dictionary<string, string>();
            if(!File.Exists(Environment.CurrentDirectory+"\\cache"))
            {
                File.Create(Environment.CurrentDirectory + "\\cache").Close();
            }
            else
            {
                string[] lines = File.ReadAllLines(Environment.CurrentDirectory + "\\cache");
                foreach(string line in lines)
                {
                    string[] args = line.Split('\t');
                    values[args[0]] = args[1];
                }
            }
        }
    }
}
