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
        private static Sentiment instance;
        private Sentiment() { }

        public static Sentiment Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Sentiment();
                }
                return instance;
            }
        }
        public int Analyse(String tweet)
        {
            int sentimentScore = 0;
            String t=Regex.Replace(tweet, @"[^\w\s]", "");
            var final = t.Split();
            List<String> positive = ReadFile("Data/Positive.txt");
            List<String> negative = ReadFile("Data/Negative.txt");
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

        public List<String> ReadFile(String file)
        {
            var dict = File.ReadAllLines(file);
            List<String> dictionary = new List<String>(dict);

            return dictionary;
        }

    }
}
