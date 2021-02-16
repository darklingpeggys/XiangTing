using LitJson;
using System;
using System.Collections.Generic;
using System.Threading;
using BlueBird_Plugin;

namespace BlueBirdFriend
{
    class Character : ICharacter
    {
        const string MAX_CHARACTER_TASKS = "MAX_CHARACTER_TASKS";
        const string MAX_CHARACTER_SHORT_TASKS = "MAX_CHARACTER_SHORT_TASKS";
        const string CHARACTER_SHORT_COOL = "CHARACTER_SHORT_COOL";
        const string CHARACTER_COOL = "CHARACTER_COOL";
        int deal_time = 0;
        int last_time = 0;
        int short_deal_time = 0;
        int short_last_time = 0;
        bool flag = true;
        string id = "";
        Queue<ITask> tasks = new Queue<ITask>();
        Queue<ITask> short_tasks = new Queue<ITask>();
        List<ISchedule> schedules = new List<ISchedule>();
        BirdAuth authdata;

        string name = "";
        string screen_name = "";
        int followers_count = 0;
        int friends_count = 0;
        int favourites_count=0;
        int statuses_count=0;


        /// <summary>
        /// Character身份
        /// </summary>
        public IBirdAuth auth { get { return authdata as IBirdAuth; } }

        /// <summary>
        /// 长任务冷却间隔
        /// </summary>
        public int cool { get { return deal_time; } set { deal_time = value; } }

        /// <summary>
        /// 短任务冷却间隔
        /// </summary>
        public int short_cool { get { return short_deal_time; } set { short_deal_time = value; } }
        
        /// <summary>
        /// Character是否存活
        /// </summary>
        public bool alive { get { return flag; } }

        /// <summary>
        /// Character的推特ID
        /// </summary>
        public string character_id { get { return id; } }

        /// <summary>
        /// Character类通过Auth进行初始化，它初始化时会作为一个Bot载入。若初始化失败或者被结束，则alive属性为假
        /// </summary>
        /// <param name="auth_data">BirdAuth类型，通过auth_token，ct0，twitter_sess进行验证</param>
        public Character(BirdAuth auth_data)
        {
            authdata = auth_data;
            string ret = SendBird.GetHome(authdata);
            JsonData data = JsonMapper.ToObject(ret);
            if (!data.ContainsKey("globalObjects"))
            {
                flag = false;
                return;
            }
            id = data["timeline"]["id"].ToString().Replace("Home-", "");
            UpdateProfile();
            Thread thread = new Thread(new ThreadStart(Auto));
            thread.Start();
        }

        public void UpdateProfile()
        {
            string ret = SendBird.GetProfile(authdata, id);
            JsonData data = JsonMapper.ToObject(ret);
            if (data.ContainsKey("name"))
            {
                name = data["name"].ToString();
                screen_name = data["screen_name"].ToString();
                followers_count = (int)data["followers_count"];
                friends_count = (int)data["friends_count"];
                favourites_count = (int)data["favourites_count"];
                statuses_count = (int)data["statuses_count"];
            }
        }

        /// <summary>
        /// 终结Bot
        /// </summary>
        public void End()
        {
            flag = false;
        }

        /// <summary>
        /// 添加任务，该函数线程安全
        /// </summary>
        /// <param name="task">任务对象，通过ITask接口实现</param>
        public void AddTask(ITask task)
        {
            if (task.short_or_long) {
                lock (short_tasks)
                {
                    short_tasks.Enqueue(task);
                    if (short_tasks.Count > int.Parse(Tool.GetConfig(MAX_CHARACTER_SHORT_TASKS)))
                    {
                        short_tasks.Dequeue();
                    }
                }
            }
            else
            {
                lock (tasks)
                {
                    tasks.Enqueue(task);
                    if (tasks.Count > int.Parse(Tool.GetConfig(MAX_CHARACTER_TASKS)))
                    {
                        tasks.Dequeue();
                    }
                }
            }
        }
        ITask GetTask(bool short_or_long)
        {
            if (short_or_long)
            {
                lock (short_tasks)
                {
                    return short_tasks.Dequeue();
                }
            }
            else
            {
                lock (tasks)
                {
                    return tasks.Dequeue();
                }
            }
        }

        int GetTime()
        {
            return DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600 + DateTime.Now.Day * 3600 * 24;
        }
        void Auto()
        {
            while (flag)
            {
                int now = GetTime();
                if (now - last_time > deal_time && tasks.Count != 0)
                {
                    deal_time = int.Parse(Tool.GetConfig(CHARACTER_COOL));
                    ITask mission = GetTask(false);
                    if (DealWithMission(mission))
                    {
                        last_time = now;
                    }
                }
                if (now - short_last_time > short_deal_time && short_tasks.Count != 0)
                {
                    short_deal_time = int.Parse(Tool.GetConfig(CHARACTER_SHORT_COOL));
                    ITask mission = GetTask(true);
                    if (DealWithMission(mission))
                    {
                        short_last_time = now;
                    }
                }
                UpdateSchedules();
                Thread.Sleep(1000);
            }
        }
        bool DealWithMission(ITask mission)
        {
            bool flag = mission.RunTask(this);
            if (flag)
            {
                Tool.Log(character_id + " : \n\t" + mission.ToInfo());
            }
            return flag;
        }

        void UpdateSchedules()
        {
            lock (schedules)
            {
                List<ISchedule> trash = new List<ISchedule>();
                for(int i = 0; i < schedules.Count; i++)
                {
                    if (schedules[i].completed)
                    {
                        trash.Add(schedules[i]);
                    }
                    else
                    {
                        foreach(ITask task in schedules[i].GetTask(this))
                        {
                            TaskQueue.AddTask(task);
                        }
                    }
                }
                foreach(ISchedule i in trash)
                {
                    schedules.Remove(i);
                }
            }
        }
        public void AddSchedule(ISchedule schedule)
        {
            lock (schedules)
            {
                if (schedule != null)
                {
                    schedules.Add(schedule);
                }
            }
        }
        public ISchedule[] GetSchedules()
        {
            return schedules.ToArray();
        }

        public string ToInfo()
        {
            string info = "名称：" + name + "\n";
            info += "显示名称：" + screen_name + "\n";
            info += "粉丝数：" + followers_count.ToString() + "\n";
            info += "关注数：" + friends_count.ToString() + "\n";
            info += "点赞数：" + favourites_count.ToString() + "\n";
            info += "推文数：" + statuses_count.ToString() + "\n";
            info += "\n";
            foreach (ISchedule schedule in schedules)
            {
                info += schedule.ToInfo() + "\n";
            }
            return info;
        }
    }
}
