using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot.Commands.QuoteCommands
{
    class QuoteOption : Command
    {
        public QuoteOption()
            : base("!setquote", "", 1, CommandLevels.Streamer)
        {
        }
        public override void Handle(string user, string stream, string command, string data, IRCHandling bot)
        {
            if(CheckCooldown(stream) && CheckLevel(user, stream, bot))
            {
                foreach (PoyoStream pS in bot.PoyoStreams)
                {
                    if (pS.Name == user.TrimStart('#') || user == "kinnin11")
                    {
                        if(data == null) {
                            if (pS.IsQuoteOn)
                            {
                                pS.IsQuoteOn = false;
                            }
                            else
                            {
                                pS.IsQuoteOn = true;
                            }
                        }
                        else if (data.Contains("off") || data.Contains("false") || data.Contains("disable"))
                        {
                            pS.IsQuoteOn = false;
                        }
                        else if (data.Contains("on") || data.Contains("true") || data.Contains("enable"))
                        {
                            pS.IsQuoteOn = true;
                        }
                        bot.sendData("PRIVMSG", stream + " :" + user + ", quotes have been turned " + ((pS.IsQuoteOn) ? "on" : "off") + " :)");
                        bot.SaveStreams();
                    }
                }
            }
        }
    }
}
