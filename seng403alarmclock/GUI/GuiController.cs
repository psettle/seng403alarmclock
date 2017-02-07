using System;
using System.Collections.Generic;

namespace seng403alarmclock.GUI {
    /// <summary>
    /// The facade for the GUI, any controls to alter the display on the GUI are called through this class
    /// 
    /// This class must be initialized by calling SetMainWindow before it is useful
    /// 
    /// Implements a singleton design pattern, i.e. 
    /// GuiController guiController = GuiController.getGuiController(); to access the functionality
    /// </summary>
    class GuiController {
        /// <summary>
        /// A reference to the main window of the application
        /// </summary>
        private static MainWindow mainWindow = null;

        /// <summary>
        /// The singleton object
        /// </summary>
        private static GuiController guiController = new GuiController();

        /// <summary>
        /// The list of currenlty rendered alarm rows
        /// </summary>
        private Dictionary<Alarm, AlarmRow> activeAlarms = new Dictionary<Alarm, AlarmRow>();

        /// <summary>
        /// Used to access the singleton object
        /// </summary>
        /// <returns>
        /// The singleton instance
        /// </returns>
        public static GuiController GetController() {
            return guiController;
        }

        /// <summary>
        /// Assign the main window to the class
        /// </summary>
        /// <param name="mainWindow">
        /// The main window object that launches the program
        /// </param>
        public static void SetMainWindow(MainWindow window) {
            mainWindow = window;
        }

        /// <summary>
        /// Private ctor
        /// </summary>
        private GuiController() { }

        /// <summary>
        /// Sets the time displayed on the GUI
        /// </summary>
        /// <param name="time">
        /// The time to display on the GUI
        /// </param>
        public void SetTime(DateTime time) {
            mainWindow.SetTimeText(time.ToLongTimeString());
            mainWindow.SetDateText(time.Date.ToLongDateString());         
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
        public void AddAlarm(Alarm alarm) {
            AlarmRow row = new AlarmRow(alarm);
            activeAlarms.Add(alarm, row);
            mainWindow.AddAlarmRow(row);
        }

        /// <summary>
        /// Triggers a row of the gui to set the alarm off
        /// </summary>
        /// <param name="alarm">
        /// The alarm the row was built from
        /// </param>
        /// <exception cref="AlarmNotSetException">
        /// If the provided alarm was never used to create a row, this exception will be thrown
        /// </exception>
        public void TriggerAlarm(Alarm alarm) {
            this.GetAlarmRow(alarm).SetDismiss();
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
        public void RemoveAlarm(Alarm alarm) {
            AlarmRow row = this.GetAlarmRow(alarm);
            mainWindow.RemoveAlarmRow(row);
            activeAlarms.Remove(alarm);
        }

       



        /// <summary>
        /// Fetches an AlarmRow from activeAlarms
        /// </summary>
        /// <param name="alarm">The alarm to lookup with</param>
        /// <returns>The fetched alarm row</returns>
        /// <exception cref="AlarmNotSetException">
        /// If the provided alarm was never used to create a row, this exception will be thrown
        /// </exception>
        private AlarmRow GetAlarmRow(Alarm alarm) {
            AlarmRow toReturn = null;

            if (this.activeAlarms.TryGetValue(alarm, out toReturn)) {
                return toReturn;
            } else {
                throw new AlarmNotSetException("The requested alarm did not exist");
            }   
        }
    }
}
