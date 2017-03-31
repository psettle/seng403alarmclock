using seng403alarmclock.GUI;
using seng403alarmclock.GUI_Interfaces;
using seng403alarmclock_silverlight_frontend.GUI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace seng403alarmclock_silverlight_frontend {
    /// <summary>
    /// This is the main page of the app
    /// </summary>
    public partial class MainPage : UserControl {
        
        #region Attributes and state enum
        
        /// <summary>
        /// there are 4 legal states for mainPage
        /// </summary>
        enum PanelState { Normal, OptionsOpen, AddEditOpen, AlarmListOpen };
        
        /// <summary>
        /// tracks which state mainPage is in
        /// </summary>
        private PanelState panelState;

        #endregion

      
        /// <summary>
        /// Initializes the main page
        /// </summary>
        public MainPage() {
            InitializeComponent();
            GuiController.GetController().assignMainPage(this);

            panelState = PanelState.Normal;

            addAlarmButton.Click += AddEditButton_Click;
            AlarmList_Button.Click += AlarmList_Button_Click;
            Options_Button.Click += OptionsButton_Click;

            this.AMPM_Analog.Visibility = System.Windows.Visibility.Collapsed;
            this.Analog_setHidden();
            this.date_analog.Visibility = System.Windows.Visibility.Collapsed;            

            new DarkButton(Snooze);
            new DarkButton(Dismiss);

            Snooze.Click += Snooze_Click;
            Dismiss.Click += Dismiss_Click;

            GuiController.GetController().SetDismissAvailable(false);
            GuiController.GetController().SetSnoozeAvailable(false);
        }       

        #region snooze / dismiss

        /// <summary>
        /// Called when the snooze button is clicked
        /// </summary>
        private void Snooze_Click(object sender, RoutedEventArgs e) {
            GuiEventCaller.GetCaller().NotifySnoozeRequested();
        }

        /// <summary>
        /// Called when the dismiss button is clicked
        /// </summary>
        private void Dismiss_Click(object sender, RoutedEventArgs e) {
            GuiEventCaller.GetCaller().NotifyDismiss();
        }

		    #endregion
		
        #region panel open/close         

        /// <summary>
        /// Clicking this affects the options pane.
        /// If no side-pane is open, it opens the options pane.
        /// If options is open, it closes the options pane.
        /// Otherwise, it does nothing.
        /// </summary>
        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (panelState == PanelState.Normal)
            {
                addAlarmButton.Visibility = Visibility.Collapsed;
                AlarmList_Button.Visibility = Visibility.Collapsed;

                panelState = PanelState.OptionsOpen;
				        GuiController.GetController().OpenOptionsPanel();				
            }
            else if (panelState == PanelState.OptionsOpen)
            {
                GuiController.GetController().CloseOptionsPanel();
                panelState = PanelState.Normal;

                addAlarmButton.Visibility = Visibility.Visible;
                AlarmList_Button.Visibility = Visibility.Visible;
            }
        }

		    /// <summary>
        /// opens the AddEdit Panel
        /// </summary>
        private void AddEditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (panelState == PanelState.Normal)
            {
                Options_Button.Visibility = Visibility.Collapsed;
                AlarmList_Button.Visibility = Visibility.Collapsed;
                panelState = PanelState.AddEditOpen;
                GuiController.GetController().OpenAddAlarmPanel();
            }
            else if (panelState == PanelState.AddEditOpen)
            {
                GuiController.GetController().CloseAddEditPanel();
                panelState = PanelState.Normal;
                Options_Button.Visibility = Visibility.Visible;
                AlarmList_Button.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// opens/closes the alarmlist panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmList_Button_Click(object sender, RoutedEventArgs e)
        {
            if (panelState == PanelState.Normal)
            {
                Options_Button.Visibility = Visibility.Collapsed;
                addAlarmButton.Visibility = Visibility.Collapsed;

                panelState = PanelState.AlarmListOpen;
                GuiController.GetController().OpenAlarmListPanel();
            }
            else if (panelState == PanelState.AlarmListOpen)
            {
                GuiController.GetController().CloseAlarmListPanel();
                panelState = PanelState.Normal;
                Options_Button.Visibility = Visibility.Visible;
                addAlarmButton.Visibility = Visibility.Visible;
            }        
        }

        #endregion

        #region analog / digital

        public void SetAnalog()
        {
            this.Analog_setVisible();
            this.Digital_setHidden();
        }

        public void SetDigital()
        {
            this.Analog_setHidden();
            this.Digital_setVisible();
        }


        private void SetAnalogTime(DateTime time)
        {
            double HourDeg = 0.5 * ((60 * (time.Hour % 12)) + time.Minute);
            RotateTransform hourTransform = new RotateTransform();
            hourTransform.Angle = HourDeg;
            hourTransform.CenterX = (HourHand.Width / 2);
            hourTransform.CenterY = (HourHand.Height / 2);
            HourHand.RenderTransform = hourTransform;
            this.date_analog.Text = time.ToLongDateString();
        }

        public void AddAlarmRow(AlarmRow row)
        {
            row.AddToGUI(this.alarmPanel);
        }

        internal void RemoveAlarmRow(AlarmRow row, bool wasPreempted)
        {
            row.RemoveFromGUI();

        }

        private void Analog_setVisible() {
            this.date_analog.Visibility = System.Windows.Visibility.Visible;
            this.HourHand.Visibility = System.Windows.Visibility.Visible;
            this.MinuteHand.Visibility = System.Windows.Visibility.Visible;
            this.ClockBack.Visibility = System.Windows.Visibility.Visible;
            this.AMPM_Analog.Visibility = System.Windows.Visibility.Visible;

        }

        private void Analog_setHidden() {
            this.HourHand.Visibility = System.Windows.Visibility.Collapsed;
            this.MinuteHand.Visibility = System.Windows.Visibility.Collapsed;
            this.ClockBack.Visibility = System.Windows.Visibility.Collapsed;
            this.AMPM_Analog.Visibility = System.Windows.Visibility.Collapsed;
        }


        private void Digital_setHidden() {
            this.date.Visibility = System.Windows.Visibility.Collapsed;
            this.time.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Digital_setVisible() {
            this.time.Visibility = System.Windows.Visibility.Visible;
            this.date.Visibility = System.Windows.Visibility.Visible;
            this.date_analog.Visibility = System.Windows.Visibility.Collapsed;
        }


        #endregion

        /// <summary>
        /// Sets the time display on the GUI
        /// </summary>
        /// <param name="time"></param>
        public void SetTime(DateTime time) {
            SetAnalogTime(time);
            this.time.Text = time.ToLongTimeString();
            this.date.Text = time.ToLongDateString();
            this.date_analog.Text = time.ToLongDateString();
        }

    }
}
