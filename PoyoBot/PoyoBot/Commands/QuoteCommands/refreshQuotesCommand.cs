using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot.Commands.QuoteCommands
{
    class refreshQuotesCommand : Command
    {
         public refreshQuotesCommand()
            :base("!refreshquotes", "", 1, CommandLevels.Kinnin11)
        {
        }

         public override void Handle(string user, string stream, string command, string data, IRCHandling bot)
         {
             if (CheckCooldown(stream, user) && CheckLevel(user, stream, bot))
             {
                 bot.quoteService.fillQuotes();
                 bot.sendData("PRIVMSG", stream + " :Quotes refreshed :)");
             }
         }
    }
}
