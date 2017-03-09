using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATA.Helper
{
    public class oath
    {
        static string CONSUMER_KEY;
        static string CONSUMER_SECRET;
        static string ACCESS_TOKEN;
        static string ACCESS_SECRET;

        //for now oauth creds are hard coded, but once db functionality is solid they will be moved to a table in the db. future implementation will use a hash to store the creds for better security or they will be moved to appsettings.json.
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

        public static string get_consumer_key()
        {
            return CONSUMER_KEY;
        }

        public static string get_consumer_secret()
        {
            return CONSUMER_SECRET;
        }

        public static string get_access_token()
        {
            return ACCESS_TOKEN;
        }

        public static string get_access_secret()
        {
            return ACCESS_SECRET;
        }
    }
}