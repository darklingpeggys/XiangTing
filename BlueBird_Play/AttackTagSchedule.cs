using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Play
{
    public class AttackTagSchedule : ISchedule
    {
        bool isCompleted = false;
        int count = 0;
        int life = 5;
        int attack_time = 20;
        int last_time = 0;
        List<string> target_tag = new List<string>();
        public bool completed { get { return isCompleted; } }

        public string name { get { return "自动攻击Tag"; } }

        public AttackTagSchedule(List<string> tag,int time,int value)
        {
            target_tag = tag;
            attack_time = time;
            life = value;
        }
        int GetTime()
        {
            return DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600 + DateTime.Now.Day * 3600 * 24;
        }
        public void End()
        {
            isCompleted = true;
        }

        public List<ITask> GetTask(ICharacter character)
        {
            int now = GetTime();
            if (now - last_time < attack_time)
            {
                return new List<ITask>(); ;
            }
            count++;
            last_time = now;
            return new List<ITask>() { new AttackSimpleTweet(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.TARGET, id = character.character_id }, Bullet.GetBullet(), target_tag, life) };
        }

        public string ToInfo()
        {
            string info = "自动攻击Tag\n";
            info += "攻击目标：";
            foreach(string tag in target_tag)
            {
                info += "#" + tag + " ";
            }
            info += "\n";
            info += "已发布" + count.ToString() + "项攻击任务";
            return info;
        }
    }
}
