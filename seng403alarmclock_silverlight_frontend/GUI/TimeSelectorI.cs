using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace seng403alarmclock_silverlight_frontend.GUI {

    /// <summary>
    /// This is an interface that GUI control classes can implement in order to take advantage of the TimeSelector class
    /// </summary>
    public interface TimeSelectorI {
        /// <summary>
        /// Gets an hour up button
        /// </summary>
        RepeatButton GetHourUpButton();

        /// <summary>
        /// Gets an hour down button
        /// </summary>
        RepeatButton GetHourDownButton();

        /// <summary>
        /// Gets a minute up button
        /// </summary>
        RepeatButton GetMinuteUpButton();

        /// <summary>
        /// Gets a minute down button
        /// </summary>
        RepeatButton GetMinuteDownButton();

        /// <summary>
        /// Gets a AMPM button
        /// </summary>
        Button GetAMPMButton();

        /// <summary>
        /// Gets the textbox for writing hours into
        /// </summary>
        TextBox GetHourInput();

        /// <summary>
        /// Gets the textbox for writing minutes into
        /// </summary>
        TextBox GetMinuteInput();

        /// <summary>
        /// Called when the time is changed
        /// </summary>
        void OnTimeUpdated(int hour, int minute);
    }
}
