using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot
{




    class Program
    {
        /*struct MyStruct
        {
            public string StructName { get; set; }
            public MyStruct(string name)
            {
                StructName = name;
            }
        }
        
        void Main()
        {
            MyStruct m = new MyStruct("name");
            ChangeStruct(m);
            Console.WriteLine(m.StructName);
        }

        void ChangeStruct(MyStruct m)
        {
            m.StructName = "changed";
        }
        */


        static void Main(string[] args)
        {
            IRCConfig conf = new IRCConfig();
            conf.name = "poyobot";
            conf.nick = "poyobot";
            conf.port = 6667;
            conf.server = "irc.chat.twitch.tv";
            IRCHandling Bot = new IRCHandling(conf);            
        }

        public void run()
        {

        }
    }
    struct IRCConfig
    {
        public string server;
        public int port;
        public string nick;
        public string name;
    }
}
