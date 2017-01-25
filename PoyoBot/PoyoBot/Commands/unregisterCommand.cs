using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot.Commands
{
    class unregisterCommand : Command
    {
        public unregisterCommand()
            :base("!unpoyo", "", 1, CommandLevels.Viewer)
        {
        }

        public override void Handle(string user, string stream, string command, string data, IRCHandling bot)
        {
            if (stream == "#poyobot")
            {
                if (bot.PoyoStreams.Contains(user) && user != "poyobot")
                {
                    bot.sendData("PART", "#" + user);
                    bot.PoyoStreams.Find(user).TurnOff();
                    bot.PoyoStreams.Remove(user);
                    bot.SaveStreams();
                    bot.sendData("PRIVMSG", "#poyobot :" + user + "-> I have succesfully left your chat (> ^ - ^)> POYO <(^ - ^ <)");
                }
                else
                {
                    bot.sendData("PRIVMSG", "#poyobot :" + user + "-> I wasn't in your chat to begin with (> ^ - ^)> POYO <(^ - ^ <)");
                }
            }
        }
    }
}
