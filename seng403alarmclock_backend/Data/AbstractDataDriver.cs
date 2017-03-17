using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seng403alarmclock_backend.Data {
    /// <summary>
    /// Generic data driver interface for all platforms
    /// </summary>
    public abstract class AbstractDataDriver {
        /// <summary>
        /// Singleton instance, prevents race conditions
        /// </summary>
        private static AbstractDataDriver _instance = null;

        /// <summary>
        /// Getter for the instance
        /// </summary>
        public static AbstractDataDriver Instance { get { return _instance; } set { _instance = value; } }

        /// <summary>
        /// Saves any existing data (all date should be reloaded in ctor)
        /// </summary>
        abstract public void Shutdown();

        /// <summary>
        /// Sets a variable in persistent data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        abstract public void SetVariable(string name, object value);

        /// <summary>
        /// gets a variable in persistent data
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">
        /// If the variable doesn't exist in the save file
        /// </exception>
        abstract public object GetVariable(string name);

    }
}
