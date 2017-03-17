using System;
using System.Runtime.Serialization;

namespace seng403alarmclock.Model {
    [Serializable]
    internal class NoMoreAlarmsException : Exception {
        public NoMoreAlarmsException() {
        }

        public NoMoreAlarmsException(string message) : base(message) {
        }

        public NoMoreAlarmsException(string message, Exception innerException) : base(message, innerException) {
        }

        protected NoMoreAlarmsException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}