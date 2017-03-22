using seng403alarmclock.Model;
using System;
using System.Collections.Generic;

namespace seng403alarmclock.GUI_Interfaces {
    /// <summary>
    /// Base interface for controlling any platform's GUI
    /// </summary>
    public abstract class AbstractGuiController {
        /// <summary>
        /// The platform dependent gui controller
        /// </summary>
        protected static AbstractGuiController guiController = null;

        /// <summary>
        /// Used to set the current GUI controller
        /// </summary>
        /// <param name="controller">
        /// The controller to use
        /// </param>
        static public void SetController(AbstractGuiController controller) {
            guiController = controller;
        }

        /// <summary>
        /// Gets the current GUI controller
        /// </summary>
        /// <returns>
        /// The current GUI controller
        /// </returns>
        static public AbstractGuiController GetController() {
            return guiController;
        }

        /// <summary>
        /// Sets the time displayed on the GUI
        /// </summary>
        /// <param name="time">
        /// The time to display on the GUI
        /// </param>
        abstract public void SetTime(DateTime time);

        /// <summary>
        /// Adds a new alarm row onto the GUI using the provided alarm
        /// </summary>
        /// <param name="alarm">
        /// The alarm to build the row from
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the alarm was set without being cleared already
        /// </exception>
        abstract public void AddAlarm(Alarm alarm);

        /// <summary>
        /// Causes the GUI to recheck an alarm object for a state change and change appropriately
        /// 
        /// The alarm must already have been added with AddAlarm
        /// </summary>
        /// <param name="alarm">The alarm object to check</param>
        /// <exception cref="AlarmNotSetException">
        /// If the provided alarm was never used to create a row, this exception will be thrown
        /// </exception>
        abstract public void UpdateAlarm(Alarm alarm);

        /// <summary>
        /// Removes a row of the gui that represents the provided alarm
        /// </summary>
        /// <param name="alarm">
        /// The alarm the row was initially built from
        /// </param>
        /// <exception cref="AlarmNotSetException">
        /// If the provided alarm was never used to create a row, this exception will be thrown
        /// </exception>
        abstract public void RemoveAlarm(Alarm alarm, bool wasPreempted);

        /// <summary>
        /// Updates the alarm being displayed
        /// </summary>
        /// <param name="alarm">The alarm to update</param>
        /// <param name="alarmList">The list of all alarms in the system</param>
        abstract public void EditAlarm(Alarm alarm, List<Alarm> allAlarms);

        /// <summary>
        /// Controls if the GUI has a snooze option available
        /// </summary>
        abstract public void SetSnoozeAvailable(bool available);

        /// <summary>
        /// Controls if the GUI has a dismiss option available
        /// </summary>
        abstract public void SetDismissAvailable(bool available);

        /// <summary>
        /// Sets the timezone on the options menu (for display only, doesn't affect the system time)
        /// </summary>
        /// <param name="offsetFromUTC"></param>
        abstract public void SetActiveTimeZoneForDisplay(double localOffset);
        
        /// <summary>
        /// Called when the model wants to shut down the program
        /// </summary>
        abstract public void Shutdown();

       
       
        
    }
}
