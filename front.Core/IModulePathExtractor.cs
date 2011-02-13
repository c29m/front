namespace front.Core
{
    public interface IModulePathExtractor
    {
        string GetModuleIdentifier(string appRelativeCurrentExecutionFilePath);
    }
}