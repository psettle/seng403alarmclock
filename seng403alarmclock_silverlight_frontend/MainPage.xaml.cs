using seng403alarmclock.GUI;
using System;
using System.Windows.Controls;

namespace seng403alarmclock_silverlight_frontend {
    /// <summary>
    /// This is the main page of the app
    /// </summary>
    public partial class MainPage : UserControl {
      
        /// <summary>
        /// Initializes the main page
        /// </summary>
        public MainPage() {
            InitializeComponent();
            GuiController.GetController().assignMainPage(this);
            button.Click += Button_Click;
            
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e) {
            GuiController.GetController().OpenAddAlarmPanel();
            
        }



        /// <summary>
        /// Sets the time display on the GUI
        /// </summary>
        /// <param name="time"></param>
        public void SetTime(DateTime time) {
            this.time.Text = time.ToLongTimeString();

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
    }
}
