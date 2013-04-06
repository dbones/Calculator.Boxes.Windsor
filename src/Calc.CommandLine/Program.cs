namespace Calc.CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Boxes;
    using Boxes.Discovering;
    using Boxes.Integration;
    using Boxes.Integration.Extensions;
    using Boxes.Loading;
    using Boxes.Tasks;
    using Boxes.Windsor;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Core;
    using Dev;

    class Program
    {
        static void Main(string[] args)
        {
            string packageDirectory = SetupPackagesDirectory();
            using (var boxes = CreateBoxesWrapper(packageDirectory))
            {
                var calculator = boxes.DependencyResolver.Resolve<Calculator>();

                //the main part of the application
                string input;
                do
                {
                    Console.Write("Command: ");
                    input = Console.ReadLine();

                    calculator.Execute(input);

                    Console.WriteLine("Value: " + calculator.CurrentValue);
                    Console.WriteLine();
                } while (input != null && input.ToLower() != "q");

                //always ensure the dispose is called at the end of your app
            }
            Console.Write("thank you - press enter");
            Console.ReadLine();
        }

        private static string SetupPackagesDirectory()
        {
            //create a packages folder in our application (if it does not exist)
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string modulesFolder = Path.Combine(location, "Packages");
            if (!Directory.Exists(modulesFolder))
            {
                Directory.CreateDirectory(modulesFolder);
            }

#if DEBUG
            //remember to recompile all projects.
            //the following copies the packages across into the packages folder, for dev only
            DeveloperWorkspace workspace = new DeveloperWorkspace("Packages");
            workspace.Scan();
            workspace.CopyAll();
#endif
            return modulesFolder;
        }

        private static IBoxesWrapper CreateBoxesWrapper(string packageDirectory)
        {
            //we want to support multiple manifest types. the default is XmlManifest2012Reader
            var xmlManifestTask = new XmlManifestTask();
            xmlManifestTask.AddXmlManifestReader(new XmlManifest2012Reader());
            xmlManifestTask.AddXmlManifestReader(new XmlManifest2012ExtensionReader());

            //setup the scanner. we can easily add extra tasks
            var packageScanner = new PackageScanner(packageDirectory);
            packageScanner.SetManifestTask(xmlManifestTask);

            //you can setup the container before hand
            IWindsorContainer container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));


            
            //the main bit for using boxes
            var boxes = new BoxesWrapper(container);
            boxes.Setup<DefaultLoader>(packageScanner);
            boxes.DiscoverPackages();
            boxes.LoadPackages();
            return boxes;
        }
    }
}
