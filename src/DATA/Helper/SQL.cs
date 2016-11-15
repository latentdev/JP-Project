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
                twitterDB.Open();
                foreach (var tweet in tweets)
                {
                    using (SqlCommand command = new SqlCommand("dbo.InsertTweet", twitterDB))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", tweet.Id);
                        command.Parameters.AddWithValue("@text", tweet.Text);
                        command.Parameters.AddWithValue("@full_text", tweet.FullText);
                        command.Parameters.AddWithValue("@tweet_length", tweet.PublishedTweetLength);
                        command.Parameters.AddWithValue("@place", " ");
                        command.Parameters.AddWithValue("@created", tweet.CreatedAt);
                        command.Parameters.AddWithValue("@created_by", tweet.CreatedBy.ScreenName);
                        command.Parameters.AddWithValue("@retweeted", tweet.Retweeted);
                        command.Parameters.AddWithValue("@retweet_count", tweet.RetweetCount);
                        command.Parameters.AddWithValue("@is_retweet", tweet.IsRetweet);
                        command.Parameters.AddWithValue("@favorited", tweet.Favorited);
                        command.Parameters.AddWithValue("@favorite_count", tweet.FavoriteCount);
                        command.Parameters.AddWithValue("@published", tweet.IsTweetPublished);
                        command.Parameters.AddWithValue("@source", tweet.Source);
                        command.Parameters.AddWithValue("@url", tweet.Url);


                        command.ExecuteNonQuery();
                    }
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
