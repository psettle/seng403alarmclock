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
        public static AudioController GetController()
        {
            return instance;
        }

        /// <summary>
        /// Initializes the audio driver
        /// </summary>
        /// 

        private Audio[] audioObjects = new Audio[0];


        private AudioController()
        {

        }
        public Audio createAudioObject(String audioID)
        {
            Audio a = new Audio(audioID);
            addAudioObjectToList(a);
            return a;
        }
        private void addAudioObjectToList(Audio a)
        {
            if (audioObjects.Length == 0)
            {
                audioObjects = new Audio[1];
                audioObjects[0] = a;
            }
            else
            {
                Audio[] temp = new Audio[audioObjects.Length + 1];
                for (int i = 0; i < audioObjects.Length; i++)
                {
                    temp[i] = audioObjects[i];
                }
                temp[audioObjects.Length] = a;
                audioObjects = temp;
            }
        }
        public Audio[] returnList()
        {
            return audioObjects;
        }
        /*
        public void beginAlarmNoise(int alarmID)
        {
           
        }
        public void endAlarmNoise(int alarmID)
        {

        }
        */
        public void endAllAlarms()
        {
            for (int i = 0; i < audioObjects.Length; i++)
            {
                audioObjects[i].end();
            }
        }
    }
}