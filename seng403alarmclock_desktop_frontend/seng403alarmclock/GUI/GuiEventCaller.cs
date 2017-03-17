using System;
using System.Collections.Generic;

namespace seng403alarmclock.GUI {
    /// <summary>
    /// The unified event caller for the observer design pattern
    /// 
    /// Implemented as a singleton, so to add a listener go:
    /// GuiEventCaller.getCaller().AddListener(new GuiEventListener() {});
    /// </summary>
    class GuiEventCaller { 
        /// <summary>
        /// The singleton variable
        /// </summary>
        private static GuiEventCaller guiEventcaller = new GuiEventCaller();

        /// <summary>
        /// The listeners to the class
        /// </summary>
        private List<GuiEventListener> listeners = new List<GuiEventListener>();

        /// <summary>
        /// Private ctor
        /// </summary>
        private GuiEventCaller() { }

        /// <summary>
        /// Gets the singleton class, used to register listeners or notify listeners of events
        /// </summary>
        /// <returns></returns>
        public static GuiEventCaller GetCaller() {
            return guiEventcaller;
        }

        /// <summary>
        /// Adds a listener to the event caller
        /// </summary>
        /// <param name="listener">
        /// The listener to register
        /// </param>
        public void AddListener(GuiEventListener listener) {
            listeners.Add(listener);
        }

        /// <summary>
        /// Removes a listener from the event caller
        /// </summary>
        /// <param name="listener">
        /// The listener to remove
        /// </param>
        public void RemoveListener(GuiEventListener listener) {
            listeners.Remove(listener);
        }

        /// <summary>
        /// Notify all listeners of a cancel event
        /// </summary>
        /// <param name="alarm">
        /// The alarm that was canceled
        /// </param>
        public void NotifyCancel(Alarm alarm) {
            foreach (GuiEventListener listener in listeners) {
                listener.AlarmCanceled(alarm);
            }
        }

        /// <summary>
        /// Notify all listeners of a dismiss event
        /// </summary>
        /// <param name="alarm">
        /// The alarm that was dismissed
        /// </param>
        public void NotifyDismiss(Alarm alarm) {
            foreach (GuiEventListener listener in listeners) {
                listener.AlarmDismissed(alarm);
            }
        }

        /// <summary>
        /// Notify all listeners that an alarm was requested
        /// </summary>
        /// <param name="hour">The hour the user wants an alarm at (24h)</param>
        /// <param name="minute">The minute the user wants an alarm at</param>
        /// <param name="repeat">True if the user wants the alarm to repeat</param>
        /// <param name="audioFile">The filename for the audio sound to play when the alarm goes off</param>
        /// <param name="weekly">True if the user wants the alarm to use a weekly scheduling period</param>
        /// <param name="days">Which days of the week the alarm should go off on, only used for weekly scheduling</param>
        public void NotifyAlarmRequested(int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days) {
            foreach (GuiEventListener listener in listeners) {
                listener.AlarmRequested(hour, minute, repeat, audioFile, weekly, days);
            }
        }

        #region snooze

        /// <summary>
        /// Notify all listeners that a snooze was requested
        /// </summary>
        /// <param name="alarm"></param>
        public void NotifySnoozeRequested(Alarm alarm) {
            foreach (GuiEventListener listener in listeners) {
                listener.SnoozeRequested(alarm);
            }
        }

        /// <summary>
        /// Notify all listeners that a snooze time period change was requested
        /// </summary>
        /// <param name="minutes">The minutes to change the snooze to</param>
        public void NotifySnoozePeriodChangeRequested(int minutes) {
            foreach (GuiEventListener listener in listeners) {
                listener.SnoozePeriodChangeRequested(minutes);
            }
        }

        #endregion

        /// <summary>
        /// Notify all listeners that a manual time request was created
        /// </summary>
        /// <param name="hours">The hour the user wants to change the time to (24h)</param>
        /// <param name="minutes">>The minute the user wants to change the time to</param>
        public void NotifyManualTimeRequested(int hours, int minutes) {
            foreach (GuiEventListener listener in listeners) {
                listener.ManualTimeRequested(hours, minutes);
            }
        }
    }
}
