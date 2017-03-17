using seng403alarmclock.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seng403alarmclock.GUI_Interfaces {
    public abstract class AbstractGuiController {

        protected static AbstractGuiController guiController = null;

        static public void SetController(AbstractGuiController controller) {
            guiController = controller;
        }

        static public AbstractGuiController GetController() {
            return guiController;
        }

        abstract public void SetTime(DateTime time);
        abstract public void AddAlarm(Alarm alarm);
        abstract public void UpdateAlarm(Alarm alarm);
        abstract public void RemoveAlarm(Alarm alarm, bool wasPreempted);
        abstract public void EditAlarm(Alarm alarm, List<Alarm> allAlarms);

        /// <summary>
        /// Called when the model wants to shut down the program
        /// </summary>
        abstract public void Shutdown();

        abstract public void Snooze_Btn_setVisible();
        abstract public void Snooze_Btn_setHidden();
        abstract public void DismissAll_Btn_setVisible();
        abstract public void Dismiss_Btn_setHidden();

        abstract public void SetActiveTimeZoneForDisplay(double localOffset);
    }
}
