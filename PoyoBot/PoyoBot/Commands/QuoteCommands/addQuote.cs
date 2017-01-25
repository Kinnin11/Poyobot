using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot.Commands.QuoteCommands
{
    class addQuote : Command
    {
         public addQuote()
            :base("!addquote", "", 1, CommandLevels.Kinnin11)
        {
        }

         public override void Handle(string user, string stream, string command, string data, IRCHandling bot)
         {
             if (CheckCooldown(stream, user) && CheckLevel(user, stream, bot))
             {
                 try
                 {
                     bot.quoteService.addQuote(data);
                     bot.sendData("PRIVMSG", stream + " :quote added :)");
                 }
                 catch (Exception e)
                 {
                     bot.sendData("PRIVMSG", stream + " :" + e.Message);
                 }
             }
         }
    }
}
