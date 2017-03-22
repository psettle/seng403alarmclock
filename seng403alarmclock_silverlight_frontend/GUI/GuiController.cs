using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using seng403alarmclock.GUI_Interfaces;
using seng403alarmclock.Model;
using seng403alarmclock_silverlight_frontend;

namespace seng403alarmclock.GUI {
    /// <summary>
    /// The methods in AbstractGuiController are triggered by TimeController and AlarmController,
    /// the rest are triggered by other gui components
    /// </summary>
    public class GuiController : AbstractGuiController {
        /// <summary>
        /// A reference to this program's main page
        /// </summary>
        private MainPage mainPage = null;

        /// <summary>
        /// Assigns the main page to the controller
        /// </summary>
        /// <param name="main"></param>
        public void assignMainPage(MainPage main) {
            mainPage = main;
        }

        /// <summary>
        /// Returns the controller instance
        /// </summary>
        /// <returns></returns>
        public static new GuiController GetController() {
            return (GuiController)guiController;
        }

        public override void AddAlarm(Alarm alarm) {

        }

        public override void EditAlarm(Alarm alarm, List<Alarm> allAlarms) {
           
        }

        public override void RemoveAlarm(Alarm alarm, bool wasPreempted) {
            
        }

        public override void SetActiveTimeZoneForDisplay(double localOffset) {
           
        }

        public override void SetDismissAvailable(bool available) {
            
        }

        public override void SetSnoozeAvailable(bool available) {
            
        }

        /// <summary>
        /// Sets the time that is displayed on the GUI
        /// </summary>
        /// <param name="time"></param>
        public override void SetTime(DateTime time) {
            mainPage.SetTime(time);
        }

        public override void Shutdown() {
            
        }

        public override void UpdateAlarm(Alarm alarm) {
            
        }
    }
}
