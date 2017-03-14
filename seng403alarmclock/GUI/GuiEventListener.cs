using System;
using System.Collections.Generic;
using static seng403alarmclock.GUI.GuiEventCaller;

namespace seng403alarmclock.GUI {
    /// <summary>
    /// This is the public interface for listening to the gui,
    /// new features may be added later (AlarmRequested will definately change)
    /// </summary>
    interface GuiEventListener {
        /// <summary>
        /// Called if the user requests the cancelation of an alarm
        /// </summary>
        /// <param name="alarm">
        /// The alarm that was initially added to the GUI
        /// </param>
        void AlarmCanceled(Alarm alarm, bool fadeIt);

        

        /// <summary>
        /// Called if the user requests the dismissal of an active alarm
        /// </summary>
        /// <param name="alarm">
        /// The alarm that was initially added to the GUI
        /// </param>
        void AlarmDismissed(bool dueToPreEmpt);

        /// <summary>
        /// Called if the user requests the placement or change of an alarm
        /// </summary>
        /// <param name="hour">The hour the user wants an alarm at (24h)</param>
        /// <param name="minute">The minute the user wants an alarm at</param>
        /// <param name="repeat">True if the user wants the alarm to repeat</param>
        /// <param name="audioFile">The filename for the audio sound to play when the alarm goes off</param>
        /// <param name="weekly">True if the user wants the alarm to use a weekly scheduling period</param>
        /// <param name="days">Which days of the week the alarm should go off on, only used for weekly scheduling</param>
        void AlarmRequested(int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days);

        /// <summary>
        /// Called if the user has hit sleep on an alarm
        /// </summary>
        /// <param name="alarm"></param>
        void SnoozeRequested();

        /// <summary>
        /// Called if the user wants to change the time that a snooze lasts
        /// </summary>
        /// <param name="minutes">The minutes to change the snooze to</param>
        void SnoozePeriodChangeRequested(int minutes);

        /// <summary>
        /// Called if the user wants to change the time on the clock manually
        /// </summary>
        /// <param name="hours">The hour the user wants to change the time to (24h)</param>
        /// <param name="minutes">>The minute the user wants to change the time to</param>
        void ManualTimeRequested(int hours, int minutes);

        /// <summary>
        /// Called if the user wants to change the date on the clock manually
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        void ManualDateRequested(int year, int month, int day);

        ///<summary>
        ///Called if the user wants to edit an alarm
        ///</summary>
        ///
        void AlarmEdited(Alarm alarm, int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days);

        /// <summary>
        /// Called when the main window is closing
        /// </summary>
        void MainWindowShutdown();

        /// <summary>
        /// Called when the timezone offset is changed
        /// </summary>
        /// <param name="offset">
        /// The new offset
        /// </param>
        void TimeZoneOffsetChanged(double offset);
    }
}
