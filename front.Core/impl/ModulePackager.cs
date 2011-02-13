using System.Collections.Generic;
using System.Text;

namespace front.Core.impl
{
    public class CombiningModulePackager : IModulePackager
    {
        private readonly IScriptModuleRepository _moduleRepository;
        private readonly ICommonJsFormatter _moduleFormatter;

        public CombiningModulePackager(IScriptModuleRepository moduleRepository, ICommonJsFormatter moduleFormatter)
        {
            _moduleRepository = moduleRepository;
            _moduleFormatter = moduleFormatter;
        }

        public string GetPackage(string modulePath)
        {
            var modules = new HashSet<ModuleInfo>();
            var moduleInfo = _moduleRepository.GetModule(modulePath);
            modules.AddIfNotExists(GetModules(moduleInfo.Dependencies));
            modules.Add(moduleInfo);
            var script = new StringBuilder();
            foreach (var module in modules)
            {
                script.AppendLine(_moduleFormatter.GetCommonJsModule(module));
            }
            return script.ToString();
        }

        public IEnumerable<ModuleInfo> GetModules(IEnumerable<string> modules)
        {
            foreach (var module in modules)
            {
                var moduleInfo = _moduleRepository.GetModule(module);
                var dependencies = GetModules(moduleInfo.Dependencies);
                foreach (var dependency in dependencies)
                {
                    yield return dependency;
                }
                yield return moduleInfo;
            }
        }
    }

    public static class SetExtensions
    {
        public static void AddIfNotExists<T>(this ISet<T> set, IEnumerable<T> candidates)
        {
            foreach (var candidate in candidates)
            {
                if (!set.Contains(candidate))
                    set.Add(candidate);
            }
        }
    }
}