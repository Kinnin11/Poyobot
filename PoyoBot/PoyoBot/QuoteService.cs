using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PoyoBot
{
     

    class QuoteService
    {
        private List<Quote> quotes;
        private int totalWeight = 0;
        private Random random = new Random();
        public QuoteService()
        {
            quotes = new List<Quote>();
            fillQuotes();
        }

        public Quote Quote
        {
            get
            {                 
                return quotes[random.Next(0, quotes.Count - 1)];
            }
        }

       public void addQuote(string quote)
        {
            foreach(Quote q in quotes)
            {
                if(quote == q.Content)
                {
                    throw new Exception("This quote already exists!");
                }
            }
            quotes.Add(new Quote(quote));
            if(!saveQuote(quote))
            {
                throw new Exception("An error occured, try again later");
            }
        }

     
        public bool saveQuote(string q)
        {
            StreamWriter SW = File.AppendText("quotes.txt");
            SW.WriteLine("\n" + q);
            SW.Close();
            return true;
        }

        public void addSuggestion(string q)
        {
            StreamWriter SW = File.AppendText("suggestions.txt");
            SW.WriteLine("\n" + q);
            SW.Close();
        }

        public void fillQuotes()
        {
            quotes.Clear();
            StreamReader SR = new StreamReader("quotes.txt");
            string s = "";
            Console.WriteLine("LOADING QUOTES");
            while (s != null)
            {
                s = SR.ReadLine();
                if (s != null && s != "")
                {
                    quotes.Add(new Quote(s));
                    totalWeight++;
                }
            }
            SR.Close();
        }
    }
}
