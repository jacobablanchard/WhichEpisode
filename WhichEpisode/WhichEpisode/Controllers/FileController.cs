using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WhichEpisode.Controllers
{
    public static class FileController
    {

        public static bool DoesTokenFileExist() {
            //Console.WriteLine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "token.txt"));
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "token.txt");
            return File.Exists(fileName);
        }
        public static void SaveToken(string token) {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "token.txt");
            DateTime now = DateTime.Now;
            File.WriteAllText(fileName, now.ToString() + "\n" + token);
        }

        public static DateTime GetTokenDate() {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "token.txt");
            string contents = File.ReadAllText(fileName);
            string[] split = contents.Split('\n');
            return DateTime.Parse(split[0]);
        }

        public static string GetToken() {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "token.txt");
            string contents = File.ReadAllText(fileName);
            string[] split = contents.Split('\n');
            return split[1];
        }
    }
}
