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
        DispatcherTimer timer;
        private double HourDeg = 0;
        private double MinDeg = 0;
        public DateTime currentTime;

        /// <summary>
        /// Initializes the main controller and assigns it to the GUI controller
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            GuiController.SetMainWindow(this);
            //note: this should probably be moved to another class, can't be arsed right now
            this.Snooze_Button_setHidden();

            this.AddAlarmButton.Click += AddAlarmButton_Click;
            this.Snooze_Button.Click += Snooze_Button_Click;
            this.Options_Button.Click += Options_Button_Click;

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Start();
            this.Analog_setHidden();
            this.DateDisplay_Analog.Visibility = Visibility.Hidden;
            App.SetupMainWindow();
            //TEST CODE BELOW THIS LINE      
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            // needs cleaning 
           
            HourDeg = 0.5 * ((60 * (currentTime.Hour % 12)) + currentTime.Minute);
            RotateTransform hourTransform = new RotateTransform(HourDeg, HourHand.Width / 2, HourHand.Height / 2);
            HourHand.RenderTransform = hourTransform;

            MinDeg = 6 * currentTime.Minute;
            RotateTransform minTransform = new RotateTransform(MinDeg, MinuteHand.Width / 2, MinuteHand.Height / 2);
            MinuteHand.RenderTransform = minTransform;
        }

        private void Options_Button_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new OptionsWindow(this.Left, this.Top, this.Height, this.Width, 0.8);
            //setup the options window while it exists
            GuiController.GetController().SetupOptionsWindow(optionsWindow);
            optionsWindow.ShowDialog();
            optionsWindow.Close();
          
 
        }

        public void SetAnalog() {
            this.Analog_setVisible();
            this.DateDisplay_Analog.Visibility = Visibility.Visible;
            this.DateDisplay.Visibility = Visibility.Hidden;
            this.TimeDisplay.Visibility = Visibility.Hidden;   

        }

        public void SetDigital() {
            this.TimeDisplay.Visibility = Visibility.Visible;
            this.DateDisplay.Visibility = Visibility.Visible;
            this.Analog_setHidden();
            this.DateDisplay_Analog.Visibility = Visibility.Hidden;

        }
        /// <summary>
        /// Sets the text for the time display directly
        /// </summary>
        /// <param name="text">
        /// The text to put onto the GUI
        /// </param>
        public void SetTimeText(string text) {
            this.TimeDisplay.Text = text;
        }

        /// <summary>
        /// Sets the text for the date display directly
        /// </summary>
        /// <param name="text">
        /// The text to put onto the GUI
        /// </param>
        public void SetDateText(string text) {
            this.DateDisplay.Text = text;
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
            Controls controlWindow = new Controls(Left, Top, ActualHeight);
            controlWindow.ShowDialog();
            controlWindow.Close();
        }

        private void Snooze_Button_Click(object sender, RoutedEventArgs e)
        {
            GuiEventCaller.GetCaller().NotifySnoozeRequested(new Alarm() );         
            // I think we should skip snoozing indivitual alarms. Users want X minutes of quiet time imo -Nathan
        }

        public void Snooze_Button_setVisible()
        {
            this.Snooze_Button.Visibility = Visibility.Visible;
        }

        public void Snooze_Button_setHidden()
        {
            this.Snooze_Button.Visibility = Visibility.Hidden;
        }

        public void Analog_setVisible()
        {
            this.HourHand.Visibility = Visibility.Visible;
            this.MinuteHand.Visibility = Visibility.Visible;
            this.ClockBack.Visibility = Visibility.Visible;

        }

        public void Analog_setHidden()
        {
            this.HourHand.Visibility = Visibility.Hidden;
            this.MinuteHand.Visibility = Visibility.Hidden;
            this.ClockBack.Visibility = Visibility.Hidden;
        }

        
    }
}
