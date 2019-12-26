using System;
using System.Collections.Generic;
using System.Text;

namespace QuantumLinkAPI
{
    public class CloudData
    {
        private Client client;
        public string API_NAME
        {
            get;
            private set;
        }

        public CloudData(ref Client client, string API_NAME)
        {
            this.client = client;
            this.API_NAME = API_NAME;
        }

        public string this[string key]
        {
            get
            {
                return Read(key);
            }
            set
            {
                Write(key, value);
            }
        }

        public void SaveVector(string name, string[] items)
        {
            Write(name + "_size", items.Length.ToString());
            for (int i = 0; i < items.Length; i++)
            {
                Write(name + "_" + i, items[i]);
            }
        }

        public string[] LoadVector(string name)
        {
            int size = int.Parse(Read(name+"_size"));
            string[] items = new string[size];
            for (int i = 0; i < size; i++)
            {
                items[i] = Read(name + "_" + i);
            }
            return items;
        }

        public bool Write(string key, string data)
        {
            if(client.runCommand("writedata\t"+API_NAME+"\t"+key+"\t"+data) == "fail")
            {
                return false;
            }
            return true;
        }

        public string Read(string key)
        {
            return client.runCommand("readdata\t" + API_NAME + "\t" + key);
        }
    }
}
