using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BlueBirdFriend
{
    static class Tool
    {
        public static Action<string> Log = Console.WriteLine;
        public static string GetConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
