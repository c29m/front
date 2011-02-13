using System.Web;
using System.Web.Routing;

namespace front.Web
{
    public class ScriptRouteHandler : IRouteHandler
    {
        private readonly ScriptHandler _scriptHandler;

        public ScriptRouteHandler(ScriptHandler scriptHandler)
        {
            _scriptHandler = scriptHandler;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return _scriptHandler;
        }
    }
}