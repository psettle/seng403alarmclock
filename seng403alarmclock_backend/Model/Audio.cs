using System;
using System.Threading;
using System.Media;
using System.IO;
using System.Reflection;

namespace seng403alarmclock.Model
{
    [Serializable]
    public class Audio
    {
        public SoundPlayer player;

        public Audio(String audioID)
        {
            player = new SoundPlayer();
         
            string directory = "../../../seng403alarmclock_backend/AudioFiles/" + audioID;     
            player.SoundLocation = directory;
          

            
        }

        public void start()
        {
            try
            {
                player.Play();
            }
            catch (FileNotFoundException)
            {

            }

        }

        public void end()
        {
            try
            {
                player.Stop();
            }
            catch (FileNotFoundException)
            {
            }

        }
    }
}