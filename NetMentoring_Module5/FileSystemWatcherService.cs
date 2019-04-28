using NetMentoring_Module5.Configuration;
using NetMentoring_Module5.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMentoring_Module5
{
    class FileSystemWatcherService
    {
        private List<DirectoryElement> _directories;

        public FileSystemWatcherService(List<DirectoryElement> directories)
        {
            this._directories = directories;
            FillListOfWatchers();
        }

        public event EventHandler<FileCreatedEventArgs> FileCreatedEvent;

        private void OnCreated(string name, string path)
        {
            FileCreatedEvent?.Invoke(this, new FileCreatedEventArgs { Name = name, FilePath = path });
        }


        private void FillListOfWatchers()
        {
            foreach (var directory in _directories)
            {
                FileSystemWatcher watcher = new FileSystemWatcher(directory.SourceDirectory);
                watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                watcher.Created += (obj, args) =>
                {
                    OnCreated(args.Name, args.FullPath);
                };
                watcher.EnableRaisingEvents = true;
            }
        }
    }
}
