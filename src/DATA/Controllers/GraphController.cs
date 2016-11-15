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
        IEnumerable<Tweetinvi.Models.ITweet> tweets = null;
        string searchterm = null;
        // GET: /Graph/
        public IActionResult Index()
        {
            return View();
        }
        // POST: /Graph/search
        public JsonResult Search(string search)
        {
            if (search != null)
            {
                searchterm = search;
                tweet = new SearchTweets(new Helper.oath());
                tweets = tweet.Search(search, 10000);
                Analysis tags = new Analysis(tweets, search);
                var temp = Json(tags.commonTags().ToJson());
                return temp;
            }
            else return null;
        }
        public ActionResult commonTags()
        {
            Analysis tags = new Analysis(tweets, searchterm);
            return Json(tags.commonTags().ToJson());
        }
    }
}
