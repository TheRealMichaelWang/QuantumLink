using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    enum Permissions
    {
        Admin,
        Moderator,
        User
    }

    class Account
    {
        public string username;
        public string password;
        public Permissions permissions;
        
        public static bool SignUp(string username, string password)
        {
            foreach(Account account in Program.accounts)
            {
                if(account.username == username)
                {
                    return false;
                }
            }
            Program.accounts.Add(new Account(username, password, Permissions.User));
            Program.SaveAccounts();
            return true;
        }

        public Account(string username, string password, Permissions permissions)
        {
            this.username = username;
            this.password = password;
            this.permissions = permissions;
        }

        public bool verify(string username, string password)
        {
            if (this.username == username && this.password == password)
            {
                return true;
            }
            return false;
        }
    }
}
