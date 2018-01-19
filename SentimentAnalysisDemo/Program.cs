using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Events;
using SimpleNetNlp;
using System.Text.RegularExpressions;

namespace SentimentAnalysisDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Authenticate
            string consumerKey = Environment.GetEnvironmentVariable("CONSUMERKEY",EnvironmentVariableTarget.Machine);
            string consumerSecret = Environment.GetEnvironmentVariable("CONSUMERSECRET", EnvironmentVariableTarget.Machine);
            string accessToken = Environment.GetEnvironmentVariable("ACCESSTOKEN", EnvironmentVariableTarget.Machine);
            string accessTokenSecret = Environment.GetEnvironmentVariable("ACCESSTOKENSECRET", EnvironmentVariableTarget.Machine);

            Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            

            //Create Stream
            var stream = Stream.CreateFilteredStream();

            //Add Topic Filters
            stream.AddTrack("cryptocurrencies");
            stream.AddTrack("bitcoin");
            stream.AddTrack("ether");
            stream.AddTrack("Litecoin");

            //Filter Languages
            stream.AddTweetLanguageFilter("en");

            //Handle Matching Tweets
            stream.MatchingTweetReceived += OnMatchedTweet;
            
            //Start Stream
            stream.StartStreamMatchingAllConditions();
        }

        private static void OnMatchedTweet(object sender, MatchedTweetReceivedEventArgs args)
        {
            var sanitized = sanitize(args.Tweet.FullText); //Sanitize Tweet

            var sentence = new Sentence(sanitized);

            //Output Tweet and Sentiment
            Console.WriteLine(sentence.Sentiment + "|" + args.Tweet);
            
            //Dispose of Sentence object
            sentence = null;
        }

        private static string sanitize(string raw)
        {
            return Regex.Replace(raw, @"(@[A-Za-z0-9]+)|([^0-9A-Za-z \t])|(\w+:\/\/\S+)", "").ToString();
        }
    }
}
