using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BlueBird_Play
{
    /// <summary>
    /// PlayScheduleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PlayScheduleWindow : Window
    {
		public PlaySchedule.TagMonitorSetting presetting = new PlaySchedule.TagMonitorSetting
		{
			target = "#Biden",
			time = 180,
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
			tweetlife = 4,
			reply_r_favorited = 0,
			reply_r_retweeted = 0,
			reply_r_reply = 0,
			reply_r_quote = 0,
			reply_r_media = false,
			reply_r_users = false,
			reply_r_hashtag = false
		};
		public PlayScheduleWindow()
        {
            InitializeComponent();
			setting_target.Text = presetting.target;
			setting_time.Text = presetting.time.ToString();
			setting_life.Text = presetting.tweetlife.ToString();

			setting_isFavorite.IsChecked = presetting.isFavorite;
			settinf_isRetweet.IsChecked = presetting.isRetweet;
			setting_isFo.IsChecked = presetting.isFo;
			setting_isCopy.IsChecked = presetting.isCopy;
			setting_isReply.IsChecked = presetting.isReply;

			setting_favorite_r_favorited.Text = presetting.favorite_r_favorited.ToString();
			setting_favorite_r_retweeted.Text = presetting.favorite_r_retweeted.ToString();
			setting_favorite_r_reply.Text = presetting.favorite_r_reply.ToString();
			setting_favorite_r_quote.Text = presetting.favorite_r_quote.ToString();

			setting_retweet_r_favorited.Text = presetting.retweet_r_favorited.ToString();
			setting_retweet_r_retweeted.Text = presetting.retweet_r_retweeted.ToString();
			setting_retweet_r_reply.Text = presetting.retweet_r_reply.ToString();
			setting_retweet_r_quote.Text = presetting.retweet_r_quote.ToString();

			setting_follow_r_favorited.Text = presetting.follow_r_favorited.ToString();
			setting_follow_r_retweeted.Text = presetting.follow_r_retweeted.ToString();
			setting_follow_r_reply.Text = presetting.follow_r_reply.ToString();
			setting_follow_r_quote.Text = presetting.follow_r_quote.ToString();

			setting_copy_r_favorited.Text = presetting.copy_r_favorited.ToString();
			setting_copy_r_retweeted.Text = presetting.copy_r_retweeted.ToString();
			setting_copy_r_reply.Text = presetting.copy_r_reply.ToString();
			setting_copy_r_quote.Text = presetting.copy_r_quote.ToString();
			setting_copy_r_media.IsChecked = presetting.copy_r_media;
			setting_copy_r_users.IsChecked = presetting.copy_r_users;
			setting_copy_r_hashtag.IsChecked = presetting.copy_r_hashtag;

			setting_reply_r_favorited.Text = presetting.reply_r_favorited.ToString();
			setting_reply_r_retweeted.Text = presetting.reply_r_retweeted.ToString();
			setting_reply_r_reply.Text = presetting.reply_r_reply.ToString();
			setting_reply_r_quote.Text = presetting.reply_r_quote.ToString();
			setting_reply_r_media.IsChecked = presetting.reply_r_media;
			setting_reply_r_users.IsChecked = presetting.reply_r_users;
			setting_reply_r_hashtag.IsChecked = presetting.reply_r_hashtag;
		}

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
			int tmp;
			presetting.target = setting_target.Text;
			presetting.time = (int.TryParse(setting_time.Text, out tmp)) ? tmp : presetting.time;
			presetting.tweetlife = (int.TryParse(setting_life.Text, out tmp)) ? tmp : presetting.tweetlife;

			presetting.isFavorite = setting_isFavorite.IsChecked == true;
			presetting.isRetweet = settinf_isRetweet.IsChecked == true;
			presetting.isFo = setting_isFo.IsChecked == true;
			presetting.isCopy = setting_isCopy.IsChecked == true;
			presetting.isReply = setting_isReply.IsChecked == true;

			presetting.favorite_r_favorited = (int.TryParse(setting_favorite_r_favorited.Text, out tmp)) ? tmp : presetting.favorite_r_favorited;
			presetting.favorite_r_retweeted = (int.TryParse(setting_favorite_r_retweeted.Text, out tmp)) ? tmp : presetting.favorite_r_retweeted;
			presetting.favorite_r_reply = (int.TryParse(setting_favorite_r_reply.Text, out tmp)) ? tmp : presetting.favorite_r_reply;
			presetting.favorite_r_quote = (int.TryParse(setting_favorite_r_quote.Text, out tmp)) ? tmp : presetting.favorite_r_quote;

			presetting.retweet_r_favorited = (int.TryParse(setting_retweet_r_favorited.Text, out tmp)) ? tmp : presetting.retweet_r_favorited;
			presetting.retweet_r_retweeted = (int.TryParse(setting_retweet_r_retweeted.Text, out tmp)) ? tmp : presetting.retweet_r_retweeted;
			presetting.retweet_r_reply = (int.TryParse(setting_retweet_r_reply.Text, out tmp)) ? tmp : presetting.retweet_r_reply;
			presetting.retweet_r_quote = (int.TryParse(setting_retweet_r_quote.Text, out tmp)) ? tmp : presetting.retweet_r_quote;

			presetting.follow_r_favorited = (int.TryParse(setting_follow_r_favorited.Text, out tmp)) ? tmp : presetting.follow_r_favorited;
			presetting.follow_r_retweeted = (int.TryParse(setting_follow_r_retweeted.Text, out tmp)) ? tmp : presetting.follow_r_retweeted;
			presetting.follow_r_reply = (int.TryParse(setting_follow_r_reply.Text, out tmp)) ? tmp : presetting.follow_r_reply;
			presetting.follow_r_quote = (int.TryParse(setting_follow_r_quote.Text, out tmp)) ? tmp : presetting.follow_r_quote;

			presetting.copy_r_favorited = (int.TryParse(setting_copy_r_favorited.Text, out tmp)) ? tmp : presetting.copy_r_favorited;
			presetting.copy_r_retweeted = (int.TryParse(setting_copy_r_retweeted.Text, out tmp)) ? tmp : presetting.copy_r_retweeted;
			presetting.copy_r_reply = (int.TryParse(setting_copy_r_reply.Text, out tmp)) ? tmp : presetting.copy_r_reply;
			presetting.copy_r_quote = (int.TryParse(setting_copy_r_quote.Text, out tmp)) ? tmp : presetting.copy_r_quote;
			presetting.copy_r_media = setting_copy_r_media.IsChecked == true;
			presetting.copy_r_users = setting_copy_r_users.IsChecked == true;
			presetting.copy_r_hashtag = setting_copy_r_hashtag.IsChecked == true;

			presetting.reply_r_favorited = (int.TryParse(setting_reply_r_favorited.Text, out tmp)) ? tmp : presetting.reply_r_favorited;
			presetting.reply_r_retweeted = (int.TryParse(setting_reply_r_retweeted.Text, out tmp)) ? tmp : presetting.reply_r_retweeted;
			presetting.reply_r_reply = (int.TryParse(setting_reply_r_reply.Text, out tmp)) ? tmp : presetting.reply_r_reply;
			presetting.reply_r_quote = (int.TryParse(setting_reply_r_quote.Text, out tmp)) ? tmp : presetting.reply_r_quote;
			presetting.reply_r_media = setting_reply_r_media.IsChecked == true;
			presetting.reply_r_users = setting_reply_r_users.IsChecked == true;
			presetting.reply_r_hashtag = setting_reply_r_hashtag.IsChecked == true;

			DialogResult = true;
			Close();
		}
    }
}
