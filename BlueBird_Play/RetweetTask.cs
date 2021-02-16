using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Play
{
    class RetweetTask : ITask
    {
        public bool short_or_long { get { return true; } }

        ITask.HALDLE_INFO character_id;
        public ITask.HALDLE_INFO handle_info { get { return character_id; } }
        string target_id = "";
        public RetweetTask(ITask.HALDLE_INFO character, string target)
        {
            character_id = character;
            target_id = target;
        }

        public bool RunTask(ICharacter character)
        {
            string ret = SendBirdTool.MakeRetweet(character.auth, target_id);
            return ret.Contains("created_at");
        }

        public string ToInfo()
        {
            return "转推了：" + target_id;
        }
    }
}
