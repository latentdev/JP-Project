#Kevin Fessler
#JR Project 
#12/1/2016

# Language:		R
# Expects:		Main r program containing the tweets object

# =========================
# FIND TOP RETWEETED TWEETS
# =========================

# this program will find all of the top retweeted tweets and display them in a 
# graph

# select top retweeted tweets
table(tweets.df$retweetCount)
selected <- which(tweets.df$retweetCount >= 9) # used "9" for a good estimator of popularity

# plot them in a graph by date using "Year, Month, Day"
dates <- strptime(tweets.df$created, format="%Y-%m-%d")

# this sets up the graph
plot(x=dates, y=tweets.df$retweetCount, type="l", col="grey",
xlab="Date", ylab="Times retweeted")

# r-specific color schema
colors <- rainbow(10)[1:length(selected)]

# formats the points on the graph
points(dates[selected], tweets.df$retweetCount[selected],pch=19, col=colors)

# formats the text 
text(dates[selected], tweets.df$retweetCount[selected],tweets.df$text[selected], col=colors, cex=.9)