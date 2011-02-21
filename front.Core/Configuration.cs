using System;
using System.Web.Configuration;
using System.Web.Routing;
using front.Core.impl;
using front.Web;

namespace front
{
    public class Configuration{
        public static void Initialize()
        {
            Initialize((c)=>c.WithRootPath("./"));
        }

        public static void Initialize(Action<IFrontConfigurator> configuration)
        {
            var config = new FrontConfigurator();
            configuration(config);
            Register(config);
        }

        private static void Register(FrontConfigurator config)
        {
            var routes = RouteTable.Routes;
            var route = new Route("script/{*script}", new ScriptRouteHandler(new ScriptHandler(
                                                                                 new CombiningModulePackager(new ScriptModuleRepository(new ScriptRepository(new PathResolver(config.RootPath)), new ModuleParser()), new CommonJsFormatter()), new ModulePathExtractor(WebConfigurationManager.AppSettings["pathPrefix"]))));
            routes.Add(route); 
        }
    }
}