using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace seng403alarmclock_backend.Timer {
    public interface DispatcherTimerI {
        /// <summary>
        /// Sets the pulse length on the timer
        /// </summary>
        /// <param name="ms">The number of milliseconds between pulses</param>
        void PulseLength(int ms);

        /// <summary>
        /// Assigns the callback for the timer to call
        /// </summary>
        /// <param name="call"></param>
        void SetCallback(Action call);

        /// <summary>
        /// Starts the timer
        /// </summary>
        void Start();

        /// <summary>
        /// Ends the timer
        /// </summary>
        void End();
    }
}
