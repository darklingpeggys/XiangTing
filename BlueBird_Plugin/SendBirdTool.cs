using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BlueBird_Plugin
{
	/// <summary>
	/// 辅助类，用于野蛮使用推特API，避免申请API KEY的麻烦和风险
	/// </summary>
    public static class SendBirdTool
    {

		public static Func<IBirdAuth, string, string> MakeSimpleBird;

		public static Func<IBirdAuth, string, string, string> MakeReplyBird;

		public static Func<IBirdAuth, string, string> MakeFollow;

		public static Func<IBirdAuth, string, string> MakeLike;

		public static Func<IBirdAuth, string, string> MakeRetweet;

		public static Func<IBirdAuth, string, string> GetTweet;

		public static Func<IBirdAuth, string> GetHome;

		public static Func<IBirdAuth, string,bool, string> GetNewTagTwitter;

		public static Func<IBirdAuth, string,bool, string> GetProfile;

		public static Func<IBirdAuth, string, string, string> MakeSample;
	}
}
