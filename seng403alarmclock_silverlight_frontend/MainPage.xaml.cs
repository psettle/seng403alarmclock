using seng403alarmclock.GUI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace seng403alarmclock_silverlight_frontend {
    /// <summary>
    /// This is the main page of the app
    /// </summary>
    public partial class MainPage : UserControl {
        /// <summary>
        /// there are 3 legal states for mainPage
        /// </summary>
        enum PageState { Normal, OptionsOpen, AddEditOpen };
        /// <summary>
        /// tracks which state mainPage is in
        /// </summary>
        private PageState mainPageState;

        /// <summary>
        /// Initializes the main page
        /// </summary>
        public MainPage() {
            InitializeComponent();
            GuiController.GetController().assignMainPage(this);            

            AddEditButton.Click += AddEditButton_Click;
            OptionsButton.Click += OptionsButton_Click;
        }

        

        #region buttons handling        
        
        private void AddEditButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            GuiController.GetController().OpenAddAlarmPanel();            
        }

        /// <summary>
        /// Clicking this affects the options pane.
        /// If no pane is visible, it opens the options pane.
        /// If options is open, it closes the options pane.
        /// Otherwise, it does nothing.
        /// </summary>
        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainPageState == PageState.Normal)
            {
                GuiController.GetController().OpenOptionsPanel();
                mainPageState = PageState.OptionsOpen;
            }
            else if (mainPageState == PageState.OptionsOpen)
            {
                GuiController.GetController().CloseOptionsPanel();
                mainPageState = PageState.Normal;
            }
        }

        #endregion

        /// <summary>
        /// Sets the time display on the GUI
        /// </summary>
        /// <param name="time"></param>
        public void SetTime(DateTime time) {
            this.time.Text = time.ToLongTimeString();
        }
    }
}
