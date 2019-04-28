using NetMentoring_Module5.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetMentoring_Module5
{
    public class FileDistributorService
    {
        private List<RuleElement> _rules;
        private DirectoryInfo _defaultDir;
        private static object _locker = new object();
        private int counter = 0;

        public FileDistributorService(List<RuleElement> rules, DirectoryInfo defaultDir)
        {
            _rules = rules;
            _defaultDir = defaultDir;
        }

        public void MoveFile(string fileName, string sourcePath)
        {
            string destinationPath;
            foreach (var rule in _rules)
            {
                Regex regex = new Regex(rule.Template);
                if (regex.IsMatch(fileName))
                {
                    Console.WriteLine(resources.FileMatchRegExpr, fileName);
                    destinationPath = CreateDestinationPath(rule, fileName, counter);
                    CopyFile(sourcePath, destinationPath);
                    return;
                }
            }
            CopyFile(sourcePath, _defaultDir + $"/{fileName}");

        }


        private string CreateDestinationPath(RuleElement rule, string name, int counter)
        {
            string ext = Path.GetExtension(name);
            string fileName = Path.GetFileNameWithoutExtension(name);
            if (rule.IsSerialNumberNeeded)
            {
                fileName = fileName.Insert(0, $"{counter}"+ "_");
                counter++;
            }
            var result = new StringBuilder().Append(Path.Combine(rule.DestinationDirectory, fileName));
            if (rule.IsDateNeeded)
            {
                var format = CultureInfo.CurrentCulture.DateTimeFormat;
                format.DateSeparator = ".";
                result.Append($"_{DateTime.Now.ToLocalTime().ToString(format.ShortDatePattern)}");
            }
         
            result.Append(ext);
            return result.ToString();
        }

        private void CopyFile(string inputPath, string outputPath)
        {
            lock (_locker)
            {
                File.Move(inputPath, outputPath);
            }
        }
    }
}