using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace front
{
    public class ModuleParser
    {
        public ModuleInfo Parse(string script)
        {
            var alreadyModuleDef = Regex.Match(script, "^require.define\\(", RegexOptions.Singleline);
            var directiveHeader = Regex.Match(script, "^[\"'](.*)[\"'];\r?\n", RegexOptions.Singleline);
            if (alreadyModuleDef.Success || !directiveHeader.Success) return new ModuleInfo() { Content = script, Dependencies = new List<string>(), Packaged = true };
            var directives = directiveHeader.Groups[1].Value.Split(';');
            var moduleInfo = new ModuleInfo();
            foreach (var directive in directives)
            {
                var parts = directive.Split(':');
                var name = parts[0].Trim();
                var value = parts[1].Trim();
                if (name == "depends")
                {
                    if (value.Length > 0)
                        moduleInfo.Dependencies.AddRange(value.Split(new char[] { ',' }).Select(v => v.Trim()));
                }
                if (name == "provides")
                {
                    moduleInfo.Name = value;
                }
            }
            moduleInfo.Content = script.Substring(directiveHeader.Index + directiveHeader.Value.Length, script.Length - directiveHeader.Value.Length);
            return moduleInfo;
        }
    }
}