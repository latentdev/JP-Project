using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATA.Models
{
    public class TagSet
    {
        public int sentiment { get; set; }
        public string tag { get; set; }

        public TagSet(int in_sent, string in_tag)
        {
            sentiment = in_sent;
            tag = in_tag;
        }
    }
}
