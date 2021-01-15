using System;
using System.Linq;

namespace Server.Core.Internal
{
    public static class Log
    {
        private static readonly string foreground = "^";
        private static readonly string background = "&";

        public static void Write(object value) => Console.Write(value);
        public static void WriteLog(string format)
        {
            WriteLine($" ^{ (int)ConsoleColor.DarkGray } [{ GetTime() }] ^{ (int)ConsoleColor.White } { format }");

            var result = "";

            for (int i = 0; i < Console.WindowWidth - 1; i++)
            {
                result += "-";
            }

            WriteLine("^8 " + result);
        }
        public static void WriteLog(string format, params string[] args)
        {
            WriteLine($" ^{ (int)ConsoleColor.DarkGray } [{ GetTime() }] ^{ (int)ConsoleColor.White } { format }", args);

            var result = "";

            for (int i = 0; i < Console.WindowWidth - 1; i++)
            {
                result += "-";
            }

            WriteLine("^8 " + result);
        }
        public static void Warn(object value)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(value);
            Console.ResetColor();
        }
        public static void Error(object value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            Console.ResetColor();
        }
        public static void WriteLine(string format)
        {
            var lines = format.Split(' ');

            for (int i = 0; i < lines.Count(); i++)
            {
                var word = lines[i];

                if (word.StartsWith(foreground))
                {
                    Console.ForegroundColor = (ConsoleColor)int.Parse(word.Remove(0, 1));
                }
                else if (word.StartsWith(background))
                {
                    Console.BackgroundColor = (ConsoleColor)int.Parse(word.Remove(0, 1));
                }
                else
                {
                    Console.Write(lines[i] + " ");
                }
            }

            Console.ResetColor();
            Console.WriteLine();
        }
        public static void WriteLine(string format, params string[] args)
        {
            var lines = format.Split(' ');
            var buildedLine = "";

            for (int i = 0; i < lines.Count(); i++)
            {
                var word = lines[i];

                if (word.Contains("{") && word.Contains("}"))
                {
                    var split = word.Split(new string[] { "{", "}" }, StringSplitOptions.None);
                    var n = int.Parse(split[1]);
                    var ca = args[n];
                    buildedLine += split[0] + ca + split[2] + " ";
                }
                else
                {
                    buildedLine += lines[i] + " ";
                }
            }

            var words = buildedLine.Split(' ');

            for (int i = 0; i < words.Count(); i++)
            {
                var word = words[i];

                if (word.StartsWith(foreground))
                {
                    Console.ForegroundColor = (ConsoleColor)int.Parse(word.Remove(0, 1));
                }
                else if (word.StartsWith(background))
                {
                    Console.BackgroundColor = (ConsoleColor)int.Parse(word.Remove(0, 1));
                }
                else
                {
                    Console.Write(word + " ");
                }
            }

            Console.ResetColor();
            Console.WriteLine();
        }

        private static string GetTime()
        {
            var local = DateTime.Now.ToLocalTime();

            return $"{ (local.Hour < 10 ? "0" + local.Hour : local.Hour.ToString()) }:" +
                $"{ (local.Minute < 10 ? "0" + local.Minute : local.Minute.ToString()) }:" +
                $"{ (local.Second < 10 ? "0" + local.Second : local.Second.ToString()) }";
        }
    }
}
