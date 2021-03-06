﻿using System;
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
            bool amplified = false;
            var final = t.Split();
            int multiply = 1;
            List<String> positive = ReadFile(System.IO.Directory.GetCurrentDirectory()+@"\wwwroot\Data\Positive.txt");
            List<String> negative = ReadFile(System.IO.Directory.GetCurrentDirectory()+@"\wwwroot\Data\Negative.txt");
            List<String> intensify = ReadFile(System.IO.Directory.GetCurrentDirectory() + @"\wwwroot\Data\Intensifiers.txt");
            foreach (var character in final)
            {
                if (amplified)
                {
                    multiply = 2;
                }
                else
                    multiply = 1;

                if (positive.Exists(x => x.ToString() == character))
                {
                    sentimentScore += 1*multiply;
                    
                }
                else if (negative.Exists(x => x.ToString() == character))
                {
                    sentimentScore -= 1*multiply;
                }
                if(intensify.Exists(x=>x.ToString() == character))
                {
                    amplified = true;
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
