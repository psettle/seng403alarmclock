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
        /// Notify all listeners of an alarm requested event
        /// </summary>
        /// <param name="hour">The hour the alarm is requested at</param>
        /// <param name="minute">The minute the alarm is requested at</param>
        public void NotifyAlarmRequested(int hour, int minute) {
            foreach(GuiEventListener listener in listeners) {
                listener.AlarmRequested(hour, minute);
            }
        }
    }
}
