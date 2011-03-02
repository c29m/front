using System;
using System.Web.Configuration;
using System.Web.Routing;
using front.Core.impl;
using front.Web;

namespace front
{
    public class Configuration{

        public const string DefaultScriptPath = "script/";

        public static void Initialize()
        {
            Initialize((c)=>c.WithRootPath(DefaultScriptPath));
        }

        public static void Initialize(Action<IFrontConfiguration> configuration)
        {
            var config = new FrontConfiguration();
            configuration(config);
            Register(config);
        }

        private static void Register(FrontConfiguration config)
        {
            var routes = RouteTable.Routes;
            var route = new Route(config.RootPath + "{*script}", new ScriptRouteHandler(new ScriptHandler(
                                                                                 new CombiningModulePackager(new ScriptModuleRepository(new ScriptRepository(new PathResolver(config.RootPath)), new ModuleParser(), config), new CommonJsFormatter()), new ModulePathExtractor(config.RootPath))));
            routes.Add(route); 
        }
    }
}