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
using System.Windows.Threading;

namespace seng403alarmclock_silverlight_frontend.GUI {
    public partial class AlarmDisplay : UserControl, IComparable {

        /// <summary>
        /// The alarm to display
        /// </summary>
        private Alarm alarm = null;
        /// <summary>
        /// some storyboard
        /// </summary>
        private Storyboard CellBackgroundChangeStory = new Storyboard();
        /// <summary>
        /// boolean to keep track of the alarm state
        /// </summary>
        private bool ringing = false;
        /// <summary>
        /// Creates a new alarm display object for displaying alarm
        /// </summary>
        /// <param name="alarm"></param>
        public AlarmDisplay(Alarm alarm) {
            InitializeComponent();
            this.alarm = alarm;
            PopulateFields();
            ColorAnimation animation;
            animation = new ColorAnimation();
            animation.To = Colors.Red;
            animation.From = Colors.DarkGray;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            //animation.AutoReverse = true;
            animation.RepeatBehavior = RepeatBehavior.Forever;
            PropertyPath colorTargetPath = new PropertyPath("(colouredGrid.Background).(SolidColorBrush.Color)");
            Storyboard.SetTarget(animation, colouredGrid);
            Storyboard.SetTargetProperty(animation, colorTargetPath);
            CellBackgroundChangeStory.Children.Add(animation);

        }
        /// <summary>
        ///  Throws everything in the proper fields
        /// </summary>
        private void PopulateFields()
        {
            
            this.lbl_alarmName.Content = alarm.alarmName;
            this.lbl_alarmTime.Content = alarm.GetAlarmTime().ToShortTimeString();
            this.tb_repeatDays.Text = CreateDayOfWeekString();
        }

        /// <summary>
        /// Makes a string to represent what days of the week the alarm repeats
        /// </summary>
        /// <returns> The string</returns>
        private string CreateDayOfWeekString()
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
            if (alarm.IsRinging)
                StartRinging();
            else if (alarm.IsSnoozing)
            {
                StopRinging();
            }
            else {
                if (ringing == true)
                    StopRinging();
                PopulateFields();

            }

        }
        /// <summary>
        /// Play the ringing animation
        /// </summary>
        public void StartRinging()
        {
            ringing = true;
           
            CellBackgroundChangeStory.Begin();
        }
        /// <summary>
        /// stop the ringing animation
        /// </summary>
        public void StopRinging()
        {
            ringing = false;
            CellBackgroundChangeStory.Stop();
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



        /// <summary>
        /// Compares this alarm display using the target time of the contained alarm
        /// </summary>
        public int CompareTo(object obj) {
            if(obj.GetType() != typeof(AlarmDisplay)) {
                return 0;
            }

            AlarmDisplay toCompareWith = (AlarmDisplay)obj;

            return alarm.GetAlarmTime().CompareTo(toCompareWith.alarm.GetAlarmTime());
        }
    }
}
