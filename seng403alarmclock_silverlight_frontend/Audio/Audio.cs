using seng403alarmclock.Audio;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace seng403alarmclock_silverlight_frontend.Audio {
    /// <summary>
    /// Audio driver for silverlight
    /// </summary>
    public class Audio : AudioI {
        /// <summary>
        /// The player for audio code
        /// </summary>
        private MediaElement me;

        /// <summary>
        /// Indicates if the audio should be playing
        /// </summary>
        private bool playing = false;

        /// <summary>
        /// Sets up the audio object for playing filename
        /// </summary>
        public void buildForAudioFile(string filename) {
            //stop what is currently running
            end();

            //create the media player
            me = new MediaElement();
            me.AutoPlay = false;

            //start opening a stream to the audio file
            WebClient client = new WebClient();
            client.OpenReadAsync(new Uri(Application.Current.Host.Source, "/" + filename));
            client.OpenReadCompleted += Client_OpenReadCompleted;

        }

        /// <summary>
        /// Connects the filestream with the media player after the connection is established
        /// </summary>
        private void Client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e) {
            Stream s = e.Result;
            me.SetSource(s);
            if (playing) {
                start();
            }
        }

        /// <summary>
        /// Stops the currently playing audio
        /// </summary>
        public void end() {
            playing = false;
            if (me != null) {
                me.Stop();
            }
        }

        /// <summary>
        /// Starts the audio
        /// 
        /// Note:
        /// If the stream hasn't been attached, it will start automatically when the remote request completes
        /// </summary>
        public void start() {
            playing = true;
            if (me != null) {
                me.Play();
            }
        }
    }
}