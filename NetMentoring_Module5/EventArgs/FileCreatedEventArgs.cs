using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMentoring_Module5.EventArgs
{
    public class FileCreatedEventArgs : System.EventArgs
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
    }
}
