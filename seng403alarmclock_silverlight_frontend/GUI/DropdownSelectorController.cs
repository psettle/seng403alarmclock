using System.Collections.Generic;
using System.Windows.Controls;

namespace seng403alarmclock_silverlight_frontend.GUI {
    /// <summary>
    /// Controls a dropdown menu
    /// </summary>
    public class DropdownSelectorController {

        /// <summary>
        /// The dropdown being controlled
        /// </summary>
        private ComboBox dropdown = null;

        /// <summary>
        /// The set of items in the drop down
        /// </summary>
        private Dictionary<ComboBoxItem, string> items_box2name = new Dictionary<ComboBoxItem, string>();

        /// <summary>
        /// The set of items in the drop down
        /// </summary>
        private Dictionary<string, ComboBoxItem> items_name2box = new Dictionary<string, ComboBoxItem>();

        /// <summary>
        /// Assigns the combobox to control
        /// </summary>
        public DropdownSelectorController(ComboBox dropdown) {
            this.dropdown = dropdown;
        }

        /// <summary>
        /// Sets the elements to display on the box
        /// </summary>
        /// <param name="elements">
        /// A table of MachineName => DisplayName
        /// </param>
        public void SetElements(Dictionary<string, string> elements) {
            //empty the dropdown
            items_name2box = new Dictionary<string, ComboBoxItem>();
            items_box2name = new Dictionary<ComboBoxItem, string>();
            dropdown.Items.Clear();

            //add the new elements
            foreach (KeyValuePair<string, string> pair in elements) {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = pair.Value;
                dropdown.Items.Add(item);

                items_box2name.Add(item, pair.Key);
                items_name2box.Add(pair.Key, item);
            }
        }

        /// <summary>
        /// Sets the selected element
        /// </summary>
        /// <param name="selected">
        /// The machine name of the selected element
        /// </param>
        public void SetSelected(string selected) {
            ComboBoxItem shouldBeSelected = items_name2box[selected];
            dropdown.SelectedItem = shouldBeSelected;
        }

        /// <summary>
        /// Gets the selected element
        /// </summary>
        /// <returns>
        /// The MachineName of the selected element
        /// </returns>
        public string GetSelected() {
            ComboBoxItem selectedComboBoxItem = (ComboBoxItem)dropdown.SelectedItem;
            return items_box2name[selectedComboBoxItem];
        }
    }
}
