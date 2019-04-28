using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMentoring_Module5.Configuration
{
    class DirectoryElement: ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true)]
        public string SourceDirectory
        {
            get
            {
                return (string)base["path"];
            }
        }
    }
}
