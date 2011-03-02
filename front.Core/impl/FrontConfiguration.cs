namespace front.Web
{
    public class FrontConfiguration : IFrontConfiguration
    {
        public string RootPath { get; private set; }

        public IFrontConfiguration WithRootPath(string path)
        {
            RootPath = path;
            return this;
        }
    }
}