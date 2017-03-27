using seng403alarmclock.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.Windows.Controls.Primitives;
using seng403alarmclock.GUI_Interfaces;

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
        /// Controls the weekday selector on the AddEdit page
        /// </summary>
        private WeekdaySelector weekdayControl = null;

        /// <summary>
        /// Controls the time selector on the AddEdit page
        /// </summary>
        private TimeSelector timeController = null;

        private DropdownSelectorController audioFileController = null;

        /// <summary>
        /// A reference to the alarm that is being edited
        /// </summary>
        private Alarm alarmBeingEdited = null;

        /// <summary>
        /// A lookup table of filename => displayname
        /// </summary>
        private Dictionary<string, string> audioFiles = new Dictionary<string, string>();
        #endregion

        public AddEditWindow(MainPage mainControl) {
            this.mainControl = mainControl;

            mainControl.SaveAlarm.Click += SaveAlarm_Click;
            mainControl.CancelAddEdit.Click += CancelAddEdit_Click;

            timeController = new TimeSelector(this);

            weekdayControl = new WeekdaySelector(mainControl);

           
            weekdayControl.SetVisibleState(Visibility.Collapsed);

            mainControl.RepeatCheckbox.Checked += RepeatCheckbox_Checked;
            mainControl.RepeatCheckbox.Unchecked += RepeatCheckbox_UnChecked;

            audioFileController = new DropdownSelectorController(mainControl.AudioFileComboBox);

            new DarkButton(mainControl.SaveAlarm);
            new DarkButton(mainControl.CancelAddEdit);
        }

        #region PublicInterface

        /// <summary>
        /// Sets the audio files displayed on the dropdown menus
        /// </summary>
        /// <param name="audioFiles">
        /// A table of filename => displayname
        /// </param>
        public void SetAudioFileNames(Dictionary<string, string> audioFiles) {
            this.audioFiles = audioFiles;
            audioFileController.SetElements(audioFiles);
        }

        /// <summary>
        /// Opens the panel in a blank state, ready to input a new alarm
        /// </summary>
        public void OpenAddAlarmPanel() {
            alarmBeingEdited = null;
            EmptyAlarmFields();
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
            PopulateEditAlarmFields();
            SetPanelState(true);
        }

        #endregion

        #region GuiEventHandlers
        /// <summary>
        /// Called when the user clicks the save button
        /// </summary>
        private void SaveAlarm_Click(object sender, RoutedEventArgs e) {
            //collect form info
            string name = mainControl.AddEditLabelBox.Text;

            int hour, minute;
            timeController.GetDisplayTime(out hour, out minute);

            bool repeat = mainControl.RepeatCheckbox.IsChecked.HasValue ? (bool)mainControl.RepeatCheckbox.IsChecked : false;

            List<DayOfWeek> days = weekdayControl.GetActiveDays();

            string audioFile = audioFileController.GetSelected();

            //pass to alarm GuiEventCaller
            if (alarmBeingEdited == null) {
                GuiEventCaller.GetCaller().NotifyAlarmRequested(hour, minute, repeat, audioFile, repeat, days, name);
            } else {
                GuiEventCaller.GetCaller().NotifyAlarmEditRequest(alarmBeingEdited, name, hour, minute, repeat, audioFile, repeat, days);
            }

            //close the panel
            SetPanelState(false);
        }

        /// <summary>
        /// Called when the user clicks the cancel button on the panel
        /// </summary>
        private void CancelAddEdit_Click(object sender, RoutedEventArgs e) {
            //just close the panel
            SetPanelState(false);
        }

        /// <summary>
        /// Called when the repeat box is checked
        /// </summary>
        private void RepeatCheckbox_Checked(object sender, RoutedEventArgs e) {
            weekdayControl.SetVisibleState(Visibility.Visible);
        }

        /// <summary>
        /// Called when the repeat box is unchecked
        /// </summary>
        private void RepeatCheckbox_UnChecked(object sender, RoutedEventArgs e) {
            weekdayControl.SetActiveDays(new List<DayOfWeek>());
            weekdayControl.SetVisibleState(Visibility.Collapsed);
        }
        #endregion

        #region SessionSetupTeardown

        private void EmptyAlarmFields() {
            mainControl.AddEditLabelBox.Text = "";
            timeController.SetDisplayTime(0, 0);

            mainControl.RepeatCheckbox.IsChecked = false;

            weekdayControl.SetActiveDays(new List<DayOfWeek>());
            weekdayControl.SetVisibleState(Visibility.Collapsed);

            string firstAudioFile = "";
            foreach(KeyValuePair<string, string> pair in audioFiles) {
                firstAudioFile = pair.Key;
                break;
            }

            audioFileController.SetSelected(firstAudioFile);
        }

        private void PopulateEditAlarmFields() {
            mainControl.AddEditLabelBox.Text = alarmBeingEdited.alarmName;
            timeController.SetDisplayTime(alarmBeingEdited.GetHour(), alarmBeingEdited.GetMinute());

            mainControl.RepeatCheckbox.IsChecked = alarmBeingEdited.IsRepeating;

            if (alarmBeingEdited.IsRepeating) {
                weekdayControl.SetActiveDays(alarmBeingEdited.GetWeekdays());
                weekdayControl.SetVisibleState(Visibility.Visible);
            } else {
                weekdayControl.SetActiveDays(new List<DayOfWeek>());
                weekdayControl.SetVisibleState(Visibility.Collapsed);
            }

            audioFileController.SetSelected(alarmBeingEdited.GetAudioFile());
            
            
        }

        #endregion

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

        public TextBox GetHourInput() {
            return mainControl.HourInput;
        }

        public TextBox GetMinuteInput() {
            return mainControl.MinuteInput;
        }

        #endregion
    }
}
