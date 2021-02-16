using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoLoginTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        string cookiefile = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            cookiefile = "";
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "账号文件|*";
            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                string[] authtext = File.ReadAllLines(path);
                LoginBot(authtext);
            }
        }
        void LoginBot(string[] users)
        {
            Task.Run(async () =>
            {
                int count = 0;
                foreach (string s in users)
                {
                    string[] r = s.Split(new char[] { ':' });
                    if (r.Length < 2)
                    {
                        continue;
                    }
                    string user = r[0];
                    string password = r[1];
                    Browser.GetCookieManager().DeleteCookies();
                    Browser.GetMainFrame().LoadUrl("https://twitter.com/login");
                    bool success = await Browser.ContainsLoginPage();
                    if (success)
                    {
                        await Browser.IsLoading();
                        Browser.GetMainFrame().ExecuteJavaScriptAsync("document.getElementsByName('session[username_or_email]')[0].value='" + user + "'" + ";"
                            + "document.getElementsByName('session[password]')[0].value='" + password + "'" + ";"
                            + "document.getElementsByName('session[password]')[0].type='submit'" + ";"
                            + "document.getElementsByName('session[password]')[0].click()");
                        //Browser.GetMainFrame().ExecuteJavaScriptAsync("document.getElementsByName('session[password]')[0].value='" + password + "'");
                        //Browser.GetMainFrame().ExecuteJavaScriptAsync("document.getElementsByName('session[password]')[0].type='submit'");
                        //Browser.GetMainFrame().ExecuteJavaScriptAsync("document.getElementsByName('session[password]')[0].click()");
                        await Browser.IsLoading();
                        string cookies = await Browser.ContainsLoginCookie();
                        Console.WriteLine(cookies);
                        cookiefile += cookies + ";" + user + "\n";
                        count++;
                    }
                    else
                    {
                        continue;
                    }
                }
                File.WriteAllText("Cookie", cookiefile);
                MessageBox.Show("已写出到Cookie文件，共成功获取" + count.ToString() + "个账号Cookie");
            });
        }
    }
    static class WebBrowserExt
    {
        public static async Task<bool> IsLoading(this ChromiumWebBrowser browser)
        {
            while (browser.GetBrowser().IsLoading)
            {
                await Task.Delay(2000);
            }
            return true;
        }
        public static async Task<bool> ContainsLoginPage(this ChromiumWebBrowser browser)
        {
            int count = 0;
            string source = await browser.GetMainFrame().GetSourceAsync();
            while ((!source.Contains("session[username_or_email]")||!source.Contains("session[password]"))&&count<1000)
            {
                await Task.Delay(200);
                source = await browser.GetMainFrame().GetSourceAsync();
                count++;
            }
            return count < 1000;
        }

        public static async Task<string> ContainsLoginCookie(this ChromiumWebBrowser browser)
        {
            int count = 0;
            List<Cookie> cookies;
            string ct0 = "";
            string auth_token = "";
            string _twitter_sess = "";

            while (!(ct0!=""&&auth_token != "" && _twitter_sess != "") &&count < 1000)
            {
                cookies = await browser.GetCookieManager().VisitAllCookiesAsync();
                foreach (Cookie cookie in cookies)
                {
                    if (cookie.Name == "ct0")
                    {
                        ct0 = cookie.Value;
                    }
                    if (cookie.Name == "auth_token")
                    {
                        auth_token = cookie.Value;
                    }
                    if (cookie.Name == "_twitter_sess")
                    {
                        _twitter_sess = cookie.Value;
                    }
                }
                await Task.Delay(200);
                count++;
            }
            if (count < 1000)
            {
                return auth_token + ";" + ct0 + ";" + _twitter_sess ;
            }
            return "";
        }
    }
}
