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
using BlueBird_Plugin;

namespace BlueBird_Play
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class FollowTaskWindow : Window
    {
        public ITask task = null;
        public FollowTaskWindow()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            task = new FollowTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.ALL }, TargetID.Text);
            Close();
        }
    }
}
