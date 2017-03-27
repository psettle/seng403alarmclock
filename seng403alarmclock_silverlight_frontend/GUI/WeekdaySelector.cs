using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace seng403alarmclock_silverlight_frontend.GUI {

    /// <summary>
    /// This class handles the weekday selector element on the AddEdit alarm page
    /// </summary>
    public class WeekdaySelector {

        private MainPage mainControl = null;

        /// <summary>
        /// The button controllers associated with each day of the week
        /// </summary>
        private Dictionary<DayOfWeek, DarkButton> controllers = null;

        /// <summary>
        /// The buttons associated with each day of the week
        /// </summary>
        private Dictionary<DayOfWeek, Button> buttons = null;

        /// <summary>
        /// The value of each day of the week (true for selected)
        /// </summary>
        private Dictionary<DayOfWeek, bool> buttonValues = null;

        /// <param name="mainControl">
        /// The main control to use when controlling buttons
        /// </param>
        public WeekdaySelector(MainPage mainControl) {
            this.mainControl = mainControl;
            BuildDictionaries();
            AssignListeners();
        }

        /// <summary>
        /// Sets the active days for display
        /// </summary>
        /// <param name="days">
        /// The days that should be active
        /// </param>
        public void SetActiveDays(List<DayOfWeek> days) {
            //set all days to unselected
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek))) {
                SetDayState(day, false);
            }

            //set the active days to selected
            foreach(DayOfWeek day in days) {
                SetDayState(day, true);
            }
       }

        /// <summary>
        /// Gets the days that were selected
        /// </summary>
        /// <returns>
        /// The set of active days
        /// </returns>
        public List<DayOfWeek> GetActiveDays() {
            List<DayOfWeek> toReturn = new List<DayOfWeek>();

            //add all active days to the return list
            foreach (KeyValuePair<DayOfWeek, bool> pair in buttonValues) {
                if(pair.Value) {
                    toReturn.Add(pair.Key);
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Sets the visibility state of the selector
        /// </summary>
        /// <param name="visibilityLevel">
        /// Visibility.Collapsed or
        /// Visibility.Visible
        /// </param>
        public void SetVisibleState(Visibility visibilityLevel) {
            foreach(KeyValuePair<DayOfWeek, Button> button in buttons) {
                button.Value.Visibility = visibilityLevel;
            }
        }

        /// <summary>
        /// Builds the control dictionaries this class uses
        /// </summary>
        private void BuildDictionaries() {
            buttons = new Dictionary<DayOfWeek, Button>();
            controllers = new Dictionary<DayOfWeek, DarkButton>();
            buttonValues = new Dictionary<DayOfWeek, bool>();

            foreach(DayOfWeek day in Enum.GetValues(typeof(DayOfWeek))) {
                buttonValues.Add(day, false);

                Button dayButton = null;


                switch(day) {
                    case DayOfWeek.Sunday:
                        dayButton = mainControl.Sunday;
                        break;
                    case DayOfWeek.Monday:
                        dayButton = mainControl.Monday;
                        break;
                    case DayOfWeek.Tuesday:
                        dayButton = mainControl.Tuesday;
                        break;
                    case DayOfWeek.Wednesday:
                        dayButton = mainControl.Wednesday;
                        break;
                    case DayOfWeek.Thursday:
                        dayButton = mainControl.Thursday;
                        break;
                    case DayOfWeek.Friday:
                        dayButton = mainControl.Friday;
                        break;
                    case DayOfWeek.Saturday:
                        dayButton = mainControl.Saturday;
                        break;
                    default:
                        throw new Exception("Invalid Day Of Week");

                }

                buttons.Add(day, dayButton);


                DarkButton button = new DarkButton(dayButton);

                button.SetIdleColors(Colors.DarkGray, Colors.Black);
                button.SetActiveColors(Colors.Black, Colors.DarkGray);

                controllers.Add(day, button);
            }
        }

        /// <summary>
        /// Adds the click listeners to the weekday buttons
        /// </summary>
        private void AssignListeners() {
            foreach(KeyValuePair<DayOfWeek, Button> pair in buttons) {
                pair.Value.Click += Button_Click;
            }
        }

        /// <summary>
        /// Flips a day state and alters the buttons to match
        /// </summary>
        private void FlipDayState(DayOfWeek target) {
            SetDayState(target, !buttonValues[target]);
        }

        /// <summary>
        /// Sets a day state and alters the buttons to match
        /// </summary>
        /// <param name="selected">true for selected</param>
        private void SetDayState(DayOfWeek target, bool selected) {
            //assign the correct value in selected
            buttonValues[target] = selected;
            //assign the correct colour scheme
            if (buttonValues[target]) {
                controllers[target].SetIdleColors(Colors.Black, Colors.DarkGray);
                controllers[target].SetActiveColors(Colors.DarkGray, Colors.Black);
            } else {
                controllers[target].SetIdleColors(Colors.DarkGray, Colors.Black);
                controllers[target].SetActiveColors(Colors.Black, Colors.DarkGray);
            }
        }

        /// <summary>
        /// Handles the click event on the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e) {
            Button button = (Button)sender;

            DayOfWeek trigger = DayOfWeek.Sunday;

            foreach(KeyValuePair<DayOfWeek, Button> pair in buttons) {
                if(pair.Value == button) {
                    trigger = pair.Key;
                    break;
                }
            }

            FlipDayState(trigger);
        }
    }
}
