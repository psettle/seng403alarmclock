using System;
using System.Threading;

public class TimeFetcher {
	public static void Main() {
		TimeSpan interval = new TimeSpan(0, 0, 0, 0, 100); //pulse rate = 100 miliseconds		
		while(true) {
			DateTime time = DateTime.Now; 				//fetch the system time
			Console.WriteLine(time.ToString("G")); 		//write time
			Thread.Sleep(interval); 					//pulse ten times a second
		}
	}
}