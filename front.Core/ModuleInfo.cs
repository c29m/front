using System.Collections.Generic;

namespace front.Core
{
    public class ModuleInfo
    {
        public ModuleInfo()
        {
            Dependencies = new List<string>();
        }

        public bool Packaged { get; set; }
        public string Name { get; set; }
        public List<string> Dependencies { get; set; }
        public string Content { get; set; }
    }
}