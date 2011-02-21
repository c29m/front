namespace front.Web
{
    public class FrontConfigurator : IFrontConfigurator
    {
        public string RootPath { get; private set; }

        public IFrontConfigurator WithRootPath(string path)
        {
            RootPath = path;
            return this;
        }
    }
}