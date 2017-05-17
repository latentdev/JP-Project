﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DATA.Helper;
using Tweetinvi;
using DATA.Models;

//cant get tweets from model

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
            try
            {
                IEnumerable<Tweetinvi.Models.ITweet> tweets = null;
                if (search != null)
                {
                    tweets = SearchTweets.Search(search, 1000);
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
                    var temp = Json(Analysis.commonTags(instance, search));
                    return temp;
                }
                else return Json("search is null"); ;
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }
        public ActionResult commonTags()
        {
            return Json(Analysis.commonTags(Tweets.getInstance(),Tweets.getInstance().searchTerm).ToJson());
        }

        public ActionResult pinmap()
        {
            return View("~/Views/Home/pinmap.cshtml");
        }
        public ActionResult time()
        {
            return View("~/Views/Home/time.cshtml");
        }
        public JsonResult timejson()
            {
            return Json(Analysis.hashtag(Tweets.getInstance(), Tweets.getInstance().searchTerm));
            }


        public ActionResult stats()
        {
            return View("~/Views/Home/linegraph.cshtml");
        }

        public JsonResult statsJson()
        {
            Tweets instance = Tweets.getInstance();
            List<statModel> stats = new List<statModel>();
            foreach (var t in instance.tweets)
            {
                bool hasGeo = false;
                if (t.tweet.Place != null || t.tweet.Coordinates != null)
                    hasGeo = true;

                if (t.tweet.CreatedBy.FollowersCount < 10000)
                {
                    stats.Add(new statModel(t.tweet.CreatedBy.CreatedAt,
                                            t.tweet.CreatedBy.FollowersCount,
                                            t.tweet.CreatedBy.FriendsCount,
                                            t.tweet.Hashtags.Count,
                                            hasGeo,
                                            t.tweet.UserMentions.Count));
                }
            }

            var orderedstats = stats.OrderBy(statModel => statModel.followerCount);

            return Json(orderedstats);
        }

        public JsonResult bubbleChartJson()
        {
            return Json(Analysis.bubbleChart(Tweets.getInstance()));
        }

        public JsonResult pinmapjson()
        {
            List<Tuple<double, double>> geocoords = new List<Tuple<double, double>>();
            Tweets instance = Tweets.getInstance();
            var avglong = 0.0;
            var avglat = 0.0;
            foreach (var t in instance.tweets)
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

            return Json(geocoords);
        }
    }
}
