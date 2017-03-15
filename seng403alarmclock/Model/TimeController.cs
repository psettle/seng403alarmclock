using System;
using System.Collections.Generic;
using seng403alarmclock.GUI;

namespace seng403alarmclock.Model {
    /// <summary>
    /// Basic controller for passes the time of day to the gui
    /// </summary>
    class TimeController : TimeListener, GuiEventListener {
        /// <summary>
        /// Called whenever a time pulse occurs
        /// </summary>
        /// <param name="currentTime"></param>
        public void TimePulse(DateTime currentTime) {
            GuiController.GetController().SetTime(currentTime);
        }

        /// <summary>
        /// Setup default timezone
        /// </summary>
        public void Setup() {
            double localOffset = GetLocalTimeZoneOffset();
            GuiController.GetController().SetActiveTimeZoneForDisplay(localOffset);
        }

        /// <summary>
        /// Called when the timezone offset changes
        /// </summary>
        /// <param name="offset"></param>
        public void TimeZoneOffsetChanged(double offset) {
            double localOffset = GetLocalTimeZoneOffset();
            GuiController.GetController().SetActiveTimeZoneForDisplay(offset + localOffset);
            TimeFetcher.setOffset(offset);
        }

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

        #endregion
    }
}
