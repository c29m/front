using System.Collections.Generic;

namespace front.Core.impl
{
    public static class ModuleInfoExtension
    {
        public static ModuleInfo WithName(this ModuleInfo moduleInfo, string name)
        {
            moduleInfo.Name = name;
            return moduleInfo;
        }
    }

    public class ScriptModuleRepository : IScriptModuleRepository
    {
        private readonly ScriptRepository _scriptRepository;
        private readonly ModuleParser _moduleParser;
        private readonly Dictionary<string, ModuleInfo> _moduleCache = new Dictionary<string, ModuleInfo>();
    
        public ScriptModuleRepository(ScriptRepository scriptRepository, ModuleParser moduleParser)
        {
            _scriptRepository = scriptRepository;
            _moduleParser = moduleParser;
        }

        public ModuleInfo GetModule(string identifier)
        {
            return (_moduleCache.ContainsKey(identifier) ? 
                    _moduleCache[identifier] : 
                    _moduleCache[identifier] = _moduleParser.Parse(_scriptRepository.GetScript(identifier)))
                .WithName(identifier);
        }

    }
}