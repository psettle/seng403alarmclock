using System;

namespace seng403alarmclock {
    /// <summary>
    /// Internal representation of an alarm, add to it as necessairy
    /// </summary>
    public class Alarm {

        private DateTime alarmTime { get; set; }

        public Alarm()
        {

        }

        public Alarm(DateTime alarmTime) {
            this.alarmTime = alarmTime;
        }

        public DateTime GetAlarmTime() {
            //TODO: return a reasonable time
            return alarmTime;
        }

        
    }
}