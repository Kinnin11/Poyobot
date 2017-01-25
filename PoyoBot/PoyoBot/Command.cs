using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot
{
    /// <summary>
    /// Only really used for commands, the level in which the command can be triggered
    /// </summary>
    public enum CommandLevels { Kinnin11, Streamer, Mod, Viewer }

    /// <summary>
    /// Abstract class that has the framework to let new commands be handled
    /// </summary>
    abstract class Command : Response
    {
        /// <summary>
        /// The userlevel needed to use the command
        /// </summary>
        protected CommandLevels commandLevel;
        /// <summary>
        /// The list of channel that have used the command and what the cooldown is for that channel
        /// </summary>
        protected Dictionary<string, DateTime> streamCooldowns = new Dictionary<string, DateTime>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText">The command that will be reacted to, must contain '!' to work</param>
        /// <param name="commandResponse">This is only if you want your command to have a fixed response</param>
        /// <param name="resCooldown">The cooldown of use in seconds</param>
        /// <param name="level">The userlevel needed to use the command</param>
        public Command(string commandText, string commandResponse, int resCooldown, CommandLevels level)
            : base(commandText, commandResponse, resCooldown)
        {
            this.commandLevel = level;
        }

        /// <summary>
        /// Checks if the current user is authorized to use this command
        /// </summary>
        /// <param name="user">The user that tries to use the command</param>
        /// <param name="stream">The channel this command is used in</param>
        /// <param name="bot">The base handler so modlists can be accessed(modlists is TODO)</param>
        /// <returns>Returns a boolean, if true the user may use the command</returns>
        protected bool CheckLevel(string user, string stream, IRCHandling bot)
        {
            if (stream == "#poyobot" && commandLevel != CommandLevels.Kinnin11)
            {
                return true;
            }
            switch (commandLevel)
            {
                case CommandLevels.Kinnin11:
                    if (user != "kinnin11")
                    {
                        return false;
                    }
                    break;

                case CommandLevels.Streamer:
                    if (user != stream.Split('#')[1] && user != "kinnin11")
                    {
                        return false;
                    }
                    break;

                case CommandLevels.Mod:
                    foreach (string mod in bot.modlists[stream])
                    {
                        if (user != mod && user != stream.Split('#')[1] && user != "kinnin11")
                        {
                            return false;
                        }
                    }
                    break;
            }
            if (streamCooldowns.ContainsKey(stream))
            {
                streamCooldowns.Remove(stream);
            }
            streamCooldowns.Add(stream, System.DateTime.Now.AddSeconds(resCooldown));
            return true;
        }

        /// <summary>
        /// Checks for the channel the command was in if the cooldown has elapsed
        /// </summary>
        /// <param name="currentStream">The stream the command was used in</param>
        /// <returns>Returns a boolean, if true the cooldown has elapsed and the command will trigger</returns>
        protected bool CheckCooldown(string currentStream, string user)
        {
            if (user != "kinnin11")
            {
                foreach (KeyValuePair<string, DateTime> stream in streamCooldowns)
                {
                    if (currentStream == stream.Key)
                    {
                        return stream.Value <= System.DateTime.Now;
                    }
                }
            }
            return true;
        }        

        /// <summary>
        /// Abstrac method that 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stream"></param>
        /// <param name="command"></param>
        /// <param name="data"></param>
        /// <param name="bot"></param>
        public abstract void Handle(string user, string stream, string command, string data, IRCHandling bot);

        public string RespondTo
        {
            get { return respondTo; }
        }
       
    }
}
