using seng403alarmclock.GUI;
using seng403alarmclock.GUI_Interfaces;
using seng403alarmclock.Model;
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

namespace seng403alarmclock.GUI
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window, TimeSelectorWindow, TimeSelectorListener {
        private static double relativeSize_ofOptionsWindow;
        private static double borderOffset_width;
        private static double borderOffset_height;

        private static int snooze_period_minutes = 5;

        /// <summary>
        /// The class that controls the time selecting widget on this window
        /// </summary>
        private TimeSelector timeSelector;

        #region TimeSelectorWindowInterfaceMembers

        /// <summary>
        /// The hours to show as default on the timezone dropdown
        /// </summary>
        public static double timezoneOffsetHours { set; get; } = 0;

        Button TimeSelectorWindow.AMPM {
            get { return AMPM; }
            set { AMPM = value; }
        }

        TextBox TimeSelectorWindow.hours {
            get { return hours; }
            set { hours = value; }
        }

        TextBox TimeSelectorWindow.minutes {
            get { return minutes; }
            set { minutes = value; }
        }

        RepeatButton TimeSelectorWindow.hrUp {
            get { return hrUp; }
            set { hrUp = value; }
        }
        RepeatButton TimeSelectorWindow.minUp {
            get { return minUp; }
            set { minUp = value; }
        }
        RepeatButton TimeSelectorWindow.minDown {
            get { return minDown; }
            set { minDown = value; }
        }
        RepeatButton TimeSelectorWindow.hrDown {
            get { return hrDown; }
            set { hrDown = value; }
        }

        #endregion

        public OptionsWindow(double leftOffset, double topOffset, double heightOfMain, double widthOfMain,  double relativeSize)
        {
            InitializeComponent();
            relativeSize_ofOptionsWindow = relativeSize;

            borderOffset_width = widthOfMain - (widthOfMain * relativeSize_ofOptionsWindow);
            borderOffset_width *= 0.5;

            borderOffset_height = heightOfMain - (heightOfMain * relativeSize_ofOptionsWindow);
            borderOffset_height *= 0.5;

            this.Left   = leftOffset + borderOffset_width;
            this.Top    = topOffset + borderOffset_height;
            this.Width  = widthOfMain * relativeSize_ofOptionsWindow;
            this.Height = heightOfMain * relativeSize_ofOptionsWindow;
            
            this.Snooze_Period_minutes_Label.Content = snooze_period_minutes.ToString();
            
            this.snooze_Minus.Visibility                    = Visibility.Visible;
            this.snooze_Plus.Visibility                     = Visibility.Visible;
            this.Snooze_Period_minutes_Label.Visibility     = Visibility.Visible;
                
            this.snooze_Minus.Click             += Snooze_Minus_Click;
            this.snooze_Plus.Click              += Snooze_Plus_Click;


            this.timeSelector = new TimeSelector(this);
            timeSelector.Add(this);


            DatePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;  

            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_period_minutes);

            SetDefaultTimeZone();

            this.analog.Click += Analog_Click;
            this.digital.Click += Digital_Click;
        }

        private void SetDefaultTimeZone() {
            //parse the timezone offset into a +- string for comparison

            int hourOffset = (int)Math.Floor(timezoneOffsetHours);
            double bonusFraction = Math.Abs(timezoneOffsetHours) - Math.Abs(hourOffset);

            string timeZoneString = Math.Sign(hourOffset) == 1 ? "+" : "-";
            timeZoneString += Math.Abs(hourOffset) + ":";


            if (bonusFraction < 0.1) {
                timeZoneString += "00";
            } else if(bonusFraction < 0.3) {
                timeZoneString += "15";
            } else if(bonusFraction < 0.6) {
                timeZoneString += "30";
            } else if(bonusFraction < 0.9) {
                timeZoneString += "45";
            }
            
            foreach(ComboBoxItem entry in Timezone.Items) {
                string entryVal = entry.Content.ToString();

                //parse the entry value to get the time

                string[] splitEntry = entryVal.Split(' ');

                if(splitEntry[1] == timeZoneString) {
                    Timezone.SelectedItem = entry;
                }
            }
        }

        private void Digital_Click(object sender, RoutedEventArgs e) {
            GuiController.GetController().SetDisplayMode(false);
        }

        private void Analog_Click(object sender, RoutedEventArgs e) {
            GuiController.GetController().SetDisplayMode(true);
        }

        private void Snooze_Plus_Click(object sender, RoutedEventArgs e)
        {
            if(snooze_period_minutes < 59)
                snooze_period_minutes++;
            Snooze_Period_minutes_Label.Content = snooze_period_minutes.ToString();
            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_period_minutes);        }

        private void Snooze_Minus_Click(object sender, RoutedEventArgs e)
        {
            if(snooze_period_minutes > 0)
                snooze_period_minutes--;
            Snooze_Period_minutes_Label.Content = snooze_period_minutes.ToString();
            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_period_minutes);
        } 
        
        public static void SetSnoozePeriodMinutes(int minutes) {
            snooze_period_minutes = minutes;
            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_period_minutes);
        }

        /// <summary>
        /// Called whenever the user changes the time on the time display
        /// </summary>
        /// <param name="newHours"></param>
        /// <param name="newMinutes"></param>
        public void TimeChanged(int newHours, int newMinutes) {
            GuiEventCaller.GetCaller().NotifyManualTimeRequested(newHours, newMinutes);
        }

        /// <summary>
        /// Called if the user changes the selected date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
            string newDate = this.DatePicker.Text;

            //this string should be in the format yyyy-mm-dd

            string[] yearMonthDay = newDate.Split('-');

            int year, month, day;

            try {
                year = int.Parse(yearMonthDay[0]);
                month = int.Parse(yearMonthDay[1]);
                day = int.Parse(yearMonthDay[2]);
            } catch (FormatException) {
                return; //the date was picked wrong in the case of both exceptions, don't pass the new data onwards
            } catch (IndexOutOfRangeException) {
                return;
            }
            

            GuiEventCaller.GetCaller().NotifyManualDateRequested(year, month, day);
        }


        public void SetTime(int hour, int minute) {
            timeSelector.SetTime(hour, minute);
        }
  
        private void Timezone_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBoxItem timeZone = (ComboBoxItem)Timezone.SelectedItem;

            string utcString = timeZone.Content.ToString();

            double offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalHours;

            string[] parts = utcString.Split(' ', ':');

            double offsetNum = double.Parse(parts[1]);


            if ((parts[1] == "0"))
            {
                offsetNum = 0;
            }
            if ((parts[2] == "15") && (parts[1].Contains("-")))
            {
                offsetNum = offsetNum - 0.25;
            }

            if ((parts[2] == "15") && (parts[1].Contains("+")))
            {
                offsetNum = offsetNum + 0.25;
            }

            else if ((parts[2] == "30") && (parts[1].Contains("-")))
            {
                offsetNum = offsetNum - 0.5;
            }

            else if ((parts[2] == "30") && (parts[1].Contains("+")))
            {
                offsetNum = offsetNum + 0.5;
            }

            else if ((parts[2] == "45") && (parts[1].Contains("-")))
            {
                offsetNum = offsetNum - 0.75;
            }

            else if ((parts[2] == "45") && (parts[1].Contains("+")))
            {
                offsetNum = offsetNum + 0.75;
            }

            double finalOffset = offsetNum - offset;

            GuiEventCaller.GetCaller().NotifyTimeZoneOffsetChanged(finalOffset);
        }
    }
}
