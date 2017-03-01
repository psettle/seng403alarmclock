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
         
            string directory = System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString();
            directory = directory + @"\AudioFiles\" + audioID;
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