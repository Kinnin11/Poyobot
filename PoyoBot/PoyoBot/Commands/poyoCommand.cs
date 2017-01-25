using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot
{
    class poyoCommand : Command
    {
        public poyoCommand()
            :base("!poyo", "", 60, CommandLevels.Viewer)
        {
        }
        public override void Handle(string user, string stream, string command, string data, IRCHandling bot)
        {
            if (CheckCooldown(stream, user) && CheckLevel(user, stream, bot))
            {
                if (stream != "#poyobot")
                {
                    bot.sendData("PRIVMSG", stream + " :" + PoyoStream.getPoyo());
                }
            }
        }
    }
}
