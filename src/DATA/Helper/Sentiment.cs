using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            List<String> positive = ReadFile("Data/Positive.txt");
            List<String> negative = ReadFile("Data/Negative.txt");
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
