using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;

namespace DATA.Helper
{
    interface Search
    {
        IEnumerable<Tweetinvi.Models.ITweet> Search(string search, oath creds, int count);
    }
    public class SearchTweets 
    {

        //sql components commented out while not in use.
        public static IEnumerable<Tweetinvi.Models.ITweet> Search(string search,int count = 1000)
        {
            //SQL sql = new SQL();
            oath cred = oath.getInstance();
            Auth.SetUserCredentials(cred.get_consumer_key(), cred.get_consumer_secret(), cred.get_access_token(), cred.get_access_secret());
            var searchParameter = new Tweetinvi.Parameters.SearchTweetsParameters(search)
            {
                Lang = Tweetinvi.Models.LanguageFilter.English,
                MaximumNumberOfResults = count
            };
            var tweets = Tweetinvi.Search.SearchTweets(searchParameter);

            //sql.StoreTweets(tweets);
            //sql.Dispose();
            return tweets;
        }
    }
    // do not use this function yet. It does nothing at the moment.
    public class StreamTweets:Search
    {
        oath creds;
        public StreamTweets(oath in_credentials)
        {
            creds = in_credentials;
        }

        public IEnumerable<Tweetinvi.Models.ITweet> Search(string search, oath creds, int count=0)
        {
            IEnumerable < Tweetinvi.Models.ITweet > tweet = null;
            return tweet;
        }
    }
}
