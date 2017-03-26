using seng403alarmclock.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.Windows.Controls.Primitives;

namespace seng403alarmclock_silverlight_frontend.GUI {
    /// <summary>
    /// This class contols the add/edit alarm panel
    /// </summary>
    public class AddEditWindow : TimeSelectorI {

        #region Attributes
        /// <summary>
        /// The current state of the Add/Edit Panel
        /// </summary>
        private bool isPanelOpen = false;

        /// <summary>
        /// A reference to the main window, used to control the main window
        /// </summary>
        private MainPage mainControl = null;

        /// <summary>
        /// A reference to the alarm that is being edited
        /// </summary>
        private Alarm alarmBeingEdited = null;
        #endregion

        public AddEditWindow(MainPage mainControl) {
            this.mainControl = mainControl;

            mainControl.SaveAlarm.Click += SaveAlarm_Click;
            mainControl.CancelAddEdit.Click += CancelAddEdit_Click;

            new DarkButton(mainControl.SaveAlarm);
            new DarkButton(mainControl.CancelAddEdit);

            new TimeSelector(this);
            
        }

        #region GuiEventHandlers
        /// <summary>
        /// Called when the user clicks the save button
        /// </summary>
        private void SaveAlarm_Click(object sender, RoutedEventArgs e) {
            //collect form info
            //pass to alarm GuiEventCaller

            SetPanelState(false);
        }

        /// <summary>
        /// Called when the user clicks the cancel button on the panel
        /// </summary>
        private void CancelAddEdit_Click(object sender, RoutedEventArgs e) {
            SetPanelState(false);
        }
        #endregion

        /// <summary>
        /// Opens the panel in a blank state, ready to input a new alarm
        /// </summary>
        public void OpenAddAlarmPanel() {
            SetPanelState(true);
        }

        /// <summary>
        /// Opens the panel preloaded with alarm info, ready to save
        /// </summary>
        /// <param name="alarmToEdit">
        /// The alarm to load defaults from and to update when to operation is complete
        /// </param>
        public void OpenEditAlarmPanel(Alarm alarmToEdit) {
            alarmBeingEdited = alarmToEdit;
            PopulateEditAlarmFields(alarmToEdit);
            SetPanelState(true);
        }

        private void PopulateEditAlarmFields(Alarm alarmToEdit) {

        }

        #region RawPanelControl

        /// <summary>
        /// Sets the add edit panel state
        /// </summary>
        /// <param name="isOpen">
        /// The desired state of the panel, true for open
        /// </param>
        private void SetPanelState(bool isOpen) {
            if(isOpen) {
                OpenPanel();
            } else {
                ClosePanel();
            }

            isPanelOpen = isOpen;
        }

        /// <summary>
        /// Opens the Add/Edit Panel
        /// </summary>
        private void OpenPanel() {
            if (!isPanelOpen) {
                mainControl.AddEditSlideIn.Begin();
            }
        }

        /// <summary>
        /// Closes the Add/Edit panel
        /// </summary>
        private void ClosePanel() {
            if(isPanelOpen) {
                mainControl.AddEditSlideOut.Begin();
            }
        }

        #endregion

        #region TimeSelectorGetters

        public RepeatButton GetHourUpButton() {
            return mainControl.HourUp;
        }

        public RepeatButton GetHourDownButton() {
            return mainControl.HourDown;
        }

        public RepeatButton GetMinuteUpButton() {
            return mainControl.MinuteUp;
        }

        public RepeatButton GetMinuteDownButton() {
            return mainControl.MinuteDown;
        }

        public Button GetAMPMButton() {
            return mainControl.AMPM;
        }

        public int GetDefaultHours() {
            if (alarmBeingEdited != null) {
                DateTime alarmTime = alarmBeingEdited.GetAlarmTime();

                return alarmTime.Hour;

            } else {
                return 0;
            }
        }

        public int GetDefaultMinutes() {
            if (alarmBeingEdited != null) {
                DateTime alarmTime = alarmBeingEdited.GetAlarmTime();

                return alarmTime.Minute;

            } else {
                return 0;
            }
        }

        public TextBox GetHourInput() {
            return mainControl.HourInput;
        }

        public TextBox GetMinuteInput() {
            return mainControl.MinuteInput;
        }

        #endregion
    }
}
