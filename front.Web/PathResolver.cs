using System.Web;
using front.Core;

namespace front.Web
{
    public class PathResolver : IPathResolver
    {
        private readonly string _rootPath;

        public PathResolver(string rootPath)
        {
            _rootPath = rootPath;
        }

        public string GetScriptPath(string moduleIdentifier)
        {
            var path =(string.IsNullOrEmpty(_rootPath) ? HttpContext.Current.Server.MapPath(moduleIdentifier) : 
               _rootPath + "\\" +  moduleIdentifier.Replace("~/", "").Replace("/", "\\"))
                + ".js";
            return path;
        }
    }
}