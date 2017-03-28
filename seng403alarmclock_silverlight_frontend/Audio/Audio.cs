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

        //private SoundPlayer player;
        private MediaElement me;


        public void buildForAudioFile(string filename) {
            //throw new NotImplementedException();
            end();

            //player = new SoundPlayer();
            me = new MediaElement();

            string directory = filename;

            //player.SoundLocation = directory;
            me.Stop();
            me.Source = new Uri(directory, UriKind.Relative);
        }

        public void end() {
            //throw new NotImplementedException();
            if (me != null)
            {
                me.Stop();
            }
        }

        public void start() {
            //throw new NotImplementedException();
            if (me != null)
            {
                me.Play();
            }
        }
    }
}