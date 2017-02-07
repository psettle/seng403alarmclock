using System;
using seng403alarmclock.GUI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seng403alarmclock {
    class TimeController : TimeListener {
        public void TimePulse(DateTime currentTime) {
            GuiController.GetController().SetTime(currentTime);
        }
    }
}
