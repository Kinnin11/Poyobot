using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot
{
    /// <summary>
    /// Object used to let poyobot respond to certain text in IRC channels
    /// </summary>

    class Response
    {
        protected string respondTo, respondWith;
        protected int resCooldown;
        protected Dictionary<string, DateTime> streamCooldowns = new Dictionary<string, DateTime>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="respondTo">The text in the channel the response will trigger on</param>
        /// <param name="respondWith">The text the bot will send to the channel when triggered</param>
        /// <param name="resCooldown">The amount of time inbetween the bot being able to trigger</param>
        public Response(string respondTo, string respondWith, int resCooldown)
        {
            this.respondTo = respondTo;
            this.respondWith = respondWith;
            this.resCooldown = resCooldown;
        }

        /// <summary>
        /// Checks if cooldown has elapsed for the current channel.
        /// </summary>
        /// <param name="currentStream">The channel that wants to try and trigger the response.</param>
        /// <returns>a boolean that's true if the cooldown has elapsed</returns>
        protected bool CheckCooldown(string currentStream)
        {
            foreach (KeyValuePair<string, DateTime> stream in streamCooldowns)
            {
                if (currentStream == stream.Key)
                {
                    return stream.Value <= System.DateTime.Now;
                }
            }
            return true;
        }   

        /// <summary>
        /// First checks if the text can be responded upon, then sends the data into the channel.
        /// </summary>
        /// <param name="user">The user that put the message on the channel</param>
        /// <param name="stream">The channel the message is on</param>
        /// <param name="data">What the message contains</param>
        /// <param name="bot">the base handler so the response can send it to the proper channel</param>
        /// <returns>a boolean that's true when the response has been triggered</returns>
        public bool CanRespond(string user, string stream, string data, IRCHandling bot)
        {
            if (user != "poyobot" && CheckCooldown(stream) && data.Contains(respondTo))
            {
                bot.sendData("PRIVMSG", stream + " :" + respondWith);
                if(streamCooldowns.ContainsKey(stream))
                {
                    streamCooldowns.Remove(stream);
                }
                streamCooldowns.Add(stream, System.DateTime.Now.AddSeconds(resCooldown)); 
                return true;
            }
            return false;
        }
    }
}
