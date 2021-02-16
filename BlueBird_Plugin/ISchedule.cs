using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Plugin
{
    public interface ISchedule
    {
        bool completed { get; }
        List<ITask> GetTask(ICharacter character);
        void End();
        string ToInfo();
        string name { get; }
    }
}
