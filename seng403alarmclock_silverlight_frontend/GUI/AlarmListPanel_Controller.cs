using seng403alarmclock.GUI;
using seng403alarmclock.GUI_Interfaces;
using seng403alarmclock.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using static seng403alarmclock_silverlight_frontend.MainPage;

namespace seng403alarmclock_silverlight_frontend.GUI
{
    /// <summary>
    /// This class controls the sliding options panel
    /// </summary>
    public class AlarmListPanel_Controller
    {
        #region Attributes
        /// <summary>
        /// How many slots there are in the list
        /// </summary>
        private readonly static int slotCount = 3;

        /// <summary>
        /// The current state of the Options Panel
        /// </summary>
        private bool isPanelOpen = false;

        /// <summary>
        /// A reference to the main window, used to control the main window
        /// </summary>
        private MainPage mainControl = null;

        /// <summary>
        /// The list of alarms that have been added to this control
        /// </summary>
        private Dictionary<Alarm, AlarmDisplay> alarmDisplayLookup = new Dictionary<Alarm, AlarmDisplay>();

        /// <summary>
        /// A list of the display panels, in the order that they should be rendered
        /// </summary>
        private List<AlarmDisplay> alarmDisplayIndexed = new List<AlarmDisplay>();

        /// <summary>
        /// The index in alarmDisplayIndexed that should be shown at the top of the list
        /// </summary>
        private int topOfTheListIndex = 0;



        #endregion

        #region PublicInterface

        /// <summary>
        /// Addes click listeners to all the elements
        /// </summary>
        /// <param name="mainControl"></param>
        public AlarmListPanel_Controller(MainPage mainControl)
        {
            this.mainControl = mainControl;
            isPanelOpen = false;

            new PolygonButton(mainControl.AlarmListTickDown);
            new PolygonButton(mainControl.AlarmListTickUp);

            mainControl.AlarmListTickDown.Click += AlarmListTickDown_Click;
            mainControl.AlarmListTickUp.Click += AlarmListTickUp;

            RenderCurrentDisplays();

        }

        

        /// <summary>
        /// Adds a new alarm display onto the list that is currently being managed
        /// </summary>
        /// <param name="alarm">
        /// The alarm to add
        /// </param>
        public void AddAlarm(Alarm alarm) {
            AlarmDisplay display = CreateAlarmDisplay(alarm);
            
            alarmDisplayLookup.Add(alarm, display);
            alarmDisplayIndexed.Add(display);

            RenderCurrentDisplays();
        }

        /// <summary>
        /// Removes the display for the provided alarm
        /// </summary>
        /// <param name="alarm"></param>
        public void RemoveAlarm(Alarm alarm) {
            //find the display object
            AlarmDisplay display = alarmDisplayLookup[alarm];

            //delete the lookup entry
            alarmDisplayLookup.Remove(alarm);

            //remove display from the render list
            alarmDisplayIndexed.Remove(display);

            //attempt to move the list back one now that an element is missing
            AttemptDecrementDisplay();

            //redraw the panel
            RenderCurrentDisplays();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alarm"></param>
        public void UpdateAlarm(Alarm alarm) {
            //find the display object
            AlarmDisplay display = alarmDisplayLookup[alarm];

            //tell it to update
            display.UpdateAlarm();

            //redraw
            RenderCurrentDisplays();
        }

        #endregion

        /// <summary>
        /// Stuart: You can do the setup and stuff in here
        /// </summary>
        /// <returns></returns>
        private AlarmDisplay CreateAlarmDisplay(Alarm alarm) {
            return new AlarmDisplay(alarm);
        }


        #region public Panel open/close controls

        /// <summary>
        /// closes options panel 
        /// </summary>
        public void CloseAlarmListPanel() {
            SetPanelState(false);
        }

        /// <summary>
        /// opens option panel
        /// </summary>
        public void OpenAlarmListPanel() {
            SetPanelState(true);
        }

        /// <summary>
        /// Hides the panel if it is closed (effectively only hiding the button
        /// </summary>
        public void HideIfClosed() {
            if (mainControl.panelState != PanelState.AlarmListOpen) {
                mainControl.AlarmListGrid.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Ensures the panel is visible
        /// </summary>
        public void Show() {
            mainControl.AlarmListGrid.Visibility = Visibility.Visible;
        }

        #endregion

        #region private Panel Controls

        /// <summary>
        /// Sets the options panel state
        /// </summary>
        /// <param name="isOpen">
        /// The desired state of the panel, true for open
        /// </param>
        private void SetPanelState(bool isOpen)
        {
            if (isOpen)
            {
                mainControl.Options_Button.Visibility = Visibility.Collapsed;
                mainControl.addAlarmButton.Visibility = Visibility.Collapsed;
                OpenPanel();
            }
            else
            {
                mainControl.Options_Button.Visibility = Visibility.Visible;
                mainControl.addAlarmButton.Visibility = Visibility.Visible;
                ClosePanel();
            }
            isPanelOpen = isOpen;
        }

        /// <summary>
        /// Opens the Options Panel
        /// </summary>
        private void OpenPanel()
        {
            if (mainControl.panelState != PanelState.AlarmListOpen) {
                if (mainControl.panelState != PanelState.Normal) {
                    GuiController.GetController().CloseAllPanels();
                }

                mainControl.AlarmListSlideIn.Begin();
                mainControl.panelState = PanelState.AlarmListOpen;

                GuiController.GetController().HideClosedPanels();
            }   
        }

        /// <summary>
        /// Closes the Options panel
        /// </summary>
        private void ClosePanel()
        {
            if (mainControl.panelState == PanelState.AlarmListOpen) {
                mainControl.AlarmListSlideOut.Begin();
                mainControl.panelState = PanelState.Normal;

                GuiController.GetController().ShowAllPanels();
            }
                
        }

        #endregion

        #region RenderControl

        /// <summary>
        /// Re draws the alarm list, putting the correct displays in the available slots
        /// </summary>
        private void RenderCurrentDisplays() {
            //sort the alarms to render
            alarmDisplayIndexed.Sort();

            //iterate over all slots and empty them
            for (int i = 1; i <= slotCount; i++) {
                Grid targetSlot = GetDisplaySlot(i);
                targetSlot.Children.Clear();
            }


            //iterate from the current top to the end of the list, and from slot 1 to 3, whichever ends first
            for (int alarmIndex = topOfTheListIndex, displaySlot = 1; alarmIndex < alarmDisplayIndexed.Count && displaySlot <= slotCount; alarmIndex++, displaySlot++) {
                Grid targetSlot = GetDisplaySlot(displaySlot);

                AlarmDisplay targetDisplay = alarmDisplayIndexed[alarmIndex];

                targetSlot.Children.Add(targetDisplay);
            }

            //set the button visibility as appropriate
            if (topOfTheListIndex == 0) {
                mainControl.AlarmListTickUp.Visibility = Visibility.Collapsed;
            } else {
                mainControl.AlarmListTickUp.Visibility = Visibility.Visible;
            }

            if(topOfTheListIndex >= alarmDisplayIndexed.Count - slotCount) {
                mainControl.AlarmListTickDown.Visibility = Visibility.Collapsed;
            } else {
                mainControl.AlarmListTickDown.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Gets the grid to display an alarm in based off of a provided index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Grid GetDisplaySlot(int index) {
            switch (index) {
                case 1:
                    return mainControl.AlarmSlot1;
                case 2:
                    return mainControl.AlarmSlot2;
                case 3:
                    return mainControl.AlarmSlot3;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Attempts to increment the display list, will succeed if there are things to increment into
        /// </summary>
        private void AttemptIncrementDisplay() {
            if (topOfTheListIndex < alarmDisplayIndexed.Count - slotCount) {
                topOfTheListIndex++;
                RenderCurrentDisplays();
            } 
        }

        /// <summary>
        /// Attempts the decrement the display list, will success if there are things to decrement into
        /// </summary>
        private void AttemptDecrementDisplay() {
            if(topOfTheListIndex > 0) {
                topOfTheListIndex--;
                RenderCurrentDisplays();
            }  
        }


        #endregion

        #region ButtonListeners

        /// <summary>
        /// Called when the down button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmListTickDown_Click(object sender, RoutedEventArgs e) {
            AttemptIncrementDisplay();
        }

        /// <summary>
        /// Called when the up button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmListTickUp(object sender, RoutedEventArgs e) {
            AttemptDecrementDisplay();
        }


        #endregion

    }
}










































