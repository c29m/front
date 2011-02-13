namespace front
{
    public interface IScriptModuleRepository
    {
        ModuleInfo GetModule(string identifier);
    }
}