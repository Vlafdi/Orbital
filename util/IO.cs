using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital.util {
    static class IO {
        public static String[][] FileToArray(String path) {
            
            string[] lines = System.IO.File.ReadAllLines(path);
            string[][] splitted = new String[lines.Length][];
            for (int i = 0; i < lines.Length; i++) {
                splitted[i] = lines[i].Split(',');
            }
            return splitted;
        }
    }
}
