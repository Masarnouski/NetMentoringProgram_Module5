using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMentoring_Module5
{
    class FilesConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("appName")]
        public string ApplicationName { get { return (string)base["appName"]; } }

        [ConfigurationProperty("inputFilesPath")]
        public InputFilePathElement InputFilePath { get { return (InputFilePathElement)this["inputFilesPath"]; } }

        [ConfigurationProperty("outputFilesPaths")]
        public OutputFilePathCollection OutputFilePaths { get { return (OutputFilePathCollection)this["outputFilesPaths"]; } }

        [ConfigurationProperty("defaultInputPath")]
        public DefaultInputFilePathElement DefaultOutputFilePath { get { return (DefaultInputFilePathElement)this["defaultInputPath"]; } }

        [ConfigurationProperty("regularExpression")]
        public RegularExpressionElement RegularExpression { get { return (RegularExpressionElement)this["regularExpression"]; } }
    }

    class OutputFilePathCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new OutputFilePathElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((OutputFilePathElement)element).Value;
        }
    }

    class OutputFilePathElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsKey = true)]
        public string Value { get { return (string)this["value"]; } }
    }

    class InputFilePathElement : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public string Value { get { return (string)this["value"]; } }

        [ConfigurationProperty("isCounterEnabled")]
        public bool isCounterEnabled { get { return (bool)this["isCounterEnabled"]; } }

        [ConfigurationProperty("isDateEnabled")]
        public bool isDateEnabled { get { return (bool)this["isDateEnabled"]; } }
    }
    class DefaultInputFilePathElement : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public string Value { get { return (string)this["value"]; } }

        [ConfigurationProperty("isCounterEnabled")]
        public bool isCounterEnabled { get { return (bool)this["isCounterEnabled"]; } }

        [ConfigurationProperty("isDateEnabled")]
        public bool isDateEnabled { get { return (bool)this["isDateEnabled"]; } }
    }
    class RegularExpressionElement : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public string Value { get { return (string)this["value"]; } }

    }
}
