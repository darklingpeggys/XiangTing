using BlueBird_Plugin;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Play
{
    class ReplyTweetTask : ITask
    {
        public bool short_or_long { get { return false; } }
        Random random = new Random(DateTime.Now.Millisecond + DateTime.Now.Second * 1000);
        ITask.HALDLE_INFO character_id;
        public ITask.HALDLE_INFO handle_info { get { return character_id; } }
        string text = "";
        List<string> tag;
        int life = 1;
        string target = "";
        public ReplyTweetTask(ITask.HALDLE_INFO character, string target_id,string tweet_text, List<string> tag_list, int task_life)
        {
            character_id = character;
            text = tweet_text;
            tag = tag_list;
            life = task_life;
            target = target_id;
        }
        public bool RunTask(ICharacter character)
        {
            string tweettext = text;
            foreach (string t in tag)
            {
                tweettext += " #" + t;
            }
            string ret = SendBirdTool.MakeReplyBird(character.auth, tweettext, target);
            JsonData json = JsonMapper.ToObject(ret);
            if (json.ContainsKey("id_str"))
            {
                LikeTask like = new LikeTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.ALL }, json["id_str"].ToString());
                TaskQueueTool.AddImportantTask(like);

                RetweetTask retweet = new RetweetTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.ALL_WITHOUT, without_id = new string[] { character.character_id } }, json["id_str"].ToString());
                TaskQueueTool.AddImportantTask(retweet);

                life--;
                if (life > 0)
                {
                    ReplyTweetTask reply = new ReplyTweetTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.RANDOM_MUL_WITHOUT, count = life - random.Next(0, life), without_id = new string[] { character.character_id } }, json["id_str"].ToString(), Bullet.GetBullet(text), tag, life - random.Next(0, life));
                    TaskQueueTool.AddImportantTask(reply);
                }
                return true;
            }
            return false;
        }
        public string ToInfo()
        {
            return "回复了" + target + "推文：" + text;
        }
    }
}
