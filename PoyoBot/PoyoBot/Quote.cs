using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot
{
   public class Quote
    {
        private string content;
        private DateTime time = new DateTime(DateTime.Now.Ticks);

        public Quote(string cont) 
        {
            this.content = cont;
            this.time.AddMinutes(-21);
        }

        public string Content
        {
            get
            {
                    return content;
            }
        }

        //adds the cooldown and returns if the quote is used or not
        public bool used()
        {
            if (time < DateTime.Now)
            {
                this.time = new DateTime(DateTime.Now.Ticks).AddMinutes(30);
                Console.WriteLine("yes quote");
                return false;
            }
            Console.WriteLine("no quote");
            return true;
        }
    }
}
