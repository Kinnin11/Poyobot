using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot.Commands
{
    class SetIntervalCommand : Command
    {
        public SetIntervalCommand()
            :base("!setinterval", "", 1, CommandLevels.Streamer)
        {
        }

        public override void Handle(string user, string stream, string command, string data, IRCHandling bot)
        {
            if (CheckCooldown(stream, user) && CheckLevel(user, stream, bot))
            {
                foreach (PoyoStream pS in bot.PoyoStreams)
                {
                    if (pS.Name == user.TrimStart('#') || user == "kinnin11")
                    {
                        Console.WriteLine("trying to change interval");
                        try
                        {
                            if (int.Parse(data.Trim()) < 3)
                            {
                                bot.sendData("PRIVMSG", stream + " :I don't want to spam your chat \"too\" much (> ^ - ^)> POYO <(^ - ^ <)");
                                return;
                            }
                            else
                            {
                                pS.Interval = int.Parse(data.Trim());
                                bot.sendData("PRIVMSG", stream + " :" + user + ", Your (> ^ - ^)> POYO <(^ - ^ <) interval has been set to: " + pS.Interval + " minutes");
                                bot.SaveStreams();
                                return;
                            }
                        }
                        catch (FormatException e)
                        {
                            bot.sendData("PRIVMSG", stream + " :Please enter only a number (> ^ - ^)> POYO <(^ - ^ <)");
                            return;
                        }
                        catch (OverflowException e)
                        {
                            bot.sendData("PRIVMSG", stream + " :I WANT TO POYO IN YOUR STREAM AT LEAST ONCE BibleThump , PLEASE LOWER YOUR NUMBER");
                            return;
                        }
                    }
                }
            }
        }
    }
}
