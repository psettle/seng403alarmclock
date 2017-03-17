using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace seng403alarmclock.Model {
    /// <summary>
    /// This class acts as an interface for grabbing the time, to make changing the system time easy
    /// </summary>
	public class TimeFetcher {

        /// <summary>
        /// The offset from the real time that is being used
        /// </summary>
        private static TimeSpan offsetFromRealTime = new TimeSpan(0);

        /// <summary>
        /// How many hours the timezone is offset from the local time
        /// </summary>
        private static double offsetHours = 0;

        /// <summary>
        /// Calculates and returns the current system time
        /// </summary>
        /// <returns>
        /// The current system time
        /// </returns>
        public static DateTime getCurrentTime() {
            DateTime currentTime = DateTime.Now.Add(offsetFromRealTime);
            currentTime = currentTime.AddHours(offsetHours);
            return currentTime;
        }
       

        /// <summary>
        /// Sets a new date for the fetcher, this will automatically propagate to all instances of TimeFetcher
        /// </summary>
        public static void SetNewDate(int year, int month, int day) {
            DateTime systemTime = getCurrentTime();

            DateTime realTime = DateTime.Now;

            DateTime newTime = new DateTime(year, month, day, systemTime.Hour, systemTime.Minute, systemTime.Second, DateTimeKind.Local);

            TimeSpan newOffset = newTime - realTime;

            offsetFromRealTime = newOffset;
        }

        /// <summary>
        /// Sets a new time for the fetcher, this will automatically propagate to all instances of TimeFetcher
        /// </summary>
        public static void SetNewTime(int hour, int minute) {
            DateTime systemTime = getCurrentTime();

            DateTime realTime = DateTime.Now;

            DateTime newTime = new DateTime(systemTime.Year, systemTime.Month, systemTime.Day, hour, minute, systemTime.Second, DateTimeKind.Local);

            TimeSpan newOffset = newTime - realTime;

            offsetFromRealTime = newOffset;
        }

        /// <summary>
        /// Sets the offset from the local timezone, in hours
        /// </summary>
        /// <param name="hours"></param>
        public static void setOffset(double hours) {
            offsetHours = hours;
        }
    }

    /// <summary>
    /// This class sends out periodic pulses to help other parts of the system keep track of time updates
    /// </summary>
    public class TimePulseGenerator {
        /// <summary>
        /// Timer for regular pulses
        /// </summary>
		private DispatcherTimer timer = new DispatcherTimer();

        /// <summary>
        /// All the registered listeners
        /// </summary>
		private List<TimeListener> listeners = new List<TimeListener>();

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static TimePulseGenerator instance = new TimePulseGenerator();

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
		public static TimePulseGenerator Instance { get { return instance; } }

        /// <summary>
        /// Registers a new time listener
        /// </summary>
        /// <param name="listener">
        /// The listener to register
        /// </param>
        public void registerListener(TimeListener listener) {
            listeners.Add(listener);
		}

        /// <summary>
        /// Launches the pulse generator by starting the pulse timer
        /// </summary>
		private TimePulseGenerator(){
			timer.Tick += Timer_Tick;
			timer.Interval = new TimeSpan(0,0,0,0,100);
			timer.Start();
		}

        /// <summary>
        /// The pulse event, calls time pulse on each listener
        /// </summary>
		private void Timer_Tick(object sender, EventArgs e) {
            //grab the time from the time fetcher
			DateTime currentTime = TimeFetcher.getCurrentTime();
            //call time pulse on each listener
			foreach(TimeListener listener in listeners) {
				listener.TimePulse(currentTime);
			}
		}	
	}

    /// <summary>
    /// Interface for receiving time pulses
    /// </summary>
	public interface TimeListener{
        /// <summary>
        /// Called ~every 100ms
        /// </summary>
        /// <param name="currentTime">
        /// the current system time
        /// </param>
		void TimePulse(DateTime currentTime);
	}


}



