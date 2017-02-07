using System;
using System.Windows.Threading;



namespace Clock {
	public class TimeFetcher {
	
		public DateTime getCurrentTime() {
			DateTime currentTime = DateTime.Now;
			return currentTime;
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

		};

		private void Timer_Tick(object sender, EventArgs e) {
			DateTime currentTime = time.getCurrentTime();
			foreach(TimeListener listener in Listeners) {
				listener.TimePulse(currentTime);
			}
		}

		private static TimePulseGenerator instance = new TimePulseGenerator();

		public TimePulseGenerator fetch() {
			return instance;
		}
	}

		public static void Main() {
			TimeFetcher t = new TimeFetcher();
			while (true) {
				DateTime currentTime = t.getCurrentTime();
				Console.WriteLine(currentTime.ToString("G"));
			}
			
		}
	}

	interface TimeListener{
		public void TimePulse(DateTime currentTime);

		

	}


}



