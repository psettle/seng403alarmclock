using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seng403alarmclock.Audio {
    /// <summary>
    /// Generic interface for a system audio controller
    /// </summary>
    public interface AudioI {
        /// <summary>
        /// Sets the audio file for this audio to filename
        /// </summary>
        /// <param name="filename">The audio file to play</param>
        void buildForAudioFile(string filename);

        /// <summary>
        /// Starts the audio
        /// </summary>
        void start();

        /// <summary>
        /// Ends the audio
        /// </summary>
        void end();
    }
}
