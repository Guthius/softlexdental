using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;

namespace CodeBase
{
    /// <summary>
    /// This is a helper class meant to be used to easily manage Windows services.
    /// </summary>
    public class ServicesHelper
    {
        /// <summary>
        /// Executes a process and waits up to 10 seconds for the process to execute.
        /// </summary>
        private static void ExecuteProcess(string fileName, string arguments, out string standardOutput, out int exitCode, string workingDirectory = "")
        {
            var process = new Process();
            if (!string.IsNullOrEmpty(workingDirectory))
            {
                process.StartInfo.WorkingDirectory = workingDirectory;
            }
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            standardOutput = process.StandardOutput.ReadToEnd();

            process.WaitForExit(10000);

            exitCode = process.ExitCode;

            process.Dispose();
        }

        /// <summary>
        /// Returns true if the service was able to start successfully. 
        /// Set hasExceptions to true if the exception is desired instead.
        /// </summary>
        public static bool Start(ServiceController service, bool hasExceptions = false)
        {
            try
            {
                // Only attempt to start services that are of status stopped or stop pending.
                // If we do not do this, an InvalidOperationException will throw that says "An instance of the service is already running"
                if (service.Status != ServiceControllerStatus.Stopped && 
                    service.Status != ServiceControllerStatus.StopPending)
                {
                    return true;
                }

                service.MachineName = Environment.MachineName;
                service.Start();

                service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 7));
            }
            catch (Exception exception)
            {
                if (hasExceptions)
                {
                    throw exception;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Starts all services passed in.
        /// If hasExceptions is set to false then a string will be returned indicating which services did not start. Empty string if all started.
        /// If hasExceptions is set to true, there is a chance that not all services will start.
        /// </summary>
        public static string StartServices(List<ServiceController> services, bool hasExceptions = false)
        {
            var errorLog = new StringBuilder();
            foreach (var service in services)
            {
                try
                {
                    if (!Start(service, hasExceptions))
                    {
                        errorLog.AppendLine(service.DisplayName);
                    }
                }
                catch (Exception exception)
                {
                    if (hasExceptions)
                    {
                        throw exception;
                    }
                    errorLog.AppendLine(service.DisplayName);
                }
            }
            return errorLog.ToString();
        }

        /// <summary>
        /// Returns true if the service was able to stop successfully.
        /// Set hasExceptions to true if the exception is desired instead.
        /// </summary>
        public static bool Stop(ServiceController service, bool hasExceptions = false)
        {
            try
            {
                if (service.Status == ServiceControllerStatus.Stopped || 
                    service.Status == ServiceControllerStatus.StopPending)
                {
                    return true;
                }

                service.MachineName = Environment.MachineName;
                service.Stop();

                service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 7));
            }
            catch (Exception exception)
            {
                if (hasExceptions)
                {
                    throw exception;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Stops all services passed in.
        /// If hasExceptions is set to false then a string will be returned indicating which services did not stop. Empty string if all stopped.
        /// If hasExceptions is set to true, there is a chance that not all services will be stopped.
        /// </summary>
        public static string StopServices(List<ServiceController> services, bool hasExceptions = false)
        {
            var errorLog = new StringBuilder();
            foreach (var service in services)
            {
                try
                {
                    if (!Stop(service, hasExceptions))
                    {
                        errorLog.AppendLine(service.DisplayName);
                    }
                }
                catch (Exception ex)
                {
                    if (hasExceptions)
                    {
                        throw ex;
                    }
                    errorLog.AppendLine(service.DisplayName);
                }
            }
            return errorLog.ToString();
        }

        /// <summary>
        /// Returns a list of all services.
        /// </summary>
        public static List<ServiceController> GetServices() => ServiceController.GetServices().ToList();

        /// <summary>
        /// Returns all services that start with "OpenDent".
        /// </summary>
        public static List<ServiceController> GetAllOpenDentServices() => GetServices().FindAll(x => x.ServiceName.StartsWith("OpenDent"));

        /// <summary>
        /// Returns true if the service passed in allows "Everyone" to manage the service (start / stop).
        /// </summary>
        public static bool GetIsEveryoneAllowedToManageService(ServiceController service)
        {
            GetSecurityDescriptorForService(service.ServiceName, out string standardOutput, out int exitCode);
            if (exitCode != 0)
            {
                throw new ApplicationException(
                    "Error showing security descriptor.  Error code: " + exitCode + "\r\n" + standardOutput);
            }

            // The security descriptor for all Open Dental services should ALWAYS contain the 
            // portion to grant all users permissions to stop and start.
            // https://msdn.microsoft.com/en-us/library/aa374928(v=vs.85)
            //
            // The "ace type" section------------------------------------------------------------------------------------------------------------------------
            // A: ACCESS_ALLOWED_ACE_TYPE
            // The "rights" section--------------------------------------------------------------------------------------------------------------------------
            // RP: ADS_RIGHT_DS_READ_PROP - Read the properties of a DS object.
            // WP: ADS_RIGHT_DS_WRITE_PROP - Write properties for a DS object.
            // CR: ADS_RIGHT_DS_CONTROL_ACCESS - Access allowed only after extended rights checks supported by the object are performed. 
            // 		This flag can be used alone to perform all extended rights checks on the object or it can be combined with an identifier of a specific 
            // 		extended right to perform only that check.
            // The last two letters define the security principal assigned with these permissions a SID or well known aliases--------------------------------
            // WD - Everyone

            return standardOutput.Contains("(A;;RPWPCR;;;WD)");
        }

        /// <summary>
        /// Adds the ability for Everyone to manage the passed in services by manipulating the security descriptor.
        /// Tries to manipulate the security descriptor for every service passed in. 
        /// Silently fails if unable to apply the new permission.
        /// </summary>
        public static void SetSecurityDescriptorToAllowEveryoneToManageServices(List<ServiceController> services)
        {
            foreach (ServiceController service in services)
            {
                try
                {
                    SetSecurityDescriptorToAllowEveryoneToManageService(service);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Adds the ability for Everyone to manage the passed in service by manipulating the security descriptor.
        /// Throws exceptions if anything goes wrong manipulating the security descriptor for the service passed in.
        /// </summary>
        public static void SetSecurityDescriptorToAllowEveryoneToManageService(ServiceController service)
        {
            if (GetIsEveryoneAllowedToManageService(service)) return;

            GetSecurityDescriptorForService(service.ServiceName, out string standardOutput, out int exitCode);
            if (exitCode != 0)
            {
                throw new ApplicationException(
                    "Error showing security descriptor.  Error code: " + exitCode + "\r\n" + standardOutput);
            }

            // All users cannot correctly manage the service yet so we need to use the security 
            // descriptor setter to add the correct permission. Always preserve whatever 
            // permissions were already set for the service. However, we need to insert the new 
            // permission just before the SACL section ("S:" section).
            // https://msdn.microsoft.com/en-us/library/aa379570(v=vs.85)

            string securityDescriptor = standardOutput.Trim();
            int startIndex = securityDescriptor.IndexOf("S:");
            if (startIndex == -1)
            {
                throw new ApplicationException(
                    "Error setting security descriptor; No SACL found.");
            }

            securityDescriptor = securityDescriptor.Insert(startIndex, "(A;;RPWPCR;;;WD)");

            SetSecurityDescriptorForService(service.ServiceName, securityDescriptor, out standardOutput, out exitCode);
            if (exitCode != 0)
            {
                throw new ApplicationException(
                    "Error setting security descriptor.  Error code: " + exitCode + "\r\n" + standardOutput);
            }
        }

        /// <summary>
        /// Uses the service controller sdshow command to get the security descriptor for the passed in serviceName.
        /// The out parameters "standardOutput" and "exitCode" will contain the results of the "Process" execution.
        /// </summary>
        private static void GetSecurityDescriptorForService(string serviceName, out string standardOutput, out int exitCode) =>
            ExecuteProcess("cmd.exe", "/C sc sdshow " + serviceName, out standardOutput, out exitCode);

        /// <summary>
        /// Uses the service controller sdset command to set the passed in serviceName to the passed in securityDescriptor.
        /// The out parameters "standardOutput" and "exitCode" will contain the results of the "Process" execution.
        /// </summary>
        private static void SetSecurityDescriptorForService(string serviceName, string securityDescriptor, out string standardOutput, out int exitCode) =>
            ExecuteProcess("cmd.exe", "/C sc sdset " + serviceName + " \"" + securityDescriptor + "\"", out standardOutput, out exitCode);
        

        /// <summary>
        /// Helper class that offers an alternative to querying windows to find out which services are installed.
        /// Use this alternative when the old "search the registry" routine fails.
        /// </summary>
        private class ODWmiService
        {
            public string Description;
            public string DisplayName;
            public string Name;
            public string PathName;
            public bool Started;
            public string StartMode;
            public string StartName;
            public string State;
            public uint ProcessId;

            public static List<ODWmiService> GetServices()
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service");
                ManagementObjectCollection collection = searcher.Get();
                return collection
                    .Cast<ManagementObject>()
                    .Select(x => new ODWmiService()
                    {
                        Description = (string)x.Properties["Description"].Value ?? "",
                        DisplayName = (string)x.Properties["DisplayName"].Value ?? "",
                        Name = (string)x.Properties["Name"].Value ?? "",
                        PathName = (string)x.Properties["PathName"].Value ?? "",
                        Started = (bool)x.Properties["Started"].Value,
                        StartMode = (string)x.Properties["StartMode"].Value ?? "",
                        StartName = (string)x.Properties["StartName"].Value ?? "",
                        State = (string)x.Properties["State"].Value ?? "",
                        ProcessId = (uint)x.Properties["ProcessId"].Value,
                    }).ToList();
            }
        }
    }
}