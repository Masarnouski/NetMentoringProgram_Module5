using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;
using NetMentoring_Module5.Configuration;
using System.Globalization;
using NetMentoring_Module5.EventArgs;

namespace NetMentoring_Module5
{
    class Program
    {
        private static List<DirectoryElement> _directories;
        private static List<RuleElement> _rules;
        private static FileDistributorService _distributor;

        static void Main(string[] args)
        {
            FileSystemSettings config = (FileSystemSettings)ConfigurationManager.GetSection("fileSystemSettings");

            if (config != null)
            {
                ReadConfig(config);
            }

            _distributor = new FileDistributorService(_rules, new DirectoryInfo(config.Rules.DefaultDirectory));
            var watcherService = new FileSystemWatcherService(_directories);
           
            watcherService.FileCreatedEvent += OnFileCreated;
            while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
        }

        private static void OnFileCreated(object sender, FileCreatedEventArgs args)
        {
            _distributor.MoveFile(args.Name, args.FilePath);
            Console.WriteLine(resources.FileFounded, args.Name);
        }

        private static void ReadConfig(FileSystemSettings config)
        {
            _directories = new List<DirectoryElement>(config.Directories.Count);
            _rules = new List<RuleElement>();

            foreach (DirectoryElement directory in config.Directories)
            {
                _directories.Add(directory);
            }

            foreach (RuleElement rule in config.Rules)
            {
                _rules.Add(rule);
            }

            CultureInfo.DefaultThreadCurrentCulture = config.Culture;
            CultureInfo.DefaultThreadCurrentUICulture = config.Culture;
            CultureInfo.CurrentUICulture = config.Culture;
            CultureInfo.CurrentCulture = config.Culture;
        }

    }
}
