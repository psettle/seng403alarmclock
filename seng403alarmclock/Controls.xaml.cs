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
            //this.hrDown.Click += DecreaseHoursClick;
            //this.minDown.Click += DecreaseMinutesClick;
            //this.hrUp.Click += IncreaseHoursClick;
            //this.minUp.Click += IncreaseMinutesClick;

            this.hours.GotKeyboardFocus += Hours_GotKeyboardFocus;
            this.minutes.GotKeyboardFocus += Minutes_GotKeyboardFocus;

        }

        private void IncreaseMinutesClick(object sender, RoutedEventArgs e)
        {
            if (this.minutes.Text == "MM" || this.minutes.Text == "")
            {
                this.minutes.Text = "0";
                this.hours.Text = "0";
            }
            int minutes = int.Parse(this.minutes.Text);
            int newTime;
            if (minutes >= 59)
            {
                newTime = 0;
            }
            else
            {
                newTime = minutes + 1;
            }
            this.minutes.Text = newTime.ToString();

        }

        private void IncreaseHoursClick(object sender, RoutedEventArgs e)
        {
            if (this.minutes.Text == "MM" || this.hours.Text == "")
            {
                this.minutes.Text = "0";
                this.hours.Text = "0";
            }
            int hours = int.Parse(this.hours.Text);
            int newTime;
            if (hours >= 23)
            {
                newTime = 0;
            }
            else
            {
                newTime = hours + 1;
            }
            this.hours.Text = newTime.ToString();
        }

        private void DecreaseMinutesClick(object sender, RoutedEventArgs e)
        {
            if (this.minutes.Text == "MM" || this.minutes.Text == "")
            {
                this.minutes.Text = "0";
                this.hours.Text = "0";
            }
            int minutes = int.Parse(this.minutes.Text);
            int newTime;
            if (minutes <= 0)
            {
                newTime = 59;
            }
            else
            {
                newTime = minutes - 1;
            }
            this.minutes.Text = newTime.ToString();
        }

        private void DecreaseHoursClick(object sender, RoutedEventArgs e)
        {
            if (this.minutes.Text == "MM" || this.hours.Text == "")
            {
                this.minutes.Text = "0";
                this.hours.Text = "0";
            }
            int hours = int.Parse(this.hours.Text);
            int newTime;
            if (hours <= 0)
            {
                newTime = 23;
            }
            else
            {
                newTime = hours - 1;
            }
            this.hours.Text = newTime.ToString();
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

            if (hoursStr == "")
            {
                this.hours.Text = "0";
            }
            if (minutesStr == "")
            {
                this.minutes.Text = "0";
            }

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
