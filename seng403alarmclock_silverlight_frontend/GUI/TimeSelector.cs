using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace seng403alarmclock_silverlight_frontend.GUI {
    /// <summary>
    /// This class manages the set of UI element for the time selector
    /// </summary>
    public class TimeSelector {
        #region Attributes

        /// <summary>
        /// A reference to an object that will feed buttons and textboxes for this class to control
        /// </summary>
        private TimeSelectorI hostWindow = null;

        /// <summary>
        /// A reference to the hour input button
        /// </summary>
        private TextBox HourInput = null;

        /// <summary>
        /// A reference to the minute input button
        /// </summary>
        private TextBox MinuteInput = null;

        /// <summary>
        /// A reference to the AMPM button
        /// </summary>
        private Button AMPM = null;

        /// <summary>
        /// The current state of the AMPM button, true for PM
        /// </summary>
        private bool pm = false;

        #endregion

        #region Construction

        /// <summary>
        /// Sets up the timeselector for action
        /// </summary>
        /// <param name="hostWindow"></param>
        public TimeSelector(TimeSelectorI hostWindow) {
            this.hostWindow = hostWindow;

            HourInput = hostWindow.GetHourInput();
            MinuteInput = hostWindow.GetMinuteInput();
            AMPM = hostWindow.GetAMPMButton();
            AssignButtonControllers();
            AssignClickListeners(); 
        }

        /// <summary>
        /// Assigns all the click listeners to the time selector elements
        /// </summary>
        private void AssignClickListeners() {
            AMPM.Click += AMPM_Click;
            HourInput.GotFocus += HourInput_GotFocus;
            MinuteInput.GotFocus += MinuteInput_GotFocus;
            HourInput.LostFocus += HourInput_LostFocus;
            MinuteInput.LostFocus += MinuteInput_LostFocus;
            hostWindow.GetHourUpButton().Click += HourUp_Click;
            hostWindow.GetHourDownButton().Click += HourDown_Click;
            hostWindow.GetMinuteUpButton().Click += MinuteUp_Click;
            hostWindow.GetMinuteDownButton().Click += MinuteDown_Click;
        }

        /// <summary>
        /// Assigns the button controlers that execute the dark theme
        /// </summary>
        private void AssignButtonControllers() {
            //iterate over the time UI elements
            foreach (UIElement button in new List<UIElement>() {   
                hostWindow.GetAMPMButton(),
                hostWindow.GetHourUpButton(),
                hostWindow.GetHourDownButton(),
                hostWindow.GetMinuteDownButton(),
                hostWindow.GetMinuteUpButton()
            }) {
                DarkButton tempButton = new DarkButton(button);
                tempButton.SetIdleColors(Colors.DarkGray, Colors.Black);
                tempButton.SetActiveColors(Colors.Black, Colors.DarkGray);
            }
        }

        #endregion

        #region TimeControlFunctions
        /// <summary>
        /// Gets the current display time
        /// </summary>
        /// <param name="hour">out: the hour on the display</param>
        /// <param name="minute">out: the minute on the display</param>
        /// <exception cref="FormatException">
        /// If the current display time is of an invalid format
        /// (will not happen unless the method is called internally)
        /// </exception>
        public void GetDisplayTime(out int hour, out int minute) {
            try {
                hour = int.Parse(HourInput.Text);         
            } catch (Exception) {
                if (HourInput.Text == "") {
                    hour = 0;
                } else {
                    throw new FormatException("Display time was of an invalid format");
                } 
            }

            try {
                minute = int.Parse(MinuteInput.Text);
            } catch (Exception) {
                if (MinuteInput.Text == "") {
                    minute = 0;
                } else {
                    throw new FormatException("Display time was of an invalid format");
                }
            }

            //undo the 0->12 assignment
            if (hour == 12) {
                hour = 0;
            }
            //handle the PM toggle
            if (pm) {
                hour += 12;
            }
        }

        /// <summary>
        /// Sets the display time on the TimeSelector
        /// </summary>
        /// <param name="hour">0-23</param>
        /// <param name="minute">0-59</param>
        public void SetDisplayTime(int hour, int minute) {
             
            if (hour >= 12) {
                hour -= 12;
                SetAMPM(true);
            } else {
                SetAMPM(false);
            }

            if (hour == 0) {
                hour = 12; //instead of saying it 0:30 am we say its 12:30 am, this is how people normally read time
            } 


            HourInput.Text = hour.ToString();
            MinuteInput.Text = minute.ToString();

            ValidateDisplayTime();      
        }

        /// <summary>
        /// Sets the AMPM buttons display value
        /// </summary>
        /// <param name="pm">true for pm</param>
        private void SetAMPM(bool pm) {
            this.pm = pm;

            if (pm) {
                SetAMPMText("PM");
            } else {
                SetAMPMText("AM");
            }
        }

        /// <summary>
        /// Sets the text on the AMPM button
        /// </summary>
        /// <param name="content">the text to set</param>
        private void SetAMPMText(string content) {
            //there are 2 types of button, standard ones,and the fancy dark ones used on the Add/Edit panel, lets check which one we have
            object buttonContent = AMPM.Content;

            if(buttonContent.GetType() == typeof(string)) {
                AMPM.Content = content;
            } else if (buttonContent.GetType() == typeof(Grid)) {
                Grid contentGrid = (Grid)buttonContent;

                //iterate over the grid children to find the label
                foreach(UIElement label in contentGrid.Children) {
                    if(label.GetType() == typeof(Label)) {
                        Label contentLabel = (Label)label;
                        contentLabel.Content = content;
                    }
                }
            }
        }

        /// <summary>
        /// Flips the AM/PM display
        /// </summary>
        private void FlipAMPM() {
            pm = !pm;
            SetAMPM(pm);
        }

        /// <summary>
        /// Adds a number of minutes onto the current display time
        /// </summary>
        /// <param name="minutes">the number of minutes to add, - values are supported</param>
        private void AddToDisplayTime(int minutesToAdd) {
            ValidateDisplayTime();
            //get the current display time
            int hour, minute;
            GetDisplayTime(out hour, out minute);
           
            
            
            //add the required number of minutes
            minute += minutesToAdd;

            //handle minute overflow (this is computationally inefficient, but it doesn't matter for this program
            while(minute < 0) {
                minute += 60;
                hour -= 1;
            }

            while(minute > 59) {
                minute -= 60;
                hour += 1;
            }

            while(hour < 0) {
                hour += 24;
            }

            while(hour > 23) {
                hour -= 24;
            }

            SetDisplayTime(hour, minute);
            
        }

        /// <summary>
        /// Normalizes the currently rendered time, this does things like changing 13:00 am to 1:00 pm, and validating the current value
        /// </summary>
        private void RenderDisplayTime() {
            ValidateDisplayTime();
            int hour, minute;
            GetDisplayTime(out hour, out minute);
            SetDisplayTime(hour, minute);
        }

        /// <summary>
        /// Checks that the time currently being displayed is in a valid range, gives the user an error and fixed the problem if one exists
        /// </summary>
        private void ValidateDisplayTime() {
           
            int hour, minute;
            try {


                try {
                    GetDisplayTime(out hour, out minute);
                } catch (FormatException) {
                    throw new FormatException("The entered display time was invalid, please enter integer values.");
                }
                
                if(hour < 0 || hour > 23) {
                    throw new FormatException("The entered hours were outside of the range 0-11, please try again.");
                }

                if (minute < 0 || minute > 59) {
                    throw new FormatException("The entered minutes were outside of the range 0-59, please try again.");
                }


            } catch (Exception e) {
                //show an error
                MessageBox.Show(e.Message);
                //set the time back to something valid
                SetDisplayTime(0, 0);
            }
        }
        #endregion

        #region ClickListeners

        private void AMPM_Click(object sender, RoutedEventArgs e) {
            FlipAMPM();
        }

        private void MinuteInput_GotFocus(object sender, RoutedEventArgs e) {
            MinuteInput.Text = "";
        }

        private void HourInput_GotFocus(object sender, RoutedEventArgs e) {
            HourInput.Text = "";
        }

        private void MinuteInput_LostFocus(object sender, RoutedEventArgs e) {
            RenderDisplayTime();
        }

        private void HourInput_LostFocus(object sender, RoutedEventArgs e) {
            RenderDisplayTime();
        }

        private void HourDown_Click(object sender, RoutedEventArgs e) {
            AddToDisplayTime(-60);
        }

        private void HourUp_Click(object sender, RoutedEventArgs e) {
            AddToDisplayTime(60);
        }

        private void MinuteDown_Click(object sender, RoutedEventArgs e) {
            AddToDisplayTime(-1);
        }

        private void MinuteUp_Click(object sender, RoutedEventArgs e) {
            AddToDisplayTime(1);
        }

        #endregion

    }
}
