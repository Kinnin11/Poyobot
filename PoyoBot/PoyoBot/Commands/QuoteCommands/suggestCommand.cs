using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot.Commands.QuoteCommands
{
    class suggestCommand : Command
    {
        public suggestCommand()
            :base("!suggestquote", "", 10, CommandLevels.Viewer)
        {
        }

         public override void Handle(string user, string stream, string command, string data, IRCHandling bot)
         {
             if (CheckCooldown(stream, user) && CheckLevel(user, stream, bot))
             {
                     bot.quoteService.addSuggestion(data);
                     bot.sendData("PRIVMSG", stream + " :suggestion added :)");
             }
         }
    }
}
