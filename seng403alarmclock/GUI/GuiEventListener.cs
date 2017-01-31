using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seng403alarmclock.GUI {
    /// <summary>
    /// This is the public interface for listening to the gui,
    /// new features may be added later (AlarmRequested will definately change)
    /// </summary>
    interface GuiEventListener {
        /// <summary>
        /// Called if the user requests the cancelation of an alarm
        /// </summary>
        /// <param name="alarm">
        /// The alarm that was initially added to the GUI
        /// </param>
        void AlarmCanceled(Alarm alarm);

        /// <summary>
        /// Called if the user requests the dismissal of an active alarm
        /// </summary>
        /// <param name="alarm">
        /// The alarm that was initially added to the GUI
        /// </param>
        void AlarmDismissed(Alarm alarm);

        /// <summary>
        /// Called if the user requests the placement of an alarm
        /// </summary>
        /// <param name="hour">The hour the user wants an alarm at</param>
        /// <param name="minute">The minute the user wants an alarm at</param>
        void AlarmRequested(int hour, int minute);
    }
}
