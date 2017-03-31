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
    public class AlarmListPanel_Controller
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

        #endregion

        public AlarmListPanel_Controller(MainPage mainControl)
        {
            this.mainControl = mainControl;
            this.isPanelOpen = false;                       
        }        

        #region public Panel open/close controls

        /// <summary>
        /// closes options panel 
        /// </summary>
        public void Close() {
            SetPanelState(false);
        }

        /// <summary>
        /// opens option panel
        /// </summary>
        public void Open() {
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
            {
                mainControl.Options_Button.Visibility = Visibility.Collapsed;
                mainControl.addAlarmButton.Visibility = Visibility.Collapsed;
                OpenPanel();
            }
            else
            {
                mainControl.Options_Button.Visibility = Visibility.Visible;
                mainControl.addAlarmButton.Visibility = Visibility.Visible;
                ClosePanel();
            }
            isPanelOpen = isOpen;
        }

        /// <summary>
        /// Opens the Options Panel
        /// </summary>
        private void OpenPanel()
        {
            if (!isPanelOpen)
                mainControl.AlarmListSlideIn.Begin();
        }

        /// <summary>
        /// Closes the Options panel
        /// </summary>
        private void ClosePanel()
        {
            if (isPanelOpen)
                mainControl.AlarmListSlideOut.Begin();
        }

        #endregion        		    

    }
}










































