using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Play
{
    public class RetweetTaskPlugin : ITaskPlugin
    {
        public string name { get { return "批量转推任务"; } }

        public string info { get { return "通过指定推文ID让所有角色转推"; } }

        public string key { get { return "Mengluu-Retweet-Task-BasePlugin"; } }

        public ITask GetTask()
        {
            RetweetTaskWindow window = new RetweetTaskWindow();
            window.ShowDialog();
            return window.task;
        }
    }
}
