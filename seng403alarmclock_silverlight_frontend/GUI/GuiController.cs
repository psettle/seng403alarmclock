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
        #region attributes

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

        /// <summary>
        /// reference to the controller of the optionsPanel
        /// </summary>
        private OptionsPanel_Controller optionsPanelController = null;

        /// <summary>
        /// reference to controller of the alarmlist panel
        /// </summary>
        private AlarmListPanel_Controller alarmListPanelController = null;
		
		    /// <summary>
        /// the most recent time retrieved
        /// </summary>
		private DateTime now;		
		    /// <summary>
        /// possibly not used 
        /// </summary>
		public DateTime Now{ get { return this.now; } }

        #endregion

        /// <summary>
        /// Assigns the main page to the controller
        /// </summary>
        /// <param name="main"></param>
        public void assignMainPage(MainPage main) {
            mainPage = main;

            addEditWindow = new AddEditWindow(main);
            optionsPanelController = new OptionsPanel_Controller(main);
            alarmListPanelController = new AlarmListPanel_Controller(main);

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
		
		    #region open/close panels

        /// <summary>
        /// Opens the panel in a blank state, ready to input a new alarm
        /// </summary>
        public void OpenAddAlarmPanel() {
            addEditWindow.OpenAddAlarmPanel();
        }

        /// <summary>
        /// closes the panel, the equivalent of clicking the cancel button
        /// </summary>
        public void CloseAddEditPanel()
        {
            addEditWindow.CloseAddAlarmPanel();
        }

        /// <summary>
        /// Opens the options panel
        /// </summary>
        public void OpenOptionsPanel() {
            PopulateCustomTimeUI();
            optionsPanelController.OpenOptionsPanel();
        }
		
		    /// <summary>
        /// Closes the options panel
        /// </summary>
        public void CloseOptionsPanel()
        {
            optionsPanelController.CloseOptionsPanel();
        }

        

        /// <summary>
        /// opens the alarmlist panel
        /// </summary>
        public void OpenAlarmListPanel()
        {
            alarmListPanelController.Open();
        }

        /// <summary>
        /// closes the alarmlist panel
        /// </summary>
        public void CloseAlarmListPanel()
        {
            alarmListPanelController.Close();
        }

        #endregion

        #region Custom Time & Timezone functionality

            /// <summary>
            /// populates the options panel controller element that controls custom time
            /// </summary>
            public void PopulateCustomTimeUI(){
                optionsPanelController.SetCustomTime_displayedInOptions(now.Hour, now.Minute);
		    }
		
		    /// <summary>
		    /// sets the timezone offset variable in optionsPanelController
		    /// </summary>
            public override void SetActiveTimeZoneForDisplay(double localOffset) {
                OptionsPanel_Controller.timezoneOffsetHours = localOffset;
            }
		
		    #endregion

		    #region Add, Edit, Remove Alarms
		
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
		
		#endregion

		    #region Snooze & Dismiss
		
        public override void SetDismissAvailable(bool available) {
            if(available) {
                mainPage.Dismiss.Visibility = Visibility.Visible;
            } else {
                mainPage.Dismiss.Visibility = Visibility.Collapsed;
            }
		}

        public override void SetSnoozeAvailable(bool available) {
            if (available) {
                mainPage.Snooze.Visibility = Visibility.Visible;
            } else {
                mainPage.Snooze.Visibility = Visibility.Collapsed;
            }
        }

		#endregion
		
        /// <summary>
        /// Sets the time that is displayed on the GUI
        /// </summary>
        /// <param name="time"></param>
        public override void SetTime(DateTime time) {
   			    now = time;
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
        /// sets the GUI display mode to analog if true, and digital if false
        /// </summary>
        public void SetDisplayMode(bool analog) {
            if (analog) {
                mainPage.SetAnalog();
            }
            else
            {
                mainPage.SetDigital();
            }
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
