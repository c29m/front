using front.Core.impl;

namespace front.Core
{
    public interface IScriptModuleRepository
    {
        ModuleInfo GetModule(string identifier);
    }
}