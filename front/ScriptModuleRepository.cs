using System.Collections.Generic;

namespace front
{
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
            return _moduleCache.ContainsKey(identifier) ? _moduleCache[identifier] : ParseModule(identifier);
        }

        private ModuleInfo ParseModule(string identifier)
        {
            return _moduleParser.Parse(_scriptRepository.GetScript(identifier));
        }
    }
}