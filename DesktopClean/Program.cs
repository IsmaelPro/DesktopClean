using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClean
{
    internal class Program
    {
        private static CleanerAgent agent = new CleanerAgent();
        public readonly string ServiceName;

        public Program()
        {
            ServiceName = "Desktop_Cleaner";
        }

        public class Service : ServiceBase
        {
            public Service()
            {
                ServiceName = "Desktop_Cleaner";
            }

            protected override void OnStart(string[] args)
            {
                Program.Start(args);
            }

            protected override void OnStop()
            {
                Program.Stop();
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new Service1()
            //};
            //ServiceBase.Run(ServicesToRun);

            if (!Environment.UserInteractive)
            {
                // running as service
                //using (var service = new Service())
                //    ServiceBase.Run(service);

                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new Service1()
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                var installJob = false;
                try
                {
                    string parameter = string.Concat(args);
                    switch (parameter)
                    {
                        case "--install":
                            installJob = true;
                            ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                            break;

                        case "--uninstall":
                            installJob = true;
                            ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                            break;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                if (!installJob)
                {
                    // running as console app
                    Start(args);

                    Console.WriteLine("Press any key to stop...");
                    Console.ReadKey(true);

                    Stop();
                }
            }
        }

        private static void Start(string[] args)
        {
            agent.Start();
        }

        private static void Stop()
        {
            agent.Stop();
        }
    }
}