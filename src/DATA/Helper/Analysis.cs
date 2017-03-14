using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using DATA.Models;

namespace DATA.Helper
{
    public class DataSet
    {
        public string title { get; set; }
        public int data { get; set; }
        public int sentiment { get; set; }

        public DataSet()
        {
            title = null;
            data = 0;
            sentiment = 0;
        }

        public DataSet(string in_title, int in_data)
        {
            title = in_title;
            data = in_data;
        }
    }

    public class Analysis
    {

        public static List<DataSet> commonTags(Tweets tweets,string search)
        {
            List<DataSet> commonTags = new List<DataSet>();
            List<string> tags = new List<string>();
            foreach (var tweet in tweets.tweets)
            {
                foreach (var tag in tweet.tweet.Hashtags)
                {
                    tags.Add('#'+tag.Text.ToLower());
                }
            }
            tags.RemoveAll(x => x == search.ToLower());
            while (tags.Count()!=0)
            {
                
                var tag = tags[0];
                int count = 0;
                foreach(var T in tags)
                {
                    if (T.ToString()==tag.ToString())
                        
                    { count++; }
                }
                DataSet x = new DataSet(tag.ToString(), count);
                commonTags.Add(x);
                tags.RemoveAll(y => y == tag.ToString());
            }
            commonTags.Sort((x, y) => x.data.CompareTo(y.data));
            return commonTags;
        }

        //
        //Analyzing hashtag used over period of time
        //

        public static Day[] hashtag(Tweets tweets, string search)
        {
            DateTime startDate = tweets.tweets[0].tweet.CreatedAt;

            foreach(var tweet in tweets.tweets)
            {
                // Finding earliest time tweet
                if(startDate > tweet.tweet.CreatedAt)
                {
                    startDate = tweet.tweet.CreatedAt;
                }
            }
             // This would give back how many days between the startdate and currentdate
            int span = (DateTime.Now - startDate).Days;

            Day[] count = new Day[span+1]; //Arr based on number of Days

            foreach(var tweet in tweets.tweets)
            {
                if (count[(tweet.tweet.CreatedAt - startDate).Days] == null) 
                {
                    count[(tweet.tweet.CreatedAt - startDate).Days] = new Day();
                }
                count[(tweet.tweet.CreatedAt - startDate).Days].date = tweet.tweet.CreatedAt;
                count[(tweet.tweet.CreatedAt - startDate).Days].count++;
            }

            return count;
        }
    }
}
