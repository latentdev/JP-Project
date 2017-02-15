using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATA.Models
{
    public class Tweet
    {
        public Tweetinvi.Models.ITweet tweet { get; set; }
        public int sentiment { get; set; }

        public Tweet()
        {

        }
    }
}
