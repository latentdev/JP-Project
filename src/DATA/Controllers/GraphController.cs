using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DATA.Helper;
using Tweetinvi;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DATA.Controllers
{
    public class GraphController : Controller
    {

        Tweets tweet = null;
        List<Models.Tweet> list_of_tweets = new List<Models.Tweet>();
        string searchterm = null;
        // GET: /Graph/
        public IActionResult Index()
        {
            return View();
        }
        // POST: /Graph/search
        public JsonResult Search(string search)
        {
            IEnumerable<Tweetinvi.Models.ITweet> tweets = null;
            if (search != null)
            {
                searchterm = search;
                tweet = new SearchTweets(new Helper.oath());
                tweets = tweet.Search(search, 100);
                Sentiment sentiment = Sentiment.Instance;
                foreach (var twit in tweets)
                {
                    Models.Tweet T = new Models.Tweet();
                    T.tweet = twit;

                    T.sentiment = sentiment.Analyse(twit.FullText);
                    list_of_tweets.Add(T);
                }
                Analysis tags = new Analysis(list_of_tweets, search);
                var temp = Json(tags.commonTags().ToJson());
                return temp;
            }
            else return null;
        }
        public ActionResult commonTags()
        {
            Analysis tags = new Analysis(list_of_tweets, searchterm);
            return Json(tags.commonTags().ToJson());
        }
    }
}
