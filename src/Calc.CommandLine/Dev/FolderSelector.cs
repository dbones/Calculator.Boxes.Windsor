namespace Calc.CommandLine.Dev
{
    using System;
    using System.IO;

    public class FolderSelector : IFolderSelector
    {
        private readonly Func<DirectoryInfo, bool> _filter;
        private readonly Func<DirectoryInfo, string> _getKey;

        public FolderSelector(Func<DirectoryInfo, bool> filter, Func<DirectoryInfo, string> getKey)
        {
            _filter = filter;
            _getKey = getKey;
        }

        public bool Filter(DirectoryInfo dir)
        {
            return _filter(dir);
        }

        public string GetKey(DirectoryInfo dir)
        {
            return _getKey(dir);
        }
    }
}