using seng403alarmclock.GUI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace seng403alarmclock_silverlight_frontend {
    /// <summary>
    /// This is the main page of the app
    /// </summary>
    public partial class MainPage : UserControl {
        /// <summary>
        /// there are 3 legal states for mainPage
        /// </summary>
        enum PageState { Normal, OptionsOpen, AddEditOpen };
        /// <summary>
        /// tracks which state mainPage is in
        /// </summary>
        private PageState mainPageState;

        /// <summary>
        /// Initializes the main page
        /// </summary>
        public MainPage() {
            InitializeComponent();
            GuiController.GetController().assignMainPage(this);            

            AddEditButton.Click += AddEditButton_Click;
            OptionsButton.Click += OptionsButton_Click;
            GuiController.GetController().assignMainPage(this);
            button.Click += Button_Click;
            DigitalButton.Click += Digital_Click;
            AnalogButton.Click += Analog_Click;

            this.AMPM_Analog.Visibility = System.Windows.Visibility.Collapsed;
            this.Analog_setHidden();
            this.date_analog.Visibility = System.Windows.Visibility.Collapsed;

        }

        

        #region buttons handling        
        
        private void AddEditButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            GuiController.GetController().OpenAddAlarmPanel();            
        }

        /// <summary>
        /// Clicking this affects the options pane.
        /// If no pane is visible, it opens the options pane.
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
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e) {
            GuiController.GetController().OpenAddAlarmPanel();
            
        }

        private void Digital_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GuiController.GetController().SetDisplayMode(false);
        }

        private void Analog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GuiController.GetController().SetDisplayMode(true);
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

            double MinDeg = 6 * time.Minute;
            RotateTransform minTransform = new RotateTransform();
            minTransform.Angle = MinDeg;
            minTransform.CenterX = (MinuteHand.Width / 2);
            minTransform.CenterY = (MinuteHand.Height / 2);
            MinuteHand.RenderTransform = minTransform;

            if (time.Hour >= 12)
            {
                this.AMPM_Analog.Text = "PM";
            }
            else
            {
                this.AMPM_Analog.Text = "AM";
            }
        }

        public void Analog_setVisible()
        {
            this.date_analog.Visibility = System.Windows.Visibility.Visible;
            this.HourHand.Visibility = System.Windows.Visibility.Visible;
            this.MinuteHand.Visibility = System.Windows.Visibility.Visible;
            this.ClockBack.Visibility = System.Windows.Visibility.Visible;
            this.AMPM_Analog.Visibility = System.Windows.Visibility.Visible;

        }

        public void Analog_setHidden()
        {
            this.HourHand.Visibility = System.Windows.Visibility.Collapsed;
            this.MinuteHand.Visibility = System.Windows.Visibility.Collapsed;
            this.ClockBack.Visibility = System.Windows.Visibility.Collapsed;
            this.AMPM_Analog.Visibility = System.Windows.Visibility.Collapsed;
        }


        public void Digital_setHidden()
        {
            this.date.Visibility = System.Windows.Visibility.Collapsed;
            this.time.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void Digital_setVisible()
        {
            this.time.Visibility = System.Windows.Visibility.Visible;
            this.date.Visibility = System.Windows.Visibility.Visible;
            this.date_analog.Visibility = System.Windows.Visibility.Collapsed;
        }

    }

}
