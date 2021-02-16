using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Play
{
    class LikeTask : ITask
    {
        ITask.HALDLE_INFO character_id;
        public bool short_or_long { get { return true; } }

        public ITask.HALDLE_INFO handle_info { get { return character_id; } }
        string target_id = "";
        public LikeTask(ITask.HALDLE_INFO character, string target)
        {
            character_id = character;
            target_id = target;
        }
        public bool RunTask(ICharacter character)
        {
            character.short_cool = 2;
            string ret = SendBirdTool.MakeLike(character.auth, target_id);
            return ret.Contains("created_at");
        }

        public string ToInfo()
        {
            return "喜欢了：" + target_id;
        }
    }
}
