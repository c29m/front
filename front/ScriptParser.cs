using System.Linq;
using System.Text.RegularExpressions;

namespace front
{
    public class ScriptParser
    {
        public ModuleInfo GetModuleInfo(string script)
        {
            var match = System.Text.RegularExpressions.Regex.Match(script, "^[\"'](.*)[\"'];\r?\n", RegexOptions.Singleline);
            if (!match.Success) return null;
            var directives = match.Groups[1].Value.Split(';');
            var moduleInfo = new ModuleInfo();
            foreach (var directive in directives)
            {
                var parts = directive.Split(':');
                var name = parts[0].Trim();
                var value = parts[1].Trim();
                if(name=="depends")
                {
                    if(value.Length>0)
                        moduleInfo.Dependencies.AddRange(value.Split(new char[]{','}).Select(v=>v.Trim()));
                }
                if(name=="provides")
                {
                    moduleInfo.Name = value;
                }
            }
            moduleInfo.Content = script.Substring(match.Index + match.Value.Length, script.Length - match.Value.Length);
            return moduleInfo;
        }
    }
}