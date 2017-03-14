using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DATA.Helper
{
    public class Sentiment
    {
        private Sentiment() { }

        public static int Analyse(String tweet)
        {
            int sentimentScore = 0;
            String t=Regex.Replace(tweet, @"[^\w\s]", "");
            var final = t.Split();
            List<String> positive = ReadFile(System.IO.Directory.GetCurrentDirectory()+@"\wwwroot\Data\Positive.txt");
            List<String> negative = ReadFile(System.IO.Directory.GetCurrentDirectory()+@"\wwwroot\Data\Negative.txt");
            foreach (var character in final)
            {
                if (positive.Exists(x => x.ToString() == character))
                {
                    sentimentScore += 1;
                }
                else if (negative.Exists(x => x.ToString() == character))
                {
                    sentimentScore -= 1;
                }

            }
            return sentimentScore;
        }

        public static List<String> ReadFile(String file)
        {
            var dict = File.ReadAllLines(file);
            List<String> dictionary = new List<String>(dict);

            return dictionary;
        }

    }
}
