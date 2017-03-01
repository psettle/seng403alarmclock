using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace seng403alarmclock.Data {
    /// <summary>
    /// Simple data driver for persistent data, can store any serializable object
    /// </summary>
    class DataDriver {
        /// <summary>
        /// The name of the save file for data
        /// </summary>
        private static readonly string saveFileName = "seng403SaveFile.bin";

        /// <summary>
        /// Singleton instance, prevents race conditions
        /// </summary>
        private static DataDriver _instance = new DataDriver();

        /// <summary>
        /// Getter for the instance
        /// </summary>
        public static DataDriver Instance { get { return _instance; } }

        /// <summary>
        /// The loaded/ ready to save data
        /// </summary>
        private Dictionary<string, object> savedData = null;

        
        /// <summary>
        /// Loads any existing data
        /// </summary>
        private DataDriver() {
            LoadData();
        }

        /// <summary>
        /// Saves any existing data
        /// </summary>
        public void shutdown() {
            SaveData();
        }

        /// <summary>
        /// Sets a variable in persistent data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetVariable(string name, object value) {
            savedData[name] = value;
        }

        /// <summary>
        /// gets a variable in persistent data
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">
        /// If the variable doesn't exist in the save file
        /// </exception>
        public object GetVariable(string name) {
            object toReturn;

            if(savedData.TryGetValue(name, out toReturn)) {
                return toReturn;
            } else {
                throw new IndexOutOfRangeException();
            } 
        }

        /// <summary>
        /// Re loads all the data from the save file
        /// </summary>
        private void LoadData() {
            BinaryFormatter serializer = new BinaryFormatter();

            FileStream reader = null;
            try {
                reader = new FileStream(saveFileName, FileMode.Open);
                savedData = (Dictionary<string, object>)serializer.Deserialize(reader);
                //a null reading is the same as a missing read
                if(savedData == null) {
                    throw new FileNotFoundException();
                }
                reader.Close();
            } catch (FileNotFoundException) {
                savedData = new Dictionary<string, object>(); //create a new save object if the file is missing
            }
   
        }

        /// <summary>
        /// Re saves the data in this object
        /// </summary>
        private void SaveData() {
            BinaryFormatter serializer = new BinaryFormatter();

            FileStream writer = new FileStream(saveFileName, FileMode.OpenOrCreate);

            serializer.Serialize(writer, savedData);

            writer.Close();
        }
    }
}
