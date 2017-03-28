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

        private AddEditWindow addEditWindow = null;

        /// <summary>
        /// Assigns the main page to the controller
        /// </summary>
        /// <param name="main"></param>
        public void assignMainPage(MainPage main) {
            mainPage = main;
            addEditWindow = new AddEditWindow(main);
            SetAudioFileNames(new Dictionary<string, string>() { { "test.wav", "Test It!" }, {"select.wav", "Select This" } });
        }

        public GuiController() : base() {
            //TODO: Get the audio files!
            

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

        public void SetDisplayMode(bool analog)
        {
            if (analog)
            {
                mainPage.SetAnalog();
            }
            else
            {
                mainPage.SetDigital();
            }

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
