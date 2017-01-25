using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot.Commands
{
    class CheckIntervalCommand : Command
    {
        public CheckIntervalCommand()
            : base("!checkinterval", "", 1, CommandLevels.Streamer)
        {
        }
        public override void Handle(string user, string stream, string command, string data, IRCHandling bot)
        {
            if (CheckCooldown(stream, user) && CheckLevel(user, stream, bot))
            {
                if (stream == "#poyobot")
                {
                    if (bot.PoyoStreams.Contains(user.TrimStart('#')))
                    {
                        bot.sendData("PRIVMSG", stream + " :" + user + ", your (> ^ - ^)> POYO <(^ - ^ <) interval is set to: " + bot.PoyoStreams.Find(user.TrimStart('#')).Interval + " minutes");
                    }
                }
                if (stream.TrimStart('#') == user && bot.PoyoStreams.Contains(stream.TrimStart('#')) || user == "kinnin11")
                {
                    bot.sendData("PRIVMSG", stream + " :" + stream.TrimStart('#') + ", your (> ^ - ^)> POYO <(^ - ^ <) interval is set to: " + bot.PoyoStreams.Find(stream.TrimStart('#')).Interval + " minutes");
                }

            }
        }

    }
}
