using seng403alarmclock.GUI;
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
using System.Windows.Shapes;

namespace seng403alarmclock.GUI
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        private static double borderOffset = 20;
        private int snooze_period_minutes = 1;
        
        public OptionsWindow(double LeftOffset, double TopOffset, double height, double width)
        {
            InitializeComponent();
            this.Left   = LeftOffset + borderOffset;
            this.Top    = TopOffset + borderOffset;
            this.Width  = width;
            this.Height = height;

            this.snooze_period_minutes = 1;

            this.Snooze_Period_minutes_Label.Content = this.snooze_period_minutes.ToString();


            SetSnoozePeriod_Button.Visibility          = Visibility.Visible;
            this.snooze_Minus.Visibility                    = Visibility.Visible;
            this.snooze_Plus.Visibility                     = Visibility.Visible;
            this.Snooze_Period_minutes_Label.Visibility     = Visibility.Visible;
                
            this.SetSnoozePeriod_Button.Click   += SetSnoozePeriod_Button_Click;
            this.snooze_Minus.Click             += Snooze_Minus_Click;
            this.snooze_Plus.Click              += Snooze_Plus_Click;
        }

        private void Snooze_Plus_Click(object sender, RoutedEventArgs e)
        {
            this.snooze_period_minutes++;
            this.Snooze_Period_minutes_Label.Content = this.snooze_period_minutes.ToString();
        }

        private void Snooze_Minus_Click(object sender, RoutedEventArgs e)
        {
            this.snooze_period_minutes--;
            this.Snooze_Period_minutes_Label.Content = this.snooze_period_minutes.ToString();

        }

        private void SetSnoozePeriod_Button_Click(object sender, RoutedEventArgs e)
        {
            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(this.snooze_period_minutes);
        }
    }
}
