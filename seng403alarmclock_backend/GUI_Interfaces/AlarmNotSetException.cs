using System;
using System.Runtime.Serialization;

namespace seng403alarmclock.GUI_Interfaces {
    /// <summary>
    /// The exception is throw is a request is made to the GUI for an alarm that 
    /// it doesn't know about
    /// </summary>
    [DataContract]
    internal class AlarmNotSetException : Exception {
        public AlarmNotSetException() {}

        public AlarmNotSetException(string message) : base(message) {}

        public AlarmNotSetException(string message, Exception innerException) : base(message, innerException) {}
    }
}