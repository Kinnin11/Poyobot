using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot.Commands.QuoteCommands
{
    class quoteCommand : Command
    {
        public quoteCommand()
              :base("!quote", "", 20, CommandLevels.Viewer)
        {
        }

        public override void Handle(string user, string stream, string command, string data, IRCHandling bot)
        {
            if (CheckCooldown(stream, user) && CheckLevel(user, stream, bot))
            {
                if (bot.PoyoStreams.Find(stream).IsQuoteOn)
                {
                    Quote q = bot.quoteService.Quote;
                    int counter = 0;
                    while (q.used())
                    {
                        q = bot.quoteService.Quote;
                        counter++;
                        if (counter > 5000)
                            break;
                    }
                        bot.sendData("PRIVMSG", stream + " :" + q.Content);
                }
            }
        }
    }
}
