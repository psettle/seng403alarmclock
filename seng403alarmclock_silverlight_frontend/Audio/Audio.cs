using System;
using seng403alarmclock.Audio;
using System.Threading;
//using System.Windows.Media;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Controls;

namespace seng403alarmclock_silverlight_frontend.Audio {
    public class Audio : AudioI {

        // used to play audio files
        private MediaElement me;

        //sets up audio object with audio file
        public void buildForAudioFile(string filename) {

            me = new MediaElement();

            string directory = filename;

            //ensure nothing is playing before assigning MediaElement to a new audio file
            me.Stop();

            //assign MediaElement to audio file provided by directory
            me.Source = new Uri(directory, UriKind.Relative);
        }
        
        //ends the currently playing audio file
        public void end() {
            if (me != null)
            {
                me.Stop();
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        //starts the currently playing audio file
        public void start() {
            if (me != null)
            {
                me.Play();
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}