using seng403alarmclock.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace seng403alarmclock_silverlight_frontend.GUI {
    public partial class AlarmDisplay : UserControl {

        /// <summary>
        /// The alarm to display
        /// </summary>
        private Alarm alarm = null;

        /// <summary>
        /// Creates a new alarm display object for displaying alarm
        /// </summary>
        /// <param name="alarm"></param>
        public AlarmDisplay(Alarm alarm) {
            InitializeComponent();
            this.alarm = alarm;
        }

        /// <summary>
        /// Redraws the UserControl base on  the new state of the alarm object
        /// </summary>
        public void UpdateAlarm() {
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
