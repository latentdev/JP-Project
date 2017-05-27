# AUTHOR: Kevin Fessler
# DATE	: 11/8/2016		
# REV	: 2.0.0
# FILE	: TSP.R (Twitter Sentiment Project)

# UPDATES
# Re-implemented word dictionaries due to loss of files 

# NEXT REV
# 1. Implement more graphs and analysis
# 2. Split up code to be a bit more modular

# ABOUT 
# This is a JR project intended for use in determining 
# the sentiment of Tweets over a given time.
# This might become more of a main program launcher for 
# sentiment analysis given a function call and parameters.
# It should implicitly be used with the ‘#DATA’ server 
# and any subsidiary projects

# CREDITS
# + Wordlists taken from the:
#     c. 2015 English Opinion Lexicon Dictionary 
# + Ideas on “Objective Mining” with Direct Opinions: 
#     Bing Liu at the University of Illinois, Chicago
#     In the Handbook of Natural Language Processing

# STANDARD R CODING CONVENTIONS
# - The preceding word “MOD” implies that the user must replace a
#   line or string with the necessary information if filled with a “__”
# - Notes will always be denoted as “NOTE:” and precede the actual note
# - Headers will contain a row of ‘^’ characters, except for the introduction
#   of documents
# - Helpful hints are written with comments on the side 


# ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
# Connect the necessary libraries
# ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
 library(twitteR)	# The Twitter API
 library(ROAuth)	# Unique Authorization App
 library(plyr)		# Tools for splitting, applying and combining Data
 library(dplyr)		# A fast, consistent tool for working with data frame objects or (similar)
 library(stringr)	# Wrappers for common string operations
 library(ggplot2)	# A plotting system for R which produces multi-layered graphics


# ^^^^^^^^^^^^^^^^^^^^^^^^^^
# Connect to the Twitter API
# ^^^^^^^^^^^^^^^^^^^^^^^^^^
 download.file(url='http://curl.haxx.se/ca/cacert.pem', destfile='cacert.pem')
 reqURL <- 'https://api.twitter.com/oauth/request_token'
 accessURL <- 'https://api.twitter.com/oauth/access_token'
 authURL <- 'https://api.twitter.com/oauth/authorize'
 consumerKey <- '____________' 			# Replace with the 'Consumer Key' from Twitter Application
 consumerSecret <- '______________'  	# Replace with the 'Consumer Secret' from Twitter Application
 Cred <- OAuthFactory$new(consumerKey=consumerKey,consumerSecret=consumerSecret,requestURL=reqURL,
                                                       accessURL=accessURL,authURL=authURL)
 Cred$handshake(cainfo = system.file('CurlSSL', 'cacert.pem', package = 'RCurl')) 


# NOTE: 
# There is URL in Console. You need to get this code and enter it in Console
# Saves your authentication from Twitter in a file within the working directory

save(Cred, file='twitter authentication.Rdata') 
 load('twitter authentication.Rdata') 

# NOTE:
# Once you launched the code first time, you can start from this line in 
# the future because libraries should be saved in the file
 registerTwitterOAuth(Cred)


# ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
# Extract and analyze tweet function
# ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
 search <- function(searchterm)
 {
list <- searchTwitter(searchterm, cainfo='cacert.pem', n=1500) # Extact tweets and create storage file
 df <- twListToDF(list)
 df <- df[, order(names(df))]
 df$created <- strftime(df$created, '%Y-%m-%d')
 
 if (file.exists(paste(searchterm, '_stack.csv'))==FALSE) write.csv(df, file=paste(searchterm, '_stack.csv'), row.names=F)
 
 stack <- read.csv(file=paste(searchterm, '_stack.csv')) # Merge the last extraction with storage file and remove duplicates
 stack <- rbind(stack, df)
 stack <- subset(stack, !duplicated(stack$text))
 write.csv(stack, file=paste(searchterm, '_stack.csv'), row.names=F)

# ^^^^^^^^^^^^^^^^^^^^^^^^^
# Tweet evaluation function
# ^^^^^^^^^^^^^^^^^^^^^^^^^
 score.sentiment <- function(sentences, pos.words, neg.words, .progress='none')
 {
 require(plyr)
 require(stringr)
 scores <- laply(sentences, function(sentence, pos.words, neg.words){
 sentence <- gsub('[[:punct:]]', "", sentence)
 sentence <- gsub('[[:cntrl:]]', "", sentence)
 sentence <- gsub('\\d+', "", sentence)
 sentence <- tolower(sentence)
 word.list <- str_split(sentence, '\\s+')
 words <- unlist(word.list)
 pos.matches <- match(words, pos.words)
 neg.matches <- match(words, neg.words)
 pos.matches <- !is.na(pos.matches)
 neg.matches <- !is.na(neg.matches)
 score <- sum(pos.matches) - sum(neg.matches)
 return(score)
 }, pos.words, neg.words, .progress=.progress)
 scores.df <- data.frame(score=scores, text=sentences)
 return(scores.df)
 }

# ^^^^^^^^^^^^^^^^^^^^^^^^
# Gets directory for words
# ^^^^^^^^^^^^^^^^^^^^^^^^
pos <- scan('C:/___________/positive-words.txt', what='character', comment.char=';') # MOD folder with positive dictionary
neg <- scan('C:/___________/negative-words.txt', what='character', comment.char=';') # MOD folder with negative dictionary
 pos.words <- c(pos, 'upgrade')
 neg.words <- c(neg, 'wtf', 'wait', 'waiting', 'epicfail')

# ^^^^^^^^^^^^^
# Word Databank
# ^^^^^^^^^^^^^
Dataset <- stack
 Dataset$text <- as.factor(Dataset$text)
 scores <- score.sentiment(Dataset$text, pos.words, neg.words, .progress='text')
 write.csv(scores, file=paste(searchterm, '_scores.csv'), row.names=TRUE) #save evaluation results

# ^^^^^^^^^^^^^^^^^^^^^^^
# Total score calculation
# ^^^^^^^^^^^^^^^^^^^^^^^
# Assigns a score of positive / negative / neutral
 stat <- scores
 stat$created <- stack$created
 stat$created <- as.Date(stat$created)
 stat <- mutate(stat, tweet=ifelse(stat$score > 0, 'positive', ifelse(stat$score < 0, 'negative', 'neutral')))
 by.tweet <- group_by(stat, tweet, created)
 by.tweet <- summarise(by.tweet, number=n())
 write.csv(by.tweet, file=paste(searchterm, '_opin.csv'), row.names=TRUE)

# ^^^^^^^^^^^^^^^^^^^^^
# Displays a line graph 
# ^^^^^^^^^^^^^^^^^^^^^
 ggplot(by.tweet, aes(created, number)) + geom_line(aes(group=tweet, color=tweet), size=2) +
 geom_point(aes(group=tweet, color=tweet), size=4) +
 theme(text = element_text(size=18), axis.text.x = element_text(angle=90, vjust=1)) +
 #stat_summary(fun.y = 'sum', fun.ymin='sum', fun.ymax='sum', colour = 'yellow', size=2, geom = 'line') +
 ggtitle(searchterm)
ggsave(file=paste(searchterm, '_plot.jpeg'))
}

search("______") # Enter keyword to search a tweet for a word
