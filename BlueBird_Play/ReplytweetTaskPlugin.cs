using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Play
{
    public class ReplytweetTaskPlugin : ITaskPlugin
    {
        public string name { get { return "回复推文任务"; } }

        public string info { get { return "随机一个角色回复推文，并根据任务寿命安排回复"; } }

        public string key { get { return "Mengluu-ReplyTweet-Task-BasePlugin"; } }

        public ITask GetTask()
        {
            ReplytweetTaskWindow window = new ReplytweetTaskWindow();
            window.ShowDialog();
            return window.task;
        }
    }
}
