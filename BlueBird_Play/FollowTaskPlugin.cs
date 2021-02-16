using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Play
{
    public class FollowTaskPlugin : ITaskPlugin
    {
        public string name { get { return "批量关注任务"; } }

        public string info { get { return "通过指定用户ID让所有角色关注"; } }

        public string key { get { return "Mengluu-Follow-Task-BasePlugin"; } }

        public ITask GetTask()
        {
            FollowTaskWindow window = new FollowTaskWindow();
            window.ShowDialog();
            return window.task;
        }
    }
}
