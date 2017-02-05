using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace seng403alarmclock
{
    public class AudioController
    {
        /*static void Main(String[] args)
        {

            //example of use:
            AudioController a = new AudioController();
            a.beginAlarmNoise(2);
            Thread.Sleep(10000);
            a.endAlarmNoise(2);
            return;
        }*/

        private Audio[] audios = new Audio[3];
        private Thread[] threads = new Thread[3];

        public AudioController()
        {
            initializeAudio();
        }
        public void initializeAudio()
        {
            for (int i = 0; i < 3; i++)
            {
                audios[i] = new Audio("location", i.ToString());
                threads[i] = new Thread(new ThreadStart(audios[i].playSound));
            }
        }

        public void beginAlarmNoise(int alarmID)
        {
            threads[alarmID].Start();
        }
        public void endAlarmNoise(int alarmID)
        {
            audios[alarmID].endSound();
            threads[alarmID].Abort();
        }
    }
}