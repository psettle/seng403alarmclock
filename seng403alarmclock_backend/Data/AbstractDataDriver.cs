using System;
using System.Collections.Generic;

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
        /// The list of types this data driver can serialize
        /// </summary>
        protected static List<Type> types = new List<Type>();

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
        /// Adds a type to the list of types that this class can serialize
        /// </summary>
        /// <param name="t"></param>
        public static void addType(Type t) {
            types.Add(t);
        }

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
