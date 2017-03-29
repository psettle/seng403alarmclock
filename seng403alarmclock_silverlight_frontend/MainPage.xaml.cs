using seng403alarmclock.GUI;
using System;
using System.Windows;
using System.Windows.Controls;

namespace seng403alarmclock_silverlight_frontend {
    /// <summary>
    /// This is the main page of the app
    /// </summary>
    public partial class MainPage : UserControl {
        
        #region Attributes and state enum
        
        /// <summary>
        /// there are 3 legal states for mainPage
        /// </summary>
        enum PageState { Normal, OptionsOpen, AddEditOpen };
        
        /// <summary>
        /// tracks which state mainPage is in
        /// </summary>
        private PageState mainPageState;

        #endregion

      
        /// <summary>
        /// Initializes the main page
        /// </summary>
        public MainPage() {
            InitializeComponent();
            GuiController.GetController().assignMainPage(this);

            mainPageState = PageState.Normal;

            AddEditButton.Click += AddEditButton_Click;

            DigitalButton.Click += Digital_Click;
            AnalogButton.Click += Analog_Click;

            this.AMPM_Analog.Visibility = System.Windows.Visibility.Collapsed;
            this.Analog_setHidden();
            this.date_analog.Visibility = System.Windows.Visibility.Collapsed;            
        }      

        #region panel open/close         

        /// <summary>
        /// Clicking this affects the options pane.
        /// If no side-pane is open, it opens the options pane.
        /// If options is open, it closes the options pane.
        /// Otherwise, it does nothing.
        /// </summary>
        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainPageState == PageState.Normal)
            {
                GuiController.GetController().OpenOptionsPanel();
                mainPageState = PageState.OptionsOpen;
            }
            else if (mainPageState == PageState.OptionsOpen)
            {
                GuiController.GetController().CloseOptionsPanel();
                mainPageState = PageState.Normal;
            }
        }

        private void AddEditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {            
            GuiController.GetController().OpenAddAlarmPanel();
        }

        #endregion

        #region analog / digital

        private void Digital_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GuiController.GetController().SetDisplayMode(false);
        }

        private void Analog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GuiController.GetController().SetDisplayMode(true);
        }

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


        public void SetAnalogTime(DateTime time)
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

        public void Digital_setVisible()
        {
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

        private void sDuration_dec_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
