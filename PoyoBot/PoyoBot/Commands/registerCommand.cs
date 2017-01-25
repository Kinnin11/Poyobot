using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot.Commands
{
    class registerCommand : Command
    {
        public registerCommand()
            :base("!poyo", "", 1, CommandLevels.Viewer)
        {
        }

        public override void Handle(string user, string stream, string command, string data, IRCHandling bot)
        {
            if (stream == "#poyobot")
            {
                if (bot.PoyoStreams.Contains(user))
                {
                    bot.sendData("PRIVMSG", "#poyobot :" + user + " -> i'm already sending you poyo in chat (> ^ - ^)> POYO <(^ - ^ <)");
                }
                else
                {
                    bot.HandleRegister(user);
                    bot.sendData("PRIVMSG", "#poyobot :" + user + "-> I will now sit in your chat and post poyo every so often, but only when you're live (> ^ - ^)> POYO <(^ - ^ <)");
                }
            }
        }       
                
    }
}
