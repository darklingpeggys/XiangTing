using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Plugin
{
    public interface IBirdAuth
    {
        string auth_token { get; }
        string x_csrf_token { get; }
        string twitter_sess { get; }

        string id { get; }
        string cookie { get; }

    }
}
