using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BlueBird_Play
{
    /// <summary>
    /// ReplytweetTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ReplytweetTaskWindow : Window
    {
        public ITask task = null;
        public ReplytweetTaskWindow()
        {
            InitializeComponent();
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string text = TweetText.Text;
            string target = ReplyTarget.Text;
            string tagtext = TweetTag.Text;
            List<string> tag = new List<string>(tagtext.Split(new char[] { '|' }));
            int life;
            if (!int.TryParse(TaskLife.Text, out life))
            {
                life = 2;
            }
            task = new ReplyTweetTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.RANDOM }, target, text, tag, life);
            Close();
        }
    }
}
