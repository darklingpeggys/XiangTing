using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Plugin
{
    public interface ISchedulePlugin:IPlugin
    {
        ISchedule[] GetSchedule(int count);
    }
}
