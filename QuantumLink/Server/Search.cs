using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    static class Search
    {
        public static string[] SearchMessageBoards(string keyword, int results = 5)
        {
            List<Tuple<string, int>> search_results = new List<Tuple<string, int>>();
            foreach (MessageBoard board in Program.messageBoards)
            {
                if (board.name.Contains(keyword))
                {
                    search_results.Add(new Tuple<string, int>(board.name, board.name.Length - keyword.Length));
                }
            }
            for (int i = 0; i < search_results.Count - 1; i++)
            {
                if (search_results[i].Item2 < search_results[i + 1].Item2)
                {
                    var temp = search_results[i];
                    search_results[i] = search_results[i + 1];
                    search_results[i + 1] = temp;
                }
            }
            string[] sorted_results = new string[search_results.Count];
            for (int i = 0; i < search_results.Count; i++)
            {
                if (i + 1 == results)
                {

                }
                sorted_results[i] = search_results[i].Item1;
            }
            return sorted_results;
        }

        public static string[] SearchUsers(string keyword, int results = 5)
        {
            List<Tuple<string,int>> search_results = new List<Tuple<string,int>>();
            foreach(Account account in Program.accounts)
            {
                if(account.username.Contains(keyword))
                {
                    search_results.Add(new Tuple<string, int>(account.username, account.username.Length - keyword.Length));
                }
            }
            for (int i = 0; i < search_results.Count-1; i++)
            {
                if(search_results[i].Item2 < search_results[i+1].Item2)
                {
                    var temp = search_results[i];
                    search_results[i] = search_results[i + 1];
                    search_results[i + 1] = temp;
                }
            }
            string[] sorted_results = new string[search_results.Count];
            for (int i = 0; i < search_results.Count; i++)
            {
                if(i+1 == results)
                {

                }
                sorted_results[i] = search_results[i].Item1;
            }
            return sorted_results;
        }
    }
}
