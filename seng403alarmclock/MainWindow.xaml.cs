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
            this.AddAlarmButton.Click += AddAlarmButton_Click;
            //TEST CODE BELOW THIS LINE      
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
            Controls controlWindow = new Controls(this.Left, this.Top, this.Height / 3, this.Width / 1.5);
            controlWindow.ShowDialog();
            controlWindow.Close();
        }
    }
}
