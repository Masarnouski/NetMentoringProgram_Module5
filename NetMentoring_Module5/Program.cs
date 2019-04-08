using System;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using resources = NetMentoring_Module5.Resources.Resource;

namespace NetMentoring_Module5
{
    class Program
    {
        private static readonly FilesConfigurationSection _section =
            (FilesConfigurationSection)ConfigurationManager.GetSection("filesSection");
        private static readonly object _locker = new object();
        private static int _counter;
        static void Main(string[] args)
        {
            foreach (OutputFilePathElement path in _section.OutputFilePaths)
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
                var culture = ConfigurationManager.AppSettings.Get("Culture");
                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
                var regex = new Regex(_section.Rules.RegularExpression);
         
                var fileName = TransformPath(e.FullPath);
                Console.WriteLine(resources.FileFounded, e.Name);
                if (regex.IsMatch(e.Name))
                {
                    Console.WriteLine(resources.FileMatchRegExpr, e.Name);
                    File.Move(e.FullPath, _section.InputFilePath.Value + $"/{fileName}");
                    Console.WriteLine(resources.FileMovedToFolder, e.Name, _section.InputFilePath.Value);
                }
                else
                {
                    Console.WriteLine(resources.NotMatchRegExpr, e.Name);
                    File.Move(e.FullPath, _section.DefaultOutputFilePath.Value + $"/{fileName}");
                    Console.WriteLine(resources.FileMovedToFolder, e.Name, _section.DefaultOutputFilePath.Value);
                }
            }
        }

        /// <summary>
        /// Tranform path according with rules
        /// </summary>
        /// <param name="path">Input string path</param>
        /// <returns>Transformed string path</returns>
        private static string TransformPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(resources.NullOrEmptyException);
            }
            var extension = Path.GetExtension(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            if (_section.Rules.IsDateEnabled)
            {
                fileName += "_" + DateTime.Now.ToShortDateString().Replace("/", ".");
            }
            if (_section.Rules.IsCounterEnabled)
            {
                fileName = fileName.Insert(0, ++_counter + "_");
            }
            return fileName += extension;
        }

        /// <summary>
        /// Subscibe to changes in folder
        /// </summary>
        /// <param name="path"> Path to the folder </param>
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
