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
            //string path = @"AudioFiles";
            //string path = @"C:\Folder1\Folder2\Folder3\Folder4";
            //string newPath = Path.GetFullPath(Path.Combine("", @"..\..\"));
            string directory = System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString();
            directory = directory + @"\AudioFiles\" + audioID + ".wav";
            player.SoundLocation = directory;
            //player.SoundLocation = @"AudioFiles\" + audioID + ".wav";

            
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