using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Configuration;

namespace front
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            var route = new Route("script/{*script}", new ScriptRouteHandler(new ScriptHandler(new ScriptResolver(new ModulePackager(), WebConfigurationManager.AppSettings["root"]))));
            routes.Add(route);
       
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
    }

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