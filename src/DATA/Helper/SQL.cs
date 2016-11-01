using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

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
                twitterDB.Open();
                foreach(Tweetinvi.Models.ITweet tweet in tweets)
                {
                    SqlCommand myCommand = new SqlCommand("INSERT INTO tweets (ID, text,full_text,tweet_length,place,created,created_by,retweeted,retweet_count,is_retweet,favorited,favorite_count,published,source,url) " +
                                     "Values ("+tweet.Id+","+
                                                "'"+tweet.Text+"'"+","+
                                                "'"+tweet.FullText+"'"+","+
                                                tweet.PublishedTweetLength+","+
                                                "'"+tweet.Place+"'"+","+
                                                "convert(datetime,'"+tweet.CreatedAt+"',101)"+","+
                                                "'"+tweet.CreatedBy+"'"+","+
                                                "'"+tweet.Retweeted+"'"+","+
                                                tweet.RetweetCount+","+
                                                "'"+tweet.IsRetweet+"'"+","+
                                                "'"+tweet.Favorited+"'"+","+
                                                tweet.FavoriteCount+","+
                                                "'"+tweet.IsTweetPublished+"'"+","+
                                                "'"+tweet.Source+"'"+","+
                                                "'"+tweet.Url+"'"+")", twitterDB);
                    myCommand.ExecuteNonQuery();
                }
                twitterDB.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

    }
}
