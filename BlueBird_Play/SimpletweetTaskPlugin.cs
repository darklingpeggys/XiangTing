using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Play
{
    public class SimpletweetTaskPlugin : ITaskPlugin
    {
        public string name { get { return "随机推文任务"; } }

        public string info { get { return "随机一个角色发布推文，并根据任务寿命安排回复"; } }

        public string key { get { return "Mengluu-SimpleTweet-Task-BasePlugin"; } }

        public ITask GetTask()
        {
            SimpletweetTaskWindow window = new SimpletweetTaskWindow();
            window.ShowDialog();
            return window.task;
        }
    }
}
