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
    /// SimpletweetTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SimpletweetTaskWindow : Window
    {
        public ITask task = null;
        public SimpletweetTaskWindow()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string text = TweetText.Text;
            string tagtext = TweetTag.Text;
            List<string> tag = new List<string>(tagtext.Split(new char[] { '|' }));
            int life;
            if(!int.TryParse(TaskLife.Text,out life))
            {
                life = 2;
            }
            task = new SimpletweetTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.RANDOM }, text, tag, life);
            Close();
        }
    }
}
