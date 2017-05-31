using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using DATA.Models;
using System.Globalization;
using System.Text.RegularExpressions;

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
        public static List<Package> GetPackages(Tweets tweets)
        {
            List<Package> packages = new List<Package>();
            packages.Add(commonTags(tweets, tweets.searchTerm));
            packages.Add(FavoriteHashtags(tweets, tweets.searchTerm));
            packages.Add(HashtagOverTime(tweets, tweets.searchTerm));
            packages.Add(BubbleChart(tweets));
            packages.Add(PinMap(tweets));
            packages.Add(Images(tweets));
            return packages;
        }
        public static Package commonTags(Tweets tweets,string search, int top=5)
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
            int hashtagCount = tags.Distinct().Count();
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
                        sentiment = (sentiment + T.sentiment);
                    }
                }
                sentiment /= count;
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
            Package data = new Package();
            string text = "The graph on the left shows the top 5 commonly used hashtags in the tweets found using the search " + tweets.searchTerm + ". We found "+hashtagCount+" unique hashtags. The most commonly used hashtag was " + TopTags[TopTags.Count-1].title + " with " + TopTags[TopTags.Count-1].data + " uses.";
            data.data = TopTags;
            data.text = text;
            return data;
        }

        public static Package FavoriteHashtags(Tweets tweets, string search)
        {
            List<string> hashtags = new List<string>();
            List<DataSet> data = new List<DataSet>();
            if (tweets!=null)
            {
                foreach (var tweet in tweets.tweets)
                {
                    foreach (var hashtag in tweet.tweet.Hashtags)
                    {
                        if(!hashtags.Contains<string>(hashtag.Text))
                        {
                            hashtags.Add(hashtag.Text.ToLower());
                        }

                    }
                }
                var term=search.Remove(0, 1);
                hashtags=hashtags.Distinct().ToList();
                hashtags.RemoveAll(x=>x==term.ToLower());
                foreach (var text in hashtags)
                {
                    DataSet tag = new DataSet();
                    tag.title = text;
                    data.Add(tag);
                }
                foreach (var tweet in tweets.tweets)
                {
                    foreach(var hashtag in tweet.tweet.Hashtags)
                    {
                        foreach (var d in data)
                        {
                            if (d.title==hashtag.Text)
                            {
                                d.data += tweet.tweet.FavoriteCount;
                            }
                        }
                    }
                }
                data.Sort((x, y) => x.data.CompareTo(y.data));
            }
            Package package = new Package();
            List<DataSet>Top5 = new List<DataSet>();
            int index = data.Count - 1;
            for(int i = 0;i<5;i++ )
            {
                Top5.Add(data[index]);
                index--;
            }
            package.data = Top5;
            return package;
        }

        public static Package BubbleChart(Tweets tweets)
        {
            List<String> commonwords = Sentiment.ReadFile(System.IO.Directory.GetCurrentDirectory() + @"\wwwroot\Data\Used.txt");
            List<bubbleModel> bubbles = new List<bubbleModel>();
            List<string> wordlist = new List<string>();
            List<int> wordcount = new List<int>();
            Tweets instance = Tweets.getInstance();

            foreach (var t in instance.tweets)
            {
                var fixedInput = Regex.Replace(t.tweet.Text, "[^a-zA-Z0-9% _]", string.Empty);
                List<string> splitted = fixedInput.Split(' ').ToList();
                foreach (var s in splitted)
                {
                    var wordpos = wordlist.IndexOf(s);
                    if (wordpos == -1)
                    {
                        wordlist.Add(s);
                        wordcount.Add(1);
                    }
                    else
                        wordcount[wordpos]++;
                }

            }

            for (var i = 0; i < wordlist.Count; i++)
            {
                if (wordcount[i] > 10 && !commonwords.Contains(wordlist[i].ToLower()) && !wordlist[i].Contains("http"))
                    bubbles.Add(new bubbleModel(wordlist[i], wordcount[i]));
            }
            Package package = new Package();
            package.data = bubbles;
            return package;
        }
        //
        //Analyzing hashtag used over period of time
        //

        public static Package HashtagOverTime(Tweets tweets, string search)
        {
            DateTime startDate = tweets.tweets[0].tweet.CreatedAt;
            DateTime endDate = startDate;
            Package package = new Package();
            foreach (var tweet in tweets.tweets)
            {
                // Finding earliest time tweet and latest.
                if(startDate > tweet.tweet.CreatedAt)
                {
                    startDate = tweet.tweet.CreatedAt;
                }
                if (endDate < tweet.tweet.CreatedAt)
                {
                    endDate = tweet.tweet.CreatedAt;
                }
            }
            if ((endDate - startDate).Days <= 3)
            {
                // This would give back how many days between the startdate and currentdate
                int span = (int)(DateTime.Now - startDate).TotalHours;
                List<Day> count = new List<Day>();
                for (int i = 0; i <=span; i++)
                {
                    count.Add(new Day());
                }
                //Day[] count = new Day[span+1]; //Arr based on number of hours

                foreach (var tweet in tweets.tweets)
                {
                    /*if (count[(tweet.tweet.CreatedAt - startDate).Hours] == null)
                    {
                        count[(tweet.tweet.CreatedAt - startDate).Hours] = new Day();
                    }*/
                    count[(tweet.tweet.CreatedAt - startDate).Hours].date = tweet.tweet.CreatedAt.ToString("g", DateTimeFormatInfo.InvariantInfo);//"d");
                    count[(tweet.tweet.CreatedAt - startDate).Hours].count++;
                }
                for (int i = 0; i <= span; i++)
                {
                    if(count[i].count==0)
                    {
                        count.RemoveAt(i);
                        span--;
                        i--;
                    }
                }
                //count.Sort();
                package.data = count.OrderBy(x => x.date);
                return package;
            }
            else
            {
                int span = (DateTime.Now - startDate).Days;

                List<Day> count = new List<Day>();
                for (int i = 0; i <=span; i++)
                {
                    count.Add(new Day());
                }
                //Day[] count = new Day[span + 1]; //Arr based on number of Days

                foreach (var tweet in tweets.tweets)
                {
                   /* if (count[(tweet.tweet.CreatedAt - startDate).Days] == null)
                    {
                        count[(tweet.tweet.CreatedAt - startDate).Days] = new Day();
                    }*/
                    count[(tweet.tweet.CreatedAt - startDate).Days].date = tweet.tweet.CreatedAt.ToString("g", DateTimeFormatInfo.InvariantInfo);//"d");
                    count[(tweet.tweet.CreatedAt - startDate).Days].count++;
                }
                for (int i = 0; i <= span; i++)
                {
                    if (count[i].count == 0)
                    {
                        count.RemoveAt(i);
                        span--;
                        i--;
                    }

                }

                package.data = count.OrderBy(x => x.date);
                return package;
            }

        }
        public static Package PinMap(Tweets tweets)
        {
            List<Tuple<double, double>> geocoords = new List<Tuple<double, double>>();
            var avglong = 0.0;
            var avglat = 0.0;
            foreach (var t in tweets.tweets)
            {
                if (t.tweet.Coordinates != null || t.tweet.Place != null)
                {
                    if (t.tweet.Coordinates != null)
                    {
                        geocoords.Add(new Tuple<double, double>(t.tweet.Coordinates.Longitude, t.tweet.Coordinates.Latitude));
                    }
                    else
                    {
                        //find center of tweet location bounding box to place pin
                        avglong = (t.tweet.Place.BoundingBox.Coordinates[0].Longitude + t.tweet.Place.BoundingBox.Coordinates[1].Longitude) / 2;
                        avglat = (t.tweet.Place.BoundingBox.Coordinates[0].Latitude + t.tweet.Place.BoundingBox.Coordinates[2].Latitude) / 2;
                        geocoords.Add(new Tuple<double, double>(avglong, avglat));
                    }
                }
            }
            Package package = new Package();
            package.data = geocoords;
            return package;
        }
        public static Package Images(Tweets tweets)
        {
           
            List<string> images = new List<string>();
            foreach (var tweet in tweets.tweets)
            {
                foreach (var image in tweet.tweet.Media)
                {
                    if (tweet.tweet.PossiblySensitive == false)
                        images.Add(image.MediaURLHttps);
                }
            }
            Package package = new Package();
            package.data = images;
            return package;
        }
    }
}
