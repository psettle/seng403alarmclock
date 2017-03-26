using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
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

        Button internalButton = null;

        public DarkButton(Button targetButton) {
            internalButton = targetButton;
            internalButton.MouseEnter += InternalButton_MouseEnter;
            internalButton.MouseLeave += InternalButton_MouseLeave;
        }

        private void InternalButton_MouseLeave(object sender, MouseEventArgs e) {
            SetForegroundAndBackground(Colors.Black, Colors.DarkGray);
        }

        private void InternalButton_MouseEnter(object sender, MouseEventArgs e) {
            SetForegroundAndBackground(Colors.DarkGray, Colors.White);
        }

        /// <summary>
        /// Sets the foreground and background colors on the button
        /// </summary>
        private void SetForegroundAndBackground(Color background, Color foreground) {
            object internalContent = internalButton.Content;

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
