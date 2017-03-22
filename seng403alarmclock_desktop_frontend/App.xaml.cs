using seng403alarmclock.GUI;
using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using seng403alarmclock.Data;
using seng403alarmclock.Audio;
using seng403alarmclock.Model;
using seng403alarmclock_backend.Data;
using seng403alarmclock.GUI_Interfaces;
using seng403alarmclock.Timer;

namespace seng403alarmclock {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// If the last time this program started, this value was true, the program will automatically start when the 
        /// computer is restarted
        /// </summary>
        private static readonly bool launchOnStartup = false;

        /// <summary>
        /// The main controller for the app
        /// </summary>
        private static AlarmController ac = null;

        /// <summary>
        /// The time controller for the app
        /// </summary>
        private static TimeController tc = null;

        /// <summary>
        /// Called when the application starts
        /// </summary>
        protected override void OnStartup (StartupEventArgs e) {
            base.OnStartup(e);
            LaunchOnRestart();
            AssignPlatformControllers();
            SetAudioFileNames();

            TimePulseGenerator.Instance = new TimePulseGenerator(new SengDispatcherTimer());


            ac = new AlarmController();
            GuiEventCaller.GetCaller().AddListener(ac);
            TimePulseGenerator.Instance.registerListener(ac);

            tc = new TimeController();
            GuiEventCaller.GetCaller().AddListener(tc);
            TimePulseGenerator.Instance.registerListener(tc);
        }

        /// <summary>
        /// Called when the application exits
        /// </summary>
        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            //turn off all alarms
            AbstractAudioController.GetController().endAllAlarms();
            //teardown the alarm/time controllers
            ac.Teardown();
            //finally save any residual state data
            AbstractDataDriver.Instance.Shutdown();
        }

        /// <summary>
        /// Does any system level setup for the new main window
        /// </summary>
        public static void SetupMainWindow() {
            ac.SetupMainWindow();
        }

        /// <summary>
        /// Assigns the platform dependent controllers at runtime
        /// </summary>
        private void AssignPlatformControllers() {
            AbstractGuiController.SetController(new GuiController());
            AbstractDataDriver.addType(typeof(List<Alarm>));
            AbstractDataDriver.addType(typeof(Alarm));
            AbstractDataDriver.addType(typeof(Audio.Audio));
            AbstractDataDriver.Instance = new DataDriver();


            AbstractAudioController.SetController(new AudioController());
        }

        /// <summary>
        /// Creates or deletes the restart shortcut as required
        /// </summary>
        private void LaunchOnRestart() {
            if (launchOnStartup) {
                CreateStartupShortcut();
            } else {
                DeleteStartupShortcut();
            }
            //if starting from shortcut, we need to reset the CWD so we can find resource files
            SetCWD();
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
        private void SetAudioFileNames() {
            Dictionary<string, string> table = new Dictionary<string, string>();

            DirectoryInfo audioFolder = new DirectoryInfo("../../../seng403alarmclock_backend/AudioFiles");

            FileInfo[] fileInfos = audioFolder.GetFiles();

            foreach(FileInfo fileInfo in fileInfos) {
                table.Add(fileInfo.FullName, Path.GetFileNameWithoutExtension(fileInfo.Name));
            }

            GuiController.GetController().SetAudioFileNames(table);

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

        /// <summary>
        /// Deletes the startup shortcut that may have been created before
        /// </summary>
        private void DeleteStartupShortcut() {
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\seng403AlarmClock.url");  
        }
    }
}
