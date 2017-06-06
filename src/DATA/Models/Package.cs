using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATA.Models
{
    public class Package
    {
        public string title { get; set; }
        public string text { set; get; }
        public IEnumerable data { set; get; }
    }
}
