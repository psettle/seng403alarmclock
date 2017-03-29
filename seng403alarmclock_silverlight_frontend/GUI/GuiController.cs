using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using seng403alarmclock.GUI_Interfaces;
using seng403alarmclock.Model;
using seng403alarmclock_silverlight_frontend;
using seng403alarmclock_silverlight_frontend.GUI;

namespace seng403alarmclock.GUI {
    /// <summary>
    /// The methods in AbstractGuiController are triggered by TimeController and AlarmController,
    /// the rest are triggered by other gui components
    /// </summary>
    public class GuiController : AbstractGuiController {
        /// <summary>
        /// A reference to this program's main page
        /// </summary>
        private MainPage mainPage = null;

        /// <summary>
        /// The list of currenlty rendered alarm rows
        /// </summary>
        private Dictionary<Alarm, AlarmRow> activeAlarms = new Dictionary<Alarm, AlarmRow>();

        /// <summary>
        /// A reference to the add/edit sub window
        /// </summary>
        private AddEditWindow addEditWindow = null;

        private OptionsPanel_Controller optionsPanelController = null;

        /// <summary>
        /// Assigns the main page to the controller
        /// </summary>
        /// <param name="main"></param>
        public void assignMainPage(MainPage main) {
            mainPage = main;
            addEditWindow = new AddEditWindow(main);
            optionsPanelController = new OptionsPanel_Controller();
            CrawlAudioFiles();
        }

        /// <summary>
        /// Crawls through the file system to scan for .wav files
        /// </summary>
        private void CrawlAudioFiles() {
            SetAudioFileNames(new Dictionary<string, string>() {
                { "TheAssumingSong.mp3", "The Assuming Song" },
                { "Cat.mp3", "Cat" },
                { "Chicken.mp3", "Chicken" },
                { "DangerAlarm.mp3", "Danger Alarm" },
                { "Dog.mp3", "Dog" },

            });
        }

        /// <summary>
        /// Returns the controller instance
        /// </summary>
        /// <returns></returns>
        public static new GuiController GetController() {
            return (GuiController)guiController;
        }

        public override void AddAlarm(Alarm alarm) {
            AlarmRow row = new AlarmRow(alarm);
            activeAlarms.Add(alarm, row);
            mainPage.AddAlarmRow(row);
        }

        public override void EditAlarm(Alarm alarm, List<Alarm> alarmList) {
            AlarmRow row = this.GetAlarmRow(alarm);
            //AlarmRow nRow = new AlarmRow(alarm);
            row.UpdateAlarm();
            foreach (Alarm a in alarmList)
            {
                this.RemoveAlarm(a, false);
            }

            foreach (Alarm a in alarmList)
            {
                AddAlarm(a);
            }
        }

        public override void RemoveAlarm(Alarm alarm, bool wasPreempted) {
            AlarmRow row = this.GetAlarmRow(alarm);
            mainPage.RemoveAlarmRow(row, wasPreempted);
            activeAlarms.Remove(alarm);
        }

        private AlarmRow GetAlarmRow(Alarm alarm)
        {
            AlarmRow toReturn = null;

            if (this.activeAlarms.TryGetValue(alarm, out toReturn))
            {
                return toReturn;
            }
            else
            {
                throw new AlarmNotSetException("The requested alarm did not exist");
            }
        }

        public override void SetActiveTimeZoneForDisplay(double localOffset) {
           
        }

        public override void SetDismissAvailable(bool available) {
            
        }

        public override void SetSnoozeAvailable(bool available) {
            
        }

        /// <summary>
        /// Sets the time that is displayed on the GUI
        /// </summary>
        /// <param name="time"></param>
        public override void SetTime(DateTime time) {
            mainPage.SetTime(time);
        }

        public override void Shutdown() {
            
        }

        /// <summary>
        /// Updates the text on the alarm, setting it into dismiss mode
        /// </summary>
        /// <param name="alarm"></param>
        public override void UpdateAlarm(Alarm alarm) {
            AlarmRow row = this.GetAlarmRow(alarm);
            row.Update();
        }


        /// <summary>
        /// Opens the panel in a blank state, ready to input a new alarm
        /// </summary>
        public void OpenAddAlarmPanel() {
            addEditWindow.OpenAddAlarmPanel();
        }


        public void OpenOptionsPanel() {
            optionsPanelController.OpenOptionsPanel();
        }

        public void CloseOptionsPanel()
        {
            optionsPanelController.CloseOptionsPanel();
        }

        /// <summary>
        /// Opens the panel preloaded with alarm info, ready to save
        /// </summary>
        /// <param name="targetAlarm">
        /// The alarm to load defaults from and to update when to operation is complete
        /// </param>
        public void OpenEditAlarmPanel(Alarm targetAlarm) {
            addEditWindow.OpenEditAlarmPanel(targetAlarm);
        }

        /// <summary>
        /// Sets the audio files displayed on the dropdown menus
        /// </summary>
        /// <param name="audioFiles">
        /// A table of filename => displayname
        /// </param>
        private void SetAudioFileNames(Dictionary<string, string> audioFiles) {
            addEditWindow.SetAudioFileNames(audioFiles);
        }
    }
}
