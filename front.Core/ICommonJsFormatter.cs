using front.Core.impl;

namespace front.Core
{
    public interface ICommonJsFormatter
    {
        string GetCommonJsModule(ModuleInfo moduleInfo);
    }
}