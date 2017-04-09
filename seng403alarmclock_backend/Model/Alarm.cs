using seng403alarmclock.Audio;
using seng403alarmclock_backend.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace seng403alarmclock.Model
{

    public enum AlarmState { Off, Ringing, Snoozing };

    /// <summary>
    /// Internal representation of an alarm, add to it as required
    /// 
    /// alarmTime is the the time the alarm will go off at
    /// 
    /// alarmName is a unique name for the alarm (the time it was created) so that we can id alarms later and remove them
    /// </summary>
    [DataContract]
    public class Alarm
    {

        #region Attributes

        /// <summary>
        /// For creating audio instances
        /// </summary>
        private static AbstractAudioController audioController = AbstractAudioController.GetController();

        /// <summary>
        /// The days this alarm goes off on, 
        /// </summary>
        [DataMember]
        private List<DayOfWeek> days = null;

        /// <summary>
        /// The time that this alarm is supposed to go off
        /// </summary>
        [DataMember]
        private DateTime alarmTime { get; set; }
        [DataMember]
        private int hour;
        [DataMember]
        private int minute;

        /// <summary>
        /// A name for this alarm, currently unused
        /// </summary>

        [DataMember]
        public string alarmName { get; set; }

        [DataMember]
        private AlarmState status;

        public AlarmState Status
        {
            get { return status; }
            set
            {
                switch (value)
                {
                    case AlarmState.Off:
                        status = AlarmState.Off;
                        if (audio != null)
                            audio.end();
                        break;

                    case AlarmState.Ringing:
                        status = AlarmState.Ringing;
                        if (audio != null)
                            audio.start();
                        break;

                    case AlarmState.Snoozing:
                        status = AlarmState.Snoozing;
                        if (audio != null)
                            audio.end();
                        break;

                    default:
                        throw new Exception("invalid alarm state");
                }
            }
        }

        /// <summary>
        /// Indicates if this alarm is going off
        /// </summary>
        public bool IsRinging
        {
            get { return (status == AlarmState.Ringing); }
        }

        public bool IsSnoozing
        {
            get { return (status == AlarmState.Snoozing); }
            
        }

        //private bool _IsRinging;
        /// <summary>
        /// Indicates if the alarm repeats or not
        /// </summary>
        [DataMember]
        public bool IsRepeating { get; set; }

        /// <summary>
        /// Indicates if the alarm is running a weekly cycle, instead of a daily cycle
        /// </summary>
        [DataMember]
        public bool IsWeekly { get; set; }

        /// <summary>
        /// The audio to play when this alarm goes off
        /// </summary>
        [DataMember]
        private AudioI audio = null;
        [DataMember]
        private string audioFile;

        #endregion

        #region Constructors
        /// <summary> 
        /// Create an alarm 5 minutes from now
        /// </summary>
        public Alarm()
        {
            status = AlarmState.Off;
            IsWeekly = false;
            IsRepeating = false;
            alarmTime = DateTime.Now.AddMinutes(5);
            alarmName = "Created:" + DateTime.Now.ToString();
        }


        /// <summary>
        /// Creates an alarm at hour:minute with a few options:
        /// </summary>
        /// 
        /// <param name="hour">The hour of day this alarm goes off at</param>
        /// <param name="minute">The minute of day this alarm goes off at</param>
        /// <param name="repeat">Whether or not this alarm repeats</param>
        /// <param name="audioFile">The name of the audio file to play as the alarm tone</param>
        /// <param name="weekly">Indicates if this alarm runs on a weekly cycle (false is a daily cycle)</param>
        /// <param name="days">If weekly, indicates which days of the week the alarm goes off on</param>
        public Alarm(int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days, string alarmName)
        {
            try
            {

                alarmName = setAlarmName(alarmName);

                this.hour = hour;
                this.minute = minute;
                this.audioFile = audioFile;
                audio = audioController.getAudio(audioFile);

                IsRepeating = repeat;
                IsWeekly = weekly;

                if (weekly)
                {
                    WeeklyCtor(hour, minute, days);
                }
                else
                {
                    NonWeeklyCtor(hour, minute);
                }
            }
            catch (NullReferenceException e) { }
            
        }


        public string setAlarmName(string name) {
            alarmName = name;
            return alarmName;
        }

        /// <summary>
        /// Contructor for an alarm that is weekly
        /// </summary>
        /// <param name="hour">The hour of day this alarm goes off at</param>
        /// <param name="minute">The minute of day this alarm goes off at</param>
        /// <param name="days">Indicates which days of the week the alarm goes off on</param>
        private void WeeklyCtor(int hour, int minute, List<DayOfWeek> days)
        {

            //the earlist the alarm could possible go off is the next occurence of hour:minute, start with that
            SetAlarmForNextOccurenceOf(hour, minute);


            //assign & prepare the days array
            this.days = new List<DayOfWeek>();

            //sort the input days to make sure they are in the correct order (Sunday -> Saturday)
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                if (days.Contains(day))
                {
                    this.days.Add(day);
                }
            }

            //increment the alarm so it occurs on a day the alarm is supposed to go off on
            IncrementAlarmTimeForNextWeekday();
        }

        /// <summary>
        /// Constructor for an alarm that is not weekly
        /// </summary>
        /// <param name="hour">The hour of day this alarm goes off at</param>
        /// <param name="minute">The minute of day this alarm goes off at</param>
        private void NonWeeklyCtor(int hour, int minute)
        {
            SetAlarmForNextOccurenceOf(hour, minute);
        }

        /// <summary>
        /// Sets this alarm for either hour:minute today, or if it has already passed, hour:minute tomorrow
        /// </summary>
        /// <param name="hour">The hour of day this alarm goes off at</param>
        /// <param name="minute">The minute of day this alarm goes off at</param>
        private void SetAlarmForNextOccurenceOf(int hour, int minute)
        {
            //get the current time
            DateTime now = TimeFetcher.getCurrentTime();
            //create an alarm at hour:minute today
            alarmTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0, DateTimeKind.Local);
            //if that time has already passed move it up one day
            if (alarmTime <= now)
            {
                AddOneDayToAlarm();
            }
        }

        #endregion

        #region TimeIncrementers

        /// <summary>
        /// Increments the alarm time to the next weekday it goes off on (weekly alarms only)
        /// 
        /// If the alarm is already set for a day it goes off on, it does nothing
        /// </summary>
        /// <exception cref="NoMoreAlarmsException">
        /// Thrown if there is not day in the array to increment into
        /// </exception>
        private void IncrementAlarmTimeForNextWeekday()
        {
            //the index to start checking for days at
            int checkIndex = (int)alarmTime.DayOfWeek;
            for (int i = 0; i < 7; i++)
            { //iterate a maximum of 7 times (for 7 weekdays)

                if (days.Contains((DayOfWeek)checkIndex))
                {
                    //if the alarm goes off on the picked day, then the alarm is already set to the correct day
                    return;
                }
                else
                {
                    //else it doesn't go off today, increment one day and keep checking
                    AddOneDayToAlarm();
                }


                //increment and rollover the day index
                checkIndex++;
                if (checkIndex >= 7)
                {
                    checkIndex = 0;
                }
            }
            //if we didn't find a day the alarm occurs on in 7 tries, there must be no days for it to go off on
            throw new NoMoreAlarmsException();
        }

        /// <summary>
        /// Adds one day to this alarm's target time
        /// </summary>
        private void AddOneDayToAlarm()
        {
            alarmTime = alarmTime.AddDays(1);
        }

        #endregion

        #region PublicFetchers

        /// <summary>
        /// Instructs the alarm to calculate its next arrival time
        /// </summary>
        /// <exception cref="NoMoreAlarmsException">
        /// Thrown if there are no more times this alarm should go off
        /// </exception>
        public void CalculateNextAlarmTime()
        {
            if (IsWeekly)
            {
                if (!IsRepeating)
                {
                    //if it doesn't repeat, remove the day that just went off from the days array
                    days.Remove(alarmTime.DayOfWeek);
                }
                //by adding one day, the increment will catch on the next day the alarm is supposed to go off on
                AddOneDayToAlarm();
                IncrementAlarmTimeForNextWeekday();
            }
            else
            {
                if (!IsRepeating)
                {
                    //if it doesn't repeat and is not weekly, there is never another time for it to go off
                    throw new NoMoreAlarmsException();
                }
                else
                {
                    //otherwise it does repeat, add one day
                    AddOneDayToAlarm();
                }
            }
        }

        /// <summary>
        /// Gets the time this alarm is supposed to go off
        /// </summary>
        /// <see cref="CalculateNextAlarmTime"/>
        /// <returns>
        /// The time this alarm is supposed to go off
        /// </returns>
        public DateTime GetAlarmTime()
        {
            return alarmTime;
        }

        /// <summary>
        /// Gets the days of the week this alarm goes off on
        /// </summary>
        /// <returns>If this.IsWeekly, returns a list of days the alarm goes off on, otherwise returns null</returns>
        public List<DayOfWeek> GetWeekdays()
        {
            return days;
        }

        public int GetHour()
        {
            return this.hour;
        }

        public int GetMinute()
        {
            return this.minute;
        }

        public string GetAudioFile()
        {
            return this.audioFile;
        }
        #endregion

        public void EditAlarm(string name,int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days)
        {
            try
            {

                this.alarmName = name;
                this.hour = hour;
                this.minute = minute;
                this.audioFile = audioFile;
                audio = audioController.getAudio(audioFile);

                IsRepeating = repeat;
                IsWeekly = weekly;

                if (weekly)
                {
                    WeeklyCtor(hour, minute, days);
                }
                else
                {
                    NonWeeklyCtor(hour, minute);
                }
            }
            catch (NullReferenceException e) { }
           
        }

    }
}