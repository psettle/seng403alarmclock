using seng403alarmclock.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using seng403alarmclock.GUI_Interfaces;

namespace seng403alarmclock.GUI
{
    /// <summary>
    /// The facade for the GUI, any controls to alter the display on the GUI are called through this class
    /// 
    /// This class must be initialized by calling SetMainWindow before it is useful
    /// 
    /// Implements a singleton design pattern, i.e. 
    /// GuiController guiController = GuiController.getGuiController(); to access the functionality
    /// </summary>
    class GuiController : AbstractGuiController {
        /// <summary>
        /// A reference to the main window of the application
        /// </summary>
        public static MainWindow mainWindow = null;

        private DateTime now; //the most recent time we have been given

        /// <summary>
        /// The list of currenlty rendered alarm rows
        /// </summary>
        private Dictionary<Alarm, AlarmRow> activeAlarms = new Dictionary<Alarm, AlarmRow>();

        new public static GuiController GetController() {
            return (GuiController)guiController;
        }

        /// <summary>
        /// Assign the main window to the class
        /// </summary>
        /// <param name="mainWindow">
        /// The main window object that launches the program
        /// </param>
        public static void SetMainWindow(MainWindow window)
        {
            mainWindow = window;
        }

        public void SetupOptionsWindow(OptionsWindow window) {
            window.SetTime(now.Hour, now.Minute);
        }

        /// <summary>
        /// Sets the time displayed on the GUI
        /// </summary>
        /// <param name="time">
        /// The time to display on the GUI
        /// </param>
        public override void SetTime(DateTime time) {
            if(mainWindow != null) {
                mainWindow.SetTime(time);
            }
            now = time;     
        }

        /// <summary>
        /// Adds a new alarm row onto the GUI using the provided alarm
        /// </summary>
        /// <param name="alarm">
        /// The alarm to build the row from
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the alarm was set without being cleared already
        /// </exception>
        
        public override void AddAlarm(Alarm alarm)
        {
            AlarmRow row = new AlarmRow(alarm);
            activeAlarms.Add(alarm, row);
            RenderAlarms();
            
        }
        
        /// <summary>
        ///Sorts the alarm panel so that the earliest alarm is on top
        /// </summary>
        public List<AlarmRow> AlarmSort() {
            List<AlarmRow> alarmList = new List<AlarmRow>();
            foreach (AlarmRow item in activeAlarms.Values)
            {
                alarmList.Add(item);
            }

            alarmList.Sort();

            return alarmList;
        }

        /// <summary>
        /// Rerenders all alarm rows
        /// </summary>
        private void RenderAlarms() {
            List<AlarmRow> toDisplay = AlarmSort();

            foreach(AlarmRow row in toDisplay) {
                row.RemoveFromGUI();
            }

            foreach (AlarmRow row in toDisplay) {
                row.UpdateAlarm();
                mainWindow.AddAlarmRow(row);
            }
        }
    
        #region snooze/dismiss set visible/hidden
        //if we have time to kill i'd- like to remove these calls and implement more locally

        /// <summary>
        /// Controls if the GUI has a snooze option available
        /// </summary>
        public override void SetSnoozeAvailable(bool available) {
            if(available) {
                mainWindow.Snooze_Button_setVisible();
            } else {
                mainWindow.Snooze_Button_setHidden();
            }
        }

        /// <summary>
        /// Controls if the GUI has a dismiss option available
        /// </summary>
        public override void SetDismissAvailable(bool available) {
           if(available) {
                mainWindow.Dismiss_Button_setVisible();
            } else {
                mainWindow.Dismiss_Button_setHidden();
            }
        }

        #endregion

        /// <summary>
        /// Causes the GUI to recheck an alarm object for a state change and change appropriately
        /// 
        /// The alarm must already have been added with AddAlarm
        /// </summary>
        /// <param name="alarm">The alarm object to check</param>
        /// <exception cref="AlarmNotSetException">
        /// If the provided alarm was never used to create a row, this exception will be thrown
        /// </exception>
        public override void UpdateAlarm(Alarm alarm)
        {
            if(mainWindow == null && alarm.IsRinging) {
                mainWindow = new MainWindow();
                mainWindow.Show();       
            } else if(mainWindow == null) {
                //no update if the alarm isn't ringing, it isn't pressing
                return;
            }

            AlarmRow row = this.GetAlarmRow(alarm);
            row.Update();
            RenderAlarms();
        }

        /// <summary>
        /// Removes a row of the gui that represents the provided alarm
        /// </summary>
        /// <param name="alarm">
        /// The alarm the row was initially built from
        /// </param>
        /// <exception cref="AlarmNotSetException">
        /// If the provided alarm was never used to create a row, this exception will be thrown
        /// </exception>
        public override void RemoveAlarm(Alarm alarm, bool wasPreempted)
        {
            AlarmRow row = this.GetAlarmRow(alarm);
            mainWindow.RemoveAlarmRow(row, wasPreempted);
            activeAlarms.Remove(alarm);
            RenderAlarms();
        }
        
        /// <summary>
        /// Immedietly removes the alarm from the GUI (without the fading effect)
        /// </summary>
        /// <param name="alarm"></param>
        private void RemoveAlarmNow(Alarm alarm) {
            AlarmRow row = this.GetAlarmRow(alarm);
            mainWindow.RemoveAlarmRowImmediately(row);
            activeAlarms.Remove(alarm);
            RenderAlarms();
        }

        /// <summary>
        /// Updates the alarm being displayed
        /// </summary>
        /// <param name="alarm">The alarm to update</param>
        /// <param name="alarmList">The list of all alarms in the system</param>
        public override void EditAlarm(Alarm alarm) {
            RenderAlarms();
        }

        /// <summary>
        /// Sets the list of files that can be used for alarm tones
        /// </summary>
        /// <param name="names">
        /// A table where filename is used as the index, and a user friendly name is used as the value, for example:
        /// "alarm.wav" => "Default Tone",
        /// "siren.wav" => "Banshee Call",
        /// 
        /// </param>
        public void SetAudioFileNames(Dictionary<string, string> names)
        {
            //deep copy the dictionary object into the controls array   
            EditAlarmWindow.audioDictionary = new Dictionary<string, string>(names);
        }

        /// <summary>
        /// Sets the default value for the snooze display field
        /// </summary>
        /// <param name="minutes">How many minutes the current snooze timer is</param>
        public void SetSnoozeDisplayTime(int minutes)
        {
            OptionsWindow.SetSnoozePeriodMinutes(minutes);
        }

        /// <summary>
        /// Fetches an AlarmRow from activeAlarms
        /// </summary>
        /// <param name="alarm">The alarm to lookup with</param>
        /// <returns>The fetched alarm row</returns>
        /// <exception cref="AlarmNotSetException">
        /// If the provided alarm was never used to create a row, this exception will be thrown
        /// </exception>
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

        public void SetDisplayMode(bool analog)
        {
            if (analog)
            {
                mainWindow.SetAnalog();
            }
            else
            {
                mainWindow.SetDigital();
            }

        }

        /// <summary>
        /// Called by the main window when it is shutting down, clears the alarm rows and notifying gui listeners that it has happened
        /// </summary>
        public void OnMainWindowShutdown() {
            GuiEventCaller.GetCaller().NotifyMainWindowClosing();
            this.activeAlarms = new Dictionary<Alarm, AlarmRow>();
            mainWindow = null;       
        }

        /// <summary>
        /// Sets the timezone on the options menu (for display only, doesn't affect the system time)
        /// </summary>
        /// <param name="offsetFromUTC"></param>
        public override void SetActiveTimeZoneForDisplay(double offsetFromUTC) {
            OptionsWindow.timezoneOffsetHours = offsetFromUTC;
        }

        /// <summary>
        /// Launches the window for creating/editing the alarm
        /// </summary>
        /// <param name="alarmToEdit">
        /// The alarm to edit, null to create a new alarm
        /// </param>
        public void LaunchEditWindow(Alarm alarmToEdit) {
            if(mainWindow.WindowState == WindowState.Maximized) {
                EditAlarmWindow eaw = new EditAlarmWindow(0, 0, mainWindow.ActualHeight, alarmToEdit);
                eaw.Show();
            } else {
                EditAlarmWindow eaw = new EditAlarmWindow(mainWindow.Left, mainWindow.Top, mainWindow.ActualHeight, alarmToEdit);
                eaw.Show();
            }
            
            
        }

        /// <summary>
        /// Called when the model wants to shut down the program
        /// </summary>
        public override void Shutdown() {
            App.Current.Shutdown();
        }
    }
}
