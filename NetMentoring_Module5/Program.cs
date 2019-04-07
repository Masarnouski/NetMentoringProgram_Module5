using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;

namespace NetMentoring_Module5
{
    class Program
    {
        private static FilesConfigurationSection section = (FilesConfigurationSection)ConfigurationManager.GetSection("filesSection");
        private static object _locker = new object();
        private static int counter = 0;
        static void Main(string[] args)
        {
            var culture = ConfigurationManager.AppSettings.Get("Culture");

            foreach (OutputFilePathElement path in section.OutputFilePaths)
            {
                ListenToFolder(path.Value);
            }

            do
            {
                Thread.Sleep(100 * 10000);
            }
            while (true);
        }
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            lock (_locker)
            {
                Console.WriteLine($"File: {e.FullPath}| changeType:{e.ChangeType}| Name: {e.Name}");
                Regex regex = new Regex(section.RegularExpression.Value);
                if (regex.IsMatch(e.Name))
                {
                    var extension = Path.GetExtension(e.FullPath);
                    var name = Path.GetFileNameWithoutExtension(e.FullPath);
                    if (section.InputFilePath.isDateEnabled)
                        name += "_" + DateTime.Now.ToShortDateString();
                    if (section.InputFilePath.isCounterEnabled)
                        name = name.Insert(0, (++counter).ToString() + "_");
                    name += extension;
                    CopyFile(e.FullPath, section.InputFilePath.Value + $"/{name}");
                }
                else
                {
                    var extension = Path.GetExtension(e.FullPath);
                    var name = Path.GetFileNameWithoutExtension(e.FullPath);
                    if (section.InputFilePath.isDateEnabled)
                        name += "_" + DateTime.Now.ToShortDateString();
                    if (section.InputFilePath.isCounterEnabled)
                       name = name.Insert(0, (++counter).ToString()+ "_");
                    name += extension;
                    CopyFile(e.FullPath, section.DefaultOutputFilePath.Value + $"/{name}");
                }
            }
        }

        /// <summary>
        /// Copy one file to another
        /// </summary>
        /// <param name="inputPath"> Full path of input file </param>
        /// <param name="outputPath"> Full path of output file </param>
        private static void CopyFile(string inputPath, string outputPath)
        {
            var outputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            var inputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);

            using (inputStream)
            using (outputStream)
            {
                outputStream.CopyTo(inputStream);
            }
        }


        private static void ListenToFolder(string path)
        {
            var watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Created += OnChanged;
            watcher.EnableRaisingEvents = true;
        }
    }
}
