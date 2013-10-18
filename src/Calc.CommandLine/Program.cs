using System.Collections.Generic;
using Boxes.Integration.Factories;
using Boxes.Integration.Setup;
using Boxes.Integration.Trust;
using Process;

namespace Calc.CommandLine
{
    using System;
    using System.IO;
    using System.Reflection;
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
                Application app = new Application();


                var loadProcess = boxes.LoadProcess();
                loadProcess.LoadPackages(app, new List<string> { "Calc.Commands", "Calc.NCalcEngine", "Calc.Core" });

                var calculator = boxes.GetDependencyResolver(app).Resolve<Calculator>();

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

            } //always ensure the dispose is called at the end of your app


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

        private static IBoxesWrapper<IWindsorContainer, IWindsorContainer> CreateBoxesWrapper(string packageDirectory)
        {
            //we want to support multiple manifest types. the default is XmlManifest2012Reader
            var xmlManifestTask = new XmlManifestTask();
            xmlManifestTask.AddXmlManifestReader(new XmlManifest2012Reader());
            xmlManifestTask.AddXmlManifestReader(new XmlManifest2012ExtensionReader());

            //setup the scanner. we can easily add extra tasks
            var packageScanner = new PackageScanner(packageDirectory);
            packageScanner.SetManifestTask(xmlManifestTask);

            //the main bit for using boxes
            var boxes = new BoxesWrapper();
            boxes.Setup<DefaultLoader>(packageScanner);
            boxes.DiscoverPackages();
            boxes.LoadPackages(); //load packages into integration, not the application
            return boxes;
        }
    }

    public class FuncIocSetup<T> : IIocSetup<T>
    {
        private readonly Action<T> _configure;
        private readonly Action<T> _configureChild;

        public FuncIocSetup(Action<T> configure, Action<T> configureChild)
        {
            _configure = configure;
            _configureChild = configureChild;
        }

        public void Configure(T builder)
        {
            _configure(builder);
        }

        public void ConfigureChild(T builder)
        {
            _configureChild(builder);
        }
    }


    /// <summary>
    /// extensions for the boxes wrapper
    /// </summary>
    public static class BoxesWrapperExtensions
    {
        ///// <summary>
        ///// A mechanism to handle type registration with the Tenant(s) IoC container.
        ///// </summary>
        //public static IDefaultContainerSetup<TBuilder> ContainerSetup<TBuilder>(this IBoxesWrapper<TBuilder> boxes)
        //{
        //    return boxes.GetService<IDefaultContainerSetup<TBuilder>>();
        //}

        public static ILoadProcess<TBuilder, TContainer> LoadProcess<TBuilder, TContainer>(this IBoxesWrapper<TBuilder, TContainer> boxes)
        {
            return boxes.GetService<ILoadProcess<TBuilder, TContainer>>();
        }
        
        ///// <summary>
        ///// the current execution context, get the current Tenant
        ///// </summary>
        //public static IExecutionContext Context<TBuilder>(this IBoxesWrapper<TBuilder> boxes)
        //{
        //    return boxes.GetService<IExecutionContext>();
        //}


        public static IDependencyResolver GetDependencyResolver<TBuilder, TContainer>(this IBoxesWrapper<TBuilder, TContainer> boxes,
            Application application)
        {
            var factory = boxes.GetService<IDependencyResolverFactory>();
            return factory.CreateResolver(application.Container);
        }
    }



}
