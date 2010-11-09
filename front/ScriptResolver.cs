using System;
using System.Web;
using System.IO;

namespace front
{
    public class ScriptResolver
    {
        private readonly ModulePackager _modulePackager;
        private readonly string _rootPath;

        public ScriptResolver(ModulePackager modulePackager, string rootPath)
        {
            _modulePackager = modulePackager;
            _rootPath = rootPath;
        }
        public string ProvideScript(string modulePath)
        {
            if(!modulePath.StartsWith("~/script"))
                throw new InvalidOperationException("script not in default location");

            var isModule = false;
            var filePath = modulePath;
            if (!modulePath.EndsWith(".js") && (isModule = true))
                filePath += ".js";
            var path = string.IsNullOrEmpty(_rootPath) ? HttpContext.Current.Server.MapPath(filePath) : Path.Combine(_rootPath, filePath.Replace("~/", "").Replace("/", "\\"));
            var script = System.IO.File.ReadAllText(path);
            if(!isModule)
                return script;
            var moduleInfo = new ScriptParser().GetModuleInfo(script);

            moduleInfo.Name = modulePath.Substring(8, modulePath.Length - 8);
            return _modulePackager.GetPackage(moduleInfo);
        }
    }
}