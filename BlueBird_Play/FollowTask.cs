using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueBird_Plugin;

namespace BlueBird_Play
{
    class FollowTask : ITask
    {
        ITask.HALDLE_INFO character_id;
        public ITask.HALDLE_INFO handle_info { get { return character_id; } }
        string target_id = "";
        public bool short_or_long { get { return true; } }
        public FollowTask(ITask.HALDLE_INFO character,string target)
        {
            character_id = character;
            target_id = target;
        }

        public bool RunTask(ICharacter character)
        {
            string ret= SendBirdTool.MakeFollow(character.auth, target_id);
            return ret.Contains("created_at");
        }

        public string ToInfo()
        {
            return "关注了：" + target_id;
        }
    }
}
