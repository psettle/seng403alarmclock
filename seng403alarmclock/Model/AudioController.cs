using System;
using System.Threading;

namespace seng403alarmclock.Model
{
    public class AudioController
    {
        /// <summary>
        /// The private instance for the singleton
        /// </summary>
        private static AudioController instance = new AudioController();

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns>
        /// The singleton instance
        /// </returns>
        public static AudioController GetController() {
            return instance;
        }
       
        /*static void Main(String[] args)
        {
            AudioController a = new AudioController();
            a.beginAlarmNoise(3);
            Thread.Sleep(10000);
            a.endAlarmNoise(3);
            return;
        }*/

        private Audio[] audios = new Audio[3];
        //private Thread[] threads = new Thread[3];

        /// <summary>
        /// Initializes the audio driver
        /// </summary>
        private AudioController()
        {
            initializeAudio();
        }

        private void initializeAudio()
        {
            for (int i = 0; i < 3; i++)
            {
                audios[i] = new Audio("location", i.ToString());
                //threads[i] = new Thread(new ThreadStart(audios[i].playSound));
            }
        }

        public void beginAlarmNoise(int alarmID)
        {
            if (alarmID >= 0 && alarmID <= 2)
            {
                //audios[alarmID].incrementAlarmCount();
                audios[alarmID].start();
                //threads[alarmID].Start();
            }
            else
            {
                // currently do nothing 
            }
            
        }
        public void endAlarmNoise(int alarmID)
        {
            if (alarmID >= 0 && alarmID <= 2)
            {
                audios[alarmID].end();
                //threads[alarmID].Abort();
            }
            else
            {
                // currently do nothing 
            }
        }
        public void endAllAlarms()
        {
            for (int i = 0; i < audios.Length; i++)
            {
                this.endAlarmNoise(i);
            }
        }
    }
}