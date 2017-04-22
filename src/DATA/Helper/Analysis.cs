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

        public DataSet(string in_title, int in_data, int in_sentiment)
        {
            title = in_title;
            data = in_data;
            sentiment = in_sentiment;
        }
    }


    public class Analysis
    {

        public static List<DataSet> commonTags(Tweets tweets,string search, int top=5)
        {
            List<DataSet> commonTags = new List<DataSet>();
            List<TagSet> tags = new List<TagSet>();
            foreach (var tweet in tweets.tweets)
            {
                foreach (var tag in tweet.tweet.Hashtags)
                {
                    tags.Add(new Models.TagSet(tweet.sentiment, '#'+tag.Text.ToLower() ));
                }
            }
            tags.RemoveAll(x => x.tag == search.ToLower());
            while (tags.Count()!=0)
            {
                
                var tag = tags[0];
                int count = 0;
                int sentiment = 0;
                foreach(var T in tags)
                {
                    if (T.tag.ToString()==tag.tag.ToString())
                        
                    {
                        count++;
                        //average the sentiment score
                        sentiment = (sentiment + T.sentiment) / count;
                    }
                }
                DataSet x = new DataSet(tag.tag.ToString(), count, sentiment);
                commonTags.Add(x);
                tags.RemoveAll(y => y.tag == tag.tag.ToString());
            }
            commonTags.Sort((x, y) => x.data.CompareTo(y.data));
            int index = commonTags.Count();
            List<DataSet> TopTags = new List<DataSet>();
            for (int x=0; x<top; x++)
            {
                TopTags.Add(commonTags[index-1]);
                index--;
            }
            TopTags.Sort((x, y) => x.data.CompareTo(y.data));
            return TopTags;
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
                count[(tweet.tweet.CreatedAt - startDate).Days].date = tweet.tweet.CreatedAt.Date.ToString("g");
                count[(tweet.tweet.CreatedAt - startDate).Days].count++;
            }

            return count;
        }
    }
}
