﻿using FastFile;
using FastFileV2;
using FastFileV3;
using System.Diagnostics;

namespace FastDirTest
{
    public static class StopwatchExtensions
    {
        public static TimeSpan End(this Stopwatch stopwatch)
        {
            if (!stopwatch.IsRunning) return TimeSpan.Zero;
            TimeSpan time = stopwatch.Elapsed;
            stopwatch.Reset();
            return time;

        }
    }
    internal class Program
    {
        internal const int nameWidth = 30, timeWidth = 30, countWidth = 30;
        internal static Stopwatch stopwatch = new();
        internal static int Count { get; set; }
        internal static string SearchPath { get; } = @"C:\";
        internal static TimeSpan Time { get; set; }

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!\n");
            string header = $"| {"Enumerator name",-nameWidth} | {"Enumerating Time",-timeWidth} | {"Enumerated Count",-countWidth} |";
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            stopwatch.Start();
            TestEnumeratingFiles(FastFileInfo.EnumerateFiles(SearchPath, "*", SearchOption.AllDirectories, null), "FastFileInfo");
            //.Select(item => item.FullName /*+ "|" + item.Attributes*/).ToList();

            stopwatch.Start();
            TestEnumeratingFiles(FastFileInfo.GetFiles(SearchPath, "*", SearchOption.AllDirectories), "FastFileInfoGetFiles");

            stopwatch.Start();
            TestEnumeratingFiles(FastFileInfoV2.EnumerateFiles(SearchPath, "*", SearchOption.AllDirectories, SearchFor.Files), "FastFileInfoV2");
            //.Select(item => item.FullName /*+ "|" + item.Attributes*/).ToList();

            stopwatch.Start();
            TestEnumeratingFiles(FastFileInfoV3.GetAllFiles(SearchPath, -1, SearchPath.Split('\\').Length), "FastFileInfoV3.GetAllFiles");

            stopwatch.Start();
            TestEnumeratingFiles(FastFileInfoV3.GetAllFilesV2(SearchPath), "FastFileInfoV3.GetAllFilesV2");

            stopwatch.Start();
            TestEnumeratingFiles(FastFileInfoV3.GetAllFilesV3(SearchPath), "FastFileInfoV3.GetAllFilesV3");

            stopwatch.Start();
            TestEnumeratingFiles(OtherGetFiles.V1GetFiles(SearchPath), "V1GetFiles");

            stopwatch.Start();
            TestEnumeratingFiles(OtherGetFiles.V2GetFiles(SearchPath), "V2GetFiles");

            stopwatch.Start();
            TestEnumeratingFiles(OtherGetFiles.GetAllFilesWithCMD(SearchPath), "GetAllFilesWithCMD").ToList();

            stopwatch.Start();
            TestEnumeratingFiles(OtherGetFiles.GetAllFilesWithPowerShell(SearchPath), "GetAllFilesWithPowerShell");

            Console.WriteLine(new string('-', header.Length));
            Console.WriteLine("\n\nPress any key to exit...");
            //Console.ReadKey();
            //var compare = lista.Except(listb).ToList();
            //foreach (var item in compare) Console.WriteLine(item);
            Console.ReadKey();
        }

        private static IEnumerable<T> TestEnumeratingFiles<T>(IEnumerable<T> listOfFiles, string name)
        {
            Count = listOfFiles.Count();
            Time = stopwatch.End();
            Console.WriteLine($"| {name,-nameWidth} | {Time,-timeWidth} | {Count,-countWidth} |");
            return listOfFiles;
        }
    }
}
