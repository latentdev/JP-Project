using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DATA.Helper;
using Tweetinvi;
using DATA.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DATA.Controllers
{
    public class GraphController : Controller
    {


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
                tweets = SearchTweets.Search(search, new Helper.oath(), 100);
                foreach (var twit in tweets)
                {
                    Models.Tweet T = new Models.Tweet();
                    T.tweet = twit;

                    T.sentiment = Sentiment.Analyse(twit.FullText);
                    list_of_tweets.Add(T);
                }
                Tweets instance = Tweets.getInstance();
                instance.tweets = list_of_tweets;
                instance.searchTerm = search;
                var temp = Json(Analysis.commonTags(instance,search).ToJson());
                return temp;
            }
            else return null;
        }
        public ActionResult commonTags()
        {
            return Json(Analysis.commonTags(Tweets.getInstance(),Tweets.getInstance().searchTerm).ToJson());
        }
    }
}
