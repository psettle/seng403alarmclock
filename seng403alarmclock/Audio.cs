using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;

namespace seng403alarmclock
{
    class Audio
    {
        bool play = true;
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

        public void playSound()
        {
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
    }
}