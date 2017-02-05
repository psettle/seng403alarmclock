using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using seng403alarmclock.GUI;

namespace seng403alarmclock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        /// <summary>
        /// Initializes the main controller and assigns it to the GUI controller
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            GuiController.SetMainWindow(this);
            //note: this should probably be moved to another class, can't be arsed right now
            this.AddAlarm.Click += AddAlarmClick;

            AlarmController ac = new AlarmController();
            GuiEventCaller.GetCaller().AddListener(ac);

            //TEST CODE BELOW THIS LINE      
        }

        /// <summary>
        /// Called when the add alarm button is clicked
        /// </summary>
        /// <param name="sender">?</param>
        /// <param name="e">?</param>
        private void AddAlarmClick(object sender, RoutedEventArgs e) {
            string hoursStr = this.hours.Text;
            string minutesStr = this.minutes.Text;

            try {
                int hours = int.Parse(hoursStr);
                int minutes = int.Parse(minutesStr);

                if(hours < 0 || hours > 23) {
                    return;
                }

                if(minutes < 0 || minutes > 59) {
                    return;
                }

                GuiEventCaller.GetCaller().NotifyAlarmRequested(hours, minutes);
            } catch (FormatException ex) {
                throw ex;
                //no handling enabled right now
            }
        }

        /// <summary>
        /// Sets the text for the time display directly
        /// </summary>
        /// <param name="text">
        /// The text to put onto the GUI
        /// </param>
        public void SetTimeText(string text) {
            this.TimeDisplay.Text = text;
        }

        /// <summary>
        /// Sets the text for the date display directly
        /// </summary>
        /// <param name="text">
        /// The text to put onto the GUI
        /// </param>
        public void SetDateText(string text) {
            this.DateDisplay.Text = text;
        }

        /// <summary>
        /// Graphically add an alarm row to the gui
        /// </summary>
        /// <param name="row">
        /// The alarm row to add to the GUI
        /// </param>
        public void AddAlarmRow(AlarmRow row) {
            row.AddToGUI(this.AlarmPanel);
        }

        /// <summary>
        /// Graphically remove an alarm row from the gui
        /// </summary>
        /// <param name="row">
        /// The row to remove from the GUI
        /// </param>
        public void RemoveAlarmRow(AlarmRow row) {
            row.RemoveFromGUI();
        }
    }
}
