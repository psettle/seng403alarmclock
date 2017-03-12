using seng403alarmclock.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace seng403alarmclock
{
    /// <summary>
    /// Interaction logic for Controls.xaml
    /// </summary>
    public partial class EditAlarmWindow : Window, TimeSelectorWindow
    {
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

        private TimeSelector timeSelector = null;

        private Alarm alarm = null;

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

        #region TimeSelectorWindowInterfaceMembers

        Button TimeSelectorWindow.AMPM
        {
            get { return AMPM; }
            set { AMPM = value; }
        }

        TextBox TimeSelectorWindow.hours
        {
            get { return hours; }
            set { hours = value; }
        }

        TextBox TimeSelectorWindow.minutes
        {
            get { return minutes; }
            set { minutes = value; }
        }

        RepeatButton TimeSelectorWindow.hrUp
        {
            get { return hrUp; }
            set { hrUp = value; }
        }
        RepeatButton TimeSelectorWindow.minUp
        {
            get { return minUp; }
            set { minUp = value; }
        }
        RepeatButton TimeSelectorWindow.minDown
        {
            get { return minDown; }
            set { minDown = value; }
        }
        RepeatButton TimeSelectorWindow.hrDown
        {
            get { return hrDown; }
            set { hrDown = value; }
        }

        #endregion

        /// <summary>
        /// Builds the window, moves it into position and assigns click listeners
        /// </summary>
        /// <param name="LeftOffset"></param>
        /// <param name="TopOffset"></param>
        /// <param name="mainWindowHeight">The height of the window that creates this one</param>
        public EditAlarmWindow(double LeftOffset, double TopOffset, double mainWindowHeight, Alarm alarm)
        {
            InitializeComponent();
            this.Left = LeftOffset + borderOffset;
            this.Top = TopOffset + borderOffset;
            this.Width = mainWindowHeight / 2; //the 2:3 ratio looks nice
            this.Height = mainWindowHeight / 3;
            this.ResizeMode = ResizeMode.CanMinimize;

            this.alarm = alarm;

            this.SaveAlarm.Click += SaveAlarmClick;

            this.Repeats.Click += Weekly_Click;

            //add the handler for each weekday
            this.Sunday.Click += Weekday_Click;
            this.Monday.Click += Weekday_Click;
            this.Tuesday.Click += Weekday_Click;
            this.Wednesday.Click += Weekday_Click;
            this.Thursday.Click += Weekday_Click;
            this.Friday.Click += Weekday_Click;
            this.Saturday.Click += Weekday_Click;

            this.timeSelector = new TimeSelector(this);
            SetUpGUI();
            

            AddAudioFilesNamesToGUI();

        }

        private void SetUpGUI()
        {
            //get the alarm info
            List<DayOfWeek> days = alarm.GetWeekdays();
            bool isRepeating = alarm.IsRepeating;
            bool isWeekly = alarm.IsWeekly;

            //set the gui elements
            timeSelector.SetTime(alarm.GetHour(), alarm.GetMinute());
            if (isRepeating)
                this.Repeats.IsChecked = true;
            if (isWeekly)
            {
                this.Repeats.IsChecked = true;
                this.WeekGrid.Visibility = Visibility.Visible;

                foreach (DayOfWeek day in days)
                {
                    switch (day)
                    {
                        case DayOfWeek.Sunday:
                            Sunday.Background = Brushes.Turquoise;
                            break;
                        case DayOfWeek.Monday:
                            Monday.Background = Brushes.Turquoise;
                            break;
                        case DayOfWeek.Tuesday:
                            Tuesday.Background = Brushes.Turquoise;
                            break;
                        case DayOfWeek.Wednesday:
                            Wednesday.Background = Brushes.Turquoise;
                            break;
                        case DayOfWeek.Thursday:
                            Thursday.Background = Brushes.Turquoise;
                            break;
                        case DayOfWeek.Friday:
                            Friday.Background = Brushes.Turquoise;
                            break;
                        case DayOfWeek.Saturday:
                            Saturday.Background = Brushes.Turquoise;
                            break;
                        default:
                            break;


                    }
                    dayStatusCodes[day] = !dayStatusCodes[day];

                }
               
            }
        }

        /// <summary>
        /// Creates the list entries for the audio file names
        /// </summary>
        private void AddAudioFilesNamesToGUI()
        {
            bool firstTimeTrip = false;

            foreach (KeyValuePair<string, string> entry in audioDictionary)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = entry.Value;
                AudioFileNames.Items.Add(item);
                //add the lookup entry to the audio combo box table
                audioComboBoxItems.Add(item, entry.Key);

                //set the default to the first item in the list
                if (!firstTimeTrip)
                {
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
        private void Weekday_Click(object sender, RoutedEventArgs e)
        {
            //get the button that was clicked
            Button clicked = (Button)sender;

            //figure out what day was clicked
            DayOfWeek key;
            switch (clicked.Name)
            {
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
            if (dayStatusCodes[key])
            {
                clicked.Background = Brushes.Turquoise;
            }
            else {
                clicked.Background = Brushes.White;
            }
        }

        /// <summary>
        /// Called when the user clicks on the weekly checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Weekly_Click(object sender, RoutedEventArgs e)
        {
            if (this.Repeats.IsChecked.Value)
            {
                this.WeekGrid.Visibility = Visibility.Visible;
            }
            else {
                this.WeekGrid.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Called when the add alarm button is clicked
        /// </summary>
        /// <param name="sender">?</param>
        /// <param name="e">?</param>
        private void SaveAlarmClick(object sender, RoutedEventArgs e)
        {

            int hours, minutes;

            try
            {
                timeSelector.GetTime(out hours, out minutes);
            }
            catch (FormatException)
            {
                MessageBox.Show("The number of hours or minutes that was input is not a number.");
                return;
            }

            if (hours < 0 || hours > 23)
            {
                MessageBox.Show(this, "The number of hours input is not between 0 and 12");
                return;
            }

            if (minutes < 0 || minutes > 59)
            {
                MessageBox.Show(this, "The number of minutes input is not between 0 and 59");
                return;
            }

            //parse out which days of the week this alarm occurs on
            List<DayOfWeek> alarmDays;
            if (Repeats.IsChecked.Value)
            {
                alarmDays = new List<DayOfWeek>();
                foreach (KeyValuePair<DayOfWeek, bool> entry in dayStatusCodes)
                {
                    if (entry.Value)
                    {
                        alarmDays.Add(entry.Key);
                    }
                }

                if (alarmDays.Count == 0)
                {
                    //if there are no days the alarm cannot go off :/
                    MessageBox.Show(this, "No days were selected for the alarm to go off on.");
                    return;
                }
            }
            else {
                alarmDays = null;
            }
            string audioFile = "";
            try
            {
                audioFile = getSelectedAudioFile();
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show(this, "No alarm tone was set.");
                return;
            }


            //call the modified version...
            GuiEventCaller.GetCaller().NotifyAlarmEditRequest(alarm,hours, minutes, Repeats.IsChecked.Value, audioFile, Repeats.IsChecked.Value, alarmDays);
            Close();
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
        private string getSelectedAudioFile()
        {
            ComboBoxItem selected = (ComboBoxItem)AudioFileNames.SelectedItem;
            string fileName = "";
            if (audioComboBoxItems.TryGetValue(selected, out fileName))
            {
                return fileName; //the keys are the file names associated with the combobox items
            }
            else {
                throw new IndexOutOfRangeException("No tone selected");
            }

        }
    }
}
