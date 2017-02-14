using seng403alarmclock.GUI;
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
using System.Windows.Shapes;

namespace seng403alarmclock {
    /// <summary>
    /// Interaction logic for Controls.xaml
    /// </summary>
    public partial class Controls : Window {
        private static double borderOffset = 20;

        public Controls(double LeftOffset, double TopOffset, double height, double width) {
            InitializeComponent();
            this.Left = LeftOffset + borderOffset;
            this.Top = TopOffset + borderOffset;
            this.Width = width;
            this.Height = height;

            this.AddAlarm.Click += AddAlarmClick;
            this.hours.GotKeyboardFocus += Hours_GotKeyboardFocus;
            this.minutes.GotKeyboardFocus += Minutes_GotKeyboardFocus;

        }

        /// <summary>
        /// Called when the user selects the textbox, clears the old content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minutes_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            this.minutes.Text = "";
        }

        /// <summary>
        /// Called when the user selects the textbox, clears the old content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hours_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            this.hours.Text = "";
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

                if (hours < 0 || hours > 23) {
                    return;
                }

                if (minutes < 0 || minutes > 59) {
                    return;
                }

                GuiEventCaller.GetCaller().NotifyAlarmRequested(hours, minutes);
                this.Close();
            } catch (FormatException ex) {
                //no handling enabled right now
            }
        }
    }
}
