using System;
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

namespace seng403alarmclock_silverlight_frontend.GUI {
    /// <summary>
    /// Controlled class for the custom dark buttons
    /// </summary>
    public class DarkButton {
        /// <summary>
        /// The button being manipulated
        /// </summary>
        UIElement internalButton = null;

        private Color IdleBackground = Colors.Black;
        private Color IdleForeground = Colors.DarkGray;
        private Color ActiveBackground = Colors.DarkGray;
        private Color ActiveForeground = Colors.Black;

        private bool isHovered = false;


        /// <summary>
        /// Assigns the click listeners to the target button
        /// </summary>
        public DarkButton(UIElement targetButton) {
            internalButton = targetButton;
            internalButton.MouseEnter += InternalButton_MouseEnter;
            internalButton.MouseLeave += InternalButton_MouseLeave;
        }

        /// <summary>
        /// Sets the idle button colours
        /// </summary>
        public void SetIdleColors(Color background, Color foreground) {
            IdleBackground = background;
            IdleForeground = foreground;
            Render();
        }

        /// <summary>
        /// Sets the active button colours
        /// </summary>
        public void SetActiveColors(Color background, Color foreground) {
            ActiveBackground = background;
            ActiveForeground = foreground;
            Render();
        }

        /// <summary>
        ///  Called when the mouse leaves the button
        /// </summary>
        private void InternalButton_MouseLeave(object sender, MouseEventArgs e) {
            isHovered = false;
            Render();
        }

        /// <summary>
        ///  Called when the mouse enters the button
        /// </summary>
        private void InternalButton_MouseEnter(object sender, MouseEventArgs e) {
            isHovered = true;
            Render();
        }

        /// <summary>
        /// Redraws the elements colours
        /// </summary>
        private void Render() {
            if(isHovered) {
                SetForegroundAndBackground(ActiveBackground, ActiveForeground);
            } else {
                SetForegroundAndBackground(IdleBackground, IdleForeground);
            }
        }

        /// <summary>
        /// Sets the foreground and background colors on the button
        /// </summary>
        private void SetForegroundAndBackground(Color background, Color foreground) {
            object internalContent = null;
            if (internalButton.GetType() == typeof(Button)) {
                internalContent = ((Button)internalButton).Content;
            } else if(internalButton.GetType() == typeof(RepeatButton)) {
                internalContent = ((RepeatButton)internalButton).Content;
            } else {
                return; //ui element isn't a button type
            }
            

            //if the internal content is not a grid, its the wrong type of button, skip it
            if (internalContent.GetType() != typeof(Grid)) {
                return;
            }

            Grid internalGrid = (Grid)internalContent;

            foreach (UIElement child in internalGrid.Children) {
                if (child.GetType() == typeof(Label)) {
                    Label label = (Label)child;
                    label.Foreground = new SolidColorBrush(foreground);
                } else if (child.GetType() == typeof(Rectangle)) {
                    Rectangle rect = (Rectangle)child;
                    rect.Fill = new SolidColorBrush(background);
                }
            }
        }
    }
}
