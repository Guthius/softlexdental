using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CodeBase
{
    /// <summary>
    /// Levels of logging to indicate importance.
    /// </summary> 
    public enum LogLevel
    {
        Verbose,
        Debug,
        Info,
        Warning,
        Error,
        FatalError,
    };

    /// <summary>
    /// Used to log messages to our internal log file, or to other resources, such as message boxes.
    /// </summary>
    public class Logger
    {
        const int MaxLogFiles = 5;
        const int MaxLogFileSize = 1024 * 1024;

        public const string DATETIME_FORMAT = "MM/dd/yy HH:mm:ss:fff";

        static string logPath;
        static string[] logFileList;
        static LogLevel logLevel = LogLevel.Debug;

        /// <summary>
        /// Initializes the logger.
        /// </summary>
        static Logger()
        {
            logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SLDental", "Logs");
            try
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
            }
            catch { }
        }

        /// <summary>
        /// Get the path of the file to write the next log entry to.
        /// </summary>
        /// <returns>The path of the log file.</returns>
        static string GetLogFilePath()
        {
            int logCounter = 0;

            try
            {
                logFileList =
                    Directory.GetFiles(
                        logPath,
                        "SLDental*.log",
                        SearchOption.TopDirectoryOnly);

                if (logFileList.Length > 0)
                {
                    Array.Sort(logFileList, 0, logFileList.Length);
                    logCounter = logFileList.Length - 1;

                    if (logFileList.Length > MaxLogFiles)
                    {
                        File.Delete(logFileList[0]);
                        for (int i = 1; i < logFileList.Length; i++)
                        {
                            File.Move(logFileList[i], logFileList[i - 1]);
                        }
                        logCounter--;
                    }

                    var fileInfo = new FileInfo(logFileList[logCounter]);
                    if (fileInfo.Length < MaxLogFileSize)
                    {
                        return logFileList[logCounter];
                    }
                    else
                    {
                        logCounter++;
                    }
                }
            }
            catch { }

            return Path.Combine(logPath, string.Format("SLDental{0:00}.log", logCounter));
        }

        /// <summary>
        /// Writes the specified entry to the log.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="formats"></param>
        /// <param name="args"></param>
        public static void Write(LogLevel logLevel, string format, params object[] args)
        {
            if (logLevel < Logger.logLevel || format == null) return;

            try
            {
                using (var stream = new FileStream(GetLogFilePath(), FileMode.Append, FileAccess.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write($"[{DateTime.Now.ToString("o")}]");
                        writer.Write($"[{GetMethodName()}] ");
                        writer.WriteLine(string.Format(format, args));
                        writer.Flush();
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Writes the specified exception ot the log.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        public static void Write(Exception exception)
        {
            if (exception == null) return;

            Write(LogLevel.Error, exception.Message); // TODO: Format this...
        }

        /// <summary>
        /// Gets the name of the method calling the <see cref="Write(LogLevel, string, object[])"/> method.
        /// </summary>
        /// <returns>The name of the calling method.</returns>
        static string GetMethodName()
        {
            try
            {
                for (int i = 2; i < 4; i++)
                {
                    StackFrame frame = new StackFrame(i);

                    System.Reflection.MethodBase method = frame.GetMethod();
                    if (!method.Name.ToLower().Contains("logtopath") && !method.Name.ToLower().Contains("logaction"))
                    {
                        return method.ReflectedType.FullName + "." + method.Name;
                    }
                }
            }
            catch { }

            return "***";
        }
    }
}