using System;

namespace front
{
    public class CommonJsFormatter : ICommonJsFormatter
    {
        private const string ModuleFormat = "require.define(\n{\n\t\"<name>\": function(require, exports){\n\t\t<content>\n\t}\n}, <dependencies>);";
        private const string indent = "  ";
        
        public string GetCommonJsModule(ModuleInfo moduleInfo)
        {
            return ModuleFormat
                .Replace("<name>", moduleInfo.Name)
                .Replace("<content>", moduleInfo.Content.Replace("\n", "\t\t\n"))
                .Replace("<dependencies>", moduleInfo.Dependencies.Count==0 ? "[]" :  string.Format("['{0}']", String.Join("','", moduleInfo.Dependencies)))
                .Replace("\t", indent);
        }
    }
}