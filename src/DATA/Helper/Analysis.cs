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
    }


}
