using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GameDemo1.Data
{
    public class Logger
    {
        private static String PATH = @"C:\huy.txt";
        public static void Save(String s)
        {
            using (StreamWriter sw = new StreamWriter(PATH, true))
            {
                sw.Write(s);
                sw.Close();
            }
        }
        public static void Save(Dictionary<int, ValueItem> obj)
        {
            using (StreamWriter sw = new StreamWriter(PATH, true))
            {
                foreach(KeyValuePair<int, ValueItem> item in obj)
                    sw.Write(item.Key + " ");
                sw.Close();
            }
        }
    }
}
