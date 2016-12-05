

// Kevin Fessler
// JR Project 
// 12/3/2016

// FILENAME:	SentimentAnalysis.cs

// Language:	C#
// Expects:		Graph Controller, Main, various dependencies

/*	== PURPOSE =====================================================

This file will read in a block of text in STRING format and determine
it's sentiment by way of assigning each word a score. This file is 
NOT intended to implement graphing functions, as complex graphical 
functionality should be moved to a separate file. */

/*	== I/O ========================================================

 INPUTS:	Detected Entities, Themes, Keywords, Citations, Slang 
			words and abbreviations. 
 OUTPUTS:	For each input you can get sentiment result as double
			polarity and score; eg. positive, negative or neutral. */


static void sentimentAnalyzer(string[] keyword)
   {
	   // This is the search keyword from the user
       var inputText = keyword; 
 
 
       // Creates a new document to examine the text as a group of words
	   var doc = new Document()
       {
		  // Passes input text to the new document and sets twitter 
		  // content to false. Will get set true after analysis was 
		  // successful by API
          DocumentText = inputText,		 
          IsTwitterContent = false,
       };
 
		// Use API to request document analysis
		// You need to use this API: http://text2data.org/Integration
       var docResult = API.GetDocumentAnalysisResult(doc);
 

		// If the status of the document shows that the request was successful
		// we can display the TOTAL results of a single tweet 
       if (docResult.Status == (int)DocumentResultStatus.OK) //check status
       {
        // Display document level score in CONSOLE
        Console.WriteLine(string.Format("This document is: {0}{1}{2}",
			docResult.DocSentimentPolarity,
			docResult.DocSentimentResultString,
			docResult.DocSentimentValue.ToString("0.000")));	// Computed sentiment
 

		// If there are any entities available, score them
		// USED FOR SARCASM
        if (docResult.Entities != null && docResult.Entities.Any())
        {
         Console.WriteLine(Environment.NewLine + "Entities:");
         foreach (var entity in docResult.Entities)
         {
           Console.WriteLine(string.Format("{0}({1}){2}{3}{4}",
		   entity.Text,
		   entity.KeywordType,
		   entity.SentimentPolarity,
		   entity.SentimentResult,
		   entity.SentimentValue.ToString("0.0000")));	// Computed sentiment
          }
       }
 
		// If there are any keywords specified in the document, score them
		// USED FOR GENERAL TWEETS
      if (docResult.Keywords != null && docResult.Keywords.Any())
      {
         Console.WriteLine(Environment.NewLine + "Keywords:");
         foreach (var keyword in docResult.Keywords)
          {
             Console.WriteLine(string.Format("{0}{1}{2}{3}",
			 keyword.Text,
			 keyword.SentimentPolarity,
			 keyword.SentimentResult,
			 keyword.SentimentValue.ToString("0.0000")));	// Computed sentiment
            }
        }
 
      }
	  
	  // If no scoring could be done, write an error message 
      else
      {
          Console.WriteLine(docResult.ErrorMessage);
      }
 
       Console.ReadLine();
  }
