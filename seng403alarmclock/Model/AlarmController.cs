using seng403alarmclock.GUI;
using System;
using System.Collections.Generic;

namespace seng403alarmclock.Model
{
    class AlarmController : GUI.GuiEventListener, TimeListener
    {
        #region fields and properties

        private List<Alarm> alarmList = new List<Alarm>();
        private AudioController audioController = AudioController.GetController();
        private GuiController guiController = GuiController.GetController();
        private TimeFetcher timeFetcher = new TimeFetcher();

        #endregion

        public void AlarmCanceled(Alarm alarm)
        {
            ///throw new NotImplementedException();
            alarmList.Remove(alarm);
            guiController.RemoveAlarm(alarm);
        }

        public void AlarmDismissed(Alarm alarm)
        {
            int ringtoneIndex = 0;
             audioController.endAlarmNoise(0);
            alarmList.Remove(alarm);
            guiController.RemoveAlarm(alarm);
            return;

            //Don't need repeating alarms yet - Patrick

            DateTime newTime = alarm.GetAlarmTime();
          
            newTime = newTime.AddDays(1);

            Alarm newAlarm = new Alarm(newTime);
            GuiController.GetController().AddAlarm(newAlarm);
            //Console.WriteLine(newAlarm.GetAlarmTime());
            alarmList.Add(newAlarm);

        
            
        }

        /// <summary>
        /// cycles through list of alarms to see which is ready to go off, then calls TriggerAlarm on each of them
        /// </summary>
        private void CheckAlarms()
        {
            DateTime now = this.timeFetcher.getCurrentTime();

            foreach (Alarm a in alarmList)
                if ( (a.GetAlarmTime().CompareTo(now) <= 0) && (!a.IsRinging) ) //possible bug: might need to be <= 0 
                    TriggerAlarm(a);
                        
                
        }

        /// <summary>
        /// requests that the audio controller begin ringing with the ringtone at said index
        /// </summary>
        /// <param name="alarm"></param>
        private void TriggerAlarm(Alarm alarm)
        {
            alarm.IsRinging = true;
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
            DateTime now = this.timeFetcher.getCurrentTime();

            int day;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, hour, minute,0,DateTimeKind.Local);
            if (d > now) {
                day = now.Day;
            } else {
                //Console.WriteLine("Tomorrow");
                day = (now.AddDays(1)).Day;
            }
            DateTime alarmTime = new DateTime(now.Year, now.Month, day, hour, minute, 0, DateTimeKind.Local);
            Alarm newAlarm = new Alarm(alarmTime);
            GuiController.GetController().AddAlarm(newAlarm);
            //Console.WriteLine(newAlarm.GetAlarmTime());
            alarmList.Add(newAlarm);
        }

        public void TimePulse(DateTime currentTime) {
            this.CheckAlarms();
        }

        public void AlarmRequested(Alarm alarm, int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days) {
            throw new NotImplementedException();
        }
    }
}
