using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlueBird_Play
{
    static class Bullet
    {
        static string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static Random random = new Random(DateTime.Now.Millisecond + DateTime.Now.Second * 1000);
        static string RemoveTag_At(string text)
        {
            Regex rgx = new Regex("@.*?\\s");
            text = rgx.Replace(text, "");
            rgx = new Regex("#.*?\\s");
            text = rgx.Replace(text, "");
            rgx = new Regex("@.*?\\z");
            text = rgx.Replace(text, "");
            rgx = new Regex("#.*?\\z");
            text = rgx.Replace(text, "");
            return text;
        }
        public static string GetBullet(string last = "")
        {
            last = RemoveTag_At(last);
            if (File.Exists(Path.Combine(path, "Bullet.txt")))
            {
                string[] bullets = File.ReadAllLines(Path.Combine(path, "Bullet.txt"));
                if (last == "")
                {
                    return bullets[random.Next(0, bullets.Length)];
                }
                int max = 0;
                List<string> b = new List<string> { last };
                foreach (string s in bullets)
                {
                    if (s == last)
                    {
                        continue;
                    }
                    if (random.Next(0, bullets.Length) > bullets.Length / 100)
                    {
                        b.Add(s);
                        continue;
                    }
                    int l = LongCommonSubstring(last, s);
                    if (l > max)
                    {
                        max = l;
                        b.Add(s);
                    }
                }
                return b[random.Next(0, b.Count)];
            }
            else
            {
                return last + random.Next(0, 1000);
            }
        }
        static int LongCommonSubstring(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            {
                return 0;
            }

            int[,] strArr = new int[str1.Length, str2.Length];
            int max = 0;
            for (int i = 0; i < str1.Length; i++)
            {
                for (int j = 0; j < str2.Length; j++)
                {
                    strArr[i, j] = 0;
                    if (str1[i] == str2[j])
                    {
                        if (i - 1 >= 0 && j - 1 >= 0)
                        {
                            strArr[i, j] = strArr[i - 1, j - 1] + 1;
                        }
                        else
                        {
                            strArr[i, j] = 1;
                        }
                        if (max < strArr[i, j])
                        {
                            max = strArr[i, j];
                        }
                    }
                }
            }
            return max;
        }
    }
}
