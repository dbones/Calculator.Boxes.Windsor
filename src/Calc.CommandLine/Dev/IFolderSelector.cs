namespace Calc.CommandLine.Dev
{
    using System.IO;

    public interface IFolderSelector
    {
        bool Filter(DirectoryInfo dir);
        string GetKey(DirectoryInfo dir);
    }
}