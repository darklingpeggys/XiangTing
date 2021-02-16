using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Play
{
    public class LikeTaskPlugin : ITaskPlugin
    {
        public string name { get { return "批量点赞任务"; } }

        public string info { get { return "通过指定推文ID让所有角色点赞"; } }

        public string key { get { return "Mengluu-Like-Task-BasePlugin"; } }

        public ITask GetTask()
        {
            LikeTaskWindow window = new LikeTaskWindow();
            window.ShowDialog();
            return window.task;
        }
    }
}
