using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Unified logging class for the application.
    /// Provides methods for informational, debug, and error logging.
    /// </summary>
    public static class FhLog
    {
        private const string LOGPrefix = "[FhLog] ";

        /// <summary>
        /// Logs an informational message to the console.
        /// These logs are typically always enabled.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="context">Optional Unity Object to associate with the message.</param>
        public static void I(object message, Object context = null)
        {
            Debug.Log(LOGPrefix + message, context);
        }

        /// <summary>
        /// Logs a debug message to the console.
        /// These logs are only compiled and executed in the Unity Editor or Development builds.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="context">Optional Unity Object to associate with the message.</param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void D(object message, Object context = null)
        {
            Debug.Log(LOGPrefix + "<color=cyan>[DEBUG]</color> " + message, context);
        }

        /// <summary>
        /// Logs an error message to the console.
        /// These logs indicate a problem that should be addressed.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="context">Optional Unity Object to associate with the message.</param>
        public static void E(object message, Object context = null)
        {
            Debug.LogError(LOGPrefix + "<color=red>[ERROR]</color> " + message, context);
        }

        /// <summary>
        /// Logs a warning message to the console.
        /// These logs indicate a potential issue or something unexpected.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="context">Optional Unity Object to associate with the message.</param>
        public static void W(object message, Object context = null)
        {
            Debug.LogWarning(LOGPrefix + "<color=yellow>[WARNING]</color> " + message, context);
        }
    }
}
