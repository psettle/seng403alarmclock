using seng403alarmclock.GUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using seng403alarmclock;
using seng403alarmclock.Model;
using System.IO;
using seng403alarmclock.Data;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using seng403alarmclock.GUI_Interfaces;

namespace seng403alarmclock
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// If the last time this program started, this value was true, the program will automatically start when the 
        /// computer is restarted
        /// </summary>
        private static readonly bool launchOnStartup = true;


        private static AlarmController ac = null;
        /// <summary>
        /// Called when the application starts
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup (StartupEventArgs e) { 
            if(launchOnStartup) {
                CreateStartupShortcut();
            } else {
                DeleteStartupShortcut();
            }
            
            SetCWD();

            base.OnStartup(e);

            AbstractGuiController.SetController(new GuiController());


            ac = new AlarmController();
            GuiEventCaller.GetCaller().AddListener(ac);
            TimeController tc = new TimeController();
            tc.Setup();
            GuiEventCaller.GetCaller().AddListener(tc);
            TimePulseGenerator.fetch().add(tc);
            TimePulseGenerator.fetch().add(ac);

            setAudioFileNames();

        }

        /// <summary>
        /// Sets the current working directory to the directory the .exe is in
        /// </summary>
        private void SetCWD() {
            string fullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            //cut the file name out:
            string dir = Path.GetDirectoryName(fullPath);

            Directory.SetCurrentDirectory(dir);
        }

        /// <summary>
        /// Sets the default audio names by crawling the audio resource folder and passing it the names to the GUI
        /// </summary>
        private void setAudioFileNames() {
            Dictionary<string, string> table = new Dictionary<string, string>();

            DirectoryInfo audioFolder = new DirectoryInfo("../../../seng403alarmclock_backend/AudioFiles");

            FileInfo[] fileInfos = audioFolder.GetFiles();

            foreach(FileInfo fileInfo in fileInfos) {
                table.Add(fileInfo.Name, Path.GetFileNameWithoutExtension(fileInfo.Name));
            }

            GuiController.GetController().SetAudioFileNames(table);

        }

        /// <summary>
        /// Called when the application exits
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            AudioController.GetController().endAllAlarms();

            ac.Teardown();
            DataDriver.Instance.shutdown();
        }

        public static void SetupMainWindow() {
            ac.SetupMainWindow();
        }

        /// <summary>
        /// Creates a startup shortcut in the startup folder of a windows machine
        /// 
        /// Thanks to: http://stackoverflow.com/questions/4897655/create-shortcut-on-desktop-c-sharp
        /// </summary>
        private void CreateStartupShortcut() {
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            StreamWriter writer = new StreamWriter(startupFolder + "\\seng403AlarmClock.url");

            string programName = System.Reflection.Assembly.GetExecutingAssembly().Location;

            writer.WriteLine("[InternetShortcut]");
            writer.WriteLine("URL=file:///" + programName);
            writer.WriteLine("IconIndex=0");
            string icon = programName.Replace('\\', '/');
            writer.WriteLine("IconFile=" + icon);
            writer.Flush();
        }

        private void DeleteStartupShortcut() {
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\seng403AlarmClock.url");  
        }
    }
}
