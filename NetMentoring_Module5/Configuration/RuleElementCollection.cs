using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMentoring_Module5.Configuration
{
    class RuleElementCollection: ConfigurationElementCollection
    {
        [ConfigurationProperty("defaultDirectory")]
        public string DefaultDirectory
        {
            get { return (string)this["defaultDirectory"]; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuleElement)element).Template;
        }
    }
}
