using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;

namespace DATA.Helper
{
    interface Tweets
    {
        IEnumerable<Tweetinvi.Models.ITweet> Search(string search, int count);
    }
    public class SearchTweets : Tweets
    {
        oath creds;
        public SearchTweets(oath in_credentials)
        {
            creds = in_credentials;
        }

        public IEnumerable<Tweetinvi.Models.ITweet> Search(string search, int count = 1000)
        {
            Auth.SetUserCredentials(creds.get_consumer_key(), creds.get_consumer_secret(), creds.get_access_token(), creds.get_access_secret());
            var searchParameter = new Tweetinvi.Parameters.SearchTweetsParameters(search)
            {
                Lang = Tweetinvi.Models.LanguageFilter.English,
                MaximumNumberOfResults = count
            };
            var tweets = Tweetinvi.Search.SearchTweets(searchParameter);
            return tweets;
        }
    }

    public class StreamTweets:Tweets
    {
        oath creds;
        public StreamTweets(oath in_credentials)
        {
            creds = in_credentials;
        }

        public IEnumerable<Tweetinvi.Models.ITweet> Search(string search, int count=0)
        {
            IEnumerable < Tweetinvi.Models.ITweet > tweet = null;
            return tweet;
        }
    }
}
