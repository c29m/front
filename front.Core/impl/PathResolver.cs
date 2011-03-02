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
            var path = HttpContext.Current.Server.MapPath("~/" + _rootPath +  moduleIdentifier); 
            if (!path.EndsWith(".js"))
                path+=".js";
            return path;
        }
    }
}