using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace BlueBirdFriend
{
    static class Tool
    {
        public static Action<string> Log = Console.WriteLine;
        public static string GetConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
		private static string GetInfoFromGit()
		{
			HttpWebResponse response;
			string responseText;

			if (Request_raw_githubusercontent_com(out response))
			{
				responseText = ReadResponse(response);
				response.Close();
				return responseText;
			}
			return "";
		}

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

		private static bool Request_raw_githubusercontent_com(out HttpWebResponse response)
		{
			response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://raw.githubusercontent.com/MengLuwa/XiangTing/master/%E5%85%AC%E5%91%8A.txt");
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
		public static string GetInfo()
        {
			return GetInfoFromGit();

		}
    }
}
