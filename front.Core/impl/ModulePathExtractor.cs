namespace front.Core.impl
{
    public class ModulePathExtractor : IModulePathExtractor
    {
        private readonly string _pathPrefix;

        public ModulePathExtractor(string pathPrefix)
        {
            _pathPrefix = pathPrefix;
        }

        public string GetModuleIdentifier(string appRelativeCurrentExecutionFilePath)
        {
            string moduleIdentifier=appRelativeCurrentExecutionFilePath;
            if (!string.IsNullOrWhiteSpace(_pathPrefix))
            {
                if (moduleIdentifier.StartsWith(_pathPrefix))
                    moduleIdentifier = moduleIdentifier.Substring(_pathPrefix.Length,
                                                                  moduleIdentifier.Length - _pathPrefix.Length);
            }
            return moduleIdentifier;
        }
    }
}