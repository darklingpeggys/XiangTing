using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Plugin
{
    public interface ICharacter
    {
        /// <summary>
        /// Character身份
        /// </summary>
        IBirdAuth auth { get; }

        /// <summary>
        /// 长任务冷却间隔
        /// </summary>
        int cool { get; set; }

        /// <summary>
        /// 短任务冷却间隔
        /// </summary>
        int short_cool { get; set; }

        /// <summary>
        /// Character是否存活
        /// </summary>
        bool alive { get; }

        /// <summary>
        /// Character的推特ID
        /// </summary>
        string character_id { get; }
        void AddTask(ITask task);
        void End();

        string ToInfo();
    }
}
