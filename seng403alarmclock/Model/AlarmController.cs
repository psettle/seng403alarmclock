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
            //the alarm is  no longer ringing
            alarm.IsRinging = false;
            try {
                //try to move the alarm to its next scheduled time
                //catch the exception if this was the last time, then remove it
                alarm.CalculateNextAlarmTime();
                //graphically update the GUI since the alarm state has changed
                guiController.UpdateAlarm(alarm);
            } catch(NoMoreAlarmsException) {
                AlarmCanceled(alarm);//code reuse -N
            }


            bool allIsQuiet = true;
            foreach (Alarm a in alarmList)
                if (a.IsRinging)
                    allIsQuiet = false;
            if (allIsQuiet)
                guiController.Snooze_Btn_setHidden();
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
            guiController.UpdateAlarm(alarm);
            //guiController.UpdateAlarm(alarm);
            guiController.Snooze_Btn_setVisible();
        }
        
        #endregion

        #region Alarm Requests

        public void AlarmRequested(int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days) {
            Alarm newAlarm = new Alarm(hour, minute, repeat, audioFile, weekly, days);
            GuiController.GetController().AddAlarm(newAlarm);
            alarmList.Add(newAlarm);
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


