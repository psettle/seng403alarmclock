using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace seng403alarmclock.GUI {
    /// <summary>
    /// This class manages the time selector widget that is used in a couple places in the app
    /// </summary>
    class TimeSelector {
        private TimeSelectorWindow window = null;

        /// <summary>
        /// Indicates if the currently entered time is PM, the current time is AM if this is false
        /// </summary>
        private bool pm = false;

        /// <summary>
        /// Attaches the click handlers to the provided window
        /// </summary>
        /// <param name="window"></param>
        public TimeSelector(TimeSelectorWindow window) {
            this.window = window;
            window.AMPM.Click += AMPM_Click;

            window.hours.GotKeyboardFocus += Hours_GotKeyboardFocus;
            window.minutes.GotKeyboardFocus += Minutes_GotKeyboardFocus;

            window.hours.LostKeyboardFocus += HoursMinutes_LostKeyboardFocus;
            window.minutes.LostKeyboardFocus += HoursMinutes_LostKeyboardFocus;

            window.hrUp.Click += IncreaseHoursClick;
            window.hrDown.Click += DecreaseHoursClick;
            window.minUp.Click += IncreaseMinutesClick;
            window.minDown.Click += DecreaseMinutesClick;
        }

        

        /// <summary>
        /// Sets the time to display on the time selector
        /// </summary>
        /// <param name="hours">0-23</param>
        /// <param name="minutes">0-59</param>
        public void SetTime(int hours, int minutes) {
            if(hours == 0) {
                hours = 12;
                pm = false;
            } else if(hours >= 12) {
                pm = true;

                if (hours > 12) {
                    hours -= 12;
                }
                
            } else {
                pm = false;
            }

            window.hours.Text = hours.ToString();
            window.minutes.Text = minutes.ToString();

            if (window.hours.Text.Length < 2) window.hours.Text = "0" + window.hours.Text;
            if (window.minutes.Text.Length < 2) window.minutes.Text = "0" + window.minutes.Text;

            flipAMPM(); flipAMPM(); //flip it twice so the GUI updates (kind of hack-y I know)
        }

        /// <summary>
        /// Attempts to parse out the minutes and hours
        /// </summary>
        /// <param name="hours">The hours, on a scale from 0 to 23</param>
        /// <param name="minutes"> The minutes on a scale from 0 to 59</param>
        /// <exception cref="FormatException">
        /// If the user input cannot be converted to integers
        /// </exception>
        public void GetTime(out int hours, out int minutes) {
            hours = int.Parse(window.hours.Text);
            minutes = int.Parse(window.minutes.Text);

            if (pm && hours != 12) {
                hours += 12;
            } else if (!pm && hours == 12) {
                hours = 0;
            }
        }

        #region GUIControllingMethods


        /// <summary>
        /// Called when the user clicks on the AM/PM button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AMPM_Click(object sender, RoutedEventArgs e) {
            flipAMPM();
            UpdateListeners();
        }


        private void flipAMPM() {
            pm = !pm;
            if (pm) {
                window.AMPM.Content = "PM";
            } else {
                window.AMPM.Content = "AM";
            }
        }

        /// <summary>
        /// Called when the user selects the textbox, clears the old content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minutes_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            window.minutes.Text = "";
        }

        /// <summary>
        /// Called when the user selects the textbox, clears the old content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hours_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            window.hours.Text = "";
        }

        /// <summary>
        /// Called when the user finishes updating the value in a textfield
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HoursMinutes_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            UpdateListeners();
        }

        private void IncreaseMinutesClick(object sender, RoutedEventArgs e) {
            if (window.minutes.Text == "MM" || window.minutes.Text == "") {
                window.minutes.Text = "00";
                window.hours.Text = "01";
            }
            int minutes = int.Parse(window.minutes.Text);
            int newTime;
            if (minutes >= 59) {
                newTime = 0;
            } else {
                newTime = minutes + 1;
            }
            if (newTime < 10) { window.minutes.Text = "0" + newTime.ToString(); } else { window.minutes.Text = newTime.ToString(); }
            UpdateListeners();
        }

        private void DecreaseMinutesClick(object sender, RoutedEventArgs e) {
            if (window.minutes.Text == "MM" || window.minutes.Text == "") {
                window.minutes.Text = "00";
                window.hours.Text = "01";
            }
            int minutes = int.Parse(window.minutes.Text);
            int newTime;
            if (minutes <= 0) {
                newTime = 59;
            } else {
                newTime = minutes - 1;
            }
            if (newTime < 10) { window.minutes.Text = "0" + newTime.ToString(); } else { window.minutes.Text = newTime.ToString(); }
            UpdateListeners();
        }

        private void IncreaseHoursClick(object sender, RoutedEventArgs e) {
            if (window.minutes.Text == "MM" || window.hours.Text == "") {
                window.minutes.Text = "00";
                window.hours.Text = "01";
            }
            int hours = int.Parse(window.hours.Text);
            int newTime;
            if (hours >= 12) {
                newTime = 1;   
            } else {
                newTime = hours + 1;
            }

            if(newTime == 12) {
                flipAMPM();
            }
            if (newTime < 10) { window.hours.Text = "0" + newTime.ToString(); } else { window.hours.Text = newTime.ToString(); }
            UpdateListeners();
        }

        private void DecreaseHoursClick(object sender, RoutedEventArgs e) {
            if (window.minutes.Text == "MM" || window.hours.Text == "") {
                window.minutes.Text = "00";
                window.hours.Text = "01";
            }
            int hours = int.Parse(window.hours.Text);
            int newTime;
            if (hours <= 1) {
                newTime = 12;    
            } else {
                newTime = hours - 1;
            }

            if(newTime == 11) {
                flipAMPM();
            }
            if (newTime < 10) { window.hours.Text = "0" + newTime.ToString(); } else { window.hours.Text = newTime.ToString(); }
            UpdateListeners();
        }

        #endregion

        #region ListenerPatternMethods

        /// <summary>
        /// the list of listeners
        /// </summary>
        private List<TimeSelectorListener> listeners = new List<TimeSelectorListener>();

        /// <summary>
        /// Registers a new listener
        /// </summary>
        /// <param name="listener"></param>
        public void Add(TimeSelectorListener listener) {
            listeners.Add(listener);
        }

        /// <summary>
        /// Sends an update to all listeners
        /// </summary>
        /// <param name="newHours"></param>
        /// <param name="newMinutes"></param>
        private void NotifyTimeChanged(int newHours, int newMinutes) {
            foreach(TimeSelectorListener listener in listeners) {
                listener.TimeChanged(newHours, newMinutes);
            }
        }

        /// <summary>
        /// Parses and sends the current time on the display to the listeners
        /// </summary>
        private void UpdateListeners() {
            int hours, minutes;

            try {
                GetTime(out hours, out minutes);
            } catch (FormatException) {
                return;
            }

            NotifyTimeChanged(hours, minutes);
        }



        #endregion

    }

    interface TimeSelectorListener {
        /// <summary>
        /// Called if the input value on the time selector widget is changed
        /// </summary>
        /// <param name="newHours"></param>
        /// <param name="newMinutes"></param>
        void TimeChanged(int newHours, int newMinutes);
    }

    interface TimeSelectorWindow {
        Button AMPM { get; set; }
        TextBox hours { get; set; }
        TextBox minutes { get; set; }
        RepeatButton hrUp { get; set; }
        RepeatButton minUp { get; set; }
        RepeatButton minDown { get; set; }
        RepeatButton hrDown { get; set; }
    }
}
