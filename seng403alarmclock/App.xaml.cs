using seng403alarmclock.GUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using seng403alarmclock;
using seng403alarmclock.Model;

namespace seng403alarmclock
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Called when the application starts
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup (StartupEventArgs e) {
            base.OnStartup(e);
            AlarmController ac = new AlarmController();
            GuiEventCaller.GetCaller().AddListener(ac);
            TimeController tc = new TimeController();
            TimePulseGenerator.fetch().add(tc);
            TimePulseGenerator.fetch().add(ac);

            GuiController.GetController().SetAudioFileNames(new Dictionary<string, string>() { { "hello", "bye" } });
        }

        /// <summary>
        /// Called when the application exits
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            AudioController.GetController().endAllAlarms();
        }
    }
}
