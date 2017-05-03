using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATA.Models
{
    public class Day
    {
        public int count;
        public string date;
        public Day ()
        {
            count = 0;
            date = DateTime.MinValue.ToString();
        }

        public int getCount()
        {
            return count;
        }
        public string getDate()
        {
            return date;
        }

        public void setCount(int in_count)
        {
            count = in_count;
        }

        public void setDate(string in_date)
        {
            date = in_date;
        }
    }
}
