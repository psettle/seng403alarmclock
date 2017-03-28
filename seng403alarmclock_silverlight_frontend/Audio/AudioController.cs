using seng403alarmclock.Audio;
using System;

namespace seng403alarmclock_silverlight_frontend.Audio {
    public class AudioController : AbstractAudioController {

        //return audio object with audio file provided by filename
        protected override AudioI createAudioObject(string fileName) {
            Audio audio = new Audio();

            //sets up audio object with audio file
            audio.buildForAudioFile(fileName);
            return audio;
        }
    }
}
