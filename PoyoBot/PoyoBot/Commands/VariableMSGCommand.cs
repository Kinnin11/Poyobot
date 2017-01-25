using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot.Commands
{
    class VariableMSGCommand : Command
    {
        public VariableMSGCommand(string resText, string resData, int resCooldown, CommandLevels level)
            : base(resText, resData, resCooldown, level)
        {
        }

        public override void  Handle(string user, string stream, string command, string data, IRCHandling bot)
        {
            if (CheckCooldown(stream, user) && CheckLevel(user, stream, bot))
            {
                bot.sendData("PRIVMSG", stream + " :" + respondWith);
            }
        }
    }
}
