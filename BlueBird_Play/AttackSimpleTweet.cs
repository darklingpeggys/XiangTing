using System;
using LitJson;
using BlueBird_Plugin;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Play
{
    class AttackSimpleTweet : ITask
    {
        public bool short_or_long { get { return false; } }
        ITask.HALDLE_INFO character_id;
        public ITask.HALDLE_INFO handle_info { get { return character_id; } }
        string text = "";
        List<string> tag;
        int life = 1;
        public AttackSimpleTweet(ITask.HALDLE_INFO character, string tweet_text, List<string> tag_list, int task_life)
        {
            character_id = character;
            text = tweet_text;
            tag = tag_list;
            life = task_life;
        }
        public bool RunTask(ICharacter character)
        {
            string tweettext = text;
            foreach (string t in tag)
            {
                tweettext += " #" + t;
            }
            string ret = SendBirdTool.MakeSimpleBird(character.auth, tweettext);
            JsonData json = JsonMapper.ToObject(ret);
            if (json.ContainsKey("id_str"))
            {
                character.cool = 5;

                LikeTask like = new LikeTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.ALL }, json["id_str"].ToString());
                TaskQueueTool.AddImportantTask(like);

                RetweetTask retweet = new RetweetTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.ALL_WITHOUT, without_id = new string[] { character.character_id } }, json["id_str"].ToString());
                TaskQueueTool.AddImportantTask(retweet);

                life--;
                if (life > 0)
                {
                    AttackReplyTweet reply = new AttackReplyTweet(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.RANDOM_MUL_WITHOUT, count = life, without_id = new string[] { character.character_id } }, json["id_str"].ToString(), Bullet.GetBullet(text), tag, life);
                    TaskQueueTool.AddImportantTask(reply);
                }
                return true;
            }
            return false;
        }
        public string ToInfo()
        {
            return "攻击式地发送了推文：" + text;
        }
    }
}
