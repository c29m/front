using System.Web;
using System.Web.Routing;

namespace front
{
    public class ScriptHandler: IHttpHandler
    {
        private readonly IModulePackager _modulePackager;
        
        public ScriptHandler(IModulePackager modulePackager)
        {
            _modulePackager = modulePackager;
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = context.Request.AcceptTypes[0];
            context.Response.Write(_modulePackager.GetPackage(context.Request.AppRelativeCurrentExecutionFilePath));
        }

        public bool IsReusable { get; private set; }
    }
}