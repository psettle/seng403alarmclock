using seng403alarmclock.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seng403alarmclock
{
    class AlarmController : GUI.GuiEventListener
    {
        #region fields and properties

        private List<Alarm> alarmList = new List<Alarm>();
        private AudioController audioController = AudioController.GetController();
        private GuiController guiController = GuiController.GetController();

        #endregion

        public void AlarmCanceled(Alarm alarm)
        {
            throw new NotImplementedException();
        }

        public void AlarmDismissed(Alarm alarm)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// cycles through list of alarms to see which is ready to go off, then calls TriggerAlarm on each of them
        /// </summary>
        public void CheckAlarms()
        {
            foreach (Alarm a in alarmList) 
                if (a.GetAlarmTime().CompareTo(DateTime.Now) >= 0 )         //possible bug: might need to be <= 0 
                    TriggerAlarm(a);
        }

        /// <summary>
        /// requests that the audio controller begin ringing with the ringtone at said index
        /// </summary>
        /// <param name="alarm"></param>
        private void TriggerAlarm(Alarm alarm)
        {
            int ringtoneIndex = 0;
            audioController.beginAlarmNoise(ringtoneIndex);
            guiController.TriggerAlarm(alarm);
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
            DateTime d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute,0,DateTimeKind.Local);
            if (d > DateTime.Now) {
                day = DateTime.Now.Day;
            } else {
                //Console.WriteLine("Tomorrow");
                day = (DateTime.Now.AddDays(1)).Day;
            }
            DateTime alarmTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day, hour, minute, 0, DateTimeKind.Local);
            Alarm newAlarm = new Alarm(alarmTime);
            GuiController.GetController().AddAlarm(newAlarm);
            //Console.WriteLine(newAlarm.GetAlarmTime());
            alarmList.Add(newAlarm);
        }
    }
}
