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
    /// AttackTagScheduleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AttackTagScheduleWindow : Window
    {
        public int level = 20;
        public int value = 5;
        public List<string> tag = new List<string>();
        public AttackTagScheduleWindow()
        {
            InitializeComponent();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            int tmp;
            level= (int.TryParse(attack_level.Text, out tmp)) ? tmp : level;
            value = (int.TryParse(attack_value.Text, out tmp)) ? tmp : value;
            tag = target_tag.Text.Split(' ').ToList();
            DialogResult = true;
            Close();
        }
    }
}
