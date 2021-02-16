using BlueBird_Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBirdFriend
{
    class BirdAuth : IBirdAuth
    {
        public string auth_token { get; }
        public string x_csrf_token { get; }
        public string twitter_sess { get; }

        public string id { get; }
        public string cookie { get { return "auth_token=" + auth_token + "; " + "ct0=" + x_csrf_token + "; " + " _twitter_sess=" + twitter_sess; } }

        public BirdAuth(string auth_token_, string ct0_, string twitter_sess_, string id_)
        {
            auth_token = auth_token_;
            x_csrf_token = ct0_;
            twitter_sess = twitter_sess_;
            id = id_;
        }
    }
}
