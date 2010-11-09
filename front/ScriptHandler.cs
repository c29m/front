using System.Web;
using System.Web.Routing;

namespace front
{
    public class ScriptHandler: IHttpHandler
    {
        private readonly ScriptResolver _scriptResolver;
        
        public ScriptHandler(ScriptResolver scriptResolver)
        {
            _scriptResolver = scriptResolver;
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/javascript";
            context.Response.Write(_scriptResolver.ProvideScript(context.Request.AppRelativeCurrentExecutionFilePath));
        }

        public bool IsReusable { get; private set; }
    }
}