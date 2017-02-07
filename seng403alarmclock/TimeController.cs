using System;
using seng403alarmclock.GUI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seng403alarmclock {
    /// <summary>
    /// Basic controller for passes the time of day to the gui
    /// </summary>
    class TimeController : TimeListener {
        /// <summary>
        /// Called whenever a time pulse occurs
        /// </summary>
        /// <param name="currentTime"></param>
        public void TimePulse(DateTime currentTime) {
            GuiController.GetController().SetTime(currentTime);
        }
    }
}
