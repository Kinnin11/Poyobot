using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Timers;
using System.Threading;
using System.Net;
using System.Web.Script.Serialization;
using System.Collections.Concurrent;

namespace PoyoBot
{
    partial class IRCHandling
    {
        private PoyoStreamList poyoStreams = new PoyoStreamList();
        private List<Command> commandList = new List<Command>();
        private List<Response> responseList = new List<Response>();
        private IRCConfig config;
        private Thread SendMessageThread;
        public Dictionary<string, string[]> modlists = new Dictionary<string, string[]>();
        public QuoteService quoteService;
        TcpClient IRCConnection = null;
        NetworkStream ns = null;
        StreamReader sr = null;
        StreamWriter sw = null;
        bool joining = false;
        bool shouldRun = true;
        bool startup = true;
        System.Timers.Timer timer = new System.Timers.Timer();
        static readonly Object _lock = new object();
        private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
        private DateTime lastMessageSent = DateTime.Now;

        public IRCHandling(IRCConfig config)
        {
            this.config = config;
            SendMessageThread = new Thread(new ThreadStart(dequeueMessage));
            SendMessageThread.Start();

            Console.WriteLine("Welcome to Poyobot version 3.1.6");

            try
            {
                IRCConnection = new TcpClient(config.server, config.port);
            }
            catch
            {
                Console.WriteLine("Connection Error");
            }

            try
            {
                ns = IRCConnection.GetStream();
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);
                sendData("PASS", "oauth:fqvokjqzezh8nl9b03334tf13gp6lm");
                sendData("USER", config.nick);
                sendData("NICK", config.nick);
                sendData("CAP REQ", " :twitch.tv/membership");                
                LoadCommands();
                timer.Interval = 8000;
                timer.Elapsed += t_TimerElapsed;
                timer.Start();

                quoteService = new QuoteService();

                IRCWork();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void t_TimerElapsed(object sender, ElapsedEventArgs e)
        {
            LoadStreams();
            timer.Stop();
        }

        public bool checkIfStreamIsLive(string stream)
        {
            /*if (stream != "poyobot")
            {
                string test = "";
                Console.WriteLine("checking stream: " + stream);
                using (WebClient strJson = new WebClient())
                {
                    try
                    {
                        test = strJson.DownloadString("https://api.twitch.tv/kraken/streams/" + stream + "?oauth_token=#access_token=b19q75u95i9jtatkt4lysr7yenecku");
                    }
                    catch (WebException we)
                    {
                        Console.WriteLine(we.Message);
                    }
                }

                if (!test.Contains("\"stream\":null"))
                {
                    return true;
                }
            }
            return false;*/
            return true;
        }


        private void checkStreams()
        {
            List<PoyoStream> checklist = new List<PoyoStream>();
            checklist = poyoStreams.ToList<PoyoStream>();

            foreach (PoyoStream stream in checklist)
            {
                if (checkIfStreamIsLive(stream.Name))
                {
                    if (!stream.IsJoined)
                    {
                        //sendData("PRIVMSG", "#" + stream.Name + " :(> ^ - ^)> POYO <(^ - ^ <)");
                        //HandleJoin(stream.Name);
                        //stream.IsJoined = true;
                    }
                }
                else if (stream.IsJoined)
                {
                    sendData("PART", "#" + stream.Name);
                    stream.IsJoined = false;
                }
            }
            checklist = null;
        }

        public void writeLog(string logEntry)
        {
            StreamWriter SW = new StreamWriter("log.txt", true);
            SW.WriteLine(DateTime.Now.ToString("dd/MM/yy H:mm:ss") + ": " + logEntry);
            SW.Close();
        }

        private void dequeueMessage()
        {
            while (true)
            {
                if (DateTime.Now > lastMessageSent.AddSeconds(1.5) && messageQueue.Count != 0)
                {
                    string message;
                    messageQueue.TryDequeue(out message);
                        sw.WriteLine(message);
                        sw.Flush();
                        Console.WriteLine(message);
                        //writeLog("Message: " + message + "| Queue size: " + messageQueue.Count + "| LastMessageSent(before edit): " + lastMessageSent);
                        lastMessageSent = DateTime.Now;
                    
                }
            }
        }
        

        public void sendData(string cmd, string param)
        {
            if (param == null)
            {
                sw.WriteLine(cmd);
                sw.Flush();
                Console.WriteLine(cmd);
            }
            else
            {
                messageQueue.Enqueue(cmd + " " + param);
            }
        }
    

        public void SaveStreams()
        {
            if (!startup)
            {
                StreamWriter SW = new StreamWriter("streams.txt", false);
                foreach (PoyoStream p in poyoStreams)
                {
                    SW.WriteLine(p.Name + ";" + p.Interval + ";" + p.IsQuoteOn);
                }
                SW.Close();
            }
        }

        public void HandleRegister(string ex)
        {
            string[] s = ex.Split(';');
            if (s.Length < 3)
            {
                PoyoStreams.Add(new PoyoStream(ex, this));
            }
            else if (s.Length == 3)
            {
                PoyoStreams.Add(new PoyoStream(s[0], this, int.Parse(s[1]), bool.Parse(s[2])));
            }

            SaveStreams();
            HandleJoin(ex.Split(';')[0]);
        }

        public void HandleJoin(string ex)
        {
            joining = true;
            try
            {
                string[] s = ex.Split(';');
                if (s.Length == 1)
                {
                    sendData("JOIN", "#" + ex);

                }
                else if (s.Length == 2)
                {
                    sendData("JOIN", "#" + s[0]);
                }
            }

            catch
            {
                Console.WriteLine("JoinException!");
            }
            joining = false;
        }

        public bool IRCWork()
        {
            string[] ex;
            string data = "";

            while (shouldRun)
            {
                if (!joining)
                {
                    try
                    {
                        data = sr.ReadLine();
                    }
                    catch (IOException io)
                    {
                        return false;
                    }
                    if (data == null)
                    {
                        HandleDC();
                    }
                    else if (data.Contains(":tmi.twitch.tv CAP * ACK"))
                    {
                        HandleJoin("poyobot");
                    }
                    else
                    {
                        Console.WriteLine(data);
                        char[] charSeparator = new char[] { ' ' };
                        ex = data.Split(charSeparator, 5);

                        if (ex[0] == "PING")
                        {
                            sendData("PONG", ex[1]);                            
                        }

                        if (ex.Length >= 3 && ex[2] != "#poyobot" && ex[0].Split('!')[0].Split(':')[1] != "poyobot")
                        {
                            if (ex[1].Contains("JOIN"))
                            {

                                string user = ex[0].Split('!')[0].Split(':')[1];
                                if (ex[2].Contains(user))
                                {
                                    PoyoStreams.Find(ex[2]).IsJoined = true;
                                }
                            }

                            if (ex[1].Contains("PART"))
                            {
                                string user = ex[0].Split('!')[0].Split(':')[1];
                                if (ex[2].Contains(user))
                                {
                                    PoyoStreams.Find(ex[2]).IsJoined = false;
                                }
                            }
                        }

                        if (ex[1].Contains("PRIVMSG"))
                        {
                            if (ex.Length >= 4 && (ex[3] != ":" && ex[3].ToCharArray()[1] == '!'))
                            {
                                string command = ex[3]; //grab the command sent
                                string user = ex[0].Split('!')[0].Split(':')[1];
                                if (user == "kinnin11")
                                {
                                    if (command.Contains("!join"))
                                    {
                                        HandleJoin(ex[4]);
                                    }
                                    else if (command.Contains("!part"))
                                    {
                                        sendData("PART", "#" + ex[4]);
                                        SaveStreams();
                                    }
                                }
                                foreach (Command cmd in commandList)
                                {
                                    if (command == ":" + cmd.RespondTo)
                                    {
                                        if (ex.Length == 5)
                                        {
                                            cmd.Handle(user, ex[2], command, ex[4], this);
                                        }
                                        else if (ex.Length == 4)
                                        {
                                            cmd.Handle(user, ex[2], command, "", this);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string user = ex[0].Split('!')[0].Split(':')[1];
                                foreach (Response res in responseList)
                                {
                                    if (ex.Length <= 4)
                                        res.CanRespond(user, ex[2], ex[3], this);
                                    else
                                        res.CanRespond(user, ex[2], ex[3] + " " + ex[4], this);

                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        public void HandleDC()
        {
            /*string[] streams;
            streams = modlists.Keys.ToArray<string>();
            for (int i = 0; i < modlists.Count; i++)
            {
                HandleJoin(new string[5] { "", "", "", "", streams[i] });
            }*/

        }        

        public void LoadStreams()
        {
            StreamReader SR = new StreamReader("streams.txt");
            string s = "";
            while (s != null)
            {
                s = SR.ReadLine();
                if (s != null)
                    HandleRegister(s);
            }
            SR.Close();
            startup = false;
        }

        public void GrabMods(string ex)
        {
            sendData("PRIVMSG", "#" + ex + " " + "/mods");
            string[] ex2;
            while (true)
            {
                joining = true;
                string data = sr.ReadLine();
                Console.WriteLine(data);
                char[] charSeparator = new char[] { ' ' };
                ex2 = data.Split(charSeparator, 5);
                string sw = ex2[0];
                string user = sw.Split('!')[0].Split(':')[1];


                if (user == "jtv" && ex2[3].Contains(":The"))
                {
                    char[] charSeparator2 = new char[] { ':', ',' };
                    string modGroup = ex2[4].Remove(0, 29).Replace(" ", "");
                    string[] modlist = modGroup.Split(charSeparator2);
                    modlists.Add(ex, modlist);
                    joining = false;
                    break;
                }
            }
        }

        public PoyoStreamList PoyoStreams
        {
            get
            {
                lock (_lock)
                {
                    return poyoStreams;
                }
            }
        }
    }
}
