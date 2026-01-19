using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace CET2007
{

        public class Logger
        {
            private static Logger instance; 
            private static string logFile = "logs.txt";
            private Logger() { }
            public static Logger GetInstance()
            {
                if (instance == null)
                    instance = new Logger();
                return instance;
            }
            public void Log(string message)
            {
                string entry = "[LOG - " + DateTime.Now.ToString("HH:mm:ss") + "] " + message;
                Console.WriteLine(entry); // still prints to console
                File.AppendAllText(logFile, entry + "\n"); // now also writes to file
            }
        }



    
}
