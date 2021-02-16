using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueBird_Plugin;

namespace BlueBirdFriend
{
    static class TaskQueue
    {
        static Queue<ITask> queue = new Queue<ITask>();
        static Queue<ITask> importantqueue = new Queue<ITask>();
        const string MAX_IMPORTANT_TASKS= "MAX_IMPORTANT_TASKS";
        const string MAX_TASKS = "MAX_TASKS";
        /// <summary>
        /// 添加普通任务
        /// </summary>
        /// <param name="task">任务对象</param>
        public static void AddTask(ITask task)
        {
            if (task == null)
            {
                return;
            }
            lock (queue)
            {
                queue.Enqueue(task);
                if (queue.Count > int.Parse(Tool.GetConfig(MAX_TASKS)))
                {
                    queue.Dequeue();
                }
            }
        }

        /// <summary>
        /// 添加重要任务
        /// </summary>
        /// <param name="task">任务对象</param>
        public static void AddImportantTask(ITask task)
        {
            if (task == null)
            {
                return;
            }
            lock (importantqueue)
            {
                importantqueue.Enqueue(task);
                if (importantqueue.Count > int.Parse(Tool.GetConfig(MAX_IMPORTANT_TASKS)))
                {
                    importantqueue.Dequeue();
                }
            }
        }

        /// <summary>
        /// 获取任务，如果存在重要任务，优先返回重要任务，如果没有任务，返回null，注意判断返回值
        /// </summary>
        /// <returns></returns>
        public static ITask GetTask()
        {
            lock (importantqueue)
            {
                if (importantqueue.Count > 0)
                {
                    return importantqueue.Dequeue();
                }
            }
            lock (queue)
            {
                if (queue.Count > 0)
                {
                    return queue.Dequeue();
                }
            }
            return null;
        }
    }
}
