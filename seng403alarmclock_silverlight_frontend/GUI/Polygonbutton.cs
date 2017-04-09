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
    /// This class manages a button with a polygon as the content
    /// </summary>
    public class PolygonButton {

        /// <summary>
        /// The scale transform applied to the button
        /// </summary>
        private ScaleTransform scale = new ScaleTransform();

        /// <summary>
        /// the translation applied to the button
        /// </summary>
        private TranslateTransform translate = new TranslateTransform();

        /// <summary>
        /// Assigns the listeners and prepares the render transforms
        /// </summary>
        /// <param name="button">
        /// A button with a polygon as the content
        /// </param>
        public PolygonButton(Button button) {
            //parse out the polygon object from the button
            Polygon p = (Polygon)button.Content;


            TransformGroup group = new TransformGroup();
            group.Children.Add(scale);
            group.Children.Add(translate);

            //add the render transform the the polygon
            p.RenderTransform = group;

            //add the hover handlers
            button.MouseEnter += Button_MouseEnter;
            button.MouseLeave += Button_MouseLeave;

            
        }

       
        /// <summary>
        /// Called when the mouse leaves the polygon
        /// </summary>
        private void Button_MouseLeave(object sender, MouseEventArgs e) {
            scale.ScaleX = 1.0;
            scale.ScaleY = 1.0;

            translate.Y += 4;
        }

        /// <summary>
        /// Called when the mouse enters the polygon
        /// </summary>
        private void Button_MouseEnter(object sender, MouseEventArgs e) {
            scale.ScaleX = 1.0;
            scale.ScaleY = 1.2;

            translate.Y -= 4;
        }
    }
}
