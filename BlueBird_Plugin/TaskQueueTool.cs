using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Plugin
{
    public static class TaskQueueTool
    {
        /// <summary>
        /// 添加普通任务
        /// </summary>
        public static Action<ITask> AddTask;

        /// <summary>
        /// 添加重要任务
        /// </summary>
        public static Action<ITask> AddImportantTask;

        /// <summary>
        /// 获取任务，如果存在重要任务，优先返回重要任务，如果没有任务，返回null，注意判断返回值
        /// </summary>
        public static Func<ITask> GetTask;
    }
}
