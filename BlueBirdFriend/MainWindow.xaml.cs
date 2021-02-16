using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace BlueBirdFriend
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ControlCenter center = new ControlCenter();
        public MainWindow()
        {
            InitializeComponent();
            Title = Tool.GetConfig("NAME");
            void Log(string log)
            {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    LogTextBox.Text += log + "\n";
                    UpdateLayout();
                    LogTextBox.ScrollToVerticalOffset(LogTextBox.ActualHeight + LogTextBox.VerticalOffset);
                }));
            }
            Tool.Log = Log;

            List<string> pluginpath = FindPlugin();
            foreach (string filename in pluginpath)
            {
                try
                {
                    //获取文件名
                    string asmfile = filename;
                    string asmname = Path.GetFileNameWithoutExtension(asmfile);
                    if (asmname != string.Empty)
                    {
                        // 利用反射,构造DLL文件的实例
                        Assembly asm = Assembly.LoadFile(asmfile);
                        //利用反射,从程序集(DLL)中,提取类,并把此类实例化
                        Type[] t = asm.GetExportedTypes();
                        foreach (Type type in t)
                        {
                            if (type.GetInterface("ITaskPlugin") != null)
                            {
                                ITaskPlugin plugin = (ITaskPlugin)Activator.CreateInstance(type);
                                MenuItem item = new MenuItem();
                                item.Header = plugin.name;
                                item.Tag = plugin;
                                item.Click += ITaskPlugin_Click;
                                TaskMenu.Items.Add(item);
                            }
                            else if (type.GetInterface("ISchedulePlugin") != null)
                            {
                                ISchedulePlugin plugin = (ISchedulePlugin)Activator.CreateInstance(type);
                                MenuItem item = new MenuItem();
                                item.Header = plugin.name;
                                item.Tag = plugin;
                                item.Click += ISchedulePlugin_Click;
                                ScheduleMenu.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

        }

        private void ITaskPlugin_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            ITaskPlugin plugin = item.Tag as ITaskPlugin;
            if (MessageBox.Show(plugin.info, plugin.name, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                TaskQueue.AddImportantTask(plugin.GetTask());
            }
        }
        private void ISchedulePlugin_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            ISchedulePlugin plugin = item.Tag as ISchedulePlugin;
            if (MessageBox.Show(plugin.info, plugin.name, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                List<Character> characters = new List<Character>();
                foreach (Grid grid in CharacterList.Items)
                {
                    CheckBox box = grid.Children[0] as CheckBox;
                    if (box.IsChecked == true)
                    {
                        Character character = box.Tag as Character;
                        characters.Add(character);
                    }
                }
                ISchedule[] schedules = plugin.GetSchedule(characters.Count);
                for (int i = 0; i < schedules.Length && i < characters.Count; i++)
                {
                    characters[i].AddSchedule(schedules[i]);
                }
            }
        }


        //查找所有插件的路径
        private List<string> FindPlugin()
        {
            List<string> pluginpath = new List<string>();
            try
            {
                //获取程序的基目录
                string path = AppDomain.CurrentDomain.BaseDirectory;
                //合并路径，指向插件所在目录。
                path = Path.Combine(path, "Plugins");
                foreach (string filename in Directory.GetFiles(path, "*.dll"))
                {
                    pluginpath.Add(filename);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return pluginpath;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            if ((string)SelectAll.Content == "全选")
            {
                foreach (Grid box in CharacterList.Items)
                {
                    CheckBox check = box.Children[0] as CheckBox;
                    check.IsChecked = false;
                }
                SelectAll.Content = "全不选";
            }
            else
            {
                foreach (Grid box in CharacterList.Items)
                {
                    CheckBox check = box.Children[0] as CheckBox;
                    check.IsChecked = true;
                }
                SelectAll.Content = "全选";
            }
        }
        private void MenuItem_Click_添加角色(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Cookie文件|Cookie";
            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                string[] authtext = File.ReadAllLines(path);
                foreach (string s in authtext)
                {
                    string[] l = s.Split(new char[] { ';' });
                    void addcharacter(object value)
                    {
                        center.AddCharacter((string[])value);
                        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                        {
                            lock (center.handle.characterList)
                            {
                                CharacterList.Items.Clear();
                                foreach (KeyValuePair<string, Character> character in center.handle.characterList)
                                {
                                    Grid grid = new Grid();
                                    CheckBox box = new CheckBox();
                                    TextBlock text = new TextBlock();
                                    box.IsChecked = true;
                                    text.Text = character.Key;
                                    text.Tag = character.Value;
                                    box.Tag = character.Value;
                                    text.MouseLeftButtonDown += CharacterBox_Click;
                                    grid.Children.Add(box);
                                    grid.Children.Add(text);
                                    box.SetValue(Grid.ColumnProperty, 0);
                                    text.SetValue(Grid.ColumnProperty, 1);
                                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                                    grid.ColumnDefinitions[0].Width = GridLength.Auto;
                                    grid.ColumnDefinitions[1].Width = GridLength.Auto;
                                    CharacterList.Items.Add(grid);
                                }
                            }
                        }));

                    }
                    Thread thread = new Thread(new ParameterizedThreadStart(addcharacter));
                    thread.Start(l);
                }
            }
        }
        private void CharacterBox_Click(object sender, RoutedEventArgs e)
        {
            TextBlock box = sender as TextBlock;
            Character character = box.Tag as Character;
            CharacterInfo.Text = character.ToInfo();
        }
        private void MenuItem_Click_关于我们(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("©CN Andy.保留所有权利\nby Mengluu");
        }
        private void MenuItem_Click_刷新角色列表(object sender, RoutedEventArgs e)
        {
            CharacterList.Items.Clear();
            center.UpdateProfile();
            foreach (KeyValuePair<string, Character> character in center.handle.characterList)
            {
                Grid grid = new Grid();
                CheckBox box = new CheckBox();
                TextBlock text = new TextBlock();
                box.IsChecked = true;
                text.Text = character.Key;
                text.Tag = character.Value;
                box.Tag = character.Value;
                text.MouseLeftButtonDown += CharacterBox_Click;
                grid.Children.Add(box);
                grid.Children.Add(text);
                box.SetValue(Grid.ColumnProperty, 0);
                text.SetValue(Grid.ColumnProperty, 1);
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions[0].Width = GridLength.Auto;
                grid.ColumnDefinitions[1].Width = GridLength.Auto;
                CharacterList.Items.Add(grid);
            }
        }
        private void MenuItem_Click_退出(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void MenuItem_Click_保存日志(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("Log.txt", LogTextBox.Text);
        }
        private void MenuItem_Click_清空日志(object sender, RoutedEventArgs e)
        {
            LogTextBox.Text = "";
        }


        private void CreateContextMenu(object positionItem)
        {
            RightButtonDownMenu.Items.Clear();
            MenuItem menuItem = new MenuItem();
            menuItem.Header = "停止计划";
            menuItem.Click += (sender, e) => {
                ISchedule schedule = positionItem as ISchedule;
                schedule.End();
            };
            RightButtonDownMenu.Items.Add(menuItem);
        }
        private void ScheduleLabel_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject source = e.OriginalSource as DependencyObject;
            while (source != null && source.GetType() != typeof(TreeViewItem))
                source = System.Windows.Media.VisualTreeHelper.GetParent(source);
            if (source != null)
            {
                TreeViewItem item = source as TreeViewItem;
                CreateContextMenu(item.Tag);
            }
        }
        private void ScheduleLabel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ScheduleList.Items.Clear();
            foreach (KeyValuePair<string, Character> character in center.handle.characterList)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = character.Key;
                item.Tag = character.Value;
                foreach(ISchedule schedule in character.Value.GetSchedules())
                {
                    TreeViewItem subitem = new TreeViewItem();
                    subitem.Header = schedule.ToInfo();
                    subitem.Tag = schedule;
                    subitem.PreviewMouseRightButtonDown += ScheduleLabel_PreviewMouseRightButtonDown;
                    item.Items.Add(subitem);
                }
                ScheduleList.Items.Add(item);
            }
        }
    }
}
