using System.Collections.Generic;

namespace seng403alarmclock.Audio
{
    /// <summary>
    /// Base class for controlling audio, needs to be specialized for the OS
    /// </summary>
    abstract public class AbstractAudioController
    {
        /// <summary>
        /// The private instance for the singleton
        /// </summary>
        public static AbstractAudioController instance = null;

        /// <summary>
        /// Sets the audio controller for the current platform
        /// </summary>
        /// <param name="instance"></param>
        public static void SetController(AbstractAudioController instance) {
            AbstractAudioController.instance = instance;
        }

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns>
        /// The singleton instance
        /// </returns>
        public static AbstractAudioController GetController() {
            return instance;
        }

        /// <summary>
        /// Initializes the audio driver
        /// </summary>
        /// 
        private List<AudioI> audioObjects = new List<AudioI>();

        /// <summary>
        /// Implement this to create the correct type of AudioI for the OS
        /// </summary>
        /// <param name="fileName">The file to create the audio object for</param>
        /// <returns>
        /// An AudioI object, ready to play
        /// </returns>
        abstract protected AudioI createAudioObject(string fileName);

        /// <summary>
        /// Creates and returns a new audio object for the provided filename
        /// </summary>
        /// <param name="fileName">The name of the file play</param>
        /// <returns></returns>
        public AudioI getAudio(string fileName) {
            AudioI audio = createAudioObject(fileName);
            audioObjects.Add(audio);
            return audio;
        }

        /// <summary>
        /// Ends the sound for all alarms, used during shutdown
        /// </summary>
        public void endAllAlarms() {
            //iterate over each audio object and end it
            foreach(AudioI audio in audioObjects) {
                audio.end();
            }
        }
    }
}