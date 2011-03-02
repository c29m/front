using System.Web;
using front.Core;

namespace front.Web
{
    public class ScriptHandler: IHttpHandler
    {
        private readonly IModulePackager _modulePackager;
        private readonly IModulePathExtractor _modulePathExtractor;

        public ScriptHandler(IModulePackager modulePackager, IModulePathExtractor modulePathExtractor)
        {
            _modulePackager = modulePackager;
            _modulePathExtractor = modulePathExtractor;
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/javascript";
            var requestedPath = context.Request.AppRelativeCurrentExecutionFilePath;
            var flags = context.Request.QueryString.ToString();
            /*
            if(flags.Contains("--bootstrap"))
                context.Response.Write();
            */
            context.Response.Write(_modulePackager.GetPackage(_modulePathExtractor.GetModuleIdentifier(context.Request.AppRelativeCurrentExecutionFilePath)));
        }

        public bool IsReusable { get; private set; }
    }
}