using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seng403alarmclock.Audio {
    /// <summary>
    /// Implementation of the audio controller for windows desktop
    /// </summary>
    class AudioController : AbstractAudioController {

        /// <summary>
        /// Builds and returns the windows desktop version of Audio
        /// </summary>
        /// <param name="fileName">The name of the file to play as a tone</param>
        /// <returns>
        /// The built audio object
        /// </returns>
        protected override AudioI createAudioObject(string fileName) {
            Audio audio = new Audio();
            audio.buildForAudioFile(fileName);
            return audio;
        }
    }
}
