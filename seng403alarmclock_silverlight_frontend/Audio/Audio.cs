

using seng403alarmclock.Audio;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace seng403alarmclock_silverlight_frontend.Audio {
    public class Audio : AudioI {

        private MediaElement me;

        private bool playing = false;

        public void buildForAudioFile(string filename) {  
            end();

            me = new MediaElement();
            me.AutoPlay = false;
            WebClient client = new WebClient();
            client.OpenReadAsync(new Uri(Application.Current.Host.Source, "/" + filename));
            client.OpenReadCompleted += Client_OpenReadCompleted;
         
        }

        private void Client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e) {
            Stream s = e.Result;
            me.SetSource(s); 
            if(playing) {
                start();
            }
        }

        public void end() {
            playing = false;
            if (me != null)
            {
                me.Stop();
            }
        }

        public void start() {
            playing = true;
            if (me != null)
            {
                me.Play();
            }
        }
    }
}