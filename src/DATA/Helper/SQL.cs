using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace DATA.Helper
{
    public class SQL
    {
        SqlConnection twitterDB;
       public SQL()
        {
            //var temp2 = ConfigurationManager.ConnectionStrings["TwitterDB"];
            //var temp = temp2.ConnectionString;
            twitterDB = new SqlConnection("Server = tcp:pounddata.database.windows.net, 1433; Initial Catalog = TwitterDB; Persist Security Info = False; uid = juniorproject; Password = DonnyCordova64; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
        }

        public void StoreTweets(IEnumerable<Tweetinvi.Models.ITweet> tweets)
        {
            try
            {
                Regex apos = new Regex("'");
                twitterDB.Open();
                SqlCommand myCommand;
                foreach (var tweet in tweets)
                {
                    tweet.Text = apos.Replace(tweet.Text, "''");
                    tweet.FullText = apos.Replace(tweet.FullText, "''");
                    string user = tweet.CreatedBy.ToString();
                      user = apos.Replace(user, "''");
                    myCommand = new SqlCommand("INSERT INTO tweets (ID, text,full_text,tweet_length,place,created,created_by,retweeted,retweet_count,is_retweet,favorited,favorite_count,published,source,url) " +
                                     "Values ("+tweet.Id+","+
                                                "'"+tweet.Text+"'"+","+
                                                "'"+tweet.FullText+"'"+","+
                                                tweet.PublishedTweetLength+","+
                                                "'"+tweet.Place+"'"+","+
                                                "convert(datetime,'"+tweet.CreatedAt+"',101)"+","+
                                                "'"+user+"'"+","+
                                                "'"+tweet.Retweeted+"'"+","+
                                                tweet.RetweetCount+","+
                                                "'"+tweet.IsRetweet+"'"+","+
                                                "'"+tweet.Favorited+"'"+","+
                                                tweet.FavoriteCount+","+
                                                "'"+tweet.IsTweetPublished+"'"+","+
                                                "'"+tweet.Source+"'"+","+
                                                "'"+tweet.Url+"'"+")", twitterDB);
                    
                    myCommand.ExecuteNonQuery();
                    myCommand.Dispose();
                }
                twitterDB.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        public void Dispose()
        {
            twitterDB.Dispose();
        }

    }
}
