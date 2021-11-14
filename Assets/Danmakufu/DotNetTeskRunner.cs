using System;
using System.IO;

namespace cs
{
    public class DotNetTeskRunner
    {
        // dotnet run --project "../Assets/Danmakufu" "../Assets/Resources/DanmakufuTest"
        public static void Main(string[] args)
        {
            var runner = new TestRunner(Read, Log, Log, Log);
            runner.RunAllTests(args[0]);
        }
        public static string Read(string filePath)
        {
            return File.ReadAllText(filePath);
        }
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
