using System;
using System.Threading;

namespace seng403alarmclock.Model
{
    class Audio
    {
        private Thread thread;// = new Thread(new ThreadStart(playSound));
        int alarmCount = 0;
        bool play = false;
        int beepFrequency;
        public Audio(String AudioLocation, String ID)
        {
            // code that will make reference to future audio files
            // for now the id will reference different beep frequencies
            switch (ID)
            {
                case "0":
                    beepFrequency = 1;
                    break;
                case "1":
                    beepFrequency = 5;
                    break;
                case "2":
                    beepFrequency = 10;
                    break;
                default:
                    beepFrequency = 1;
                    break;

            }
            
        }

        public void start()
        {
            if (alarmCount == 0)
            {
                thread = new Thread(new ThreadStart(playSound));
                thread.Start();
            }
            incrementAlarmCount();
            
        }

        public void end()
        {
            if (alarmCount == 1)
            {
                endSound();
                thread.Abort();
            }
            decrementAlarmCount();
        }

        public void playSound()
        {
            play = true;
            while (play)
            {
                Console.Beep();
                Thread.Sleep(1000 / beepFrequency);
            }
        }
        public void endSound()
        {
            play = false;
        }

        public void incrementAlarmCount()
        {
            alarmCount++;
        }

        public void decrementAlarmCount()
        {
            alarmCount--;
        }
    }
}