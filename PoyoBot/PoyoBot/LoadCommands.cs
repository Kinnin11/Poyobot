using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoyoBot.Commands;
using PoyoBot.Commands.QuoteCommands;

namespace PoyoBot
{ 
    partial class IRCHandling
    {
        private void LoadCommands()
        {
            commandList.Add(new registerCommand());
            commandList.Add(new unregisterCommand());
            commandList.Add(new VariableMSGCommand("!dedede", "http://i.imgur.com/LExjQ9x.png", 6, CommandLevels.Viewer));
            commandList.Add(new SetIntervalCommand());
            commandList.Add(new poyoCommand());
            commandList.Add(new CheckIntervalCommand());
            commandList.Add(new VariableMSGCommand("!whatispoyo", "(> ^ - ^)> POYO <(^ - ^ <) https://www.youtube.com/watch?v=XBvRzwXxzSQ (> ^ - ^)> POYO <(^ - ^ <)" , 60, CommandLevels.Viewer));
            commandList.Add(new addQuote());
            commandList.Add(new quoteCommand());
            //commandList.Add(new QwoteCommand());
            commandList.Add(new suggestCommand());
            commandList.Add(new refreshQuotesCommand());
            commandList.Add(new QuoteOption());
            commandList.Add(new VariableMSGCommand("!meta", "<<<<|=Q^( `´Q)^ FIGHT ME", 10, CommandLevels.Viewer));
            //responseList.Add(new Response("*** you poyobot", "fuck you too t('w't)", 10));
            //responseList.Add(new Response("fuck you poyobot", "fuck you too t('w't)", 10));
            //responseList.Add(new Response("(> ^ - ^)> POYO <(^ - ^ <)", "(> ^ - ^)> POYO <(^ - ^ <)", 10));
            //responseList.Add(new Response("NastyPlot", "NastyPlot http://i.imgur.com/gjP68Ip.png NastyPlot", 100));
        }
    }
}
