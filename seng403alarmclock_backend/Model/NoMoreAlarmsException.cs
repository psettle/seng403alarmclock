using System;
using System.Runtime.Serialization;

namespace seng403alarmclock.Model {
    
    [DataContract]
    internal class NoMoreAlarmsException : Exception {
        public NoMoreAlarmsException() {
        }

        public NoMoreAlarmsException(string message) : base(message) {
        }

        public NoMoreAlarmsException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}