using System.Collections.Generic;
using System.IO;
using System.Web;

namespace front
{
    public class ScriptRepository : IScriptRepository
    {
        private readonly Dictionary<string, string> _scriptCache = new Dictionary<string, string>();
        private string _rootPath;

        public string GetScript(string identifier)
        {
            return _scriptCache.ContainsKey(identifier) ? _scriptCache[identifier] : fetch(identifier);
        }

        private string fetch(string identifier)
        {
            var path = string.IsNullOrEmpty(_rootPath) ? HttpContext.Current.Server.MapPath(identifier) : Path.Combine(_rootPath, identifier.Replace("~/", "").Replace("/", "\\"));
            return _scriptCache[identifier] = File.ReadAllText(path);
        }
    }
}