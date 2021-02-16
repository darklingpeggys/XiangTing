using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Play
{
    public class AttackTagSchedulePlugin : ISchedulePlugin
    {
        public string name { get { return "自动攻击Tag插件"; } }

        public string info { get { return "根据设定，让角色定时发送带Tag的内容，并找其它角色评论"; } }

        public string key { get { return "Mengluu-AttackTag-Schedule-BasePlugin"; } }

        public ISchedule[] GetSchedule(int count)
        {
            List<AttackTagSchedule> attackTagSchedules = new List<AttackTagSchedule>();
            AttackTagScheduleWindow attackTagScheduleWindow = new AttackTagScheduleWindow();
            if (attackTagScheduleWindow.ShowDialog() == true)
            {
                for (int i = 0; i < count; i++)
                {
                    AttackTagSchedule attackTagSchedule = new AttackTagSchedule(attackTagScheduleWindow.tag, attackTagScheduleWindow.level, attackTagScheduleWindow.value);
                    attackTagSchedules.Add(attackTagSchedule);
                }
            }
            return attackTagSchedules.ToArray();
        }
    }
}
