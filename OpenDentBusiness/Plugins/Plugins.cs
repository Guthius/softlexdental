using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenDentBusiness
{
    public class Plugin
    {
        static Dictionary<string, List<PluginActionHandler>> actionHandlersDict = new Dictionary<string, List<PluginActionHandler>>();
        static Dictionary<string, List<object>> filterHandlersDict = new Dictionary<string, List<object>>();

        /// <summary>
        /// Triggers the specified action.
        /// </summary>
        /// <param name="sender">The object that is triggering the action.</param>
        /// <param name="action">The action to trigger.</param>
        /// <param name="args">Arguments for the action.</param>
        /// <returns>True if a action handler was invoked; otherwise false.</returns>
        public static bool Trigger(object sender, string action, params object[] args)
        {
            if (action == null) return false;

            action = action.Trim().ToLower();
            if (action.Length == 0)
            {
                return false;
            }

            lock (actionHandlersDict)
            {
                if (actionHandlersDict.TryGetValue(action, out var actionHandlersList))
                {
                    if (actionHandlersList.Count == 0) return false;

                    foreach (var actionHandler in actionHandlersList)
                    {
                        if (actionHandler == null) continue;

                        try
                        {
                            actionHandler(sender, args);
                        }
                        catch (Exception ex)
                        {
                            HandleExceptionFromAction(sender, action, args, actionHandler, ex);
                        }
                    }

                    // TODO: Maybe it would be better to only return true if 1 or more handlers executed 
                    //       without encountering any unhandled exceptions. We are using the Trigger method in 
                    //       some areas to allow plugins to override default behaviour, however if all handlers
                    //       fail due to an unhandled exception it might be better to fall back to the default 
                    //       behaviour.
                    //
                    //       Another alternative might be to create a new method called 'Hook', which functions
                    //       similarly to 'Trigger', but with some additionals parameters allowing us to specify
                    //       what to do when a hook fails and what would constitute failure.

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Triggers the filter with the specified name and returns the resulting value.
        /// </summary>
        /// <typeparam name="TValue">The type of value to return.</typeparam>
        /// <param name="sender">The object that is triggering the filter.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="args">Arguments for the filter.</param>
        /// <returns>The value.</returns>
        public static TValue Trigger<TValue>(object sender, string filter, params object[] args) => Filter(sender, filter, default(TValue), args);

        /// <summary>
        /// Triggers the specified action asynchronously.
        /// </summary>
        /// <param name="sender">The object that is triggering the action.</param>
        /// <param name="action">The action to trigger.</param>
        /// <param name="args">Arguments for the action.</param>
        public static async System.Threading.Tasks.Task<bool> TriggerAsync(object sender, string action, params object[] args)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return Trigger(sender, action, args);
            });
        }

        /// <summary>
        /// Handles a exception that occured when invoking a <see cref="PluginActionHandler"/>.
        /// </summary>
        /// <param name="sender">The object that is triggering the action.</param>
        /// <param name="action">The action.</param>
        /// <param name="args">That arguments that were passed to the handler.</param>
        /// <param name="actionHandler">The handler that threw the exception.</param>
        /// <param name="exception">The exception.</param>
        static void HandleExceptionFromAction(object sender, string action, object[] args, PluginActionHandler actionHandler, Exception exception)
        {
            // TODO: Implement me.
        }

        /// <summary>
        /// Registers a new handler for the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="actionHandler">The handlers to add.</param>
        /// <returns>True if the handler was added; otherwise, false.</returns>
        public static bool AddAction(string action, PluginActionHandler actionHandler)
        {
            if (action == null || actionHandler == null) return false;

            action = action.Trim().ToLower();
            if (action.Length == 0)
            {
                return false;
            }

            lock (actionHandlersDict)
            {
                if (!actionHandlersDict.TryGetValue(action, out var actionHandlersList))
                {
                    actionHandlersList = new List<PluginActionHandler>();
                    actionHandlersDict.Add(action, actionHandlersList);
                }

                if (!actionHandlersList.Contains(actionHandler))
                {
                    actionHandlersList.Add(actionHandler);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the handler for the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="actionHandler">The handler to remove.</param>
        /// <returns>True if the handler was removed; otherwise, false.</returns>
        public static bool RemoveAction(string action, PluginActionHandler actionHandler)
        {
            if (action == null || actionHandler == null) return false;

            action = action.Trim().ToLower();
            if (action.Length == 0)
            {
                return false;
            }

            lock (actionHandlersDict)
            {
                if (actionHandlersDict.TryGetValue(action, out var actionHandlersList))
                {
                    return actionHandlersList.Remove(actionHandler);
                }
            }

            return false;
        }

        /// <summary>
        /// Executes the specified filter on the specified value.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="sender">The object that is filtering the value.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="value">The value to filter.</param>
        /// <param name="args">Arguments for the filter.</param>
        /// <returns>The filtered value.</returns>
        public static TValue Filter<TValue>(object sender, string filter, TValue value, params object[] args)
        {
            if (filter == null) return value;

            filter = filter.Trim().ToLower();
            if (filter.Length == 0)
            {
                return value;
            }

            lock (filterHandlersDict)
            {
                if (filterHandlersDict.TryGetValue(filter, out var filterHandlersList))
                {
                    foreach (var handlerObject in filterHandlersList)
                    {
                        if (handlerObject is PluginFilterHandler<TValue> filterHandler)
                        {
                            try
                            {
                                var result = filterHandler(sender, value, args);

                                value = result;
                            }
                            catch (Exception ex)
                            {
                                HandleExceptionFromFilter(
                                    sender,
                                    filter,
                                    value,
                                    args,
                                    filterHandler,
                                    ex);
                            }
                        }
                    }
                }
            }

            // After filtering we trigger a action with the same name as the filter. 
            // The filtered value will be passed as the first argument.
            var triggerArgs = new List<object>() { value };
            triggerArgs.AddRange(args);

            Trigger(sender, filter, triggerArgs.ToArray());

            return value;
        }

        /// <summary>
        /// Executes the specified filter on the specified value asynchronously.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="sender">The object that is filtering the value.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="value">The value to filter.</param>
        /// <param name="args">Arguments for the filter.</param>
        /// <returns>The filtered value.</returns>
        public static async Task<TValue> FilterAsync<TValue>(object sender, string filter, TValue value, params object[] args)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return Filter(sender, filter, value, args);
            });
        }

        /// <summary>
        /// Handles a exception that occured when invoking a <see cref="PluginFilterHandler{TValue}"/>.
        /// </summary>
        /// <param name="sender">The object that is filtering the value.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="value">The value before the handler was called.</param>
        /// <param name="args">That arguments that were passed to the handler.</param>
        ///  <param name="filterHandler">The handler that threw the exception.</param>
        /// <param name="exception">The exception.</param>
        static void HandleExceptionFromFilter<TValue>(object sender, string filter, TValue value, object[] args, PluginFilterHandler<TValue> filterHandler, Exception exception)
        {
            // TODO: Implement me.
        }

        /// <summary>
        /// Registers a new handler for the specified filter.
        /// </summary>
        /// <typeparam name="TValue">The type of value to filter.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="filterHandler">The handler to add.</param>
        /// <returns>True if the handler was added; otherwise, false.</returns>
        public static bool AddFilter<TValue>(string filter, PluginFilterHandler<TValue> filterHandler)
        {
            if (filter == null || filterHandler == null) return false;

            filter = filter.Trim().ToLower();
            if (filter.Length == 0)
            {
                return false;
            }

            lock (filterHandlersDict)
            {
                if (!filterHandlersDict.TryGetValue(filter, out var filterHandlersList))
                {
                    filterHandlersList = new List<object>();
                    filterHandlersDict.Add(filter, filterHandlersList);
                }

                // Prevent the same filter from being registered more than once.
                foreach (var filterHandlerObject in filterHandlersList)
                {
                    if (filterHandlerObject == (object)filterHandler)
                    {
                        return false;
                    }
                }

                filterHandlersList.Add(filterHandler);
            }

            return false;
        }

        /// <summary>
        /// Removes the handler for the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="filterHandler">The handler to remove.</param>
        /// <returns>True if the handler was removed; otherwise, false.</returns>
        public static bool RemoveFilter<TValue>(string filter, PluginFilterHandler<TValue> filterHandler)
        {
            if (filter == null || filterHandler == null) return false;

            filter = filter.Trim().ToLower();
            if (filter.Length == 0)
            {
                return false;
            }

            lock (filterHandlersDict)
            {
                if (filterHandlersDict.TryGetValue(filter, out var filterHandlersList))
                {
                    return filterHandlersList.Remove(filterHandler);
                }
            }

            return false;
        }
    }

    public delegate void PluginActionHandler(object sender, object[] args);
    public delegate TValue PluginFilterHandler<TValue>(object sender, TValue value, object[] args);

    public static class PluginManager
    {
        static readonly List<Plugin> pluginsList = new List<Plugin>();

        /// <summary>
        /// Gets a list of all the plugins.
        /// </summary>
        /// <remarks>We return a copy of the list to remove the risk of anything changing our internal list.</remarks>
        public static List<Plugin> Plugins => new List<Plugin>(pluginsList);

        /// <summary>
        /// Loads all plugins from the specified path.
        /// </summary>
        /// <param name="path"></param>
        public static void LoadDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string[] pluginFileNames = Directory.GetFiles(Path.GetFullPath(path), "*.dll", SearchOption.TopDirectoryOnly);

                lock (pluginsList)
                {
                    foreach (string pluginFileName in pluginFileNames)
                    {
                        try
                        {
                            var assembly = Assembly.LoadFile(pluginFileName);
                            if (assembly == null)
                            {
                                continue;
                            }

                            var types = assembly.GetTypes();
                            foreach (var type in types)
                            {
                                var plugin = Register(type);
                                if (plugin != null)
                                {
                                    pluginsList.Add(plugin);
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            catch { }

            // TODO: Log exceptions that occur while loading plugins...
        }

        /// <summary>
        /// Registers the specified type as a plugin.
        /// </summary>
        /// <param name="type">The type.</param>
        static Plugin Register(Type type)
        {
            if (type.IsInterface || type.IsAbstract) return null;

            if (typeof(Plugin).IsAssignableFrom(type))
            {
                if (Activator.CreateInstance(type) is Plugin plugin)
                {
                    return plugin;
                }
            }

            return null;
        }
    }

    public class Plugins
    {
        public static void LaunchToolbarButton(long programNum, long patNum)
        {
            //if (ListPlugins == null)
            //{
            //    return;//Fail silently if plugins could not be loaded.
            //}
            //PluginContainer pluginContainer = ListPlugins.FirstOrDefault(x => x.ProgramNum == programNum && x.Plugin != null);
            //if (pluginContainer != null)
            //{
            //    try
            //    {
            //        pluginContainer.Plugin.LaunchToolbarButton(patNum);
            //    }
            //    catch (Exception e)
            //    {
            //        pluginContainer.Plugin.HookException(e);
            //    }
            //}
        }
    }
}