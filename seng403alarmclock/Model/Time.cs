using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace seng403alarmclock.Model {
	public class TimeFetcher {

        /// <summary>
        /// The offset from the real time that is being used
        /// </summary>
        private static TimeSpan offsetFromRealTime = new TimeSpan(0);

		public DateTime getCurrentTime() {
			DateTime currentTime = DateTime.Now.Add(offsetFromRealTime);
			return currentTime;
		}

        /// <summary>
        /// Sets a new date for the fetcher, this will automatically propagate to all instances of TimeFetcher
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public void SetNewDate(int year, int month, int day) {
            DateTime systemTime = this.getCurrentTime();

            DateTime realTime = DateTime.Now;

            DateTime newTime = new DateTime(year, month, day, systemTime.Hour, systemTime.Minute, systemTime.Second, DateTimeKind.Local);

            TimeSpan newOffset = newTime - realTime;

            offsetFromRealTime = newOffset;
        }

        /// <summary>
        /// Sets a new time for the fetcher, this will automatically propagate to all instances of TimeFetcher
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public void SetNewTime(int hour, int minute) {
            DateTime systemTime = this.getCurrentTime();

            DateTime realTime = DateTime.Now;

            DateTime newTime = new DateTime(systemTime.Year, systemTime.Month, systemTime.Day, hour, minute, systemTime.Second, DateTimeKind.Local);

            TimeSpan newOffset = newTime - realTime;

            offsetFromRealTime = newOffset;
        }

	
	}



	public class TimePulseGenerator {
		private TimeFetcher time = new TimeFetcher();
		private DispatcherTimer timer = new DispatcherTimer();
		private List<TimeListener> Listeners = new List<TimeListener>();

		public void add(TimeListener listener) {
			Listeners.Add(listener);
		}

		private TimePulseGenerator(){

			timer.Tick += Timer_Tick;
			timer.Interval = new TimeSpan(0,0,0,0,100);
			timer.Start();

		}

		private void Timer_Tick(object sender, EventArgs e) {
			DateTime currentTime = time.getCurrentTime();
			foreach(TimeListener listener in Listeners) {
				listener.TimePulse(currentTime);
			}
		}

		private static TimePulseGenerator instance = new TimePulseGenerator();

		public static TimePulseGenerator fetch() {
			return instance;
		}
	}

	public interface TimeListener{
		void TimePulse(DateTime currentTime);
	}


}



