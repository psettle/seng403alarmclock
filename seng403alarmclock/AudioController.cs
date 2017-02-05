using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace eng403alarmclock
{
    public class AudioController
    {
        ~AudioController() {

            endAllAlarms();
        }

        static void Main(String[] args)
        {
            AudioController a = new AudioController();
            a.beginAlarmNoise(3);
            Thread.Sleep(10000);
            a.endAlarmNoise(3);
            return;
        }

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

            Console.Write("ass");
            if(alarmID >= 0 && alarmID <= 2)
            {
                threads[alarmID].Start();
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
                audios[alarmID].endSound();
                threads[alarmID].Abort();
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
                audios[i].endSound();
                threads[i].Abort();
            }
        }
    }
}