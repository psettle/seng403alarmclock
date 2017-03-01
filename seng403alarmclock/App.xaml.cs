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

namespace seng403alarmclock
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static AlarmController ac = new AlarmController();
        /// <summary>
        /// Called when the application starts
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup (StartupEventArgs e) {
            base.OnStartup(e);
            GuiEventCaller.GetCaller().AddListener(ac);
            TimeController tc = new TimeController();
            TimePulseGenerator.fetch().add(tc);
            TimePulseGenerator.fetch().add(ac);

            setAudioFileNames();

        }

        /// <summary>
        /// Sets the default audio names by crawling the audio resource folder and passing it the names to the GUI
        /// </summary>
        private void setAudioFileNames() {
            Dictionary<string, string> table = new Dictionary<string, string>();

            DirectoryInfo audioFolder = new DirectoryInfo("../../AudioFiles");

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
    }
}
