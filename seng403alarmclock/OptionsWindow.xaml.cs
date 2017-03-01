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
        private static double relativeSize_ofOptionsWindow;
        private static double borderOffset_width;
        private static double borderOffset_height;

        private static int snooze_period_minutes = 1;
        
        public OptionsWindow(double leftOffset, double topOffset, double heightOfMain, double widthOfMain,  double relativeSize)
        {
            InitializeComponent();
            relativeSize_ofOptionsWindow = relativeSize;

            borderOffset_width = widthOfMain - (widthOfMain * relativeSize_ofOptionsWindow);
            borderOffset_width *= 0.5;

            borderOffset_height = heightOfMain - (heightOfMain * relativeSize_ofOptionsWindow);
            borderOffset_height *= 0.5;

            this.Left   = leftOffset + borderOffset_width;
            this.Top    = topOffset + borderOffset_height;
            this.Width  = widthOfMain * relativeSize_ofOptionsWindow;
            this.Height = heightOfMain * relativeSize_ofOptionsWindow;
            
            this.Snooze_Period_minutes_Label.Content = snooze_period_minutes.ToString();
            
            this.snooze_Minus.Visibility                    = Visibility.Visible;
            this.snooze_Plus.Visibility                     = Visibility.Visible;
            this.Snooze_Period_minutes_Label.Visibility     = Visibility.Visible;
                
            this.snooze_Minus.Click             += Snooze_Minus_Click;
            this.snooze_Plus.Click              += Snooze_Plus_Click;

            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_period_minutes);
        }

        private void Snooze_Plus_Click(object sender, RoutedEventArgs e)
        {
            if(snooze_period_minutes < 59)
                snooze_period_minutes++;
            Snooze_Period_minutes_Label.Content = snooze_period_minutes.ToString();
            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_period_minutes);        }

        private void Snooze_Minus_Click(object sender, RoutedEventArgs e)
        {
            if(snooze_period_minutes > 0)
                snooze_period_minutes--;
            Snooze_Period_minutes_Label.Content = snooze_period_minutes.ToString();
            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_period_minutes);
        } 
        
        public static void SetSnoozePeriodMinutes(int minutes) {
            snooze_period_minutes = minutes;
            GuiEventCaller.GetCaller().NotifySnoozePeriodChangeRequested(snooze_period_minutes);
        }
    }
}
