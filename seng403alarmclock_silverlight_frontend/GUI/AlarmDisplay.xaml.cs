using seng403alarmclock.GUI;
using seng403alarmclock.GUI_Interfaces;
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
            populateFields();

        }
        /// <summary>
        ///  Throws everything in the proper fields
        /// </summary>
        private void populateFields()
        {
            
            this.lbl_alarmName.Content = alarm.alarmName;
            this.lbl_alarmTime.Content = alarm.GetAlarmTime().ToShortTimeString();
            this.tb_repeatDays.Text = createDayOfWeekString();
        }

        /// <summary>
        /// Makes a string to represent what days of the week the alarm repeats
        /// </summary>
        /// <returns> The string</returns>
        private string createDayOfWeekString()
        {
            List<DayOfWeek> days = alarm.GetWeekdays();
            bool[] dayBool = new bool[10];
            string ret = "";
            if (days == null || days.Count <= 0)
            {
                ret = "Never";
            }
            else if (days.Count == 7)
            {
                ret = "Everyday";
            }
            else if (days.Count == 2 && days.Contains(DayOfWeek.Sunday) && days.Contains(DayOfWeek.Saturday))
            {
                ret = "Weekends";
            }
            else if (days.Count == 5 && days.Contains(DayOfWeek.Monday) && days.Contains(DayOfWeek.Tuesday) && days.Contains(DayOfWeek.Wednesday) && days.Contains(DayOfWeek.Thursday) && days.Contains(DayOfWeek.Friday))
            {
                ret = "Weekdays";
            }
            else
            {
                foreach (DayOfWeek day in days)
                {
                    ret += day.ToString().Substring(0, 3) + ", ";
                }

                ret = ret.Substring(0, ret.Length - 2);
            }

            return ret;
        }

        /// <summary>
        /// Redraws the UserControl base on  the new state of the alarm object
        /// </summary>
        public void UpdateAlarm() {
            populateFields();
        }


        private void Cancel_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_cancelFill.Fill = new SolidColorBrush(Color.FromArgb(255, 17, 17, 17));
        }

        private void Cancel_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_cancelFill.Fill = new SolidColorBrush(Color.FromArgb(255, 56, 56, 56));
        }

        private void Edit_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_editFill.Fill = new SolidColorBrush(Color.FromArgb(255, 17,17,17));
        }

        private void Edit_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_editFill.Fill = new SolidColorBrush(Color.FromArgb(255, 56, 56, 56));
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            GuiEventCaller.GetCaller().NotifyCancel(this.alarm, false);
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            GuiController.GetController().OpenEditAlarmPanel(this.alarm);
        }
    }
}
