using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Plugin
{
    public interface ITask
    {
        public struct HALDLE_INFO
        {
            public HALDLE_TYPE type;
            public string[] without_id;
            public int count;
            public string id;
        }
        enum HALDLE_TYPE
        {
            RANDOM,
            ALL,
            ALL_WITHOUT,
            RANDOM_MUL,
            RANDOM_WITHOUT,
            RANDOM_MUL_WITHOUT,
            TARGET
        }
        /// <summary>
        /// 短任务则为true，长任务则为false，是否是长任务取决与角色操作的频率，如点赞为短任务，发推为长任务
        /// </summary>
        bool short_or_long { get; }

        /// <summary>
        /// 目标角色ID。
        /// 如果为random，则为随机角色。
        /// 如果为all，则为全部角色。
        /// 如果为在角色ID前加入#，则目标为除此ID之外全部角色。
        /// </summary>
        HALDLE_INFO handle_info { get; }

        /// <summary>
        /// 任务执行内容
        /// </summary>
        /// <param name="character">执行任务的角色</param>
        /// <returns></returns>
        bool RunTask(ICharacter character);

        /// <summary>
        /// 任务信息，类似于ToString
        /// </summary>
        /// <returns></returns>
        string ToInfo();
    }
}
