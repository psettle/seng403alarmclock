using System;
using System.Threading;
using System.Media;
using System.IO;
using System.Reflection;

namespace seng403alarmclock.Audio
{
    /// <summary>
    /// Audio driver for windows desktop
    /// </summary>
    [Serializable]
    public class Audio : AudioI
    {
        /// <summary>
        /// The windows desktop sound player
        /// </summary>
        private SoundPlayer player;

        /// <summary>
        /// Starts the audio tone
        /// </summary>
        public void start() {
            if (player != null) {
                player.Play();
            }
        }

        /// <summary>
        /// Ends the audio tone for this object
        /// </summary>
        public void end() {
           if(player != null) {
                player.Stop();
           }
        }

        /// <summary>
        /// Sets this audio object to play filename
        /// </summary>
        /// <param name="filename">The audio file to play</param>
        public void buildForAudioFile(string filename) {
            //safety end so we don't have a lose audio object still playing
            end();
            player = new SoundPlayer();
            string directory = filename;
            player.SoundLocation = directory;
        }
    }
}