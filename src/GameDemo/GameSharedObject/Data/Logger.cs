using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GameSharedObject.Data
{
    public class Logger
    {
        private static String LOG_FILE = "log.txt";

        public static void Write(String text)
        {
            using (StreamWriter sw = new StreamWriter(LOG_FILE, true)){
                sw.Write(text);
                sw.Close();
            }
        }
        public static void WriteLine(String text)
        {
            using (StreamWriter sw = new StreamWriter(LOG_FILE, true)){
                sw.WriteLine(text);
                sw.Close();
            }
        }
        public static void Clear()
        {
            try { File.Delete(LOG_FILE); }
            catch { }
        }
    }
}
