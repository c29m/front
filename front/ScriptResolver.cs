using System;

namespace front
{
    public class ScriptResolver
    {
        private readonly IModulePackager _modulePackager;
        private readonly ScriptModuleRepository _moduleRepository;
        private readonly string _rootPath;

        public ScriptResolver(IModulePackager modulePackager, ScriptModuleRepository moduleRepository)
        {
            _modulePackager = modulePackager;
            _moduleRepository = moduleRepository;
            _rootPath = "";
        }
        public string ProvideScript(string modulePath)
        {
            var moduleInfo = _moduleRepository.GetModule(modulePath);
            
            return moduleInfo.Packaged ? moduleInfo.Content : _modulePackager.GetPackage(moduleInfo);
        }
    }
}