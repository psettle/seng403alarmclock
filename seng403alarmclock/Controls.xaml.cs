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
        /// <summary>
        /// The currently set audio files to display
        /// </summary>
        public static Dictionary<string, string> audioDictionary = new Dictionary<string, string>();

        /// <summary>
        /// A dynamic set of the combo box items to use for looking up audio indexes
        /// </summary>
        private Dictionary<ComboBoxItem, string> audioComboBoxItems = new Dictionary<ComboBoxItem, string>();

        /// <summary>
        /// The offset from the creating windows top and left side
        /// </summary>
        private static double borderOffset = 20;

        /// <summary>
        /// Indicates if the currently entered time is PM, the current time is AM if this is false
        /// </summary>
        private bool pm = false;

        /// <summary>
        /// This is a list of day status codes for the weekly alarms, true means the weekly alarm goes off on the specified day
        /// </summary>
        private Dictionary<DayOfWeek, bool> dayStatusCodes = new Dictionary<DayOfWeek, bool> {
            { DayOfWeek.Sunday, false },
            { DayOfWeek.Monday, false },
            { DayOfWeek.Tuesday, false },
            { DayOfWeek.Wednesday, false },
            { DayOfWeek.Thursday, false },
            { DayOfWeek.Friday, false },
            { DayOfWeek.Saturday, false },
        };

        /// <summary>
        /// Builds the window, moves it into position and assigns click listeners
        /// </summary>
        /// <param name="LeftOffset"></param>
        /// <param name="TopOffset"></param>
        /// <param name="mainWindowHeight">The height of the window that creates this one</param>
        public Controls(double LeftOffset, double TopOffset, double mainWindowHeight) {
            InitializeComponent();
            this.Left = LeftOffset + borderOffset;
            this.Top = TopOffset + borderOffset;
            this.Width = mainWindowHeight / 2; //the 2:3 ratio looks nice
            this.Height = mainWindowHeight / 3;
            this.ResizeMode = ResizeMode.CanMinimize;

            this.AddAlarm.Click += AddAlarmClick;
     

            this.hours.GotKeyboardFocus += Hours_GotKeyboardFocus;
            this.minutes.GotKeyboardFocus += Minutes_GotKeyboardFocus;

            this.AMPM.Click += AMPM_Click;

            this.Weekly.Click += Weekly_Click;

            //add the handler for each weekday
            this.Sunday.Click += Weekday_Click;
            this.Monday.Click += Weekday_Click;
            this.Tuesday.Click += Weekday_Click;
            this.Wednesday.Click += Weekday_Click;
            this.Thursday.Click += Weekday_Click;
            this.Friday.Click += Weekday_Click;
            this.Saturday.Click += Weekday_Click;

            AddAudioFilesNamesToGUI();

        }

        /// <summary>
        /// Creates the list entries for the audio file names
        /// </summary>
        private void AddAudioFilesNamesToGUI() {
            bool firstTimeTrip = false;

            foreach(KeyValuePair<string, string> entry in audioDictionary) {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = entry.Value;
                AudioFileNames.Items.Add(item);
                //add the lookup entry to the audio combo box table
                audioComboBoxItems.Add(item, entry.Key);

                //set the default to the first item in the list
                if (!firstTimeTrip) {
                    AudioFileNames.SelectedValue = item;
                    firstTimeTrip = true;
                }
            }
        }

        /// <summary>
        /// The click handler for when a weekday button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Weekday_Click(object sender, RoutedEventArgs e) {
            //get the button that was clicked
            Button clicked = (Button)sender;

            //figure out what day was clicked
            DayOfWeek key;
            switch(clicked.Name) {
                case "Sunday":
                    key = DayOfWeek.Sunday;
                    break;
                case "Monday":
                    key = DayOfWeek.Monday;
                    break;
                case "Tuesday":
                    key = DayOfWeek.Tuesday;
                    break;
                case "Wednesday":
                    key = DayOfWeek.Wednesday;
                    break;
                case "Thursday":
                    key = DayOfWeek.Thursday;
                    break;
                case "Friday":
                    key = DayOfWeek.Friday;
                    break;
                case "Saturday":
                    key = DayOfWeek.Saturday;
                    break;
                default:
                    throw new Exception("Invalid day code");
            }
            //flip the day's status
            dayStatusCodes[key] = !dayStatusCodes[key];
            //set the color as required
            if(dayStatusCodes[key]) {
                clicked.Background = Brushes.Turquoise;
            } else {
                clicked.Background = Brushes.White;
            }
        }

        /// <summary>
        /// Called when the user clicks on the weekly checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Weekly_Click(object sender, RoutedEventArgs e) {
            if(this.Weekly.IsChecked.Value) {
                this.WeekGrid.Visibility = Visibility.Visible;
            } else {
                this.WeekGrid.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Called when the user clicks on the AM/PM button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AMPM_Click(object sender, RoutedEventArgs e) {
            this.flipAMPM();
        }

        private void flipAMPM() {
            pm = !pm;
            if (pm) {
                this.AMPM.Content = "PM";
            } else {
                this.AMPM.Content = "AM";
            }
        }

        private void IncreaseMinutesClick(object sender, RoutedEventArgs e)
        {
            if (this.minutes.Text == "MM" || this.minutes.Text == "")
            {
                this.minutes.Text = "00";
                this.hours.Text = "01";
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
            if (newTime < 10) { this.minutes.Text = "0" + newTime.ToString(); }
            else { this.minutes.Text = newTime.ToString(); }

        }

        private void DecreaseMinutesClick(object sender, RoutedEventArgs e)
        {
            if (this.minutes.Text == "MM" || this.minutes.Text == "")
            {
                this.minutes.Text = "00";
                this.hours.Text = "01";
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
            if (newTime < 10) { this.minutes.Text = "0" + newTime.ToString(); }
            else { this.minutes.Text = newTime.ToString(); }
        }

        private void IncreaseHoursClick(object sender, RoutedEventArgs e)
        {
            if (this.minutes.Text == "MM" || this.hours.Text == "")
            {
                this.minutes.Text = "00";
                this.hours.Text = "01";
            }
            int hours = int.Parse(this.hours.Text);
            int newTime;
            if (hours >= 12)
            {
                newTime = 1;
                this.flipAMPM();
            }
            else
            {
                newTime = hours + 1;
            }
            if (newTime < 10) { this.hours.Text = "0" + newTime.ToString(); }
            else { this.hours.Text = newTime.ToString(); }
        }

        private void DecreaseHoursClick(object sender, RoutedEventArgs e)
        {
            if (this.minutes.Text == "MM" || this.hours.Text == "")
            {
                this.minutes.Text = "00";
                this.hours.Text = "01";
            }
            int hours = int.Parse(this.hours.Text);
            int newTime;
            if (hours <= 1)
            {
                newTime = 12;
                this.flipAMPM();
            }
            else
            {
                newTime = hours - 1;
            }
            if (newTime < 10) { this.hours.Text = "0" + newTime.ToString(); }
            else { this.hours.Text = newTime.ToString(); }
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


            /*Why is this here?  
            if (hoursStr == "")
            {
                this.hours.Text = "0";
            }
            if (minutesStr == "")
            {
                this.minutes.Text = "0";
            }
            */

            string hoursStr = this.hours.Text;
            string minutesStr = this.minutes.Text;

            int hours;
            int minutes;
            try {
                hours = int.Parse(hoursStr);
                minutes = int.Parse(minutesStr); 
            } catch (FormatException) {
                MessageBox.Show("The number of hours or minutes that was input is not a number.");
                return;
            }

            if (hours < 0 || hours > 12) {
                MessageBox.Show(this, "The number of hours input is not between 0 and 12");
                return;
            }

            if (minutes < 0 || minutes > 59) {
                MessageBox.Show(this, "The number of minutes input is not between 0 and 59");
                return;
            }

            if (this.pm && hours != 12) {
                hours += 12;
            } else if (!this.pm && hours == 12) {
                hours = 0;
            }

            //parse out which days of the week this alarm occurs on
            List<DayOfWeek> alarmDays;
            if (Weekly.IsChecked.Value) {
                alarmDays = new List<DayOfWeek>();
                foreach(KeyValuePair<DayOfWeek, bool> entry in dayStatusCodes) {
                    if(entry.Value) {
                        alarmDays.Add(entry.Key);
                    }
                }   

                if(alarmDays.Count == 0) {
                    //if there are no days the alarm cannot go off :/
                    MessageBox.Show(this, "No days were selected for the alarm to go off on.");
                    return;
                }
            } else {
                alarmDays = null;
            }
            string audioFile = "";
            try {
                audioFile = getSelectedAudioFile();
            } catch (IndexOutOfRangeException) {
                MessageBox.Show(this, "No alarm tone was set.");
                return;
            }
           
 
            //call the modified version...
            GuiEventCaller.GetCaller().NotifyAlarmRequested(hours, minutes, Repeats.IsChecked.Value, audioFile, Weekly.IsChecked.Value, alarmDays);
            this.Close();
        }

        /// <summary>
        /// Trys to get the audio file name to use as the alarm tone
        /// </summary>
        /// <returns>
        /// The selected audio file name
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// If no audio file was selected
        /// </exception>
        private string getSelectedAudioFile() {
            ComboBoxItem selected = (ComboBoxItem)AudioFileNames.SelectedItem;
            string fileName = "";
            if(audioComboBoxItems.TryGetValue(selected, out fileName)) {
                return fileName; //the keys are the file names associated with the combobox items
            } else {
                throw new IndexOutOfRangeException("No tone selected");
            }
            
        }
    }
}
