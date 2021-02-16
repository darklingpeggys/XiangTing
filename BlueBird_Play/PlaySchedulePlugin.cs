using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueBird_Plugin;

namespace BlueBird_Play
{
    public class PlaySchedulePlugin : ISchedulePlugin
    {
        public string name { get { return "自动活跃插件"; } }

        public string info { get { return "根据设定，让角色监视指定的搜索内容，然后进行点赞转推等任务"; } }

        public string key { get { return "Mengluu-Play-Schedule-BasePlugin"; } }

        public ISchedule[] GetSchedule(int count)
        {
            List<PlaySchedule> playSchedules = new List<PlaySchedule>();
            PlayScheduleWindow playScheduleWindow = new PlayScheduleWindow();
            if (playScheduleWindow.ShowDialog() == true)
            {
                for(int i = 0; i < count; i++)
                {
                    PlaySchedule playSchedule = new PlaySchedule();
                    playSchedule.setting = playScheduleWindow.presetting;
                    playSchedules.Add(playSchedule);
                }
            }
            return playSchedules.ToArray();
        }
    }
}
