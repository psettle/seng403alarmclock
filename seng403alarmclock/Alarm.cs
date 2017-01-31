using System;

namespace seng403alarmclock {
    /// <summary>
    /// Internal representation of an alarm, add to it as necessairy
    /// </summary>
    public class Alarm {


        public Alarm() {

        }

        public DateTime GetAlarmTime() {
            //TODO: return a reasonable time
            return DateTime.Now;
        }
    }
}