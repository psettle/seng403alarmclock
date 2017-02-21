using System;
using System.Collections.Generic;

namespace seng403alarmclock.GUI {
    /// <summary>
    /// Internal representation of an alarm, add to it as necessairy
    /// 
    /// alarmTime is the the time the alarm will go off at
    /// 
    /// alarmName is a unique name for the alarm (the time it was created) so that we can id alarms later and remove them
    /// </summary>
    public class Alarm {

        private DateTime alarmTime { get; set; }
        private String alarmName { get; set; }

        public Boolean IsRinging { get; set; }
        /// <summary>
        public Alarm()
        /// Create an alarm 5 minutes from now
        /// </summary>
        {
            IsRinging = false;
            IsWeekly = false;
            IsRepeating = false;
            alarmTime = DateTime.Now.AddMinutes(5);
            alarmName = "Created:" + DateTime.Now.ToString();
        }

        public Alarm(DateTime alarmTime) {
            IsRinging = false;
            IsWeekly = false;
            IsRepeating = false;
            this.alarmTime = alarmTime;
            alarmName = "Created:" + DateTime.Now.ToString();
        }

        public DateTime GetAlarmTime() {
            //TODO: return a reasonable time
            return alarmTime;
        }

        /// <summary>
        /// Indicates if the alarm repeats or not
        /// </summary>
        public bool IsRepeating { get; set; } = false;

        /// <summary>
        /// Indicates if the alarm is running a weekly cycle, instead of a daily cycle
        /// </summary>
        public bool IsWeekly { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>If this.IsWeekly, returns a list of days the alarm goes off on, otherwise returns null</returns>
        public List<DayOfWeek> GetWeekdays() {
            return new List<DayOfWeek>() {DayOfWeek.Monday };
        }




    }
}