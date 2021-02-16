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

namespace BlueBirdFriend
{
	/// <summary>
	/// 辅助类，用于野蛮使用推特API，避免申请API KEY的麻烦和风险
	/// </summary>
    public static class SendBird
    {
		private static string ReadResponse(HttpWebResponse response)
		{
			using (Stream responseStream = response.GetResponseStream())
			{
				Stream streamToRead = responseStream;
				if (response.ContentEncoding.ToLower().Contains("gzip"))
				{
					streamToRead = new GZipStream(streamToRead, CompressionMode.Decompress);
				}
				else if (response.ContentEncoding.ToLower().Contains("deflate"))
				{
					streamToRead = new DeflateStream(streamToRead, CompressionMode.Decompress);
				}

				using (StreamReader streamReader = new StreamReader(streamToRead, Encoding.UTF8))
				{
					return streamReader.ReadToEnd();
				}
			}
		}
		/// <summary>
		/// 发送推文
		/// </summary>
		/// <param name="auth">角色认证信息</param>
		/// <param name="text">推文内容</param>
		/// <returns></returns>
		public static string MakeSimpleBird(IBirdAuth auth, string text)
		{
			HttpWebResponse response;
			string responseText;

			if (send_simple_bird(out response, auth, text))
			{
				responseText = ReadResponse(response);
				//Console.WriteLine(auth.id + " SendSimple: " + text);
				response.Close();
				return responseText;
			}
			return "";
		}
		/// <summary>
		/// 发送带回复推文
		/// </summary>
		/// <param name="auth">角色认证信息</param>
		/// <param name="text">推文内容</param>
		/// <param name="replay">回复对象ID</param>
		/// <returns></returns>
		public static string MakeSimpleBird(IBirdAuth auth, string text, string replay)
		{
			HttpWebResponse response;
			string responseText;
			if (send_simple_bird(out response, auth, text, replay))
			{
				responseText = ReadResponse(response);
				//Console.WriteLine(auth.id + " SendReply to " + replay + ": " + text);
				response.Close();
				return responseText;
			}
			return "";
		}
		private static bool send_simple_bird(out HttpWebResponse response, IBirdAuth auth, string text)
		{
			response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://twitter.com/i/api/1.1/statuses/update.json");

				request.KeepAlive = true;
				request.Headers.Set(HttpRequestHeader.Authorization, "Bearer AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA");
				request.Headers.Add("x-csrf-token", auth.x_csrf_token);
				request.Headers.Add("x-twitter-auth-type", @"OAuth2Session");
				request.Headers.Add("x-twitter-active-user", @"yes");
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
				request.ContentType = "application/x-www-form-urlencoded";
				request.Headers.Set(HttpRequestHeader.Cookie, auth.cookie);
				request.Method = "POST";
				request.ServicePoint.Expect100Continue = false;

				string body = @"include_profile_interstitial_type=1&include_blocking=1&include_blocked_by=1&include_followed_by=1&include_want_retweets=1&include_mute_edge=1&include_can_dm=1&include_can_media_tag=1&skip_status=1&cards_platform=Web-12&include_cards=1&include_ext_alt_text=true&include_quote_count=true&include_reply_count=1&tweet_mode=extended&simple_quoted_tweet=true&trim_user=false&include_ext_media_color=true&include_ext_media_availability=true&auto_populate_reply_metadata=false&batch_mode=off&status=" + text;
				byte[] postBytes = Encoding.UTF8.GetBytes(body);
				request.ContentLength = postBytes.Length;
				Stream stream = request.GetRequestStream();
				stream.Write(postBytes, 0, postBytes.Length);
				stream.Close();

				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
				else return false;
			}
			catch (Exception)
			{
				if (response != null) response.Close();
				return false;
			}

			return true;
		}
		private static bool send_simple_bird(out HttpWebResponse response, IBirdAuth auth, string text, string reply)
		{
			response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://twitter.com/i/api/1.1/statuses/update.json");

				request.KeepAlive = true;
				request.Headers.Set(HttpRequestHeader.Authorization, "Bearer AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA");
				request.Headers.Add("x-csrf-token", auth.x_csrf_token);
				request.Headers.Add("x-twitter-auth-type", @"OAuth2Session");
				request.Headers.Add("x-twitter-active-user", @"yes");
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
				request.ContentType = "application/x-www-form-urlencoded";
				request.Headers.Set(HttpRequestHeader.Cookie, auth.cookie);

				request.Method = "POST";
				request.ServicePoint.Expect100Continue = false;

				string body = @"include_profile_interstitial_type=1&include_blocking=1&include_blocked_by=1&include_followed_by=1&include_want_retweets=1&include_mute_edge=1&include_can_dm=1&include_can_media_tag=1&skip_status=1&cards_platform=Web-12&include_cards=1&include_ext_alt_text=true&include_quote_count=true&include_reply_count=1&tweet_mode=extended&simple_quoted_tweet=true&trim_user=false&include_ext_media_color=true&include_ext_media_availability=true&auto_populate_reply_metadata=true&batch_mode=subsequent&in_reply_to_status_id=" + reply + "&status=" + text;
				byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
				request.ContentLength = postBytes.Length;
				Stream stream = request.GetRequestStream();
				stream.Write(postBytes, 0, postBytes.Length);
				stream.Close();

				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
				else return false;
			}
			catch (Exception)
			{
				if (response != null) response.Close();
				return false;
			}

			return true;
		}

		/// <summary>
		/// 关注用户
		/// </summary>
		/// <param name="auth">角色认证信息</param>
		/// <param name="id">目标用户ID</param>
		public static string MakeFollow(IBirdAuth auth, string id)
		{
			HttpWebResponse response;
			string responseText;

			if (send_follow(out response, auth, id))
			{
				responseText = ReadResponse(response);
				//Console.WriteLine(auth.id + " Follow: " + id);
				response.Close();
				return responseText;
			}
			return "";
		}
		private static bool send_follow(out HttpWebResponse response, IBirdAuth auth, string id)
		{
			response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://twitter.com/i/api/1.1/friendships/create.json");

				request.KeepAlive = true;
				request.Headers.Add("x-csrf-token", auth.x_csrf_token);
				request.Headers.Set(HttpRequestHeader.Authorization, "Bearer AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA");
				request.ContentType = "application/x-www-form-urlencoded";
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
				request.Headers.Add("x-twitter-auth-type", @"OAuth2Session");
				request.Headers.Add("x-twitter-active-user", @"yes");
				request.Headers.Set(HttpRequestHeader.Cookie, auth.cookie);

				request.Method = "POST";
				request.ServicePoint.Expect100Continue = false;

				string body = @"include_profile_interstitial_type=1&include_blocking=1&include_blocked_by=1&include_followed_by=1&include_want_retweets=1&include_mute_edge=1&include_can_dm=1&include_can_media_tag=1&skip_status=1&id=" + id;
				byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
				request.ContentLength = postBytes.Length;
				Stream stream = request.GetRequestStream();
				stream.Write(postBytes, 0, postBytes.Length);
				stream.Close();

				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
				else return false;
			}
			catch (Exception)
			{
				if (response != null) response.Close();
				return false;
			}

			return true;
		}

		/// <summary>
		/// 点赞推文
		/// </summary>
		/// <param name="auth">角色认证信息</param>
		/// <param name="id">目标推文ID</param>
		public static string MakeLike(IBirdAuth auth, string id)
		{
			HttpWebResponse response;
			string responseText;

			if (send_like(out response, auth, id))
			{
				responseText = ReadResponse(response);
				//Console.WriteLine(auth.id + " MakeLike: " + id);
				response.Close();
				return responseText;
			}
			return "";
		}
		private static bool send_like(out HttpWebResponse response, IBirdAuth auth, string id)
		{
			response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://twitter.com/i/api/1.1/favorites/create.json");

				request.KeepAlive = true;
				request.Headers.Add("x-csrf-token", auth.x_csrf_token);
				request.Headers.Set(HttpRequestHeader.Authorization, "Bearer AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA");
				request.ContentType = "application/x-www-form-urlencoded";
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
				request.Headers.Add("x-twitter-auth-type", @"OAuth2Session");
				request.Headers.Add("x-twitter-active-user", @"yes");
				request.Headers.Set(HttpRequestHeader.Cookie, auth.cookie);

				request.Method = "POST";
				request.ServicePoint.Expect100Continue = false;

				string body = @"tweet_mode=extended&id=" + id;
				byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
				request.ContentLength = postBytes.Length;
				Stream stream = request.GetRequestStream();
				stream.Write(postBytes, 0, postBytes.Length);
				stream.Close();

				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
				else return false;
			}
			catch (Exception)
			{
				if (response != null) response.Close();
				return false;
			}

			return true;
		}

		/// <summary>
		/// 转发推文
		/// </summary>
		/// <param name="auth">角色认证信息</param>
		/// <param name="id">目标推文ID</param>
		public static string MakeRetweet(IBirdAuth auth, string id)
		{
			HttpWebResponse response;
			string responseText;

			if (send_retweet(out response, auth, id))
			{
				responseText = ReadResponse(response);
				//Console.WriteLine(auth.id + " MakeRetweet: " + id);
				response.Close();
				return responseText;
			}
			return "";
		}
		private static bool send_retweet(out HttpWebResponse response, IBirdAuth auth, string id)
		{
			response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://twitter.com/i/api/1.1/statuses/retweet.json");

				request.KeepAlive = true;
				request.Headers.Set(HttpRequestHeader.Authorization, "Bearer AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA");
				request.Headers.Add("x-csrf-token", auth.x_csrf_token);
				request.Headers.Add("x-twitter-auth-type", @"OAuth2Session");
				request.Headers.Add("x-twitter-active-user", @"yes");
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
				request.ContentType = "application/x-www-form-urlencoded";
				request.Headers.Set(HttpRequestHeader.Cookie, auth.cookie);

				request.Method = "POST";
				request.ServicePoint.Expect100Continue = false;

				string body = @"tweet_mode=extended&id=" + id;
				byte[] postBytes = Encoding.UTF8.GetBytes(body);
				request.ContentLength = postBytes.Length;
				Stream stream = request.GetRequestStream();
				stream.Write(postBytes, 0, postBytes.Length);
				stream.Close();

				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
				else return false;
			}
			catch (Exception)
			{
				if (response != null) response.Close();
				return false;
			}

			return true;
		}

		/// <summary>
		/// 获取推文第一页内容
		/// </summary>
		/// <param name="auth">角色认证信息</param>
		/// <param name="id">目标推文ID</param>
		public static string GetTweet(IBirdAuth auth, string id)
		{
			HttpWebResponse response;
			string responseText;

			if (get_tweet(out response, auth, id))
			{
				responseText = ReadResponse(response);
				//Console.WriteLine(auth.id + " GetTweet: " + id);
				response.Close();
				return responseText;
			}
			return "";
		}
		private static bool get_tweet(out HttpWebResponse response, IBirdAuth auth, string id)
		{
			response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://twitter.com/i/api/2/timeline/conversation/" + id + ".json?include_profile_interstitial_type=1&include_blocking=1&include_blocked_by=1&include_followed_by=1&include_want_retweets=1&include_mute_edge=1&include_can_dm=1&include_can_media_tag=1&skip_status=1&cards_platform=Web-12&include_cards=1&include_ext_alt_text=true&include_quote_count=true&include_reply_count=1&tweet_mode=extended&include_entities=true&include_user_entities=true&include_ext_media_color=true&include_ext_media_availability=true&send_error_codes=true&simple_quoted_tweet=true&count=20&include_ext_has_birdwatch_notes=false&ext=mediaStats%2ChighlightedLabel");

				request.KeepAlive = true;
				request.Headers.Set(HttpRequestHeader.Authorization, "Bearer AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA");
				request.Headers.Add("x-csrf-token", auth.x_csrf_token);
				request.Headers.Add("x-twitter-auth-type", @"OAuth2Session");
				request.Headers.Add("x-twitter-active-user", @"yes");
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
				request.Headers.Set(HttpRequestHeader.Cookie, auth.cookie);

				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
				else return false;
			}
			catch (Exception)
			{
				if (response != null) response.Close();
				return false;
			}

			return true;
		}

		public static string GetHome(IBirdAuth auth)
		{
			HttpWebResponse response;
			string responseText;

			if (get_home(out response, auth))
			{
				responseText = ReadResponse(response);
				//Console.WriteLine(auth.id + " GetHome");
				response.Close();
				return responseText;
			}
			return "";
		}
		private static bool get_home(out HttpWebResponse response, IBirdAuth auth)
		{
			response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://twitter.com/i/api/2/timeline/home.json?include_profile_interstitial_type=1&include_blocking=1&include_blocked_by=1&include_followed_by=1&include_want_retweets=1&include_mute_edge=1&include_can_dm=1&include_can_media_tag=1&skip_status=1&cards_platform=Web-12&include_cards=1&include_ext_alt_text=true&include_quote_count=true&include_reply_count=1&tweet_mode=extended&include_entities=true&include_user_entities=true&include_ext_media_color=true&include_ext_media_availability=true&send_error_codes=true&simple_quoted_tweet=true&earned=1&count=20&lca=true&ext=mediaStats%2ChighlightedLabel");

				request.KeepAlive = true;
				request.Headers.Set(HttpRequestHeader.Authorization, "Bearer AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA");
				request.Headers.Add("x-csrf-token", auth.x_csrf_token);
				request.Headers.Add("x-twitter-auth-type", @"OAuth2Session");
				request.Headers.Add("x-twitter-active-user", @"yes");
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
				request.Headers.Set(HttpRequestHeader.Cookie, auth.cookie);

				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
				else return false;
			}
			catch (Exception)
			{
				if (response != null) response.Close();
				return false;
			}

			return true;
		}

		public static string GetNewTagTwitter(IBirdAuth auth, string tag,bool isnew=true)
		{
			HttpWebResponse response;
			string responseText;

			if (get_new_tag_twitter(out response, auth, HttpUtility.UrlEncode(tag), isnew))
			{
				responseText = ReadResponse(response);
				response.Close();
				return responseText;
			}
			return "";
		}
		private static bool get_new_tag_twitter(out HttpWebResponse response, IBirdAuth auth, string tag,bool isnew)
		{
			response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://twitter.com/i/api/2/search/adaptive.json?include_profile_interstitial_type=1&include_blocking=1&include_blocked_by=1&include_followed_by=1&include_want_retweets=1&include_mute_edge=1&include_can_dm=1&include_can_media_tag=1&skip_status=1&cards_platform=Web-12&include_cards=1&include_ext_alt_text=true&include_quote_count=true&include_reply_count=1&tweet_mode=extended&include_entities=true&include_user_entities=true&include_ext_media_color=true&include_ext_media_availability=true&send_error_codes=true&simple_quoted_tweet=true&q=" + tag + (isnew? "&tweet_search_mode=live" : "")+"&count=20&query_source=recent_search_click&pc=1&spelling_corrections=1&ext=mediaStats%2ChighlightedLabel");

				request.KeepAlive = true;
				request.Headers.Set(HttpRequestHeader.Authorization, "Bearer AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA");
				request.Headers.Add("x-csrf-token", auth.x_csrf_token);
				request.ContentType = "application/x-www-form-urlencoded";
				request.Headers.Add("x-twitter-auth-type", @"OAuth2Session");
				request.Headers.Add("x-twitter-active-user", @"yes");
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
				request.Headers.Set(HttpRequestHeader.Cookie, auth.cookie);

				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
				else return false;
			}
			catch (Exception)
			{
				if (response != null) response.Close();
				return false;
			}

			return true;
		}

		public static string GetProfile(IBirdAuth auth, string id,bool is_screen_name=false)
		{
			HttpWebResponse response;
			string responseText;

			if (send_profile(out response, auth, id,is_screen_name))
			{
				responseText = ReadResponse(response);
				response.Close();
				return responseText;
			}
			return "";
		}
		private static bool send_profile(out HttpWebResponse response, IBirdAuth auth,string id, bool is_screen_name)
		{
			response = null;
			string text = (is_screen_name) ? "&screen_name=" + id : "&user_id=" + id;
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitter.com/1.1/users/show.json?include_entities=true" + text);

				request.KeepAlive = true;
				request.Headers.Set(HttpRequestHeader.Authorization, "Bearer AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA");
				request.Headers.Add("x-twitter-client-language", @"zh-cn");
				request.Headers.Add("x-csrf-token", auth.x_csrf_token);
				request.Headers.Add("x-twitter-auth-type", @"OAuth2Session");
				request.Headers.Add("x-twitter-active-user", @"yes");
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.104 Safari/537.36";
				request.Headers.Set(HttpRequestHeader.Cookie, auth.cookie);

				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
				else return false;
			}
			catch (Exception)
			{
				if (response != null) response.Close();
				return false;
			}

			return true;

		}

		public static string MakeSample(IBirdAuth auth, string url, string body)
		{
			HttpWebResponse response;
			string responseText;

			if (send_sample(out response, auth, url,body))
			{
				responseText = ReadResponse(response);
				response.Close();
				return responseText;
			}
			return "";
		}
		private static bool send_sample(out HttpWebResponse response, IBirdAuth auth, string url,string body)
		{
			response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

				request.KeepAlive = true;
				request.Headers.Add("x-csrf-token", auth.x_csrf_token);
				request.Headers.Set(HttpRequestHeader.Authorization, "Bearer AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA");
				request.ContentType = "application/x-www-form-urlencoded";
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
				request.Headers.Add("x-twitter-auth-type", @"OAuth2Session");
				request.Headers.Add("x-twitter-active-user", @"yes");
				request.Headers.Set(HttpRequestHeader.Cookie, auth.cookie);

				request.Method = "POST";
				request.ServicePoint.Expect100Continue = false;

				byte[] postBytes = Encoding.UTF8.GetBytes(body);
				request.ContentLength = postBytes.Length;
				Stream stream = request.GetRequestStream();
				stream.Write(postBytes, 0, postBytes.Length);
				stream.Close();

				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
				else return false;
			}
			catch (Exception)
			{
				if (response != null) response.Close();
				return false;
			}

			return true;
		}
	}
}
