using seng403alarmclock.Audio;
using System;

namespace seng403alarmclock_silverlight_frontend.Audio {
    public class AudioController : AbstractAudioController {
        protected override AudioI createAudioObject(string fileName) {
            Audio audio = new Audio();
            audio.buildForAudioFile(fileName);
            return audio;
        }
    }
}
