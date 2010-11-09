using System;
using System.Web;

namespace front
{
    public class ScriptResolver
    {
        private readonly ModulePackager _modulePackager;

        public ScriptResolver(ModulePackager modulePackager)
        {
            _modulePackager = modulePackager;
        }
        public string ProvideScript(string modulePath)
        {
            if(!modulePath.StartsWith("~/script"))
                throw new InvalidOperationException("script not in default location");

            var isModule = false;
            var filePath = modulePath;
            if (!modulePath.EndsWith(".js") && (isModule = true))
                filePath += ".js";
            var path = HttpContext.Current.Server.MapPath(filePath);
            var script = System.IO.File.ReadAllText(path);
            if(!isModule)
                return script;
            var moduleInfo = new ScriptParser().GetModuleInfo(script);

            moduleInfo.Name = modulePath.Substring(8, modulePath.Length - 8);
            return _modulePackager.GetPackage(moduleInfo);
        }
    }
}