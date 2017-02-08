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

        //sql components commented out while not in use.
        public IEnumerable<Tweetinvi.Models.ITweet> Search(string search, int count = 50)
        {
            //SQL sql = new SQL();

            //google api key = AIzaSyA_gOxu6KoIsMa-ZlBF5uoxRwIo4qMfWiQ

            Auth.SetUserCredentials(creds.get_consumer_key(), creds.get_consumer_secret(), creds.get_access_token(), creds.get_access_secret());
            var searchParameter = new Tweetinvi.Parameters.SearchTweetsParameters(search)
            {
                Lang = Tweetinvi.Models.LanguageFilter.English,
                MaximumNumberOfResults = count,
            };
            var tweets = Tweetinvi.Search.SearchTweets(searchParameter);


            var geotweets = tweets.Where(x => x.Coordinates != null || x.Place != null);
            
            //sql.StoreTweets(tweets);
            //sql.Dispose();
            
            return tweets;
        }
    }
    // do not use this function yet. It does nothing at the moment.
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
