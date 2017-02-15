using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATA.Models
{
    //Singleton
    public class Tweets
    {
        public List<Tweet> tweets { get; set; }
        public String searchTerm { get; set; }

        private static Tweets instance;

        private Tweets()
        { }

        public static Tweets getInstance()
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = new Tweets();
                return instance;
            }
        }

    }

}
