using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

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
        List<Models.Tweet> tweets = null;
        string search = null;
        public Analysis()
        {

        }

        public Analysis(List<Models.Tweet> in_tweets,string in_search)
        {
            tweets = in_tweets;
            search = in_search;
        }
        public List<DataSet> commonTags()
        {
            List<DataSet> commonTags = new List<DataSet>();
            List<string> tags = new List<string>();
            foreach (var tweet in tweets)
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
    }


}
