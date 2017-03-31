using seng403alarmclock.GUI;
using seng403alarmclock.GUI_Interfaces;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace seng403alarmclock_silverlight_frontend.GUI
{
    /// <summary>
    /// This class controls the sliding options panel
    /// </summary>
    public class OptionsPanel_Controller : TimeSelectorI
    {
        #region Attributes
        /// <summary>
        /// The current state of the Options Panel
        /// </summary>
        private bool isPanelOpen = false;

        /// <summary>
        /// A reference to the main window, used to control the main window
        /// </summary>
        private MainPage mainControl = null;

        /// <summary>
        /// Controls the weekday selector on the Options page
        /// </summary>
        private WeekdaySelector weekdayControl = null;

        /// <summary>
        /// Controls the time selector on the Options page
        /// </summary>
        private TimeSelector timeController = null;

        /// <summary>
        /// tracks snooze duration. The alarm controller is informed if it changes
        /// </summary>
        private static int snooze_duration_minutes = 1;
		
		/// <summary>
        /// helps make the timezone selector gud. Not sure how yet...
        /// </summary>
		private DropdownSelectorController dropdownSelectorController = null;
		
		/// <summary>
        /// represents current timezone offset
        /// </summary>
		public static double timezoneOffsetHours { set; get; } = 0;


        #endregion

        public OptionsPanel_Controller(MainPage mainControl)
        {
            this.mainControl = mainControl;
            this.weekdayControl = new WeekdaySelector(mainControl);
            this.isPanelOpen = false;
            snooze_duration_minutes = 1;
            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_duration_minutes);
            this.mainControl.sDuration_Label.Content = snooze_duration_minutes.ToString();
			this.timeController = new TimeSelector(this, true);
			this.dropdownSelectorController = new DropdownSelectorController(mainControl.timezoneComboBox);
			
			InitDefaultTimeZone();

            mainControl.sDuration_dec.Click += SDuration_MinuteDown_Click;
            mainControl.sDuration_inc.Click += SDuration_MinuteUp_Click;
            mainControl.cdDatePicker.SelectedDateChanged += CdDatePicker_SelectedDateChanged;
            weekdayControl.SetVisibleState(Visibility.Collapsed);

            mainControl.timezoneComboBox.SelectionChanged += Timezone_SelectionChanged;

            mainControl.AnalogButton.Click += Analog_Click;
            mainControl.DigitalButton.Click += Digital_Click;

            new DarkButton(mainControl.sDuration_dec);
            new DarkButton(mainControl.sDuration_inc);

            new DarkButton(mainControl.AnalogButton);
            new DarkButton(mainControl.DigitalButton);
        }
		
		#region Custom Date 
		
		//note: currently not working in silverlight, nor in desktop
		
        /// <summary>
        /// modifies internal date representation according to selection.
        /// 99% copied from seng403alarmclock_desktop_frontend.OptionsWindow.xaml.cs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CdDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {        
            string newDate = this.mainControl.cdDatePicker.Text;

            //this string should be in the format yyyy-mm-dd

            string[] yearMonthDay = newDate.Split('-');

            int year, month, day;

            try
            {
                year = int.Parse(yearMonthDay[0]);
                month = int.Parse(yearMonthDay[1]);
                day = int.Parse(yearMonthDay[2]);
            }
            catch (FormatException)
            {
                return; //the date was picked wrong in the case of both exceptions, don't pass the new data onwards
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }

            GuiEventCaller.GetCaller().NotifyManualDateRequested(year, month, day);
        }

		#endregion
		
		#region custom Time
		

		
		/// <summary>
        /// when options panel is opened, it calles GUIController, who sets this in order to populate the custom time UI with the current time.
        /// </summary>
		public void SetCustomTime_displayedInOptions(int hour, int minute){
			timeController.SetDisplayTime(hour, minute);			
		}
				
		#endregion
		
		#region Timezones

		/// <summary>
		/// When the timezone combobox is changed, this is called to update the timezone via changing the offset.
		/// 99% copied from desktop version
		/// </summary>
		private void Timezone_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBoxItem timeZone = (ComboBoxItem)mainControl.timezoneComboBox.SelectedItem;

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

		/// <summary>
		/// Initializes the timezone to default value
		/// 99% copied from desktop version
		/// </summary>
		private void InitDefaultTimeZone() {
			
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
            
            foreach(ComboBoxItem entry in mainControl.timezoneComboBox.Items) {
                string entryVal = entry.Content.ToString();

                //parse the entry value to get the time

                string[] splitEntry = entryVal.Split(' ');

                if(splitEntry[1] == timeZoneString) {
                    mainControl.timezoneComboBox.SelectedItem = entry;
                }
            }
        }

		#endregion
		
        #region Snooze Duration
		/// <summary>
		/// increases snooze duration by one minute
		/// </summary>
        private void SDuration_MinuteUp_Click(object sender, RoutedEventArgs e)
        {
            if (snooze_duration_minutes < 59)
                snooze_duration_minutes++;
            this.mainControl.sDuration_Label.Content = snooze_duration_minutes.ToString();
            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_duration_minutes);
        }
		
		/// <summary>
		/// decreases snooze duration by 1 minute
		/// </summary>
        private void SDuration_MinuteDown_Click(object sender, RoutedEventArgs e)
        {
            if (snooze_duration_minutes > 0 )
                snooze_duration_minutes--;
            this.mainControl.sDuration_Label.Content = snooze_duration_minutes.ToString();
            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_duration_minutes);
        }

        #endregion

        #region public Panel open/close controls

        /// <summary>
        /// closes options panel 
        /// </summary>
        public void CloseOptionsPanel() {
            SetPanelState(false);
        }

        /// <summary>
        /// opens option panel
        /// </summary>
        public void OpenOptionsPanel() {
            SetPanelState(true);
        }

        #endregion

        #region private Panel Controls

        /// <summary>
        /// Sets the options panel state
        /// </summary>
        /// <param name="isOpen">
        /// The desired state of the panel, true for open
        /// </param>
        private void SetPanelState(bool isOpen)
        {
            if (isOpen)
                OpenPanel();
            else
                ClosePanel();

            isPanelOpen = isOpen;
        }

        /// <summary>
        /// Opens the Options Panel
        /// </summary>
        private void OpenPanel()
        {
            if (!isPanelOpen)
                mainControl.OptionsSlideIn.Begin();
        }

        /// <summary>
        /// Closes the Options panel
        /// </summary>
        private void ClosePanel()
        {
            if (isPanelOpen)
                mainControl.OptionsSlideOut.Begin();
        }

        #endregion        
		
		#region TimeSelectorI Interface
		
		/// <summary>
        /// Gets an hour up button
        /// </summary>
        RepeatButton TimeSelectorI.GetHourUpButton(){
			return mainControl.ctHourUp;
		}

        /// <summary>
        /// Gets an hour down button
        /// </summary>
        RepeatButton TimeSelectorI.GetHourDownButton(){
			return mainControl.ctHourDown;
		}

        /// <summary>
        /// Gets a minute up button
        /// </summary>
        RepeatButton TimeSelectorI.GetMinuteUpButton(){
			return mainControl.ctMinuteUp;
		}

        /// <summary>
        /// Gets a minute down button
        /// </summary>
        RepeatButton TimeSelectorI.GetMinuteDownButton(){
			return mainControl.ctMinuteDown;
		}

        /// <summary>
        /// Gets a AMPM button
        /// </summary>
        Button TimeSelectorI.GetAMPMButton(){
			return mainControl.ctAMPM;
		}

        /// <summary>
        /// Gets the textbox for writing hours into
        /// </summary>
        TextBox TimeSelectorI.GetHourInput(){
			return mainControl.ctHourInput;
		}

        /// <summary>
        /// Gets the textbox for writing minutes into
        /// </summary>
        TextBox TimeSelectorI.GetMinuteInput(){
			return mainControl.ctMinuteInput;
		}

        /// <summary>
        /// Called when the time is updated
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        void TimeSelectorI.OnTimeUpdated(int hour, int minute) { 
            GuiEventCaller.GetCaller().NotifyManualTimeRequested(hour, minute);
        }

        #endregion


        private void Digital_Click(object sender, System.Windows.RoutedEventArgs e) {
            GuiController.GetController().SetDisplayMode(false);
        }

        private void Analog_Click(object sender, System.Windows.RoutedEventArgs e) {
            GuiController.GetController().SetDisplayMode(true);
        }
    }
}










































