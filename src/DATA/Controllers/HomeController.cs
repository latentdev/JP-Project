using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using DATA.Helper;

namespace DATA.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Graphs()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public JsonResult Search(string search)
        {

            Tweets tweet = new SearchTweets(new Helper.oath());
            var temp = Json(tweet.Search(search, 10000).ToJson());
            return temp;
            /*string sURL = "https://api.twitter.com/1.1/search/tweets.json" + "?q=" + WebUtility.UrlEncode(search);
            WebRequest request;
            request = WebRequest.Create(sURL);
            Stream objStream;
            objStream = request.GetResponse().GetResponseStream();*/
            //string tweets =null;
            /*bool searching = true;

            Auth.SetUserCredentials("6AJKMgHTk5L0qUZX1tumVrcf6", "Z6LQQdlzH7vQca56wRdTaLaokvrORtWKUzYovwLLKXDwnGh9O2", "23288297-GKQtuIQzm8xEXGrpocnYiqn2HGMLE84BqWvcYH11n", "r1KgCO7P3TxkhJKzmkwbUPCCWMsy9GYKf1EMMXFUA0h1t");
            var searchParameter = new Tweetinvi.Parameters.SearchTweetsParameters(search)
            {
                Lang = Tweetinvi.Models.LanguageFilter.English,
                MaximumNumberOfResults = 1000,
            };
            var tweets = Tweetinvi.Search.SearchTweets(searchParameter);
            /*var stream = Tweetinvi.Stream.CreateFilteredStream();
            stream.AddTrack(search);
            stream.MatchingTweetReceived += (sender, args) =>
            {
               tweets = tweets + "A tweet containing "+search+" has been found; the tweet is '" + args.Tweet + "'";
            };
            stream.StreamStopped += (sender, args) =>
            {
                var exceptionThatCausedTheStreamToStop = args.Exception;
                var twitterDisconnectMessage = args.DisconnectMessage;
                searching = false;
            };
            stream.LimitReached += (sender, args) =>
            {
                searching = false;
            };
            while (searching)
            {
                stream.StartStreamMatchingAllConditions();
            }*/

        }
    }
}
