using System.Collections.Generic;
using System.IO;

namespace front.Core.impl
{
    public class ScriptRepository : IScriptRepository
    {
        private readonly IPathResolver _pathResolver;

        public ScriptRepository(IPathResolver pathResolver)
        {
            _pathResolver = pathResolver;
        }

        public string GetScript(string identifier)
        {
            return File.ReadAllText(_pathResolver.GetScriptPath(identifier));
        }
    }
}