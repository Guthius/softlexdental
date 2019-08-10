using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace OpenDentServer
{
    [RunInstaller(true)]
    public class MyProjectInstaller : Installer
    {
        public MyProjectInstaller()
        {
            var serviceInstaller = new ServiceInstaller
            {
                StartType = ServiceStartMode.Automatic,
                ServiceName = "OpenDentHL7"
            };

            var args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("/ServiceName"))
                {
                    serviceInstaller.ServiceName = args[i].Substring(13);
                }
            }

            Installers.Add(serviceInstaller);
            Installers.Add(new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            });
        }
    }
}