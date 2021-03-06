﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace CodeBase
{
    public static class MiscUtils
    {
        const string CryptKey = "AKQjlLUjlcABVbqp";

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        public static string CreateRandomAlphaNumericString(int length)
        {
            string result = "";
            string randChrs = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            for (int i = 0; i < length; i++)
            {
                result += randChrs[ODRandom.Next(0, randChrs.Length - 1)];
            }
            return result;
        }

        public static string Encrypt(string encrypt)
        {
            byte[] arrayEncryptBytes = Encoding.UTF8.GetBytes(encrypt);

            using (var memoryStream = new MemoryStream())
            {
                var aes = new AesCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(CryptKey),
                    IV = new byte[16]
                };

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(arrayEncryptBytes, 0, arrayEncryptBytes.Length);
                    cryptoStream.FlushFinalBlock();

                    aes.Dispose();

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static string Decrypt(string encString, bool doThrow = false)
        {
            try
            {
                using (var memoryStream = new MemoryStream(Convert.FromBase64String(encString)))
                {
                    var aes = new AesCryptoServiceProvider
                    {
                        Key = Encoding.UTF8.GetBytes(CryptKey),
                        IV = new byte[16]
                    };

                    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    using (var streamReader = new StreamReader(cryptoStream))
                    {
                        var result = streamReader.ReadToEnd();

                        aes.Dispose();

                        return result;
                    }
                }
            }
            catch
            {
                if (doThrow) throw;

                MessageBox.Show("Text entered was not valid encrypted text.");
                return "";
            }
        }

        /// <summary>
        /// Accepts a 3 character string which represents a neutral culture (for example, "eng" for English) in the ISO639-2 format. 
        /// Returns null if the three letter ISO639-2 name is not standard (useful for determining custom languages).
        /// </summary>
        /// <param name="iso3Code">The three-letter ISO code.</param>
        public static CultureInfo GetCultureFromThreeLetter(string iso3Code)
        {
            if (iso3Code == null || iso3Code.Length != 3) return null;

            var cultureInfoArray = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            foreach (var cultureInfo in cultureInfoArray)
            {
                if (cultureInfo.ThreeLetterISOLanguageName == iso3Code)
                {
                    return cultureInfo;
                }
            }

            return null;
        }

        ///<summary>Universal extension for IN statement.  Use like this: if(!x.In(2,3,61,71))</summary>
        public static bool In<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }

        ///<summary>Universal extension for IN statement.  Use like this: if(!x.In(list))</summary>
        public static bool In<T>(this T item, IEnumerable<T> list)
        {
            return list.Contains(item);
        }

        ///<summary>Extension for BETWEEN statement.  Use like this: if(x.Between(0,9))</summary>
        public static bool Between<T>(this T item, T lowerBound, T upperBound, bool isLowerBoundInclusive = true, bool isUpperBoundInclusive = true)
            where T : IComparable
        {
            if (isLowerBoundInclusive && isUpperBoundInclusive)
            {
                return (item.CompareTo(lowerBound) >= 0 && item.CompareTo(upperBound) <= 0);
            }
            if (isLowerBoundInclusive && !isUpperBoundInclusive)
            {
                return (item.CompareTo(lowerBound) >= 0 && item.CompareTo(upperBound) < 0);
            }
            if (!isLowerBoundInclusive && isUpperBoundInclusive)
            {
                return (item.CompareTo(lowerBound) > 0 && item.CompareTo(upperBound) <= 0);
            }
            if (!isLowerBoundInclusive && !isUpperBoundInclusive)
            {
                return (item.CompareTo(lowerBound) > 0 && item.CompareTo(upperBound) < 0);
            }
            return false;//This code is unreachable but the compiler doesn't realize it.
        }

        ///<summary>Filters the current IEnumerable of objects based on the func provided.
        ///C# does not provide a way to do listObj.Distinct(x => x.Field).  This extension allows us to do listObj.DistinctBy(x => x.Field)</summary>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> listSource, Func<T, TKey> keySelector)
        {
            HashSet<TKey> hashKeys = new HashSet<TKey>();
            foreach (T source in listSource)
            {
                if (hashKeys.Add(keySelector(source)))
                {
                    yield return source;//Manipulates the current sourceList instead of having to return an entire list.
                }
            }
        }

        ///<summary>Loops through the IEnumerable and performs the action on each item.</summary>
        public static void ForEach<T>(this IEnumerable<T> listSource, Action<T> action)
        {
            foreach (T source in listSource)
            {
                action(source);
            }
        }

        ///<summary>Finds installed IE version for this workstation and attempts to modify registry to force browser emualtion to this version.
        ///Typically used in conjunction with WebBrowser control to ensure that the WebBrowser is running in the latest available emulation mode.
        ///Returns true if the emulation version was previously wrong but was successfully updated. Otherwise returns false.
        ///If true is returned than this application will need to be restarted in order for the changes to take effect.</summary>
        public static bool TryUpdateIeEmulation()
        {
            bool ret = false;
            try
            {
                int browserVersion;
                //Get the installed IE version.
                using (WebBrowser wb = new WebBrowser())
                {
                    browserVersion = wb.Version.Major;
                }
                int regVal;
                //Set the appropriate IE version
                if (browserVersion >= 11)
                {
                    regVal = 11001;
                }
                else if (browserVersion == 10)
                {
                    regVal = 10001;
                }
                else if (browserVersion == 9)
                {
                    regVal = 9999;
                }
                else if (browserVersion == 8)
                {
                    regVal = 8888;
                }
                else if (browserVersion == 7)
                {
                    regVal = 7000;
                }
                else
                {//Unknown version.  This will happen when version 12 and beyond are released.
                    regVal = browserVersion * 1000 + 1;//Guess the regVal code needed based on the historic pattern.
                }
                //Set the actual key.  This key can be set without admin rights, because it is within the current user's registry store.
                string applicationName = Process.GetCurrentProcess().ProcessName + ".exe";//This is OpenDental.vhost.exe when debugging, different for distributors.
                string keyPath = @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(keyPath, true);
                if (key == null)
                {
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(keyPath);
                }
                object keyValueCur = key.GetValue(applicationName);
                if (keyValueCur == null || keyValueCur.ToString() != regVal.ToString())
                {
                    key.SetValue(applicationName, regVal, Microsoft.Win32.RegistryValueKind.DWord);
                    ret = true;
                }
                key.Close();
            }
            catch
            {
            }
            return ret;
        }

        ///<summary>Returns true if the two time slots overlap in time. Slot1 and Slot2 are interchangeable.</summary>
        public static bool DoSlotsOverlap(DateTime slot1Start, DateTime slot1End, DateTime slot2Start, DateTime slot2End)
        {
            return (slot1End > slot2Start && slot1Start < slot2End);
        }

        ///<summary>Returns exception string that includes the threadName if provided and exception type and up to 5 inner exceptions.
        ///Used for both bugSubmissions and the MsgBoxCopyPaste shown to customers when a UE occurs.</summary>
        public static string GetExceptionText(Exception e, string threadName = null)
        {
            return "Unhandled exception" + (string.IsNullOrEmpty(threadName) ? "" : " from " + threadName) + ":  "
                    + (string.IsNullOrEmpty(e.Message) ? "No Exception Message" : e.Message + "\r\n")
                    + (string.IsNullOrEmpty(e.GetType().ToString()) ? "No Exception Type" : e.GetType().ToString()) + "\r\n"
                    + (string.IsNullOrEmpty(e.StackTrace) ? "No StackTrace" : e.StackTrace)
                    + InnerExceptionToString(e);//New lines handled in method.
        }

        ///<summary>Formats the inner exception (and all its inner exceptions) as a readable string. Okay to pass in an exception with no inner 
        ///exception.</summary>
        ///<param name="depth">The recursive depth of the current method call.</param>
        public static string InnerExceptionToString(Exception ex, int depth = 0)
        {
            if (ex.InnerException == null
                || depth >= 5)//Limit to 5 inner exceptions to prevent infinite recursion
            {
                return "";
            }
            return "\r\n-------------------------------------------\r\n"
                + "Inner exception:  " + ex.InnerException.Message + "\r\n" + ex.InnerException.GetType().ToString() + "\r\n"
                + ex.InnerException.StackTrace
                + InnerExceptionToString(ex.InnerException, ++depth);
        }

        ///<summary>Gets the ordinal indicator e.g.passing in 13 will return "th". Translations should be done in the calling class and 
        ///should include the number in the translation. This is because different languages have different ordinal rules for each number.</summary>
        public static string GetOrdinalIndicator(string num)
        {
            try
            {
                return GetOrdinalIndicator(Convert.ToInt32(num));
            }
            catch
            {
                return "";//invalid number
            }
        }

        ///<summary>Gets the ordinal indicator e.g.passing in 13 will return "th". Translations should be done in the calling class and 
        ///should include the number in the translation. This is because different languages have different ordinal rules for each number.</summary>
        public static string GetOrdinalIndicator(int num)
        {
            if (num <= 0)
            {
                return "";
            }
            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return "th";
            }
            switch (num % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }

        ///<summary>Gets the most recent date in the past (or today) that is the specified day of week.</summary>
        public static DateTime GetMostRecentDayOfWeek(DateTime date, DayOfWeek dayOfWeek)
        {
            if (DateTime.MinValue.AddDays(7) > date)
            {
                throw new ArgumentException("Date must be at least 7 days greater than MinDate: " + date);
            }
            for (int i = 0; i < 7; i++)
            {
                DateTime newDate = date.AddDays(-i);
                if (newDate.DayOfWeek == dayOfWeek)
                {
                    return newDate;
                }
            }
            throw new Exception("Unable to find day of the week: " + dayOfWeek.ToString());
        }

        ///<summary>Gets the soonest upcoming date in the future (or today) that is the specified day of week.</summary>
        public static DateTime GetUpcomingDayOfWeek(DateTime date, DayOfWeek dayOfWeek)
        {
            if (DateTime.MaxValue.AddDays(-7) < date)
            {
                throw new ArgumentException("Date must be at least 7 days smaller than MaxValue: " + date);
            }
            for (int i = 0; i < 7; i++)
            {
                DateTime newDate = date.AddDays(i);
                if (newDate.DayOfWeek == dayOfWeek)
                {
                    return newDate;
                }
            }
            throw new Exception("Unable to find day of the week: " + dayOfWeek.ToString());
        }

        /// <summary>This class is used to create a tree of actions who depend on each other.
        /// If action x depends on action y, then action x will be a leaf of action y.
        /// When StartNodeAction() is called, the node action is completed, then each leaf action is added to the thread pool.
        /// This is faster than creating a new thread for each action because of the overhead when starting a large amount of threads.
        /// This class is best used for a longer list of actions that are dependent on one another that can be described by a tree graph.
        /// </summary>
        public class ActionNode
        {
            private Action _actionParent;
            private List<ActionNode> _listChildActionNodes;

            ///<summary>Creates an action node with an empty action and no children.  Good for use as a root.</summary>
            public ActionNode() : this(() => { })
            {
            }

            /// <summary>Creates an action node with no children and the passed in action.</summary>
            public ActionNode(Action actionParent) : this(actionParent, new List<ActionNode>())
            {
            }

            /// <summary>Creates an action node with children via the parameters passed in.</summary>
            public ActionNode(Action actionParent, List<ActionNode> listChildActionNodes)
            {
                _actionParent = actionParent;
                _listChildActionNodes = listChildActionNodes;
            }

            /// <summary>Synchronously invokes _actionParent on the main thread then asynchronously invokes all _listChildActionNodes (run in parallel)
            /// and then waits until all children and their children's children (utilizing recursion) have finished executing.</summary>
            public void StartNodeAction()
            {
                //Always complete the parent action on the main thread (synchronous).
                //The ActionNode class is designed to have the children nodes dependant on _actionParent.
                _actionParent.Invoke();
                //Create a list of tasks that will be run asynchronously so that we can "join" with them all before returning.
                List<System.Threading.Tasks.Task> listTask = new List<System.Threading.Tasks.Task>();
                //We don't create a new thread for each because of the overhead.  Adding to the thread pool is more economical and faster.
                foreach (ActionNode n in _listChildActionNodes)
                {
                    //Using Tasks is much faster than individual threads.
                    listTask.Add(System.Threading.Tasks.Task.Run(() => n.StartNodeAction()));
                }
                //Wait for all threads to complete before returning.
                System.Threading.Tasks.Task.WaitAll(listTask.ToArray());
            }
        }

        ///<summary>System.Random is not thread-safe. 
        ///This class syncronizes a single instance of System.Random and performs a lock anytime it is accessed, which makes it thread-safe.</summary>
        public static class ODRandom
        {
            private static object _lock = new object();
            private static Random _rand = new Random();
            ///<summary>System.Random is not thread-safe. This method makes it thread-safe.</summary>
            public static int Next(int minValue, int maxValue)
            {
                lock (_lock)
                {
                    return _rand.Next(minValue, maxValue);
                }
            }
            ///<summary>System.Random is not thread-safe. This method makes it thread-safe.</summary>
            public static int Next()
            {
                lock (_lock)
                {
                    return _rand.Next();
                }
            }
            ///<summary>System.Random is not thread-safe. This method makes it thread-safe.</summary>
            public static int Next(int maxValue)
            {
                lock (_lock)
                {
                    return _rand.Next(maxValue);
                }
            }
            ///<summary>System.Random is not thread-safe. This method makes it thread-safe.</summary>
            public static void NextBytes(byte[] buffer)
            {
                lock (_lock)
                {
                    _rand.NextBytes(buffer);
                }
            }
            ///<summary>System.Random is not thread-safe. This method makes it thread-safe.</summary>
            public static double NextDouble()
            {
                lock (_lock)
                {
                    return _rand.NextDouble();
                }
            }

        }
    }
}