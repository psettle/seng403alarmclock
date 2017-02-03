using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seng403alarmclock
{
    class AlarmController : GUI.GuiEventListener
    {
        private List<Alarm> alarmList = new List<Alarm>();

        public void AlarmCanceled(Alarm alarm)
        {
            throw new NotImplementedException();
        }

        public void AlarmDismissed(Alarm alarm)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When the user creates a new alarm, determine if the alarm is for today or tomorrow.
        /// Add the alarm to a list of alarms
        /// 
        /// ****NOTE: Should use 24H entry of Alarm for now
        /// </summary>
        /// <param name="hour">The hour the alarm should go off</param>
        /// <param name="minute">The minute the alarm should go off</param>
        public void AlarmRequested(int hour, int minute)
        {
            int day;
            if (hour >= DateTime.Now.Hour) {
                day = DateTime.Now.Day;
            } else {
                //Console.WriteLine("Tomorrow");
                day = (DateTime.Now.AddDays(1)).Day;
            }
            DateTime alarmTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day, hour, minute, 0, DateTimeKind.Local);
            Alarm newAlarm = new Alarm(alarmTime);
            Console.WriteLine(newAlarm.GetAlarmTime());
            alarmList.Add(newAlarm);
        }

    }
}
