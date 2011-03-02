namespace front.Web
{
    public interface IFrontConfiguration
    {
        string RootPath { get; }
        IFrontConfiguration WithRootPath(string path);
    }
}