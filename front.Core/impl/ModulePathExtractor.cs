namespace front.Core.impl
{
    public class ModulePathExtractor : IModulePathExtractor
    {
        private readonly string _scriptRoot;

        public ModulePathExtractor(string scriptRoot)
        {
            _scriptRoot = scriptRoot;
        }

        public string GetModuleIdentifier(string appRelativeCurrentExecutionFilePath)
        {
            string moduleIdentifier=appRelativeCurrentExecutionFilePath;
            if (!string.IsNullOrWhiteSpace(_scriptRoot))
            {
                var prefix = "~/" + _scriptRoot;
                if (moduleIdentifier.StartsWith(prefix))
                    moduleIdentifier = moduleIdentifier.Substring(prefix.Length,
                                                                  moduleIdentifier.Length - prefix.Length);
            }
            return moduleIdentifier;
        }
    }
}