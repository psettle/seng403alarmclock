using seng403alarmclock.GUI;
using System;
using System.Windows.Controls;

namespace seng403alarmclock_silverlight_frontend {
    /// <summary>
    /// This is the main page of the app
    /// </summary>
    public partial class MainPage : UserControl {

        private static bool AddEditPanelIsIn = false;
        
        /// <summary>
        /// Initializes the main page
        /// </summary>
        public MainPage() {
            InitializeComponent();
            GuiController.GetController().assignMainPage(this);
            button.Click += Button_Click;
            //AddEditSlide.AutoReverse = true;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e) {
            if(AddEditPanelIsIn) {
                AddEditSlideOut.Begin();
            } else {
                AddEditSlideIn.Begin();
            }

            AddEditPanelIsIn = !AddEditPanelIsIn;
            
        }



        /// <summary>
        /// Sets the time display on the GUI
        /// </summary>
        /// <param name="time"></param>
        public void SetTime(DateTime time) {
            this.time.Text = time.ToLongTimeString();
        }
    }
}
