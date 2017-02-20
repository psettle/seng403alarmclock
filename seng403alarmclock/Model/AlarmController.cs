using seng403alarmclock.GUI;
using System;
using System.Collections.Generic;

namespace seng403alarmclock.Model
{
    class AlarmController : GUI.GuiEventListener, TimeListener
    {
        #region fields and Properties
        
        //locals
        private List<Alarm>     alarmList;
        private AudioController audioController; 
        private GuiController   guiController;   
        private TimeFetcher     timeFetcher;
        private DateTime        snoozeUntilTime;

        //statics
        private static int      snoozePeriod_minutes    = 1;
        private static int      maxSnoozePeriod_minutes = 40;

        //Properties
        public DateTime         SnoozeUntilTime
        {
            get
            {
                return this.snoozeUntilTime;
            }
        }

        #endregion
        
        #region constructors

        /// <summary>
        /// Constructor: initializes snoozeUntilTime to harmless value
        /// </summary>
        public AlarmController()
        {
            this.alarmList          = new List<Alarm>();
            this.audioController    = AudioController.GetController();
            this.guiController      = GuiController.GetController();
            this.timeFetcher        = new TimeFetcher();

            this.snoozeUntilTime = this.timeFetcher.getCurrentTime();
        }

        #endregion

        #region dismiss/cancel alarms

        public void AlarmCanceled(Alarm alarm)
        {
            alarmList.Remove(alarm);
            guiController.RemoveAlarm(alarm);
        }

        public void AlarmDismissed(Alarm alarm)
        {
            int ringtoneIndex = 0;
            audioController.endAlarmNoise(ringtoneIndex);

            AlarmCanceled(alarm);               //code reuse -N
            //alarmList.Remove(alarm);
            //guiController.RemoveAlarm(alarm);

            bool allIsQuiet = true;
            foreach (Alarm a in alarmList)
                if (a.IsRinging)
                    allIsQuiet = false;
            if (allIsQuiet)
                guiController.Snooze_Btn_setHidden();

            return;

            //Don't need repeating alarms yet - Patrick

            DateTime newTime = alarm.GetAlarmTime();
          
            newTime = newTime.AddDays(1);

            Alarm newAlarm = new Alarm(newTime);
            GuiController.GetController().AddAlarm(newAlarm);
            //Console.WriteLine(newAlarm.GetAlarmTime());
            alarmList.Add(newAlarm);
            
        }

        #endregion

        #region Check / Trigger Alarm

        public void TimePulse(DateTime currentTime)
        {
            this.CheckAlarms(currentTime);
        }

        /// <summary>
        /// cycles through list of alarms to see which is ready to go off, then calls TriggerAlarm on each of them
        /// </summary>
        private void CheckAlarms(DateTime now)
        {
            if (CheckIfSnoozeOver(now))
               foreach (Alarm a in alarmList)
                    if ( CheckIfAlarmIsDue(a, now) && (!a.IsRinging) )
                        TriggerAlarm(a);
            else
              UpdateGUI_SnoozeRemaining_minutes(now);
        }
        
        /// <summary>
        /// requests that the audio controller begin ringing with the ringtone associated with the alarm
        /// </summary>
        /// <param name="alarm"></param>
        private void TriggerAlarm(Alarm alarm)
        {
            int ringtoneIndex = 0;
            audioController.beginAlarmNoise(ringtoneIndex);
            alarm.IsRinging = true;
            guiController.TriggerAlarm(alarm);
            //guiController.UpdateAlarm(alarm);
            guiController.Snooze_Btn_setVisible();
        }
        
        #endregion

        #region Alarm Requests

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
            DateTime d = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0, DateTimeKind.Local);
            if (d > now)
            {
                day = now.Day;
            }
            else
            {
                //Console.WriteLine("Tomorrow");
                day = (now.AddDays(1)).Day;
            }
            DateTime alarmTime = new DateTime(now.Year, now.Month, day, hour, minute, 0, DateTimeKind.Local);
            Alarm newAlarm = new Alarm(alarmTime);
            GuiController.GetController().AddAlarm(newAlarm);
            //Console.WriteLine(newAlarm.GetAlarmTime());
            alarmList.Add(newAlarm);
        }

        public void AlarmRequested(Alarm alarm, int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days) {
            throw new NotImplementedException();
        }

        public void AlarmRequested(int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days) {
            throw new NotImplementedException();
        }

        #endregion

        #region snooze
        /// <summary>
        /// wrapper for version not requiring alarm argument
        /// </summary>
        public void SnoozeRequested(Alarm a) {
            this.SnoozeRequested();
        }

        /// <summary>
        /// snooze alarms from being able to ring for snoozePeriod_minutes
        /// </summary>
        public void SnoozeRequested() {
            DateTime now = this.timeFetcher.getCurrentTime();
            if ( CheckIfSnoozeOver(now) )
            {
                updateSnoozeUntilTime(now);
                guiController.Snooze_Btn_setHidden();

                foreach (Alarm a in alarmList)
                {
                    a.IsRinging = false;
                    audioController.endAllAlarms();
                }
            }
        }
        
        /// <summary>
        /// sets snoozePeriod_minutes. if period > maxSnoozePeriod_minutes, sets it to maxSnoozePeriod_minutes instead. 
        /// </summary>
        public void SnoozePeriodChangeRequested(int minutes)
        {
            if (minutes < maxSnoozePeriod_minutes)
                AlarmController.snoozePeriod_minutes = minutes;
            else
                AlarmController.snoozePeriod_minutes = maxSnoozePeriod_minutes;
        }
        
        #endregion

        #region helper functions
        //these are mostly to make the above functions more readable

        private bool CheckIfAlarmIsDue(Alarm alarm, DateTime now)
        {
            return (alarm.GetAlarmTime().CompareTo(now) <= 0);
        }

        private void updateSnoozeUntilTime(DateTime now)
        {
            this.snoozeUntilTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, (now.Minute + AlarmController.snoozePeriod_minutes), 0);
        }

        private bool CheckIfSnoozeOver(DateTime currentTime)
        {
            return (this.snoozeUntilTime.CompareTo(currentTime) <= 0);
        }

        private void UpdateGUI_SnoozeRemaining_minutes(DateTime now)
        {
            this.guiController.SetSnoozeDisplayTime(this.snoozeUntilTime.Subtract(now).Minutes);
        }

        #endregion


        public void ManualTimeRequested(int hours, int minutes) {
            throw new NotImplementedException();
        }

    }
}


