namespace Calc.CommandLine.Dev
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class DeveloperWorkspace
    {
        private readonly IDictionary<string, DirectoryInfo> _registeredDirectories = new Dictionary<string, DirectoryInfo>();


        public DeveloperWorkspace(string packageFolderName)
        {
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string modulesFolder = Path.Combine(location, packageFolderName);
            if (!Directory.Exists(modulesFolder))
            {
                Directory.CreateDirectory(modulesFolder);
            }
            PackageDirectory = modulesFolder;

            DirectoryInfo dir = new DirectoryInfo(location);
            while (!dir.GetFiles("*.sln").Any())
            {
                dir = dir.Parent;
            }
            SlnDirectory = dir.FullName;
            ApplicationDirectory = location;
        }

        public string ApplicationDirectory { get; set; }
        public string PackageDirectory { get; set; }
        public string SlnDirectory { get; set; }

        public void Scan()
        {
            var selector = new FolderSelector(
                dir => dir.GetFiles().Any(x => x.Name.ToLower().Contains("manifest")),
                dir => dir.Name.ToLower());

            var directories = Directory.GetDirectories(SlnDirectory).Select(x => new DirectoryInfo(x)).ToList();
            foreach (var directoryInfo in directories)
            {
                if (!selector.Filter(directoryInfo))
                    continue;

                var binDirectory = BinDirectory(directoryInfo);
                if (binDirectory == null)
                    continue;


                _registeredDirectories.Add(selector.GetKey(directoryInfo), binDirectory);
            }
        }

        public DirectoryInfo BinDirectory(DirectoryInfo projectDir)
        {
            string binFolder;
#if DEBUG
            binFolder = "Debug";
#endif
#if !DEBUG
            binFolder = "Release";
#endif

            var bin = projectDir.GetDirectories("bin").FirstOrDefault();
            if (bin == null) return null;

            return bin.GetDirectories(binFolder).FirstOrDefault();

        }

        public void CopyAll()
        {
            foreach (var item in _registeredDirectories.Keys)
            {
                CopyFolder(item);
            }
        }

        public void CopyFolder(string key)
        {
            key = key.ToLower();

            var registeredDirectory = _registeredDirectories[key];
            var dest = string.IsNullOrWhiteSpace(PackageDirectory)
                           ? Path.Combine(PackageDirectory, key)
                           : Path.Combine(PackageDirectory, PackageDirectory, key);
            DirectoryCopy(registeredDirectory.FullName, dest, true);
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            var dir = new DirectoryInfo(sourceDirName);
            var dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            var files = dir.GetFiles();
            foreach (var file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            if (copySubDirs)
            {
                foreach (var subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

    }
}