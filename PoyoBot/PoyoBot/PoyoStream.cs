using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PoyoBot
{
    class PoyoStream
    {
        private static string[] poyos = { "(> ^ - ^)> PUYO <(^ - ^ <)", "(> ^ - ^)> PAIYO <(^ - ^ <)", "(> ^ - ^)> HAAIII <(^ - ^ <)", "(> ^ - ^) > PUUYAAA < (^ - ^ <)" };
        private System.Timers.Timer timer;
        private string streamName;
        private int interval;
        private IRCHandling bot;
        private bool isJoined = false;
        private bool turnOffQuote = false;
        

        public PoyoStream(string streamName, IRCHandling bot, int interval = 10, bool quote = true)
        {
            timer = new System.Timers.Timer(5000);
            timer.Start();
            timer.Elapsed += t_Timer2Elapsed;
            this.streamName = streamName;
            this.bot = bot;
            Interval = interval;
            IsQuoteOn = quote;
            //timer.Interval = 5000;
        }

        public static string getPoyo()
        {
            Random rand = new Random();
            int seed = rand.Next(0, 100);
            Console.WriteLine("poyo seed: " + seed);
            if (seed == 0)
            {
                seed = rand.Next(0, int.MaxValue);
                if (seed == 0)
                    return "(> ^ - ^)> KINNIN <(^ - ^ <)";
                return poyos[rand.Next(0, 3)];
            }
            return "(> ^ - ^)> POYO <(^ - ^ <)";
        }

        public void t_Timer2Elapsed(object sender, EventArgs e)
        {
            if (IsJoined && streamName != "poyobot")
                bot.sendData("PRIVMSG", "#" + Name + " :" + getPoyo());
            timer.Stop();
            timer.Interval = new Random().Next(interval - 150000, interval + 150000);
            Console.WriteLine(timer.Interval);
            timer.Start();
        }
    
        
        public void TurnOff()
        {
            timer.Stop();
        }

        public int Interval
        {
            get { return interval / 60000; }
            set
            {
                if (value >= 3 && value < int.MaxValue)
                {
                    interval = value * 60000;
                    timer.Stop();
                    timer.Interval = new Random().Next(interval - 150000, interval + 150000);
                    timer.Start();
                }
            }
        }

        public string Name
        {
            get { return streamName; }
        }

        public bool IsJoined
        {
            get { return isJoined; }
            set { isJoined = value; }
        }

        public bool IsQuoteOn
        {
            get { return turnOffQuote; }
            set { turnOffQuote = value; }
        }
    }
}
