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

        [ConfigurationProperty("rules")]
        public RulesElement Rules { get { return (RulesElement)this["rules"]; } }
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
    }

    class DefaultInputFilePathElement : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public string Value { get { return (string)this["value"]; } }
    }
    class RulesElement : ConfigurationElement
    {
        [ConfigurationProperty("regularExpression")]
        public string RegularExpression { get { return (string)this["regularExpression"]; } }

        [ConfigurationProperty("isCounterEnabled")]
        public bool IsCounterEnabled { get { return (bool)this["isCounterEnabled"]; } }

        [ConfigurationProperty("isDateEnabled")]
        public bool IsDateEnabled { get { return (bool)this["isDateEnabled"]; } }

    }
}
