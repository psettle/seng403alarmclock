using seng403alarmclock_backend.Timer;
using System;
using System.Windows.Threading;

namespace seng403alarmclock.Timer {
    /// <summary>
    /// Implementation of the Dispatch Timer for desktop
    /// </summary>
    public class SengDispatcherTimer : DispatcherTimerI {
        /// <summary>
        /// The dispatch timer we will use to implement the behavior
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer();

        /// <summary>
        /// The callback to run for the timer
        /// </summary>
        private Action callback = null;

        /// <summary>
        /// Creates a new SengDispatchTimer
        /// </summary>
        public SengDispatcherTimer() {
            timer.Tick += Timer_Tick;
        }

        /// <summary>
        /// Stops the dispatch timer
        /// </summary>
        public void End() {
            timer.Stop();
        }

        /// <summary>
        /// Sets the pulse length for the timer
        /// </summary>
        /// <param name="ms">The number of milliseconds between pulses</param>
        public void PulseLength(int ms) {
            timer.Interval = new TimeSpan(0, 0, 0, 0, ms);
        }

        /// <summary>
        /// Assigns the callback for the timer
        /// </summary>
        /// <param name="call"></param>
        public void SetCallback(Action call) {
            callback = call;
        }

        /// <summary>
        /// Called by the internal timer, invokes the actual callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e) {
           if(callback != null) {
                callback.Invoke();
            }
        }

        /// <summary>
        /// Starts the dispatch timer
        /// </summary>
        public void Start() {
            timer.Start();
        }
    }
}