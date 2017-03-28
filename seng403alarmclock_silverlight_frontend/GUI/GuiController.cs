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
using System.IO;

namespace seng403alarmclock.GUI {
    /// <summary>
    /// The methods in AbstractGuiController are triggered by TimeController and AlarmController,
    /// the rest are triggered by other gui components
    /// </summary>
    public class GuiController : AbstractGuiController {
      
        #region Attributes
        /// <summary>
        /// A reference to this program's main page
        /// </summary>
        private MainPage mainPage = null;
        private AddEditWindow addEditWindow = null;

        /// <summary>
        /// reference to optionsPanelController
        /// </summary>
        private OptionsPanel_Controller optionsPanelController = null;

        #endregion

        /// <summary>
        /// Assigns the main page to the controller
        /// </summary>
        /// <param name="main"></param>
        public void assignMainPage(MainPage main) {
            mainPage = main;
            addEditWindow = new AddEditWindow(main);
            optionsPanelController = new OptionsPanel_Controller(main);
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
            
        }

        public override void EditAlarm(Alarm alarm, List<Alarm> allAlarms) {
           
        }

        public override void RemoveAlarm(Alarm alarm, bool wasPreempted) {
            
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

        public override void UpdateAlarm(Alarm alarm) {

        }

        #region open/close panels

        /// <summary>
        /// Opens the options panel. Only call from MainPage
        /// </summary>
        public void OpenOptionsPanel()
        {
            optionsPanelController.OpenOptionsPanel();
        }

        /// <summary>
        /// Closes the options panel. Only call from MainPage
        /// </summary>
        public void CloseOptionsPanel()
        {
            optionsPanelController.CloseOptionsPanel();
        }

        /// <summary>
        /// Opens the panel in a blank state, ready to input a new alarm
        /// </summary>
        public void OpenAddAlarmPanel() {
            addEditWindow.OpenAddAlarmPanel();
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

        #endregion

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
