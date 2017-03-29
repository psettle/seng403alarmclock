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

        #endregion

        public OptionsPanel_Controller(MainPage mainControl)
        {
            this.mainControl = mainControl;
            timeController = new TimeSelector(this);
            weekdayControl = new WeekdaySelector(mainControl);
            this.isPanelOpen = false;

            weekdayControl.SetVisibleState(Visibility.Collapsed);
        }

        #region public Panel controls

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

        #region TimeSelector Getters NOT READY FOR SERVICE DO NOT USE

        // DO NOT USE IN CURRENT VERSION

        public RepeatButton GetHourUpButton()   {   return mainControl.HourUp; }
        public RepeatButton GetHourDownButton() {   return mainControl.HourDown; }

        public RepeatButton GetMinuteUpButton() {   return mainControl.MinuteUp; }
        public RepeatButton GetMinuteDownButton(){  return mainControl.MinuteDown; }

        public Button GetAMPMButton()           {   return mainControl.AMPM; }

        public TextBox GetHourInput()           {   return mainControl.HourInput; }
        public TextBox GetMinuteInput()         {   return mainControl.MinuteInput; }

        #endregion

    }
}
