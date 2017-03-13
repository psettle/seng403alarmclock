using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using seng403alarmclock.GUI;
using System.Windows.Threading;

namespace seng403alarmclock
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
       
        /// <summary>
        /// Initializes the main controller and assigns it to the GUI controller
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            GuiController.SetMainWindow(this);
            //note: this should probably be moved to another class, can't be arsed right now
            this.Snooze_Button_setHidden();
            this.DismissAll_Button_setHidden();

            this.AddAlarmButton.Click += AddAlarmButton_Click;
            this.Snooze_Button.Click += Snooze_Button_Click;
            this.Options_Button.Click += Options_Button_Click;
            this.DismissAll_Button.Click += DismissAll_Button_Click;

            this.AMPM_Analog.Visibility = Visibility.Hidden;
            this.Analog_setHidden();
            this.DateDisplay_Analog.Visibility = Visibility.Hidden;
            App.SetupMainWindow();
            //TEST CODE BELOW THIS LINE      
        }

        #region dismiss

        private void DismissAll_Button_Click(object sender, RoutedEventArgs e)
        {
            GuiEventCaller.GetCaller().NotifyDismiss();            
        }

        public void DismissAll_Button_setVisible()
        {
            this.DismissAll_Button.Visibility = Visibility.Visible;
        }

        public void DismissAll_Button_setHidden() {
            this.DismissAll_Button.Visibility = Visibility.Hidden;
        }

        #endregion

        #region snooze

        private void Snooze_Button_Click(object sender, RoutedEventArgs e)
        {
            GuiEventCaller.GetCaller().NotifySnoozeRequested();
        }

        public void Snooze_Button_setVisible()
        {
            this.Snooze_Button.Visibility = Visibility.Visible;
        }

        public void Snooze_Button_setHidden()
        {
            this.Snooze_Button.Visibility = Visibility.Hidden;
        }

        #endregion

        #region analog vs digital

        public void SetAnalog()
        {
            this.Analog_setVisible();
            //this.DateDisplay_Analog.Visibility = Visibility.Visible;
            this.DateDisplay.Visibility = Visibility.Hidden;
            this.TimeDisplay.Visibility = Visibility.Hidden;

        }

        public void SetDigital()
        {
            this.TimeDisplay.Visibility = Visibility.Visible;
            this.DateDisplay.Visibility = Visibility.Visible;
            this.Analog_setHidden();

            //this.DateDisplay_Analog.Visibility = Visibility.Hidden;

        }

        public void Analog_setVisible()
        {
            this.HourHand.Visibility = Visibility.Visible;
            this.MinuteHand.Visibility = Visibility.Visible;
            this.ClockBack.Visibility = Visibility.Visible;
            this.AMPM_Analog.Visibility = Visibility.Visible;

        }

        public void Analog_setHidden()
        {
            this.HourHand.Visibility = Visibility.Hidden;
            this.MinuteHand.Visibility = Visibility.Hidden;
            this.ClockBack.Visibility = Visibility.Hidden;
            this.AMPM_Analog.Visibility = Visibility.Hidden;
        }

        #endregion




        private void Options_Button_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new OptionsWindow(this.Left, this.Top, this.Height, this.Width, 0.8);
            //setup the options window while it exists
            GuiController.GetController().SetupOptionsWindow(optionsWindow);
            optionsWindow.ShowDialog();
            optionsWindow.Close();
        }

        
        /// <summary>
        /// Sets the text for the time display directly
        /// </summary>
        /// <param name="text">
        /// The text to put onto the GUI
        /// </param>
        private void SetTimeText(string text) {
            this.TimeDisplay.Text = text;
        }

        /// <summary>
        /// Sets the text for the date display directly
        /// </summary>
        /// <param name="text">
        /// The text to put onto the GUI
        /// </param>
        private void SetDateText(string text) {
            this.DateDisplay.Text = text;
            this.DateDisplay_Analog.Text = text;
        }

        /// <summary>
        /// Set the time display on the clock
        /// </summary>
        /// <param name="time"></param>
        public void SetTime(DateTime time) {
            SetAnalogTime(time);
            SetDateText(time.Date.ToLongDateString());
            SetTimeText(time.ToLongTimeString());
        }

        /// <summary>
        /// Sets the display time on the analog clock
        /// </summary>
        /// <param name="time">The current time</param>
        public void SetAnalogTime(DateTime time) {
            double HourDeg = 0.5 * ((60 * (time.Hour % 12)) + time.Minute);
            RotateTransform hourTransform = new RotateTransform(HourDeg, HourHand.Width / 2, HourHand.Height / 2);
            HourHand.RenderTransform = hourTransform;

            double MinDeg = 6 * time.Minute;
            RotateTransform minTransform = new RotateTransform(MinDeg, MinuteHand.Width / 2, MinuteHand.Height / 2);
            MinuteHand.RenderTransform = minTransform;

            if(time.Hour >= 12) {
                this.AMPM_Analog.Text = "PM";
            } else {
                this.AMPM_Analog.Text = "AM";
            }
        }

        /// <summary>
        /// Graphically add an alarm row to the gui
        /// </summary>
        /// <param name="row">
        /// The alarm row to add to the GUI
        /// </param>
        public void AddAlarmRow(AlarmRow row) {
            row.AddToGUI(this.AlarmPanel);
        }

        /// <summary>
        /// Graphically remove an alarm row from the gui
        /// </summary>
        /// <param name="row">
        /// The row to remove from the GUI
        /// </param>
        public void RemoveAlarmRow(AlarmRow row) {
            row.RemoveFromGUI();
        }

        private void AddAlarmButton_Click(object sender, RoutedEventArgs e) {
            EditAlarmWindow controlWindow = new EditAlarmWindow(Left, Top, ActualHeight, null);
            controlWindow.ShowDialog();
            controlWindow.Close();
        }

    }
}
