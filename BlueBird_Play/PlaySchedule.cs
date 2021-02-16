using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueBird_Plugin;
using LitJson;

namespace BlueBird_Play
{
    public class PlaySchedule : ISchedule
    {
        bool isCompleted = false;
		int LikeTaskCount = 0;
		int RetweetTaskCount = 0;
		int FollowTaskCount = 0;
		int SendTaskCount = 0;
		int ReplyTaskCount = 0;
		int last_time = 0;
		static List<string> historytweet = new List<string>();
		public struct TagMonitorSetting
		{
			/// <summary>
			/// 搜索目标
			/// </summary>
			public string target;
			/// <summary>
			/// 扫描间隔
			/// </summary>
			public int time;
			/// <summary>
			/// 是否关注
			/// </summary>
			public bool isFo;
			/// <summary>
			/// 是否点赞
			/// </summary>
			public bool isFavorite;
			/// <summary>
			/// 是否转推
			/// </summary>
			public bool isRetweet;
			/// <summary>
			/// 是否复刻高热推文
			/// </summary>
			public bool isCopy;
			/// <summary>
			/// 是否复刻热评
			/// </summary>
			public bool isReply;
			/// <summary>
			/// 点赞推文要求点赞数
			/// </summary>
			public int favorite_r_favorited;
			/// <summary>
			/// 点赞推文要求转推数
			/// </summary>
			public int favorite_r_retweeted;
			/// <summary>
			/// 点赞推文要求回复数
			/// </summary>
			public int favorite_r_reply;
			/// <summary>
			/// 点赞推文要求引用数
			/// </summary>
			public int favorite_r_quote;
			/// <summary>
			/// 转推要求点赞数
			/// </summary>
			public int retweet_r_favorited;
			/// <summary>
			/// 转推要求转推数
			/// </summary>
			public int retweet_r_retweeted;
			/// <summary>
			/// 转推要求回复数
			/// </summary>
			public int retweet_r_reply;
			/// <summary>
			/// 转推要求引用数
			/// </summary>
			public int retweet_r_quote;
			/// <summary>
			/// 关注要求点赞数
			/// </summary>
			public int follow_r_favorited;
			/// <summary>
			/// 关注要求转推数
			/// </summary>
			public int follow_r_retweeted;
			/// <summary>
			/// 关注要求回复数
			/// </summary>
			public int follow_r_reply;
			/// <summary>
			/// 关注要求引用数
			/// </summary>
			public int follow_r_quote;
			/// <summary>
			/// 复刻推文是否包含媒体资源内容
			/// </summary>
			public bool copy_r_media;
			/// <summary>
			/// 复刻推文是否包括艾特他人
			/// </summary>
			public bool copy_r_users;
			/// <summary>
			/// 复刻推文是否包含Tag
			/// </summary>
			public bool copy_r_hashtag;
			/// <summary>
			/// 复刻推文要求点赞数
			/// </summary>
			public int copy_r_favorited;
			/// <summary>
			/// 复刻推文要求转推数
			/// </summary>
			public int copy_r_retweeted;
			/// <summary>
			/// 复刻推文要求回复数
			/// </summary>
			public int copy_r_reply;
			/// <summary>
			/// 复刻推文要求引用数
			/// </summary>
			public int copy_r_quote;
			/// <summary>
			/// 推文任务生命
			/// </summary>
			public int tweetlife;
			/// <summary>
			/// 复刻热评要求点赞数
			/// </summary>
			public int reply_r_favorited;
			/// <summary>
			/// 复刻热评要求转推数
			/// </summary>
			public int reply_r_retweeted;
			/// <summary>
			/// 复刻热评要求回复数
			/// </summary>
			public int reply_r_reply;
			/// <summary>
			/// 复刻热评要求引用数
			/// </summary>
			public int reply_r_quote;
			/// <summary>
			/// 复刻热评是否包含媒体资源内容
			/// </summary>
			public bool reply_r_media;
			/// <summary>
			/// 复刻热评是否包含艾特他人
			/// </summary>
			public bool reply_r_users;
			/// <summary>
			/// 复刻热评是否包含Tag
			/// </summary>
			public bool reply_r_hashtag;
		}
		public TagMonitorSetting setting = new TagMonitorSetting
		{
			target="Biden",
			time=180,
			isFo = false,
			isFavorite = false,
			isRetweet = false,
			isCopy = false,
			isReply = false,
			favorite_r_favorited = 0,
			favorite_r_retweeted = 0,
			favorite_r_reply = 0,
			favorite_r_quote = 0,
			retweet_r_favorited = 0,
			retweet_r_retweeted = 0,
			retweet_r_reply = 0,
			retweet_r_quote = 0,
			follow_r_favorited = 0,
			follow_r_retweeted = 0,
			follow_r_reply = 0,
			follow_r_quote = 0,
			copy_r_media = false,
			copy_r_users = false,
			copy_r_hashtag = false,
			copy_r_favorited = 0,
			copy_r_retweeted = 0,
			copy_r_reply = 0,
			copy_r_quote = 0,
			tweetlife=4,
			reply_r_favorited = 0,
			reply_r_retweeted = 0,
			reply_r_reply = 0,
			reply_r_quote = 0,
			reply_r_media = false,
			reply_r_users = false,
			reply_r_hashtag = false
		};
		public bool completed { get { return isCompleted; } }

        public string name { get { return "自动活跃任务"; } }
        public PlaySchedule()
        {
			
        }
        public void End()
        {
            isCompleted = true;
        }
		int GetTime()
		{
			return DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600 + DateTime.Now.Day * 3600 * 24;
		}

		public List<ITask> GetTask(ICharacter character)
        {
			List<ITask> tasks = new List<ITask>();
			int now = GetTime();
			if (now - last_time < setting.time)
            {
				return tasks;
            }
			string result = SendBirdTool.GetNewTagTwitter(character.auth, setting.target,false);
            JsonData data = JsonMapper.ToObject(result);
            if (!data.ContainsKey("globalObjects"))
            {
				last_time = GetTime();
				return tasks;
            }
            JsonData tweets = data["globalObjects"]["tweets"];
            JsonData users = data["globalObjects"]["users"];
			foreach (KeyValuePair<string, JsonData> tweet in tweets)
            {
				if (setting.isFavorite 
					&& !(bool)tweet.Value["favorited"] 
					&& (int)tweet.Value["favorite_count"] >= setting.favorite_r_favorited 
					&& (int)tweet.Value["retweet_count"] >= setting.favorite_r_retweeted 
					&& (int)tweet.Value["reply_count"] >= setting.favorite_r_reply 
					&& (int)tweet.Value["quote_count"] >= setting.favorite_r_quote)
				{
					LikeTask likeTask = new LikeTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.TARGET,id=character.character_id }, tweet.Key);
					tasks.Add(likeTask);
					LikeTaskCount++;
				}

				if (setting.isRetweet 
					&& !(bool)tweet.Value["retweeted"] 
					&& (int)tweet.Value["favorite_count"] >= setting.retweet_r_favorited 
					&& (int)tweet.Value["retweet_count"] >= setting.retweet_r_retweeted 
					&& (int)tweet.Value["reply_count"] >= setting.retweet_r_reply 
					&& (int)tweet.Value["quote_count"] >= setting.retweet_r_quote)
				{
					RetweetTask retweetTask = new RetweetTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.TARGET, id = character.character_id }, tweet.Key);
					tasks.Add(retweetTask);
					RetweetTaskCount++;
				}

				string user_id = tweet.Value["user_id_str"].ToString();
				if (setting.isFo 
					&& (!(bool)users[user_id]["following"]) 
					&& (!(bool)users[user_id]["follow_request_sent"]) 
					&& (int)tweet.Value["favorite_count"] >= setting.follow_r_favorited 
					&& (int)tweet.Value["retweet_count"] >= setting.follow_r_retweeted 
					&& (int)tweet.Value["reply_count"] >= setting.follow_r_reply 
					&& (int)tweet.Value["quote_count"] >= setting.follow_r_quote)
				{
					FollowTask followTask = new FollowTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.TARGET, id = character.character_id }, user_id);
					tasks.Add(followTask);
					FollowTaskCount++;
				}

				if (setting.isCopy
					&& !historytweet.Contains(tweet.Key)
					&& tweet.Value["in_reply_to_status_id_str"] == null 
					&& (setting.copy_r_media || (!tweet.Value["entities"].ContainsKey("media"))) 
					&& (setting.copy_r_users || (!tweet.Value["entities"].ContainsKey("user_mentions"))) 
					&& (setting.copy_r_hashtag || (!tweet.Value["entities"].ContainsKey("hashtags"))) 
					&& (int)tweet.Value["favorite_count"] >= setting.copy_r_favorited 
					&& (int)tweet.Value["retweet_count"] >= setting.copy_r_retweeted 
					&& (int)tweet.Value["reply_count"] >= setting.copy_r_reply 
					&& (int)tweet.Value["quote_count"] >= setting.copy_r_quote)
				{
					SimpletweetTask simpletweetTask = new SimpletweetTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.TARGET, id = character.character_id }, tweet.Value["full_text"].ToString(), new List<string>(), setting.tweetlife);
					tasks.Add(simpletweetTask);
					historytweet.Add(tweet.Key);
					SendTaskCount++;
				}

				if (setting.isReply)
				{
					string ret = SendBirdTool.GetTweet(character.auth, tweet.Key);
					JsonData retdata = JsonMapper.ToObject(ret);
					if (retdata.ContainsKey("globalObjects") && retdata["globalObjects"]["tweets"].Count > 0)
					{
						JsonData replytweets = retdata["globalObjects"]["tweets"];
						string f = "";
						int max = 0;
						string id = "";
						foreach (KeyValuePair<string, JsonData> replytweet in replytweets)
						{
							if (!historytweet.Contains(replytweet.Key)
								&& (int)replytweet.Value["favorite_count"] > max 
								&& replytweet.Value.ContainsKey("in_reply_to_status_id_str") 
								&& replytweet.Value["in_reply_to_status_id_str"].ToString() == tweet.Key 
								&& (setting.reply_r_media || (!replytweet.Value["entities"].ContainsKey("media"))) 
								&& (setting.reply_r_users || (!replytweet.Value["entities"].ContainsKey("user_mentions"))) 
								&& (setting.reply_r_hashtag || (!replytweet.Value["entities"].ContainsKey("hashtags"))) 
								&& (int)replytweet.Value["favorite_count"] >= setting.reply_r_favorited 
								&& (int)replytweet.Value["retweet_count"] >= setting.reply_r_retweeted 
								&& (int)replytweet.Value["reply_count"] >= setting.reply_r_reply 
								&& (int)replytweet.Value["quote_count"] >= setting.reply_r_quote)
							{
								max = (int)replytweet.Value["favorite_count"];
								f = replytweet.Value["full_text"].ToString();
								id = replytweet.Key;
							}
						}
						if (f != "")
						{
							ReplyTweetTask replyTweetTask = new ReplyTweetTask(new ITask.HALDLE_INFO { type = ITask.HALDLE_TYPE.TARGET, id = character.character_id }, tweet.Key, f, new List<string>(), setting.tweetlife);
							tasks.Add(replyTweetTask);
							historytweet.Add(id);
							ReplyTaskCount++;
						}
					}
				}
			}
			last_time = GetTime();
			return tasks;
        }

        public string ToInfo()
        {
			string info = "监控内容：" + setting.target + "\n";
			info += "已发布点赞任务：" + LikeTaskCount.ToString() + "\n";
			info += "已发布转推任务：" + RetweetTaskCount.ToString() + "\n";
			info += "已发布关注任务：" + FollowTaskCount.ToString() + "\n";
			info += "已发布发推任务：" + SendTaskCount.ToString() + "\n";
			info += "已发布回复任务：" + ReplyTaskCount.ToString() + "\n";
            if (setting.isFavorite)
            {
				info += "点赞要求至少 "
					+ setting.favorite_r_favorited.ToString() + " 个点赞，"
					+ setting.favorite_r_retweeted.ToString() + " 个转推，"
					+ setting.follow_r_reply.ToString() + " 个回复，"
					+ setting.follow_r_quote.ToString() + " 个引用" + "\n";
			}
            if (setting.isRetweet)
            {
				info += "转推要求至少 "
					+ setting.retweet_r_favorited.ToString() + " 个点赞，"
					+ setting.retweet_r_retweeted.ToString() + " 个转推，"
					+ setting.retweet_r_reply.ToString() + " 个回复，"
					+ setting.retweet_r_quote.ToString() + " 个引用" + "\n";
			}
            if (setting.isFo)
            {
				info += "关注要求至少 "
					+ setting.follow_r_favorited.ToString() + " 个点赞，"
					+ setting.follow_r_retweeted.ToString() + " 个转推，"
					+ setting.follow_r_reply.ToString() + " 个回复，"
					+ setting.follow_r_quote.ToString() + " 个引用" + "\n";
			}
            if (setting.isCopy)
            {
				info += "复刻热推要求至少 "
					+ setting.copy_r_favorited.ToString() + " 个点赞，"
					+ setting.copy_r_retweeted.ToString() + " 个转推，"
					+ setting.copy_r_reply.ToString() + " 个回复，"
					+ setting.copy_r_quote.ToString() + " 个引用"
					+ (setting.copy_r_users ? "" : "，不包含艾特")
					+ (setting.copy_r_hashtag ? "" : "，不包含Tag")
					+ (setting.copy_r_media ? "" : "，不包含媒体资源") + "\n";
			}
			if (setting.isReply)
			{
				info += "复刻热评要求至少 "
					+ setting.reply_r_favorited.ToString() + " 个点赞，"
					+ setting.reply_r_retweeted.ToString() + " 个转推，"
					+ setting.reply_r_reply.ToString() + " 个回复，"
					+ setting.reply_r_quote.ToString() + " 个引用"
					+ (setting.reply_r_users ? "" : "，不包含艾特")
					+ (setting.reply_r_hashtag ? "" : "，不包含Tag")
					+ (setting.reply_r_media ? "" : "，不包含媒体资源") + "\n";
			}
			return info;
        }
    }
}
