using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATA.Models
{
    public class bubbleModel
    {
        public string word;
        public int count;

        public bubbleModel()
        {

        }

        public bubbleModel(string s, int c)
        {
            word = s;
            count = c;
        }
    }
}