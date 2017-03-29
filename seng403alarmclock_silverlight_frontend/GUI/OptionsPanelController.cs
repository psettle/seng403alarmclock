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
    public class OptionsPanel_Controller
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

        #endregion

        public OptionsPanel_Controller(MainPage mainControl)
        {
            this.mainControl = mainControl;
            this.weekdayControl = new WeekdaySelector(mainControl);
            this.isPanelOpen = false;
            snooze_duration_minutes = 1;
            SetSnoozePeriodMinutes(snooze_duration_minutes);
            this.mainControl.sDuration_Label.Content = snooze_duration_minutes.ToString();

            mainControl.sDuration_dec.Click += SDuration_MinuteDown_Click;
            mainControl.sDuration_inc.Click += SDuration_MinuteUp_Click;
            mainControl.cdDatePicker.SelectedDateChanged += CdDatePicker_SelectedDateChanged;
            weekdayControl.SetVisibleState(Visibility.Collapsed);          
        }

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

        #region Snooze Duration
        private void SDuration_MinuteUp_Click(object sender, RoutedEventArgs e)
        {
            if (snooze_duration_minutes < 59)
                snooze_duration_minutes++;
            this.mainControl.sDuration_Label.Content = snooze_duration_minutes.ToString();
            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_duration_minutes);
        }

        private void SDuration_MinuteDown_Click(object sender, RoutedEventArgs e)
        {
            if (snooze_duration_minutes > 0 )
                snooze_duration_minutes--;
            this.mainControl.sDuration_Label.Content = snooze_duration_minutes.ToString();
            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_duration_minutes);
        }

        public static void SetSnoozePeriodMinutes(int minutes)
        {
            snooze_duration_minutes = minutes;
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
    }
}
