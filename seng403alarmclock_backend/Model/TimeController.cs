using seng403alarmclock.Gui_Interfaces;
using seng403alarmclock.GUI_Interfaces;
using System;
using System.Collections.Generic;

namespace seng403alarmclock.Model {
    /// <summary>
    /// Basic controller for passes the time of day to the gui
    /// </summary>
    public class TimeController : TimeListener, GuiEventListener {
        
        /// <summary>
        /// Sets up the time controller for the current timezone
        /// </summary>
        public TimeController() {
            Setup();
        }
        
        /// <summary>
        /// Called whenever a time pulse occurs
        /// </summary>
        /// <param name="currentTime"></param>
        public void TimePulse(DateTime currentTime) {
            AbstractGuiController.GetController().SetTime(currentTime);
        }

        /// <summary>
        /// Setup default timezone
        /// </summary>
        public void Setup() {
            double localOffset = GetLocalTimeZoneOffset();
            AbstractGuiController.GetController().SetActiveTimeZoneForDisplay(localOffset);
        }

        /// <summary>
        /// Called when the timezone offset changes
        /// </summary>
        /// <param name="offset"></param>
        public void TimeZoneOffsetChanged(double offset) {
            double localOffset = GetLocalTimeZoneOffset();
            AbstractGuiController.GetController().SetActiveTimeZoneForDisplay(offset + localOffset);
            TimeFetcher.setOffset(offset);
        }

        /// <summary>
        /// Calculates and returns the offset in hours from this machine's timezone
        /// </summary>
        /// <returns></returns>
        private double GetLocalTimeZoneOffset() {
            return TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalHours;
        }

        #region UnusedEvents
        public void AlarmCanceled(Alarm alarm, bool fadeIt) {}

        public void AlarmDismissed(bool dueToPreEmpt) {}

        public void AlarmRequested(int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days, string AlarmName) {}

        public void SnoozeRequested() {}

        public void SnoozePeriodChangeRequested(int minutes) {}

        public void ManualTimeRequested(int hours, int minutes) {}

        public void ManualDateRequested(int year, int month, int day) {}

        public void AlarmEdited(Alarm alarm, int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days) {}

        public void MainWindowShutdown() {}

        public void AlarmEdited(Alarm alarm, string name, int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days){}

        #endregion
    }
}
