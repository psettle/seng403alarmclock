using System;
using System.Threading;
using System.Media;
using System.IO;
using System.Reflection;

namespace seng403alarmclock.Model
{
    public class Audio
    {
        public SoundPlayer player;

        public Audio(String audioID)
        {
            player = new SoundPlayer();
            player.SoundLocation = @"AudioFiles\" + audioID + ".wav";
        }

        public void start()
        {
            try
            {
                player.Play();
            }
            catch (FileNotFoundException e)
            {

            }

        }

        public void end()
        {
            try
            {
                player.Stop();
            }
            catch (FileNotFoundException e)
            {
            }

        }
    }
}