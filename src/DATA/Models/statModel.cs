using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATA.Models
{
    public class statModel
    {
        public DateTime accountCreationDate { get; set; }
        public int followerCount { get; set; }
        public int friendsCount { get; set; }
        public int hashtagCount { get; set; }
        public bool hasGeo { get; set; }
        public int userMentionCount { get; set; }
        public int sentimentScore { get; set; }

        public statModel(DateTime creation, int followers, int friends, int hashcount, bool geo, int usermentions, int sentiment)
        {
            accountCreationDate = creation;
            followerCount = followers;
            friendsCount = friends;
            hashtagCount = hashcount;
            hasGeo = geo;
            userMentionCount = usermentions;
            sentimentScore = sentiment;
        }

        public statModel()
        {

        }
    }

    
}
