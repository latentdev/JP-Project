using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATA.Helper
{
    public class oath
    {
        string CONSUMER_KEY;
        string CONSUMER_SECRET;
        string ACCESS_TOKEN;
        string ACCESS_SECRET;

        public oath()
        {
            CONSUMER_KEY = "6AJKMgHTk5L0qUZX1tumVrcf6";
            CONSUMER_SECRET = "Z6LQQdlzH7vQca56wRdTaLaokvrORtWKUzYovwLLKXDwnGh9O2";
            ACCESS_TOKEN = "23288297-GKQtuIQzm8xEXGrpocnYiqn2HGMLE84BqWvcYH11n";
            ACCESS_SECRET = "r1KgCO7P3TxkhJKzmkwbUPCCWMsy9GYKf1EMMXFUA0h1t";
        }

        public oath(string in_consumer_key, string in_consumer_secret, string in_access_token, string in_access_secret)
        {
            CONSUMER_KEY = in_consumer_key;
            CONSUMER_SECRET = in_consumer_secret;
            ACCESS_TOKEN = in_access_token;
            ACCESS_SECRET = in_access_secret;
        }

        public string get_consumer_key()
        {
            return CONSUMER_KEY;
        }

        public string get_consumer_secret()
        {
            return CONSUMER_SECRET;
        }

        public string get_access_token()
        {
            return ACCESS_TOKEN;
        }

        public string get_access_secret()
        {
            return ACCESS_SECRET;
        }
    }
}